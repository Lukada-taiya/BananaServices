using Banana.Web.Models;

namespace Banana.Web.Service.IService
{
    public interface IOrderService
    { 
        Task<ResponseDto> CreateOrderAsync(CartDto cartDto); 

    }
}
