using System.Text;
using Application.Interface;
using Application.Model;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Service
{
    public class JWTService : IJWTService
    {
        private readonly IConfiguration configuration;

        public JWTService(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public string GenerateToken(UserDTO user)
        {
            string secretKey = this.configuration["JWT:SecretKey"];

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));

            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new System.Security.Claims.ClaimsIdentity(new[]
                {
                    new System.Security.Claims.Claim("username", user.Username),
                    new System.Security.Claims.Claim("role", user.Role.ToString())
                }),
                Expires = DateTime.UtcNow.AddMinutes(Convert.ToDouble(this.configuration["JWT:ExpiryMinutes"])),
                SigningCredentials = credentials,
                Issuer = this.configuration["JWT:Issuer"],
                Audience = this.configuration["JWT:Audience"]
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            
            return tokenHandler.WriteToken(token);
        }
    }
}