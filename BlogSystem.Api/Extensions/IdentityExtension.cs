using BlogSystem.Core.Entities.Identity;
using BlogSystem.Core.Services;
using BlogSystem.Repository.Identity;
using BlogSystem.Service;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
namespace BlogSystem.Api.Extensions
{
    public static class IdentityExtension
    {
        public static IServiceCollection AddIdentityServices(this IServiceCollection Services,IConfiguration configuration)
        {
            Services.AddScoped<ITokenService, TokenService>();

            Services.AddIdentity<AppUser, IdentityRole>(options =>
            {
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireUppercase = true;
                options.Password.RequireDigit = true;
            }).AddEntityFrameworkStores<AppIdentityDbContext>();

            Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                    .AddJwtBearer(options =>
                    {
                        options.TokenValidationParameters = new TokenValidationParameters()
                        {
                            ValidateIssuer = true,
                            ValidIssuer = configuration["Jwt:ValidIssure"],
                            ValidateAudience = true,
                            ValidAudience = configuration["Jwt:ValidAudience"],
                            ValidateLifetime = true,
                            ValidateIssuerSigningKey = true,
                            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"])),
                            
                        };
                    });

            return Services;
        }
    }
}
