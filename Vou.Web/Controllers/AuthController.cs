using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Vou.Web.Models;
using Vou.Web.Service.IService;
using Vou.Web.Ultility;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
namespace Vou.Web.Controllers
{
    public class AuthController : Controller
    {
        private readonly IAuthService _authService;
        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }
        [HttpGet]
        public IActionResult Login()
        {
            LoginRequestDto loginRequestDto = new ();
            return View(loginRequestDto);   
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginRequestDto obj)
        {
            ResponseDto responseDto = await _authService.LoginAsync(obj);

            if (responseDto != null && responseDto.IsSuccess == true)
            {
                LoginResponeDto loginResponeDto = JsonConvert.DeserializeObject<LoginResponeDto>(Convert.ToString(responseDto.Result));  
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ModelState.AddModelError("CustomError", responseDto.Message);
                return View(obj);
            }


        }
        [HttpGet]
        public IActionResult Register()
        {
            var roleList = new List<SelectListItem>()
            {
                new SelectListItem{Text=SD.RoleAdmin,Value=SD.RoleAdmin},
                new SelectListItem{Text=SD.RoleUser,Value=SD.RoleUser},
                new SelectListItem{Text=SD.RoleBrand,Value=SD.RoleBrand},
            };

            ViewBag.RoleList = roleList;
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegistrationRequestDto obj)
        {
            ResponseDto result = await _authService.RegisterAsync(obj);
            ResponseDto assingRole;

            if (result != null && result.IsSuccess==true)
            {
                if (string.IsNullOrEmpty(obj.RoleName))
                {
                    obj.RoleName = SD.RoleUser;
                }
                assingRole = await _authService.AssignRoleAsync(obj);
                if (assingRole != null && assingRole.IsSuccess==true)
                {
                    TempData["success"] = "Registration Successful";
                    return RedirectToAction(nameof(Login));
                }
            }
            else
            {
                TempData["error"] = result.Message;
            }

            var roleList = new List<SelectListItem>()
            {
                new SelectListItem{Text=SD.RoleAdmin,Value=SD.RoleAdmin},
                new SelectListItem{Text=SD.RoleUser,Value=SD.RoleUser},
                new SelectListItem{Text=SD.RoleBrand,Value=SD.RoleBrand},
            };

            ViewBag.RoleList = roleList;
            return View(obj);
        }
        public IActionResult Logout()
        {
            return View();
        }
    }
}
