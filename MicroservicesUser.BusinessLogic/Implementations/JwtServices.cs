using MicroservicesUser.BusinessLogic.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MicroservicesUser.BusinessLogic.Implementations
{
    public class JwtServices : IJwtServices
    {
        private readonly IConfiguration _configuration;
        public JwtServices(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public string GenerateJwtToken(string Email)
        {
            SymmetricSecurityKey securityKey = new(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"] ?? string.Empty));
            SigningCredentials credentials = new(securityKey, SecurityAlgorithms.HmacSha256);
            List<Claim> claims = new()
            {
                new Claim(ClaimTypes.Email, Email)
            };
            DateTime time = DateTime.UtcNow.AddHours(1);
            JwtSecurityToken token = new(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: time,
                signingCredentials: credentials
            );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }

}
