using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using ProductionHouse.Core.Entities;
using ProductionHouse.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ProductionHouse.Infrastructure.Service
{
    public class JwtService : IJwtService
    {
        private readonly IConfiguration _configuration;

        public JwtService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GenerateToken(Admin admin)
        {
            var key = Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]);
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, admin.Id.ToString()),

                new Claim(ClaimTypes.Name, admin.Name),

                new Claim(ClaimTypes.Email, admin.Email),

                new Claim(ClaimTypes.Role, admin.Role)
            };

            var signingKey= new SymmetricSecurityKey(key);


            var credentails= new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken( 
                
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                 expires:
                    DateTime.UtcNow.AddMinutes(
                        Convert.ToDouble(
                        _configuration["Jwt:DurationInMinutes"])),
                signingCredentials: credentails
            );


            return new JwtSecurityTokenHandler()
                .WriteToken(token);
        }
    }
}
