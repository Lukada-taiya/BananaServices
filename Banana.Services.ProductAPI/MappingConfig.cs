using AutoMapper;
using Banana.Services.CouponAPI.Models;
using Banana.Services.CouponAPI.Models.Dto;
using Banana.Services.ProductAPI.Models;
using Banana.Services.ProductAPI.Models.Dto;

namespace Banana.Services.ProductAPI
{
    public class MappingConfig : Profile
    {
        public MappingConfig()
        {
            CreateMap<Product, ProductDto>().ReverseMap();
        }
    }
}
