using Microsoft.AspNetCore.Identity;

namespace Banana.Services.AuthApi.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string Name { get; set; }
    }
}
