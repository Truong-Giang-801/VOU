using Vou.Web.Models;

namespace Vou.Web.Service.IService
{
    public interface IAuthService
    {
        Task<ResponseDto?> LoginAsync(LoginRequestDto loginRequestDto);
        Task<ResponseDto?> RegisterAsync(RegistrationRequestDto registrationDto);
        Task<ResponseDto?> AssignRoleAsync(RegistrationRequestDto registrationDto);

        Task<ResponseDto?> GetAllUserAsync();

    }
}
