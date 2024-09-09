using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
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

                // Loop through each user and fetch their roles and lockout status
                foreach (var user in users)
                {
                    var roles = await _userManager.GetRolesAsync(user); // Fetch roles for the user
                    bool isLockedOut = await _userManager.IsLockedOutAsync(user); // Fetch lockout status

                    // Map the user to the UserDto
                    var userDto = _mapper.Map<UserDto>(user);

                    // Add the first role (or multiple if needed)
                    userDto.Role = roles.FirstOrDefault() ?? string.Empty;

                    // Set the lockout status
                    userDto.Lockout = isLockedOut ? "Locked" : "Activated";

                    // Add the UserDto to the list
                    userDtos.Add(userDto);
                }

                // Set the result in the response DTO
                _responeDto.Result = userDtos;
                _responeDto.IsSuccess = true;
                _responeDto.Message = "Users retrieved successfully.";
            }
            catch (Exception ex)
            {
                _responeDto.IsSuccess = false;
                _responeDto.Message = $"An error occurred: {ex.Message}";
            }
            return _responeDto;
        }


        // Example usage in controller
        [HttpPut("activate-deactivate")]
        public async Task<IActionResult> ActivateDeactivateUser([FromBody] ActivateDeactivateUserDto dto)
        {
            try
            {
                var userDto = await _iAuthService.ActivateDeactivateUser(dto.Identifier, dto.IsActive);

                if (userDto == null)
                {
                    return BadRequest(new { Message = "Failed to activate/deactivate user." });
                }

                var userStatus = dto.IsActive ? "activated" : "deactivated";
                return Ok(new
                {
                    Message = $"User {userStatus} successfully.",
                    IsActive = dto.IsActive,
                    User = userDto
                });
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
        [HttpGet("user/phone/{phoneNumber}")]
        public async Task<ResponeDto> GetByPhoneNumber(string phoneNumber)
        {
            var response = new ResponeDto();
            try
            {
                // Retrieve the user by phone number
                var user = await _db.ApplicationUsers
                    .FirstOrDefaultAsync(u => u.PhoneNumber == phoneNumber);

                if (user == null)
                {
                    response.IsSuccess = false;
                    response.Message = "Người dùng không tồn tại";
                    return response;
                }

                response.Result = _mapper.Map<UserDto>(user);
                response.IsSuccess = true;
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ex.Message;
            }
            return response;
        }

        [HttpGet("user/email/{email}")]
        public async Task<ResponeDto> GetByEmail(string email)
        {
            var response = new ResponeDto();
            try
            {
                // Retrieve the user by email
                var user = await _db.ApplicationUsers
                    .FirstOrDefaultAsync(u => u.Email == email);

                if (user == null)
                {
                    response.IsSuccess = false;
                    response.Message = "User not found.";
                    return response;
                }

                response.Result = _mapper.Map<UserDto>(user);
                response.IsSuccess = true;
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ex.Message;
            }
            return response;
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
            var response = new ResponeDto();

            // Manually validate the model
            var validationResults = new List<ValidationResult>();
            var validationContext = new ValidationContext(model);
            bool isValid = Validator.TryValidateObject(model, validationContext, validationResults, true);

            if (!isValid)
            {
                // Collect all validation errors
                var errors = validationResults.Select(vr => vr.ErrorMessage).ToList();

                response.IsSuccess = false;
                response.Message = "Validation failed.";
                response.Result = errors; // Return the list of errors

                return BadRequest(response);
            }

            // Proceed with registration
            var result = await _iAuthService.Register(model);

            if (result.IsSuccess == false)
            {
                // Return the error result from the registration service
                response.IsSuccess = false;
                response.Message = result.Message;
                response.Result = result.Result; // This can include details about the error

                return BadRequest(response); // Return custom error message
            }

            // Success case
            response.IsSuccess = true;
            response.Message = "Registration successful";
            response.Result = result.Result; // Return the success result

            return Ok(response);
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
                    Message = "Vui lòng điền đủ các trường"
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
                    Message = "Thông tin đăng nhập không hợp lệ"
                });
            }

            if (loginRespone.User == null)
            {
                return BadRequest(new ResponeDto
                {
                    IsSuccess = false,
                    Message = "Thông tin đăng nhập sai"
                });
            }

            return Ok(new ResponeDto
            {
                IsSuccess = true,
                Result = loginRespone,
                Message = "Đăng nhập thành công"
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
