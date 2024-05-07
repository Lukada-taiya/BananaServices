using Banana.Services.CouponAPI.Models.Dto;
using Banana.Web.Models;

namespace Banana.Web.Service.IService
{
    public interface IBaseService
    {
        Task<ResponseDto?> SendAsync(RequestDto requestDto);
    }
}
