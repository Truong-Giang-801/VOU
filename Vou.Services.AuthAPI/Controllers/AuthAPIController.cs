using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Vou.Services.AuthAPI.Data;
using Vou.Services.AuthAPI.Models;
using Vou.Services.AuthAPI.Models.Dto;
using Vou.Services.AuthAPI.Service.IService;

namespace Vou.Services.AuthAPI.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthAPIController : ControllerBase
    {
        private readonly IAuthService? _iAuthService;
        protected ResponeDto _responeDto;
        private IMapper _mapper;
        private readonly AppDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUserBrandService _userBrandService;

        public AuthAPIController(IUserBrandService userBrandService, IAuthService? iAuthService, UserManager<ApplicationUser> userManager, ResponeDto responeDto, AppDbContext appDbContext, IMapper mapper)
        {
            _iAuthService = iAuthService;
            _responeDto = responeDto;
            _mapper = mapper;
            _db = appDbContext;
            _userManager = userManager;
            _userBrandService = userBrandService;
        }

        [HttpGet("user-brand/{brandId}")]
        public async Task<IActionResult> GetUsersByBrandId(int brandId)
        {
            try
            {
                var usersByBrand = await _userBrandService.GetUsersByBrandIdAsync(brandId);
                if (usersByBrand == null || !usersByBrand.Any())
                {
                    return NotFound(new ResponeDto
                    {
                        IsSuccess = false,
                        Message = "No users found for the specified brand ID"
                    });
                }

                return Ok(new ResponeDto
                {
                    IsSuccess = true,
                    Result = usersByBrand
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResponeDto
                {
                    IsSuccess = false,
                    Message = ex.Message
                });
            }
        }

        [HttpGet("user")]
        //[Authorize(Roles = "ADMIN")]
        public async Task<ResponeDto> Get()
        {
            try
            {
                // Fetch all users from the database
                var users = _db.ApplicationUsers.ToList();

                // Create a list to store the user DTOs
                var userDtos = new List<UserDto>();

                // Loop through each user and fetch their roles
                foreach (var user in users)
                {
                    var roles = await _userManager.GetRolesAsync(user); // Fetch roles for the user

                    // Map the user to the UserDto
                    var userDto = _mapper.Map<UserDto>(user);

                    // Add the first role (or multiple if needed)
                    userDto.Role = roles.FirstOrDefault() ?? string.Empty;

                    // Add the UserDto to the list
                    userDtos.Add(userDto);
                }

                // Set the result in the response DTO
                _responeDto.Result = userDtos;
            }
            catch (Exception ex)
            {
                _responeDto.IsSuccess = false;
                _responeDto.Message = ex.Message;
            }
            return _responeDto;
        }

        [HttpPost("activate-deactivate")]
        public async Task<IActionResult> ActivateDeactivateUser([FromBody] ActivateDeactivateUserDto dto)
        {
            try
            {
                bool success = await _iAuthService.ActivateDeactivateUser(dto.Identifier, dto.IsActive);

                if (success)
                {
                    var userStatus = dto.IsActive ? "activated" : "deactivated";
                    return Ok(new
                    {
                        Message = $"User {userStatus} successfully.",
                        IsActive = dto.IsActive
                    });
                }
                else
                {
                    return BadRequest(new
                    {
                        Message = "Failed to activate/deactivate user.",
                        Details = "Please contact support if the problem persists."
                    });
                }
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.WriteLine($"Error activating/deactivating user: {ex.Message}");
                return StatusCode(500, new
                {
                    Message = "An unexpected error occurred.",
                    Details = "Our team has been notified and will investigate the issue."
                });
            }
        }

        [HttpGet("user/{id}")]
        //[Authorize(Roles = "ADMIN")]
        public ResponeDto Get(string id)
        {
            try
            {
                ApplicationUser objList = _db.ApplicationUsers.First(u => u.Id == id);
                _responeDto.Result = _mapper.Map<UserDto>(objList);
            }
            catch (Exception ex)
            {
                _responeDto.IsSuccess = false;
                _responeDto.Message = ex.Message;
            }
            return _responeDto;
        }

        [HttpPut("update")]
        //[Authorize(Roles = "ADMIN")]
        public ResponeDto Put([FromBody] UserDto userDto)
        {
            try
            {
                ApplicationUser obj = _mapper.Map<ApplicationUser>(userDto);
                _db.ApplicationUsers.Update(obj); // Updated to Use Update
                _db.SaveChanges();

                _responeDto.Result = _mapper.Map<UserDto>(obj);
            }
            catch (Exception ex)
            {
                _responeDto.IsSuccess = false;
                _responeDto.Message = ex.Message;
            }
            return _responeDto;
        }
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegistrationRequestDto model)
        {
            if (!ModelState.IsValid)
            {
                // Collect all validation errors
                var errors = ModelState.Values.SelectMany(v => v.Errors)
                                              .Select(e => e.ErrorMessage)
                                              .ToList();

                var response = new ResponeDto
                {
                    IsSuccess = false,
                    Message = "Validation failed.",
                    Result = errors // Returning the list of errors
                };

                return BadRequest(response);
            }

            var result = await _iAuthService.Register(model);

            if (result.IsSuccess == false)
            {
                return BadRequest(result);  // Return custom error message
            }

            return Ok(result);  // Success case
        }





        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto loginRequestDto)
        {
            // Check that at least one of Username or PhoneNumber is provided
            if (string.IsNullOrEmpty(loginRequestDto.Username) && string.IsNullOrEmpty(loginRequestDto.PhoneNumber))
            {
                return BadRequest(new ResponeDto
                {
                    IsSuccess = false,
                    Message = "Username or phone number is required"
                });
            }

            LoginResponeDto loginRespone;

            // Check if Username is provided and perform login by username
            if (!string.IsNullOrEmpty(loginRequestDto.Username))
            {
                loginRespone = await _iAuthService.Login(loginRequestDto);
            }
            // Check if PhoneNumber is provided and perform login by phone number
            else if (!string.IsNullOrEmpty(loginRequestDto.PhoneNumber))
            {
                loginRespone = await _iAuthService.LoginByPhoneNumber(loginRequestDto);
            }
            else
            {
                return BadRequest(new ResponeDto
                {
                    IsSuccess = false,
                    Message = "Invalid login details"
                });
            }

            if (loginRespone.User == null)
            {
                return BadRequest(new ResponeDto
                {
                    IsSuccess = false,
                    Message = "Username or phone number or password is incorrect"
                });
            }

            return Ok(new ResponeDto
            {
                IsSuccess = true,
                Result = loginRespone,
                Message = "Login successful"
            });
        }

        [HttpPost("assignRole")]
        public async Task<IActionResult> AssignRole([FromBody] RegistrationRequestDto registrationRequestDto)
        {
            try
            {
                // Step 1: Check if the user exists
                var user = await _userManager.FindByEmailAsync(registrationRequestDto.Email);
                if (user == null)
                {
                    _responeDto.IsSuccess = false;
                    _responeDto.Message = "User not found.";
                    return NotFound(_responeDto);
                }

                // Step 2: Check if the role name is provided
                if (string.IsNullOrEmpty(registrationRequestDto.RoleName))
                {
                    _responeDto.IsSuccess = false;
                    _responeDto.Message = "Role name is required.";
                    return BadRequest(_responeDto);
                }

                // Step 3: Check if the role exists (if necessary, depending on your setup)
                var roleExists = await _userManager.IsInRoleAsync(user, registrationRequestDto.RoleName);
                if (roleExists)
                {
                    _responeDto.IsSuccess = false;
                    _responeDto.Message = "User already has this role.";
                    return BadRequest(_responeDto);
                }

                // Step 4: Assign the role to the user
                var result = await _userManager.AddToRoleAsync(user, registrationRequestDto.RoleName);

                if (result.Succeeded)
                {
                    _responeDto.IsSuccess = true;
                    _responeDto.Message = "Role assigned successfully.";
                    return Ok(_responeDto);
                }

                // Handle failures in assigning role
                _responeDto.IsSuccess = false;
                _responeDto.Message = "Failed to assign role.";
                return BadRequest(_responeDto);
            }
            catch (Exception ex)
            {
                _responeDto.IsSuccess = false;
                _responeDto.Message = $"An error occurred: {ex.Message}";
                return StatusCode(500, _responeDto);
            }
        }


        [HttpGet("user-brand")]
        //[Authorize(Roles = "ADMIN")]
        public ResponeDto GetUserBrand()
        {
            try
            {
                IEnumerable<UserBrand> objList = _db.UserBrand.ToList();
                _responeDto.Result = _mapper.Map<IEnumerable<UserBrand>>(objList);
            }
            catch (Exception ex)
            {
                _responeDto.IsSuccess = false;
                _responeDto.Message = ex.Message;
            }
            return _responeDto;
        }

        [HttpGet("user-brand/{brandId}/{userId}")]
        public async Task<IActionResult> GetUserBrandById(int brandId, string userId)
        {
            try
            {
                var userBrand = await _userBrandService.GetUserBrandByIdAsync(brandId, userId);
                if (userBrand == null)
                {
                    return NotFound();
                }
                return Ok(new ResponeDto
                {
                    IsSuccess = true,
                    Result = userBrand
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResponeDto
                {
                    IsSuccess = false,
                    Message = ex.Message
                });
            }
        }
    }
}
