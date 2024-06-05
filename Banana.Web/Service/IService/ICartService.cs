using Banana.Web.Models;

namespace Banana.Web.Service.IService
{
    public interface ICartService
    {
        Task<ResponseDto> GetCartByUserId(string userId);
        Task<ResponseDto> AddToCart(CartDto cartDto);
        Task<ResponseDto> RemoveFromCart(int cardDetailsId);
        Task<ResponseDto> ApplyCoupon(CartDto cartDto);
        Task<ResponseDto> EmailCart(CartDto cartDto);
    }
}
