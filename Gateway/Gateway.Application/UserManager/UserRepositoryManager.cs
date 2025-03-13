using Application.Encryption;
using Application.mapping;
using Gateway.Domain.models;
using DTOs;
using Gateway.Infrastructure.UserRepository;

namespace Application.UserManager
{
    public class UserRepositoryManager(IUserRepository userRepository, IPasswordEncryption passwordEncryption) : IUserManager
    {
        private readonly IUserRepository _userRepository = userRepository;
        private readonly IPasswordEncryption _passwordEncryption = passwordEncryption;

        public async Task<Guid> AddUserAsync(RequestCreateUserDTO user)
        {
            UserModel userModel = user.ToUserModelAsUser();
            (userModel.Password, userModel.PasswordKey) = _passwordEncryption.EncryptionPassword(userModel.Password);
            var userId = await _userRepository.InsertUser(userModel).ConfigureAwait(false);
            return userId;
        }

        public Task<bool> DeleteUserByEmailAsync(string userEmail)
        {
            return _userRepository.DeleteUserByEmail(userEmail);
        }
    }
}