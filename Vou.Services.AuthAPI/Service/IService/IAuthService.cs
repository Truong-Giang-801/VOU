using Vou.Services.AuthAPI.Models.Dto;

namespace Vou.Services.AuthAPI.Service.IService
{
    public interface IAuthService
    {
        Task<ResponeDto> Register(RegistrationRequestDto registerationRequestDto);
        Task<LoginResponeDto> Login(LoginRequestDto loginRequestDto);
        Task<LoginResponeDto> LoginByPhoneNumber(LoginRequestDto loginRequestDto);
        Task<bool> ActivateDeactivateUser(string identifier, bool isActive);
        Task<bool> AssignRole(string roleName, string email, string phoneNumber);
    }
}
