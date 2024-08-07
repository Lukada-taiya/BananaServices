﻿using AutoMapper;
using Banana.Services.CouponAPI.Data; 
using Banana.Services.CouponAPI.Models.Dto;
using Microsoft.AspNetCore.Authorization; 
using Microsoft.AspNetCore.Mvc; 
using Stripe;
using Coupon = Banana.Services.CouponAPI.Models.Coupon;

namespace Banana.Services.CouponAPI.Controllers
{
    [Route("api/coupon")]
    [ApiController]
    [Authorize]
    public class CouponsApiController(AppDbContext context, IMapper mapper) : ControllerBase
    {
        private readonly AppDbContext _context = context;
        private readonly ResponseDto _response = new ResponseDto();
        private readonly IMapper _mapper = mapper;

        [HttpGet]
        public ResponseDto Get()
        {
            try
            {
                IEnumerable<Coupon> couponlist = _context.Coupons.ToList();
                _response.Result = _mapper.Map<IEnumerable<CouponDto>>(couponlist);
            }catch(Exception ex)
            { 
                _response.Message = ex.Message;
                _response.IsSuccess = false;
            }
            return _response;
        }
        
        [HttpGet]
        [Route("{id:int}")]
        public ResponseDto Get(int id)
        {
            try
            {
                Coupon coupon = _context.Coupons.First(x => x.CouponId == id);
                _response.Result = _mapper.Map<CouponDto>(coupon);
            }catch(Exception ex)
            {
                _response.Message = ex.Message;
                _response.IsSuccess = false;
            }
            return _response;
        }
        [HttpGet]
        [Route("GetByCode/{code}")]
        public ResponseDto GetByCode(string code)
        {
            try
            {
                Coupon coupon = _context.Coupons.First(x => x.CouponCode.ToLower() == code.ToLower());
                _response.Result = _mapper.Map<CouponDto>(coupon);
            }catch(Exception ex)
            {
                _response.Message = ex.Message;
                _response.IsSuccess = false;
            }
            return _response;
        }
        [HttpPost]
        [Authorize(Roles ="ADMIN")]
        public ResponseDto Post(CouponDto couponDto)
        {
            try
            {
                Coupon coupon = _mapper.Map<Coupon>(couponDto);
                _context.Coupons.Add(coupon);
                _context.SaveChanges();

                var options = new CouponCreateOptions
                {
                    AmountOff = (long)(couponDto.DiscountAmount * 100),
                    Name = couponDto.CouponCode,
                    Currency = "usd",
                    Id = couponDto.CouponCode
                };
                var service = new CouponService();
                service.Create(options);
            }catch(Exception ex)
            {
                _response.Message = ex.Message;
                _response.IsSuccess = false;
            }
            return _response;
        }
        [HttpPut] 
        [Authorize(Roles ="ADMIN")]
        public ResponseDto Put(CouponDto couponDto)
        {
            try
            {
                Coupon coupon = _mapper.Map<Coupon>(couponDto);
                _context.Coupons.Update(coupon);
                _context.SaveChanges();
            }catch(Exception ex)
            {
                _response.Message = ex.Message;
                _response.IsSuccess = false;
            }
            return _response;
        }
        [HttpDelete]
        [Route("{id:int}")]
        [Authorize(Roles ="ADMIN")]
        public ResponseDto Delete(int id)
        {
            try
            {
                Coupon coupon = _context.Coupons.First(x => x.CouponId == id);
                _context.Coupons.Remove(coupon);
                _context.SaveChanges();

                var service = new CouponService();
                service.Delete(coupon.CouponCode);
            }
            catch(Exception ex)
            {
                _response.Message = ex.Message;
                _response.IsSuccess = false;
            }
            return _response;
        }

    }
}
