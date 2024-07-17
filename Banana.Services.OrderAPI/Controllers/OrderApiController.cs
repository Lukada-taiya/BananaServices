using AutoMapper;
using Banana.Services.OrderAPI.Data;
using Banana.Services.OrderAPI.Models;
using Banana.Services.OrderAPI.Models.Dtos;
using Banana.Services.OrderAPI.Services.IService;
using Banana.Services.OrderAPI.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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
    }
}
