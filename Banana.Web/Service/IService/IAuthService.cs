using Banana.Web.Models;
using Banana.Web.Models.Dto; 

namespace Banana.Web.Service.IService
{
    public interface IAuthService
    {
        Task<ResponseDto> LoginAsync(LoginRequestDto loginRequest);
        Task<ResponseDto> RegisterAsync(RegisterDto loginRequest);
        Task<ResponseDto> AssignRole(RegisterDto loginRequest);
    }
}
