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
using Vou.Web.Service;
namespace Vou.Web.Controllers
{
    public class AuthController : Controller
    {
        private readonly IAuthService _authService;
        private readonly ITokenProvider _tokenProvider;
        public AuthController(IAuthService authService, ITokenProvider tokenProvider)
        {
            _authService = authService;
            _tokenProvider = tokenProvider;
        }
        [HttpGet]
        public IActionResult Login()
        {
            LoginRequestDto loginRequestDto = new ();
            return View(loginRequestDto);   
        }
        [HttpGet]
        public async Task<IActionResult> UserIndex()
        {
            List<UserDto>? list = new();

            ResponseDto? response = await _authService.GetAllUserAsync();

            if (response != null && response.IsSuccess == true)
            {
                list = JsonConvert.DeserializeObject<List<UserDto>>(Convert.ToString(response.Result));
            }
            else
            {
                TempData["error"] = response?.Message;
            }

            return View(list);
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginRequestDto obj)
        {
            var responseDto = await _authService.LoginAsync(obj);

            if (responseDto != null && responseDto.IsSuccess == true)
            {
                var resultJson = Convert.ToString(responseDto.Result);
                Console.WriteLine("API Response: " + resultJson);

                if (!string.IsNullOrEmpty(resultJson))
                {
                    var apiResponse = JsonConvert.DeserializeObject<ResponseDto>(resultJson);
                    if (apiResponse != null && apiResponse.Result != null)
                    {
                        var loginResponeDto = JsonConvert.DeserializeObject<LoginResponeDto>(Convert.ToString(apiResponse.Result));
                        if (loginResponeDto != null)
                        {
                            await SigninUser(loginResponeDto);
                            _tokenProvider.SetToken(loginResponeDto.Token);
                            return RedirectToAction("Index", "Home");
                        }
                        else
                        {
                            ModelState.AddModelError("CustomError", "Invalid login response data");
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("CustomError", "Empty login response");
                    }
                }
                else
                {
                    ModelState.AddModelError("CustomError", "No result returned from API");
                }
            }
            else
            {
                ModelState.AddModelError("CustomError", responseDto?.Message ?? "An error occurred during login");
            }

            return View(obj);
        }






        [HttpGet]
        public IActionResult Register()
        {
            var roleList = new List<SelectListItem>()
            {
                new SelectListItem{Text=SD.RoleAdmin,Value=SD.RoleAdmin},
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
                new SelectListItem{Text=SD.RoleBrand,Value=SD.RoleBrand},
            };

            ViewBag.RoleList = roleList;
            return View(obj);
        }
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            _tokenProvider.ClearToken();
            return RedirectToAction("Index", "Home");
        }
        private async Task SigninUser(LoginResponeDto model)
        {
            var handler = new JwtSecurityTokenHandler();
            var jwt = handler.ReadJwtToken(model.Token);
            var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
            identity.AddClaim(new Claim(JwtRegisteredClaimNames.Name,
                jwt.Claims.FirstOrDefault(u => u.Type == JwtRegisteredClaimNames.Name).Value));
            identity.AddClaim(new Claim(JwtRegisteredClaimNames.Email,
                jwt.Claims.FirstOrDefault(u => u.Type == JwtRegisteredClaimNames.Email).Value));
            identity.AddClaim(new Claim(JwtRegisteredClaimNames.NameId,
                jwt.Claims.FirstOrDefault(u => u.Type == JwtRegisteredClaimNames.NameId).Value));


            identity.AddClaim(new Claim(ClaimTypes.Role ,
                jwt.Claims.FirstOrDefault(u => u.Type == "role").Value));

            identity.AddClaim(new Claim(ClaimTypes.Name,
                jwt.Claims.FirstOrDefault(u => u.Type == JwtRegisteredClaimNames.Email).Value));

            var principle = new ClaimsPrincipal(identity);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principle);
        }
    }
}
