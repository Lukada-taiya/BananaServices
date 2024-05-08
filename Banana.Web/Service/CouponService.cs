using Banana.Web.Models;
using Banana.Web.Service.IService; 
using static Banana.Web.Utility.StaticData;

namespace Banana.Web.Service
{
    public class CouponService(IBaseService baseService) : ICouponService
    {
        private readonly IBaseService _service = baseService;

        public async Task<ResponseDto> CreateCouponAsync(CouponDto couponDto)
        {
            return await _service.SendAsync(new RequestDto()
            {
                ApiType = ApiType.POST,
                Data = couponDto,
                Url = CouponApiBase+"api/coupon"
            });
        }

        public async Task<ResponseDto> DeleteCouponAsync(int id)
        {
            return await _service.SendAsync(new RequestDto()
            {
                ApiType = ApiType.DELETE, 
                Url = CouponApiBase + "/api/coupon/" + id
            });
        }

        public async Task<ResponseDto> GetAllCouponsAsync()
        {
            return await _service.SendAsync(new RequestDto()
            {
                ApiType = ApiType.GET,
                Url = CouponApiBase + "/api/coupon"
            });
        }

        public async Task<ResponseDto> GetCouponAsync(int id)
        {
            return await _service.SendAsync(new RequestDto()
            {
                ApiType = ApiType.GET,
                Url = CouponApiBase + "/api/coupon/"+id
            });
        }

        public async Task<ResponseDto> GetCouponByCodeAsync(string code)
        {
            return await _service.SendAsync(new RequestDto()
            {
                ApiType = ApiType.GET,
                Url = CouponApiBase + "/api/coupon/GetByCode/"+code
            });
        }
         
        public async Task<ResponseDto> UpdateCouponAsync(CouponDto couponDto)
        {
            return await _service.SendAsync(new RequestDto()
            {
                ApiType = ApiType.PUT,
                Url = CouponApiBase + "/api/coupon",
                Data = couponDto
            });
        }
    }
}
