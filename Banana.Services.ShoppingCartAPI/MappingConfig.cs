using AutoMapper;
using Banana.Services.ShoppingCartAPI.Models;
using Banana.Services.ShoppingCartAPI.Models.Dto;

namespace Banana.Services.ShoppingCartAPI
{
    public class MappingConfig : Profile
    {
        public MappingConfig()
        {
            CreateMap<CartHeader, CartHeaderDto>().ReverseMap();
            CreateMap<CartDetails, CartDetailsDto>().ReverseMap();
        }
    }
}
