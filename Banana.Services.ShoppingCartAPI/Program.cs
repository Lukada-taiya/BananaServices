using AutoMapper;
using Banana.Services.ShoppingCartAPI;
using Banana.Services.ShoppingCartAPI.Data;
using Banana.Services.ShoppingCartAPI.Extensions;
using Banana.Services.ShoppingCartAPI.Services;
using Banana.Services.ShoppingCartAPI.Services.IService;
using Banana.Services.ShoppingCartAPI.Utility;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});
var mapperConfig = new MapperConfiguration(mc =>
{
    mc.AddProfile(new MappingConfig());
});
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<BackendApiAuthTokenHandler>();
builder.Services.AddHttpClient("Products", u => u.BaseAddress = new Uri(builder.Configuration["ServiceUrls:Product"])).AddHttpMessageHandler<BackendApiAuthTokenHandler>();
builder.Services.AddHttpClient("Coupons", u => u.BaseAddress = new Uri(builder.Configuration["ServiceUrls:Coupon"])).AddHttpMessageHandler<BackendApiAuthTokenHandler>();

IMapper mapper = mapperConfig.CreateMapper();
builder.Services.AddSingleton(mapper);
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<ICouponService, CouponService>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(opt => {
    opt.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Name = "Authorisation",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });
    opt.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference =  new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id =  JwtBearerDefaults.AuthenticationScheme
                }
            }, Array.Empty<string>()
        }
    });
});

builder.AddAppAuth();
builder.Services.AddAuthorization();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
ApplyMigration();
app.Run();

void ApplyMigration()
{
    using (var scope = app.Services.CreateScope())
    {
        var _db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        if (_db.Database.GetPendingMigrations().Count() > 0)
        {
            _db.Database.Migrate();
        }
    }
}
