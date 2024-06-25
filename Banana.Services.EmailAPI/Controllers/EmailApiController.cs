using Microsoft.AspNetCore.Mvc;

namespace Banana.Services.EmailAPI.Controllers
{
    public class EmailApiController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
