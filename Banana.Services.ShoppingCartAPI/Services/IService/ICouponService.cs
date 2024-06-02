using Banana.Services.ShoppingCartAPI.Models.Dto;

namespace Banana.Services.ShoppingCartAPI.Services.IService
{
    public interface ICouponService
    {
        Task<CouponDto> GetCoupon(string code);
    }
}
