using AutoMapper;
using Banana.Services.OrderAPI.Models;
using Banana.Services.OrderAPI.Models.Dtos;

namespace Banana.Services.OrderAPI
{
    public class MappingConfig : Profile
    {
        public MappingConfig()
        { 
            CreateMap<OrderHeaderDto,CartHeaderDto>()
                .ForMember(dest => dest.CartTotal, u => u.MapFrom(src => src.OrderTotal)).ReverseMap();
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
