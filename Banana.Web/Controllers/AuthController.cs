using Banana.Web.Models.Dto;
using Banana.Web.Service.IService;
using Banana.Web.Utility;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Banana.Web.Controllers
{
    public class AuthController(IAuthService authService, ITokenProvider tokenProvider) : Controller
    {
        private readonly IAuthService _authService = authService;
        private readonly ITokenProvider _tokenProvider = tokenProvider;

        [HttpGet]
        public IActionResult Login()
        {
            LoginRequestDto loginDto = new();
            return View(loginDto);
        } 
        [HttpPost]
        public async Task<IActionResult> Login(LoginRequestDto loginDto)
        {
            var responseDto = await _authService.LoginAsync(loginDto);

            if(responseDto != null && responseDto.IsSuccess)
            {
                var loginResponseDto = JsonConvert.DeserializeObject<LoginResponseDto>(Convert.ToString(responseDto.Result));
                await SignInUser(loginResponseDto);
                _tokenProvider.SetToken(loginResponseDto.Token);
                return RedirectToAction("Index", "Home");                
            }
            TempData["error"] = responseDto.Message;
            ModelState.AddModelError("CustomError", responseDto.Message); 
            return View(loginDto);
        }
        [HttpGet]
        public IActionResult Register()
        {
            var selectlist = new List<SelectListItem>
            {
                new() {Text = StaticData.RoleCustomer, Value = StaticData.RoleCustomer},
                new() {Text = StaticData.RoleAdmin, Value = StaticData.RoleAdmin}
            };
            ViewBag.RoleList = selectlist;
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterDto registerDto)
        {
            var responseDto = await _authService.RegisterAsync(registerDto);

            if(responseDto != null && responseDto.IsSuccess)
            {
                if (string.IsNullOrEmpty(registerDto.Role)) {
                    registerDto.Role = StaticData.RoleCustomer;
                }  
                var assignRole = await _authService.AssignRole(registerDto);
                if(assignRole != null && assignRole.IsSuccess) {
                    TempData["success"] = "User registered successfully";
                    return RedirectToAction(nameof(Login));
                }
            }
            var selectlist = new List<SelectListItem>
            {
                new(){Text = StaticData.RoleCustomer, Value = StaticData.RoleCustomer},
                new(){Text = StaticData.RoleAdmin, Value = StaticData.RoleAdmin}
            };
            ViewBag.RoleList = selectlist;
            TempData["error"] = responseDto.Message;
            return View(registerDto);
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            _tokenProvider.ClearToken();
            return RedirectToAction("Index", "Home");
        }
         public async Task SignInUser(LoginResponseDto responseDto)
        {
            var handler = new JwtSecurityTokenHandler();
            var jwt = handler.ReadJwtToken(responseDto.Token);

            var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
            identity.AddClaim(new Claim(JwtRegisteredClaimNames.Email, jwt.Claims.FirstOrDefault(u => u.Type == JwtRegisteredClaimNames.Email).Value));
            identity.AddClaim(new Claim(JwtRegisteredClaimNames.Sub, jwt.Claims.FirstOrDefault(u => u.Type == JwtRegisteredClaimNames.Sub).Value));
            identity.AddClaim(new Claim(JwtRegisteredClaimNames.Name, jwt.Claims.FirstOrDefault(u => u.Type == JwtRegisteredClaimNames.Name).Value));
            identity.AddClaim(new Claim(ClaimTypes.Name, jwt.Claims.FirstOrDefault(u => u.Type == JwtRegisteredClaimNames.Name).Value));
            identity.AddClaim(new Claim(ClaimTypes.Role, jwt.Claims.FirstOrDefault(u => u.Type == "role").Value));
            var principal = new ClaimsPrincipal(identity);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
        }

    }
}
