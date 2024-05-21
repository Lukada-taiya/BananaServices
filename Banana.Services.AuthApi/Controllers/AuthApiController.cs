using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Banana.Services.AuthApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthApiController : ControllerBase
    {
        [HttpPost]
        public IActionResult Register()
        {
            return Ok();
        }
        [HttpPost]
        public IActionResult Login()
        {
            return Ok();
        }
    }
}
