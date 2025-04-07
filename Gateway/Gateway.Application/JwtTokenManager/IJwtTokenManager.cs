using Gateway.Domain.models;

namespace Gateway.Application.TokenManager.Jwt
{
    public interface IJwtTokenManager
    {
        string GenerateToken(UserModel user);
        string GetEmailFromToken(string token);
    }
}