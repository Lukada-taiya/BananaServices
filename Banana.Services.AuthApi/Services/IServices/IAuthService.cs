using Banana.Services.AuthApi.Models.Dto;

namespace Banana.Services.AuthApi.Services.IServices
{
    public interface IAuthService
    {
        Task<string> Register(RegisterDto registerDto);
        Task<LoginResponseDto> Login(LoginRequestDto loginRequest);
        Task<bool> AssignRole(string email, string roleName);
    }
}
