using AutoMapper;
using Azure;
using Banana.Services.OrderAPI.Data;
using Banana.Services.OrderAPI.Models;
using Banana.Services.OrderAPI.Models.Dtos;
using Banana.Services.OrderAPI.Services.IService;
using Banana.Services.OrderAPI.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Stripe; 
using Stripe.Checkout;
using Session = Stripe.Checkout.Session;
using SessionCreateOptions = Stripe.Checkout.SessionCreateOptions;
using SessionService = Stripe.Checkout.SessionService;

namespace Banana.Services.OrderAPI.Controllers
{
    [Route("api/order")]
    [ApiController]
    public class OrderApiController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly IProductService _productService;
        protected ResponseDto _responseDto;

        public OrderApiController(AppDbContext context, IMapper mapper, IProductService productService)
        {
            _context = context;
           _mapper = mapper;
            _productService = productService;
            _responseDto = new();
    }

        [Authorize]
        [HttpPost("CreateOrder")]
        public async Task<ResponseDto> CreateOrder([FromBody]CartDto cart)
        {
            try
            {
                OrderHeaderDto orderHeader = _mapper.Map<OrderHeaderDto>(cart.CartHeader);
                orderHeader.OrderTime = DateTime.Now;
                orderHeader.Status = SD.Status_Pending;
                orderHeader.OrderDetails = _mapper.Map<IEnumerable<OrderDetailsDto>>(cart.CartDetailsList);

                OrderHeader orderCreated = (await _context.AddAsync(_mapper.Map<OrderHeader>(orderHeader))).Entity;
                await _context.SaveChangesAsync();
                orderHeader.OrderHeaderId = orderCreated.OrderHeaderId;
                _responseDto.Result = orderHeader;
            }catch(Exception ex)
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = ex.Message;
            }
            return _responseDto;
        }

        [Authorize]
        [HttpPost("CreateStripeSession")]
        public async Task<ResponseDto> CreateStripeSession([FromBody] StripeRequestDto stripeRequestDto)
        {
            try
            {

                var options = new SessionCreateOptions
                {
                    SuccessUrl = stripeRequestDto.ApprovedUrl,
                    CancelUrl = stripeRequestDto.CancelUrl,
                    LineItems = new List<SessionLineItemOptions>(),
                    Mode = "payment"
                };



                var Discounts = new List<SessionDiscountOptions>
            {
                new SessionDiscountOptions
                {
                    Coupon = stripeRequestDto.OrderHeader.CouponCode
                }
            }; 

                foreach(var item in stripeRequestDto.OrderHeader.OrderDetails)
                {
                    var sessionLineItem = new SessionLineItemOptions
                    {
                        PriceData = new SessionLineItemPriceDataOptions
                        {
                            UnitAmount = (long)(item.Price * 100), //20.99 -> 2099
                            Currency = "usd",
                            ProductData = new SessionLineItemPriceDataProductDataOptions
                            {
                                Name = item.ProductDto.Name,

                            }
                        },
                        Quantity = item.Count
                    };
                    options.LineItems.Add(sessionLineItem);
                }
                if (stripeRequestDto.OrderHeader.Discount > 0)
                {
                    options.Discounts = Discounts;
                }
                var service = new SessionService();
                Session session = service.Create(options);
                stripeRequestDto.StripeSessionUrl = session.Url;
                OrderHeader orderHeader = _context.OrderHeaders.First(u => u.OrderHeaderId == stripeRequestDto.OrderHeader.OrderHeaderId);
                orderHeader.StripeSessionId = session.Id;
                _context.SaveChanges();
                _responseDto.Result = stripeRequestDto;
            }
            catch (Exception e)
            {
                _responseDto.Message = e.Message;
                _responseDto.IsSuccess=false;
            }
            return _responseDto;
        }

        [Authorize]
        [HttpPost("ValidateStripeSession")]
        public async Task<ResponseDto> ValidateStripeSession([FromBody] int OrderHeaderId)
        {
            try
            {
                OrderHeader orderHeader = _context.OrderHeaders.First(u => u.OrderHeaderId == OrderHeaderId);
                var service = new SessionService();
                Session session = service.Get(orderHeader.StripeSessionId);
                var paymentIntentService = new PaymentIntentService();
                PaymentIntent paymentIntent = paymentIntentService.Get(session.PaymentIntentId);
                if(paymentIntent.Status == "succeeded")
                {
                    orderHeader.PaymentIntentId = paymentIntent.Id;
                    orderHeader.Status = SD.Status_Approved;
                    _context.SaveChanges();
                    _responseDto.Result = _mapper.Map<OrderHeaderDto>(orderHeader);
                } 
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
