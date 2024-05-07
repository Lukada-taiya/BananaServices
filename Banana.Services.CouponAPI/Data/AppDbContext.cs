using Banana.Services.CouponAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace Banana.Services.CouponAPI.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
            
        }

        public DbSet<Coupon> Coupons { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Coupon>().HasData(new Coupon
            {
                CouponId = 1,
                CouponCode = "A233D",
                DiscountAmount = 15,
                MinAmount = 30
            });
            modelBuilder.Entity<Coupon>().HasData(new Coupon
            {
                CouponId = 2,
                CouponCode = "F33E2",
                DiscountAmount = 35,
                MinAmount = 60
            });
            modelBuilder.Entity<Coupon>().HasData(new Coupon
            {
                CouponId = 3,
                CouponCode = "S345G",
                DiscountAmount = 15,
                MinAmount = 45
            });
        }
    }
}
