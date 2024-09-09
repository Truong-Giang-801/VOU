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
        private async Task<string> GetUserRoleAsync(ApplicationUser user)
        {
            var roles = await _userManager.GetRolesAsync(user);
            return roles.FirstOrDefault() ?? string.Empty;
        }
        public async Task<LoginResponeDto> Login(LoginRequestDto loginRequestDto)
        {
            var user = _db.ApplicationUsers.FirstOrDefault(u => u.UserName.ToLower() == loginRequestDto.Username.ToLower());

            // Check if the user exists and if their lockout status is "Activated"
            if (user == null || user.LockoutEnabled != false )
            {
                return new LoginResponeDto() { User = null, Token = "User is locked" };
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
                    Lockout = user.LockoutEnabled ? "Locked" : "Activated",
                    Role = role.FirstOrDefault() // Assuming the user has one role
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
            if (user == null || user.LockoutEnabled != false)
            {
                return new LoginResponeDto() { User = null, Token = "User is locked" };
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
                    Lockout = user.LockoutEnabled ? "Locked" : "Activated",
                    Role = role.FirstOrDefault() // Assuming the user has one role
                },
                Token = jwtToken
            };
        }

        public async Task<ResponeDto> Register(RegistrationRequestDto registrationRequestDto)
        {
            ResponeDto response = new ResponeDto();

            // Check if either email or phone number is provided
            if (string.IsNullOrEmpty(registrationRequestDto.Email) && string.IsNullOrEmpty(registrationRequestDto.PhoneNumber))
            {
                response.IsSuccess = false;
                response.Message = "Either Email or PhoneNumber must be provided.";
                return response;
            }

            // Check if user with the same email or phone number already exists
            ApplicationUser existingUserByEmail = null;
            if (!string.IsNullOrEmpty(registrationRequestDto.Email))
            {
                existingUserByEmail = await _userManager.FindByEmailAsync(registrationRequestDto.Email);
                if (existingUserByEmail != null)
                {
                    response.IsSuccess = false;
                    response.Message = "User with this email already exists.";
                    return response;
                }
            }

            var existingUserByPhoneNumber = _db.ApplicationUsers.FirstOrDefault(u => u.PhoneNumber == registrationRequestDto.PhoneNumber);
            if (existingUserByPhoneNumber != null)
            {
                response.IsSuccess = false;
                response.Message = "User with this phone number already exists.";
                return response;
            }

            // Create the user object
            ApplicationUser user = new()
            {
                Name = registrationRequestDto.Name,
                PhoneNumber = registrationRequestDto.PhoneNumber,
                UserName = string.IsNullOrEmpty(registrationRequestDto.Email) ? registrationRequestDto.PhoneNumber : registrationRequestDto.Email,
                Email = registrationRequestDto.Email,
                NormalizedEmail = registrationRequestDto.Email?.ToUpper()
            };

            try
            {
                // Try to create the user
                var result = await _userManager.CreateAsync(user, registrationRequestDto.Password);

                if (result.Succeeded)
                {
                    var userToReturn = _db.ApplicationUsers.FirstOrDefault(u => u.UserName == user.UserName);

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
                    // Return specific error messages
                    var errorMessage = result.Errors.FirstOrDefault()?.Description ?? "User creation failed for unknown reasons.";
                    response.IsSuccess = false;
                    response.Message = errorMessage;
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
            var user = await _userManager.FindByIdAsync(identifier) ?? await _userManager.FindByEmailAsync(identifier);

            if (user == null)
            {
                Console.WriteLine($"User not found with identifier: {identifier}");
                return false; // User not found
            }

            try
            {
                user.LockoutEnabled = !isActive;
                user.LockoutEnd = isActive ? null : DateTimeOffset.MaxValue;

                var result = await _userManager.UpdateAsync(user);
                if (!result.Succeeded)
                {
                    Console.WriteLine($"Failed to update user lockout status: {string.Join(", ", result.Errors)}");
                    return false; // Update failed
                }

                await _userManager.UpdateSecurityStampAsync(user); // Ensure security stamp is updated

                Console.WriteLine($"User activation status updated successfully. Current status: {(isActive ? "Active" : "Inactive")}");

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error activating/deactivating user: {ex.Message}");
                return false;
            }
        }




    }
}
