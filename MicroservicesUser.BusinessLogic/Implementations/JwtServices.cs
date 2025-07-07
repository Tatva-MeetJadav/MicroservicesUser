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
        public string GenerateJwtToken(int id, string role)
        {
            SymmetricSecurityKey securityKey = new(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"] ?? string.Empty));
            SigningCredentials credentials = new(securityKey, SecurityAlgorithms.HmacSha256);
            List<Claim> claims = new()
            {
                new Claim(ClaimTypes.NameIdentifier, id.ToString()),
                new Claim(ClaimTypes.Role,role)
            };
            double hours = Convert.ToDouble(_configuration["AuthTokenExpiryTime:Hours"]);
            DateTime time = DateTime.UtcNow.AddHours(hours);
            JwtSecurityToken token = new(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: time,
                signingCredentials: credentials
            );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public int GetUserId(string token)
        {
            int id = Convert.ToInt32(new JwtSecurityTokenHandler().ReadJwtToken(token).Claims.First(claim => claim.Type == ClaimTypes.NameIdentifier).Value);
            return id;
        }

        public string GetRole(string token)
        {
            string role = new JwtSecurityTokenHandler().ReadJwtToken(token).Claims.First(claim => claim.Type == ClaimTypes.Role).Value;
            return role;
        }
    }

}
