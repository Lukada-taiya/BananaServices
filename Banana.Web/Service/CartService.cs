using Banana.Web.Models;
using Banana.Web.Service.IService;
using static Banana.Web.Utility.StaticData;

namespace Banana.Web.Service
{
    public class CartService(IBaseService baseService) : ICartService
    {
        private readonly IBaseService _baseService = baseService;
        public async Task<ResponseDto> AddToCart(CartDto cartDto)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = ApiType.POST,
                Data = cartDto,
                Url = ShoppingCartApiBase + "/api/cart/CartInsert"
            });
        }

        public async Task<ResponseDto> ApplyCoupon(CartDto cartDto)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = ApiType.POST,
                Data = cartDto,
                Url = ShoppingCartApiBase + "/api/cart/ApplyCoupon"
            });
        }
        public async Task<ResponseDto> EmailCart(CartDto cartDto)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = ApiType.POST,
                Data = cartDto,
                Url = ShoppingCartApiBase + "/api/cart/EmailCart"
            });
        }

        public async Task<ResponseDto> GetCartByUserId(string userId)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = ApiType.GET, 
                Url = ShoppingCartApiBase + "/api/cart/GetCart/"+ userId
            });
        }

        public async Task<ResponseDto> RemoveFromCart(int cardDetailsId)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = ApiType.POST,
                Data = cardDetailsId,
                Url = ShoppingCartApiBase + "/api/cart/CartRemove"
            });
        }
    }
}
