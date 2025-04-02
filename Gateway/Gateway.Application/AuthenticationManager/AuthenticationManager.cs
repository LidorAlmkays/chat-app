using Common.DTOs;
using Common.Enums;
using Gateway.Application.Encryption;
using Gateway.Application.TokenManager.Jwt;
using Gateway.Domain.Exceptions;
using Gateway.Domain.models;
using Gateway.Infrastructure.UserRepository;

namespace Gateway.Application.AuthenticationManager
{
    public class AuthenticationManager(IJwtTokenManager jwtTokenManager, IUserRepository userRepository, IPasswordEncryption passwordEncryption) : IAuthenticationManager
    {
        IJwtTokenManager _jwtTokenManager = jwtTokenManager;
        private readonly IUserRepository _userRepository = userRepository;
        private readonly IPasswordEncryption _passwordEncryption = passwordEncryption;
        public async Task<ResponseLoginDTO> Login(RequestLoginDTO loginData)
        {
            ArgumentNullException.ThrowIfNull(loginData);
            var user = await _userRepository.GetUserByEmailAsync(loginData.Email).ConfigureAwait(false);
            if (!_passwordEncryption.CheckPasswordValid(loginData.Password, user.Password, user.PasswordKey))
                throw new IncorrectPasswordException();
            var token = _jwtTokenManager.GenerateToken(user);
            return new ResponseLoginDTO { Token = token };
        }

        public async Task<ResponseUserRoleFromTokenDTO> GetUserRoleFromToken(string token)
        {
            var email = _jwtTokenManager.GetEmailFromToken(token);

            UserModel user = await _userRepository.GetUserByEmailAsync(email).ConfigureAwait(false);

            return new ResponseUserRoleFromTokenDTO { Role = user.Role };
        }
    }
}