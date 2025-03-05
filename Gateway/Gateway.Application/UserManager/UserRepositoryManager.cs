using Application.Encryption;
using Application.mapping;
using Domain.models;
using DTOs;
using Gateway.Infrastructure.UserRepository;

namespace Application.UserManager
{
    public class UserRepositoryManager(IUserRepository userRepository, IPasswordEncryption passwordEncryption) : IUserManager
    {
        private readonly IUserRepository _userRepository = userRepository;
        private readonly IPasswordEncryption _passwordEncryption = passwordEncryption;

        public Task<int> AddUserAsync(RequestCreateUserDTO user)
        {
            UserModel userModel = user.ToUserModel();
            (userModel.Password, userModel.PasswordKey) = _passwordEncryption.EncryptionPassword(userModel.Password);
            return _userRepository.InsertUser(userModel);
        }

        public Task<bool> DeleteUserByEmailAsync(string userEmail)
        {
            return _userRepository.DeleteUserByEmail(userEmail);
        }
    }
}