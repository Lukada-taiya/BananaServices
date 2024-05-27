using Banana.Services.AuthAPI.Models.Dto;
using Banana.Services.AuthAPI.Services.IServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace Banana.Services.AuthAPI.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthApiController(IAuthService service) : ControllerBase
    {
        private readonly IAuthService _service = service;
        private readonly ResponseDto _responseDto = new();
        
        [HttpPost("register")] 
        public async Task<IActionResult> Register(RegisterDto dto)
        {
            var res = await _service.Register(dto);
            if(!res.IsNullOrEmpty())
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = res;
                return BadRequest(_responseDto);
            }
            return Ok(_responseDto);
        }
        [HttpPost("login")] 
        public async Task<IActionResult> Login(LoginRequestDto requestDto)
        {
            var loginRes =await _service.Login(requestDto);
            if(loginRes.User == null)
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = "Username or password is incorrect";
                return BadRequest(_responseDto);
            }
            _responseDto.Result = loginRes;
            return Ok(_responseDto);
        }
        [HttpPost("assignRole")] 
        public async Task<IActionResult> AssignRole(RegisterDto registerDto)
        {
            var assignSuccessful =await _service.AssignRole(registerDto.Email, registerDto.Role.ToUpper());
            if(!assignSuccessful)
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = "Unable to assign role to user";
                return BadRequest(_responseDto);
            } 
            return Ok(_responseDto);
        }
    }
}
