using AutoMapper;
using Banana.Services.OrderAPI.Data;
using Banana.Services.OrderAPI.Models;
using Banana.Services.OrderAPI.Models.Dtos;
using Banana.Services.OrderAPI.Services.IService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Banana.Services.OrderAPI.Controllers
{
    [Route("api/order")]
    [ApiController]
    public class OrderApiController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly AppDbContext _appDbContext;
        private readonly IProductService _productService;
        private ResponseDto _responseDto;

        public OrderApiController(IMapper mapper, AppDbContext context, IProductService productService)
        {
            _mapper = mapper;
            _appDbContext = context;
            _productService = productService;
            _responseDto = new();
        }

        [Authorize]
        [HttpPost]
        public async Task<ResponseDto> CreateOrder(CartDto cart)
        {
            try
            {
                OrderHeaderDto orderHeader = _mapper.Map<OrderHeaderDto>(cart.CartHeader);
                orderHeader.OrderTime = DateTime.Now;
                orderHeader.OrderDetails = _mapper.Map<IEnumerable<OrderDetailsDto>>(cart.CartDetailsList);
                OrderHeader orderCreated = _appDbContext.OrderHeaders.Add(_mapper.Map<OrderHeader>(orderHeader)).Entity;
                await _appDbContext.SaveChangesAsync();

                orderHeader.OrderHeaderId = orderCreated.OrderHeaderId;
                _responseDto.Result = orderHeader;
            }catch(Exception e)
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = e.Message;
            }
            return _responseDto;
        }
    }
}
