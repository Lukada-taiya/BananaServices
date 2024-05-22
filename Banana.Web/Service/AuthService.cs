using Banana.Web.Models;
using Banana.Web.Models.Dto;
using Banana.Web.Service.IService; 
using static Banana.Web.Utility.StaticData;

namespace Banana.Web.Service
{
    public class AuthService(IBaseService baseService) : IAuthService
    { 
        private readonly IBaseService _service = baseService;

        public async Task<ResponseDto> AssignRole(RegisterDto registerRequest)
        {
            return await _service.SendAsync(new RequestDto()
            {
                ApiType = ApiType.POST,
                Data = registerRequest,
                Url = AuthApiBase + "/api/auth/assignRole"
            });
        }

        public async Task<ResponseDto> LoginAsync(LoginRequestDto loginRequest)
        {
            return await _service.SendAsync(new RequestDto()
            {
                ApiType = ApiType.POST,
                Data = loginRequest,
                Url = AuthApiBase + "/api/auth/login"
            },false);
        }

        public async Task<ResponseDto> RegisterAsync(RegisterDto registerRequest)
        {
            return await _service.SendAsync(new RequestDto()
            {
                ApiType = ApiType.POST,
                Data = registerRequest,
                Url = AuthApiBase + "/api/auth/register"
            },false);
        } 
    }
}
