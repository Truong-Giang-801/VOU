using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
        public AuthAPIController(IAuthService? iAuthService, ResponeDto responeDto,AppDbContext appDbContext, IMapper mapper)
        {
            _iAuthService = iAuthService;
            _responeDto = responeDto;
            _mapper = mapper;
            _db = appDbContext;
        }
        [HttpGet("user")]
        //[Authorize(Roles = "ADMIN")]
        public ResponeDto Get()
        {
            try
            {
                IEnumerable<ApplicationUser> objList = _db.ApplicationUsers.ToList();
                _responeDto.Result = _mapper.Map<IEnumerable<UserDto>>(objList);
            }
            catch (Exception ex)
            {
                _responeDto.IsSuccess = false;
                _responeDto.Message = ex.Message;
            }
            return _responeDto;
        }

		[HttpGet("user/{id}")]
		//		[Authorize(Roles = "ADMIN")]
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
		[HttpPut("update")]
//		[Authorize(Roles = "ADMIN")]
		public ResponeDto Put([FromBody] UserDto userDto)
		{
			try
			{
				ApplicationUser obj = _mapper.Map<ApplicationUser>(userDto);
				_db.ApplicationUsers.Add(obj);
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
        public async Task<IActionResult> Login([FromBody] LoginRequestDto loginRequestDto)
        {
            var loginRespone = await _iAuthService.Login(loginRequestDto);
            if (loginRespone.User == null) { 
                _responeDto.IsSuccess= false;
                _responeDto.Message = "Username or password is incorrect";
                return BadRequest(_responeDto);
            }
            _responeDto.IsSuccess = true;
            _responeDto.Result = loginRespone;
            _responeDto.Message = "Login successful";
            return Ok(_responeDto);
        }
        [HttpPost("assignRole")]
        public async Task<IActionResult> AssignRole([FromBody] RegistrationRequestDto registrationRequestDto)
        {
            var assignRoleSuccessful = await _iAuthService.AssignRole(registrationRequestDto.Email, registrationRequestDto.RoleName.ToUpper());
            if (!assignRoleSuccessful)
            {
                _responeDto.IsSuccess = false;
                _responeDto.Message = "Error encounter at assign role";
                return BadRequest(_responeDto);
            }

            return Ok(_responeDto);
        }

    }
}
