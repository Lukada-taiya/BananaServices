using AutoMapper;
using Banana.Services.CouponAPI.Models;
using Banana.Services.CouponAPI.Models.Dto;

namespace Banana.Services.CouponAPI
{
    public class MappingConfig : Profile
    {
        public MappingConfig()
        {
            CreateMap<Coupon, CouponDto>().ReverseMap();
        }
    }
}
