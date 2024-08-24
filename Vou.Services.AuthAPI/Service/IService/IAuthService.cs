using Vou.Services.AuthAPI.Models.Dto;

namespace Vou.Services.AuthAPI.Service.IService
{
    public interface IAuthService
    {
        Task<string> Register(RegistrationRequestDto registerationRequestDto);
        Task<LoginResponeDto> Login(LoginRequestDto loginRequestDto);
        Task<bool> AssignRole(string email, string roleName);
    }
}
