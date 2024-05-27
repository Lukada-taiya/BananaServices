using Banana.Services.AuthAPI.Models;

namespace Banana.Services.AuthAPI.Services.IServices
{
    public interface IJwtGenerator
    {
        string GenerateToken(ApplicationUser user, IEnumerable<string> roles);
    }
}
