using AutoMapper;
using Banana.MessageBus;
using Banana.Services.ShoppingCartAPI.Data;
using Banana.Services.ShoppingCartAPI.Models;
using Banana.Services.ShoppingCartAPI.Models.Dto;
using Banana.Services.ShoppingCartAPI.Services.IService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Banana.Services.ShoppingCartAPI.Controllers
{
    [Route("api/cart")]
    [ApiController]
    public class CartsController(IMapper mapper, AppDbContext context, IProductService productService, ICouponService couponService, IMessageBus messageBus, IConfiguration configuration) : ControllerBase
    {
        private readonly IMapper _mapper = mapper;
        private readonly AppDbContext _appDbContext = context;
        private readonly IProductService _productService = productService;
        private readonly ICouponService _couponService = couponService;
        private readonly IConfiguration _configuration = configuration;
        private readonly IMessageBus _messageBus = messageBus;
        private ResponseDto _responseDto = new();

        [HttpGet("GetCart/{userId}")]
        public async Task<ResponseDto> Get(string userId)
        {
            try
            {
                var cartHeader = _mapper.Map<CartHeaderDto>(_appDbContext.CartHeaders.First(u => u.UserId == userId));
                CartDto cartDto = new()
                {
                    CartHeader = cartHeader
                };
                var products = await _productService.GetAllProducts();
                cartDto.CartDetailsList = _mapper.Map<IEnumerable<CartDetailsDto>>(_appDbContext.CartDetails.Where(u => u.CartHeaderId == cartHeader.CartHeaderId));
                foreach (var item in cartDto.CartDetailsList)
                {
                    item.ProductDto = products.First(u => u.ProductId == item.ProductId);
                    cartDto.CartHeader.CartTotal += item.ProductDto.Price * item.Count;
                }
                if(!string.IsNullOrEmpty(cartDto.CartHeader.CouponCode))
                {
                    CouponDto coupon = await _couponService.GetCoupon(cartDto.CartHeader.CouponCode);
                    if(coupon != null && cartDto.CartHeader.CartTotal > coupon.MinAmount)
                    {
                        cartDto.CartHeader.CartTotal -= coupon.DiscountAmount;
                        cartDto.CartHeader.Discount = coupon.DiscountAmount;
                    }
                }
                _responseDto.Result = cartDto;
            }catch(Exception e)
            {
                _responseDto.Message = e.Message;
                _responseDto.IsSuccess = false;
            }
            return _responseDto;
        }

        [HttpPost("ApplyCoupon")]
        public async Task<ResponseDto> ApplyCoupon(CartDto cart)
        {
            try
            {
                var cartHeaderFromDb = await _appDbContext.CartHeaders.AsNoTracking().FirstAsync(u => u.UserId == cart.CartHeader.UserId);
                cartHeaderFromDb.CouponCode = cart.CartHeader.CouponCode;
                _appDbContext.CartHeaders.Update(cartHeaderFromDb);
                await _appDbContext.SaveChangesAsync();                
                _responseDto.Result = true;
            }catch(Exception e)
            {
                _responseDto.Message = e.Message;
                _responseDto.IsSuccess = false;
            }
            return _responseDto;
        }

        [HttpPost("EmailCart")]
        public async Task<ResponseDto> EmailCart(CartDto cart)
        {
            try
            {
                await _messageBus.Publish(cart,_configuration.GetValue<string>("TopicsAndQueueNames:EmailShoppingCartQueue"));                
                _responseDto.Result = true;
            }
            catch (Exception e)
            {
                _responseDto.Message = e.Message;
                _responseDto.IsSuccess = false;
            }
            return _responseDto;
        }
        //Combining removecoupon to apply coupon
        //[HttpPost("RemoveCoupon")]
        //public async Task<ResponseDto> RemoveCoupon(CartDto cart)
        //{
        //    try
        //    {
        //        var cartHeaderFromDb = await _appDbContext.CartHeaders.AsNoTracking().FirstAsync(u => u.UserId == cart.CartHeader.UserId);
        //        cartHeaderFromDb.CouponCode = "";
        //        _appDbContext.CartHeaders.Update(cartHeaderFromDb);
        //        await _appDbContext.SaveChangesAsync();                
        //        _responseDto.Result = true;
        //    }catch(Exception e)
        //    {
        //        _responseDto.Message = e.Message;
        //        _responseDto.IsSuccess = false;
        //    }
        //    return _responseDto;
        //}

        [HttpPost("CartInsert")]
        public async Task<ResponseDto> Insert(CartDto cart)
        {
            try
            {
                var cartHeaderFromDb = _appDbContext.CartHeaders.AsNoTracking().FirstOrDefault(u => u.UserId == cart.CartHeader.UserId);
                if (cartHeaderFromDb == null)
                {
                    //create Cart Header
                    var newCartHeader = _mapper.Map<CartHeader>(cart.CartHeader);
                    _appDbContext.CartHeaders.Add(newCartHeader);
                    await _appDbContext.SaveChangesAsync();
                    //create Cart Details
                    cart.CartDetailsList.First().CartHeaderId = newCartHeader.CartHeaderId;
                    var newCartDetails = _mapper.Map<CartDetails>(cart.CartDetailsList.First());
                    _appDbContext.CartDetails.Add(newCartDetails);
                    await _appDbContext.SaveChangesAsync();
                }else
                {
                    //if header !== null
                    var cartDetailfromdb = _appDbContext.CartDetails.AsNoTracking() .FirstOrDefault(u => u.ProductId == cart.CartDetailsList.First().ProductId &&
                    u.CartHeaderId == cartHeaderFromDb.CartHeaderId);
                    if (cartDetailfromdb == null)
                    {
                        cart.CartDetailsList.First().CartHeaderId = cartHeaderFromDb.CartHeaderId;
                        var newCartDetails = _mapper.Map<CartDetails>(cart.CartDetailsList.First());
                        _appDbContext.CartDetails.Add(newCartDetails);
                        await _appDbContext.SaveChangesAsync();
                    }else
                    {
                        cart.CartDetailsList.First().Count += cartDetailfromdb.Count;
                        cart.CartDetailsList.First().CartHeaderId = cartDetailfromdb.CartHeaderId;
                        cart.CartDetailsList.First().CartDetailsId = cartDetailfromdb.CartDetailsId;
                        var newCartDetails = _mapper.Map<CartDetails>(cart.CartDetailsList.First());
                        _appDbContext.CartDetails.Update(newCartDetails);
                        await _appDbContext.SaveChangesAsync();
                    }
                }
                _responseDto.Result = cart;
            }catch(Exception e)
            {
                _responseDto.Message = e.Message;
                _responseDto.IsSuccess = false;
            }
            return _responseDto;
        }

        [HttpPost("CartRemove")]
        public async Task<ResponseDto> Remove([FromBody]int cartDetailsId)
        {
            try
            {
                var cartDetails = _appDbContext.CartDetails.AsNoTracking().FirstOrDefault(u => u.CartDetailsId == cartDetailsId);
                if (cartDetails == null) throw new Exception("Invalid Card Details Id");
                _appDbContext.CartDetails.Remove(cartDetails); 
                var cardDetailsCount = _appDbContext.CartDetails.Where(u => u.CartHeaderId == cartDetails.CartHeaderId).Count();
                if (cardDetailsCount == 1)
                {
                    var cartHeaderToRemove = _appDbContext.CartHeaders.FirstOrDefault(u => u.CartHeaderId == cartDetails.CartHeaderId);
                    _appDbContext.CartHeaders.Remove(cartHeaderToRemove);   
                }
                 await _appDbContext.SaveChangesAsync();               
                _responseDto.Result = true;
            }
            catch (Exception e)
            {
                _responseDto.Message = e.Message;
                _responseDto.IsSuccess = false;
            }
            return _responseDto;
        }
    
}

}
