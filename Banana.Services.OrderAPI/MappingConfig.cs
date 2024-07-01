using AutoMapper;
using Banana.Services.OrderAPI.Models;
using Banana.Services.OrderAPI.Models.Dtos;

namespace Banana.Services.OrderAPI
{
    public class MappingConfig : Profile
    {
        public MappingConfig()
        { 
            CreateMap<CartHeaderDto,OrderHeaderDto>()
                .ForMember(dest => dest.OrderTotal, u => u.MapFrom(src => src.CartTotal)).ReverseMap();
            CreateMap<CartDetailsDto, OrderDetailsDto>()
                .ForMember(dest => dest.ProductName, u => u.MapFrom(src => src.ProductDto.Name));
            CreateMap<CartDetailsDto, OrderDetailsDto>()
                .ForMember(dest => dest.Price, u => u.MapFrom(src => src.ProductDto.Price));
            CreateMap<OrderDetailsDto, CartDetailsDto>();
            CreateMap<OrderHeaderDto, OrderHeader>().ReverseMap();
            CreateMap<OrderDetails, OrderDetailsDto>().ReverseMap();
        }
    }
}
