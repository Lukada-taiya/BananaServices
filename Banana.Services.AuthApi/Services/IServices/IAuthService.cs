using Banana.Services.AuthAPI.Models.Dto;

namespace Banana.Services.AuthAPI.Services.IServices
{
    public interface IAuthService
    {
        Task<string> Register(RegisterDto registerDto);
        Task<LoginResponseDto> Login(LoginRequestDto loginRequest);
        Task<bool> AssignRole(string email, string roleName);
    }
}
