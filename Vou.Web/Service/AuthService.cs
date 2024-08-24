using Vou.Web.Models;
using Vou.Web.Service.IService;
using Vou.Web.Ultility;

namespace Vou.Web.Service
{
    public class AuthService : IAuthService
    {
        private readonly IBaseService _baseService;
        public AuthService(IBaseService baseService)
        {   
            _baseService = baseService;
        }
        public async Task<ResponseDto?> AssignRoleAsync(RegistrationRequestDto registrationDto)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = Ultility.SD.ApiType.POST,
                Data = registrationDto,
                Url = SD.AuthAPIBase + "/api/auth/assignRole"
            });
        }

        public async Task<ResponseDto?> LoginAsync(LoginRequestDto loginRequestDto)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = Ultility.SD.ApiType.POST,
                Data = loginRequestDto,
                Url = SD.AuthAPIBase + "/api/auth/login"
            });
        }

        public async Task<ResponseDto?> RegisterAsync(RegistrationRequestDto registrationDto)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = Ultility.SD.ApiType.POST,
                Data = registrationDto,
                Url = SD.AuthAPIBase + "/api/auth/register"
            });
        }
    }
}
