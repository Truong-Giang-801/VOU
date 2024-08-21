using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
        public AuthAPIController(IAuthService? iAuthService, ResponeDto responeDto)
        {
            _iAuthService = iAuthService;
            _responeDto = responeDto;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegistrationRequestDto model)
        {
            var errorMessage = await _iAuthService.Register(model);
            if (!string.IsNullOrEmpty(errorMessage))
            {
                _responeDto.IsSuccess = false;
                _responeDto.Message = errorMessage;
                return BadRequest(_responeDto);
            }

            _responeDto.IsSuccess = true;
            _responeDto.Message = "Registration successful";
            return Ok(_responeDto);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login()
        {
            return Ok();
        }

    }
}
