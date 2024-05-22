using Banana.Services.AuthApi.Models;

namespace Banana.Services.AuthApi.Services.IServices
{
    public interface IJwtGenerator
    {
        string GenerateToken(ApplicationUser user, IEnumerable<string> roles);
    }
}
