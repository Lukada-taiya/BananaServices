using Banana.Web.Models;
using Banana.Web.Service.IService;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Banana.Web.Controllers
{
    public class CouponController(ICouponService couponService) : Controller
    {
        private readonly ICouponService _couponService = couponService;
        public async Task<IActionResult> CouponIndex()
        {
            List<CouponDto>? list = new();
            ResponseDto? response = await _couponService.GetAllCouponsAsync();
            if(response != null && response.IsSuccess)
            {
                list = JsonConvert.DeserializeObject<List<CouponDto>>(Convert.ToString(response.Result));
            }
            return View(list);
        }
    }
}
