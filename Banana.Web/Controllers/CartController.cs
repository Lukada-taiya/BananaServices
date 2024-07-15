using Banana.Web.Models;
using Banana.Web.Service.IService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;

namespace Banana.Web.Controllers
{
    public class CartController(ICartService cartService, IOrderService order) : Controller
    {
        private readonly ICartService _cartService = cartService;
        private readonly IOrderService _orderService = order;

        [Authorize]
        public async Task<IActionResult> CartIndex()
        {
            return View(await GetCartOfLoggedInUser());
        }
        [Authorize]
        public async Task<IActionResult> Checkout()
        {
            return View(await GetCartOfLoggedInUser());
        }
        [Authorize]
        [HttpPost] 
        public async Task<IActionResult> Checkout(CartDto cartDto)
        { 
            CartDto cart = await GetCartOfLoggedInUser();
            cart.CartHeader.Email = cartDto.CartHeader.Email;
            cart.CartHeader.Phone = cartDto.CartHeader.Phone;
            cart.CartHeader.Name = cartDto.CartHeader.Name;

            var res = await _orderService.CreateOrderAsync(cart);

            OrderHeaderDto order = JsonConvert.DeserializeObject<OrderHeaderDto>(Convert.ToString(res.Result)); 
            if(res != null && res.IsSuccess)
            {
                //get stripe session
            }
            return View(cart);
        }
        public async Task<IActionResult> Remove(int CartDetailsId)
        {
            var userId = User.Claims.Where(u => u.Type == JwtRegisteredClaimNames.Sub).FirstOrDefault()?.Value;
            ResponseDto response = await _cartService.RemoveFromCart(CartDetailsId);
           
            if (response != null && response.IsSuccess)
            {
                TempData["success"] = "Item has been removed successfully";
                return RedirectToAction(nameof(CartIndex));
            }
            
            return View();
        }
        public async Task<IActionResult> ApplyCoupon(CartDto cartDto)
        { 
            ResponseDto response = await _cartService.ApplyCoupon(cartDto);
           
            if (response != null && response.IsSuccess)
            {
                TempData["success"] = "Coupon has been applied successfully";
                return RedirectToAction(nameof(CartIndex));
            }
            
            return View();
        }
        public async Task<IActionResult> EmailCart(CartDto cartDto)
        {
            var cart = await GetCartOfLoggedInUser();
            var email = User.Claims.Where(u => u.Type == JwtRegisteredClaimNames.Email).FirstOrDefault()?.Value;
            cart.CartHeader.Email = email;
            ResponseDto response = await _cartService.EmailCart(cart);
           
            if (response != null && response.IsSuccess)
            {
                TempData["success"] = "Message will be processed and sent shortly";
            }else
            TempData["error"] = "An error occured. Try again later";
            return RedirectToAction(nameof(CartIndex));
             
        }
        public async Task<IActionResult> RemoveCoupon(CartDto cartDto)
        {
            cartDto.CartHeader.CouponCode = "";
            ResponseDto response = await _cartService.ApplyCoupon(cartDto);
           
            if (response != null && response.IsSuccess)
            {
                TempData["success"] = "Coupon has been removed successfully";
                return RedirectToAction(nameof(CartIndex));
            }
            
            return View();
        }

        private async Task<CartDto> GetCartOfLoggedInUser()
        {
            var userId = User.Claims.Where(u => u.Type == JwtRegisteredClaimNames.Sub).FirstOrDefault()?.Value;
            if(userId != null)
            {
                ResponseDto response = await _cartService.GetCartByUserId(userId);
                if(response != null && response.IsSuccess)
                {
                    CartDto cartDto = JsonConvert.DeserializeObject<CartDto>(Convert.ToString(response.Result));
                    return cartDto;
                }
            }
            return new CartDto();
        }
    }
}
