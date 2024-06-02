using Banana.Services.ShoppingCartAPI.Models.Dto;
using Banana.Services.ShoppingCartAPI.Services.IService;
using Newtonsoft.Json;
using System.Net.Http;

namespace Banana.Services.ShoppingCartAPI.Services
{
    public class CouponService(IHttpClientFactory httpClientFactory) : ICouponService
    {
        private readonly IHttpClientFactory _httpClientFactory = httpClientFactory;
        public async Task<CouponDto> GetCoupon(string code)
        {
            var client = _httpClientFactory.CreateClient("Coupons");
            var response = await client.GetAsync("/api/coupon/GetByCode/"+code);
            var apiContent = await response.Content.ReadAsStringAsync();
            var responseDto = JsonConvert.DeserializeObject<ResponseDto>(apiContent);
            if (responseDto.IsSuccess)
            {
                return JsonConvert.DeserializeObject<CouponDto>(Convert.ToString(responseDto.Result));
            }
            return new CouponDto();
        }
    }
}
