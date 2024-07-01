using Banana.Web.Models;
using Banana.Web.Service.IService; 
using static Banana.Web.Utility.StaticData;

namespace Banana.Web.Service
{
    public class OrderService(IBaseService baseService) : IOrderService
    {
        private readonly IBaseService _service = baseService;

        public async Task<ResponseDto> CreateOrderAsync(CartDto cartDto)
        {
            return await _service.SendAsync(new RequestDto()
            {
                ApiType = ApiType.POST,
                Data = cartDto,
                Url = CouponApiBase+"/api/order"
            });
        } 
    
    }
}
