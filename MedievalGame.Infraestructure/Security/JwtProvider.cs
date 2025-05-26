using MedievalGame.Application.Interfaces;
using MedievalGame.Domain.Entities;
using MedievalGame.Domain.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MedievalGame.Infraestructure.Security
{
    public class JwtProvider(IConfiguration configuration, IUserRepository userRepository) : IJwtProvider
    {
        public string GenerateToken(User user)
        {
            var key = configuration["Jwt:Key"];
            var issuer = configuration["Jwt:Issuer"];
            var audiencie = configuration["Jwt:Audience"];
            var expires = DateTime.UtcNow.AddMinutes(int.Parse(configuration["Jwt:Expires"] ?? "60"));

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var roles = userRepository.GetRolesAsync(user.Id);
            var rolesClaims = roles.Select(r => new Claim(ClaimTypes.Role, r));

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Name, user.Username),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            }.Concat(rolesClaims);

            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audiencie,
                claims: claims,
                expires: expires,
                signingCredentials: credentials
                );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
