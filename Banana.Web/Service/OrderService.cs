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
                Url = OrderApiBase + "/api/order/CreateOrder"
            });
        }

        public async Task<ResponseDto> CreateStripeSession(StripeRequestDto stripeRequestDto)
        {
            return await _service.SendAsync(new RequestDto()
            {
                ApiType = ApiType.POST,
                Data = stripeRequestDto,
                Url = OrderApiBase + "/api/order/CreateStripeSession"
            });
        }

        public async Task<ResponseDto> ValidateStripeSession(int orderHeaderId)
        {
            return await _service.SendAsync(new RequestDto()
            {
                ApiType = ApiType.POST,
                Data = orderHeaderId,
                Url = OrderApiBase + "/api/order/ValidateStripeSession"
            });
        }
    }
}
