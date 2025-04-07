using Gateway.Domain.models;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Gateway.Application.TokenManager.Jwt
{
    public class SelfJwtTokenManager(string issuer, string audience, string secretKey) : IJwtTokenManager
    {
        private readonly string _issuer = issuer;
        private readonly string _audience = audience;
        private readonly string _secretKey = secretKey;

        public string GenerateToken(UserModel user)
        {
            ArgumentNullException.ThrowIfNull(user);
            if (user.Id == null)
            {
                throw new InvalidOperationException("User ID cannot be null when creating a JWT claim.");
            }
            var tokenHandler = new JwtSecurityTokenHandler();
            List<Claim> claims =
            [
                new(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()),
                new(JwtRegisteredClaimNames.Email,user.Email),
                new(JwtRegisteredClaimNames.Sub,user.Id.ToString()!),

            ];
            SecurityTokenDescriptor tokenDescriptor = new()
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(1),
                Issuer = _issuer,
                Audience = _audience,
                SigningCredentials = new SigningCredentials(
                  new SymmetricSecurityKey(Convert.FromHexString(_secretKey)),
              SecurityAlgorithms.HmacSha256Signature)
            };

            string token = tokenHandler.WriteToken(
                tokenHandler.CreateToken(tokenDescriptor));
            return token;

        }
        public string GetEmailFromToken(string token)
        {
            var email = GetClaimValue(token, JwtRegisteredClaimNames.Email);
            ArgumentNullException.ThrowIfNull(email);
            return email;
        }
        private static string? GetClaimValue(string token, string claimType)
        {
            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadJwtToken(token);
            var claim = jsonToken.Claims.FirstOrDefault(c => c.Type == claimType);

            return claim?.Value;
        }
    }

}