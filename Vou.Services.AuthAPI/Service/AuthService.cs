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

        public async Task<LoginResponeDto> Login(LoginRequestDto loginRequestDto)
        {
            var user = _db.ApplicationUsers.FirstOrDefault(u=>u.UserName.ToLower() == loginRequestDto.Username.ToLower());
            bool isValid = await _userManager.CheckPasswordAsync(user, loginRequestDto.Password);
            if (isValid == false || user == null)
            {
                return new LoginResponeDto() { User = null, Token = "" };
            }
            var _jwtToken = _jwtTokenGenerator.GenerateToken(user);
            UserDto userDto = new UserDto()
            {
                Email = user.Email,
                Id = user.Id,
                Name = user.Name,
                PhoneNumber = user.PhoneNumber
            };
            var result = new LoginResponeDto()
            {
                User = userDto,
                Token = _jwtToken,
            };
              return result;
        }

        public async Task<string> Register(RegistrationRequestDto registrationRequestDto)
        {
            ApplicationUser user = new()
            {
                UserName = registrationRequestDto.Email,
                Email = registrationRequestDto.Email,
                NormalizedEmail = registrationRequestDto.Email.ToUpper(),
                Name = registrationRequestDto.Name,
                PhoneNumber = registrationRequestDto.PhoneNumber
            };

            try
            {
                var result = await _userManager.CreateAsync(user, registrationRequestDto.Password);
                if (result.Succeeded)
                {
                    // Fetch the user to return (ensure that no exceptions are thrown here)
                    var userToReturn = _db.ApplicationUsers.FirstOrDefault(u => u.UserName == registrationRequestDto.Email);

                    if (userToReturn == null)
                    {
                        return "User creation succeeded, but user could not be retrieved.";
                    }

                    UserDto userDto = new()
                    {
                        Email = userToReturn.Email,
                        Id = userToReturn.Id, // Make sure the Id is convertible to an int
                        Name = userToReturn.Name,
                        PhoneNumber = userToReturn.PhoneNumber
                    };

                    return ""; // Indicate success
                }
                else
                {
                    // Return the first error encountered during user creation
                    return result.Errors.FirstOrDefault()?.Description ?? "User creation failed for unknown reasons.";
                }
            }
            catch (Exception ex)
            {
                // Log the exception message or return it for debugging
                return $"Error Encountered: {ex.Message}";
            }
        }


    }
}
