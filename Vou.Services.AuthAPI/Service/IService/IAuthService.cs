using Vou.Services.AuthAPI.Models.Dto;

namespace Vou.Services.AuthAPI.Service.IService
{
    public interface IAuthService
    {
        Task<string> Register(RegistrationRequestDto registerationRequestDto);
        Task<LoginResponeDto> LoginResponeDto(LoginRequestDto loginRequestDto);
    }
}
