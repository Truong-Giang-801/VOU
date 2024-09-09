using Microsoft.AspNetCore.Identity;
using Vou.Services.AuthAPI.Data;
using Vou.Services.AuthAPI.Models;
using Vou.Services.AuthAPI.Models.Dto;
using Vou.Services.AuthAPI.Service.IService;

namespace Vou.Services.AuthAPI.Service
{
    public class AuthService : IAuthService
    {
        private readonly AppDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;
        public AuthService(AppDbContext appDbContext, UserManager<ApplicationUser> userManager, IJwtTokenGenerator jwtTokenGenerator,
            RoleManager<IdentityRole> roleManager)
        {
            _db = appDbContext;
            _userManager = userManager;
            _roleManager = roleManager;
            _jwtTokenGenerator = jwtTokenGenerator;
        }
        public async Task<bool> AssignRole(string email, string roleName)
        {
            var user = _db.ApplicationUsers.FirstOrDefault(u => u.Email.ToLower() == email.ToLower());
            if (user != null) 
            {
                if (!_roleManager.RoleExistsAsync(roleName).GetAwaiter().GetResult())
                {
                    //create role if not exist
                    _roleManager.CreateAsync(new IdentityRole(roleName)).GetAwaiter().GetResult();
                }
                await _userManager.AddToRoleAsync(user, roleName);
                return true;
            }
            return false;
        }

        public async Task<LoginResponeDto> Login(LoginRequestDto loginRequestDto)
        {
            var user = _db.ApplicationUsers.FirstOrDefault(u => u.UserName.ToLower() == loginRequestDto.Username.ToLower());
            if (user == null || !await _userManager.CheckPasswordAsync(user, loginRequestDto.Password))
            {
                return new LoginResponeDto() { User = null, Token = "" };
            }

            var role = await _userManager.GetRolesAsync(user);
            var jwtToken = _jwtTokenGenerator.GenerateToken(user, role);

            return new LoginResponeDto()
            {
                User = new UserDto
                {
                    Email = user.Email,
                    Id = user.Id,
                    Name = user.Name,
                    PhoneNumber = user.PhoneNumber,
                },
                Token = jwtToken
            };
        }

        public async Task<LoginResponeDto> LoginByPhoneNumber(LoginRequestDto loginRequestDto)
        {
            var user = _db.ApplicationUsers.FirstOrDefault(u => u.PhoneNumber.ToLower() == loginRequestDto.PhoneNumber.ToLower());
            if (user == null || !await _userManager.CheckPasswordAsync(user, loginRequestDto.Password))
            {
                return new LoginResponeDto() { User = null, Token = "" };
            }

            var role = await _userManager.GetRolesAsync(user);
            var jwtToken = _jwtTokenGenerator.GenerateToken(user, role);

            return new LoginResponeDto()
            {
                User = new UserDto
                {
                    Email = user.Email,
                    Id = user.Id,
                    Name = user.Name,
                    PhoneNumber = user.PhoneNumber,
                },
                Token = jwtToken
            };
        }

        public async Task<ResponeDto> Register(RegistrationRequestDto registrationRequestDto)
        {
            ResponeDto response = new ResponeDto();
            ApplicationUser user = new()
            {
                Name = registrationRequestDto.Name
            };

            // Assign either email or phone number as the UserName and other fields
            if (!string.IsNullOrEmpty(registrationRequestDto.Email))
            {
                user.Email = registrationRequestDto.Email;
                user.UserName = registrationRequestDto.Email;
                user.NormalizedEmail = registrationRequestDto.Email.ToUpper();
            }

            if (!string.IsNullOrEmpty(registrationRequestDto.PhoneNumber))
            {
                user.PhoneNumber = registrationRequestDto.PhoneNumber;

                // If the email is not provided, use the phone number as the username
                if (string.IsNullOrEmpty(user.UserName))
                {
                    user.UserName = registrationRequestDto.PhoneNumber;
                }
            }

            try
            {
                // Try to create the user
                var result = await _userManager.CreateAsync(user, registrationRequestDto.Password);

                if (result.Succeeded)
                {
                    var userToReturn = _db.ApplicationUsers.FirstOrDefault(u =>
                        u.UserName == user.UserName); // Search by UserName, which is now either email or phone number

                    if (userToReturn == null)
                    {
                        response.IsSuccess = false;
                        response.Message = "User creation succeeded, but user could not be retrieved.";
                        return response;
                    }

                    UserDto userDto = new()
                    {
                        Email = userToReturn.Email,
                        Id = userToReturn.Id,
                        Name = userToReturn.Name,
                        PhoneNumber = userToReturn.PhoneNumber
                    };

                    response.IsSuccess = true;
                    response.Result = userDto;
                    response.Message = "User created successfully.";
                    return response;
                }
                else
                {
                    response.IsSuccess = false;
                    response.Message = result.Errors.FirstOrDefault()?.Description ?? "User creation failed for unknown reasons.";
                    return response;
                }
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = $"Error Encountered: {ex.Message}";
                return response;
            }
        }



        public async Task<bool> ActivateDeactivateUser(string identifier, bool isActive)
        {
            var user = await _userManager.FindByIdAsync(identifier);
            if (user == null)
            {
                user = await _userManager.FindByEmailAsync(identifier);
            }

            if (user == null)
            {
                return false;
            }

            try
            {
                user.LockoutEnabled = !isActive;
                user.LockoutEnd = isActive ? null : DateTimeOffset.MaxValue;

                var result = await _userManager.UpdateAsync(user);
                return result.Succeeded;
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.WriteLine($"Error activating/deactivating user: {ex.Message}");
                return false;
            }
        }

    }
}
