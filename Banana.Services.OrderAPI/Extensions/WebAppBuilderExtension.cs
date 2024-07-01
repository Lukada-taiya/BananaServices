using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Banana.Services.OrderAPI.Extensions
{
    public static class WebAppBuilderExtension
    {
        public static WebApplicationBuilder AddAppAuth(this WebApplicationBuilder builder) 
        {
            var issuer = builder.Configuration.GetValue<string>("ApiSettings:Issuer");
            var secret = builder.Configuration.GetValue<string>("ApiSettings:Secret");
            var audience = builder.Configuration.GetValue<string>("ApiSettings:Audience");
            var key = Encoding.ASCII.GetBytes(secret);
            builder.Services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(o =>
            {
                o.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = true,
                    ValidIssuer = issuer,
                    ValidateAudience = true,
                    ValidAudience = audience
                };
            });
            return builder;
        }
    }
}
