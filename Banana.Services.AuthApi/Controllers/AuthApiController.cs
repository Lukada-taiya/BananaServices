using Banana.MessageBus;
using Banana.Services.AuthAPI.Models.Dto;
using Banana.Services.AuthAPI.Services.IServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Banana.Services.AuthAPI.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthApiController(IAuthService service, IMessageBus messageBus, IConfiguration configuration) : ControllerBase
    {
        private readonly IAuthService _service = service;
        private readonly IMessageBus _messageBus = messageBus;
        private readonly IConfiguration _configuration = configuration;
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
            await _messageBus.Publish(dto.Email, _configuration.GetValue<string>("TopicsAndQueueNames:UserEmailLogQueue"), _configuration.GetValue<string>("MessageBusConnString"));
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
