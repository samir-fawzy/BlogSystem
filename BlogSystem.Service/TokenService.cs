using BlogSystem.Core.Entities.Identity;
using BlogSystem.Core.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace BlogSystem.Service
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration configuration;

        public TokenService(IConfiguration configuration)
        {
            this.configuration = configuration;
        }
        public async Task<string> CreateTokenAsync(AppUser user,UserManager<AppUser> userManger)
        {
            // handle claims
            var authClaims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier,user.Id),
                new Claim(ClaimTypes.GivenName,user.DisplayName),
                new Claim(ClaimTypes.Email,user.Email)
            };
            // add role to claims
            var roles = await userManger.GetRolesAsync(user);
            foreach (var role in roles)
                authClaims.Add(new Claim(ClaimTypes.Role, role));
            // SecurityKey
            var authKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]));
            //
            var token = new JwtSecurityToken(
                issuer: configuration["Jwt:ValidIssure"],
                audience: configuration["Jwt:ValidAudience"],
                claims: authClaims,
                expires: DateTime.Now.AddDays(double.Parse(configuration["Jwt:DurationDays"])),
                signingCredentials: new SigningCredentials(authKey, SecurityAlgorithms.HmacSha256Signature)
                );
            //return 
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
