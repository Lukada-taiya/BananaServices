using Banana.Web.Models;

namespace Banana.Web.Service.IService
{
    public interface ICouponService
    {
        Task<ResponseDto> GetCouponAsync(int id);
        Task<ResponseDto> GetAllCouponsAsync(); 
        Task<ResponseDto> GetCouponByCodeAsync(string code);
        Task<ResponseDto> CreateCouponAsync(CouponDto couponDto);
        Task<ResponseDto> UpdateCouponAsync(CouponDto couponDto);
        Task<ResponseDto> DeleteCouponAsync(int id);

    }
}
