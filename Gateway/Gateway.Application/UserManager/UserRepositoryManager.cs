using Gateway.Application.Encryption;
using Gateway.Domain.mapping;
using Gateway.Domain.models;
using Common.DTOs;
using Gateway.Infrastructure.UserRepository;

namespace Gateway.Application.UserManager
{
    public class UserRepositoryManager(IUserRepository userRepository, IPasswordEncryption passwordEncryption) : IUserManager
    {
        private readonly IUserRepository _userRepository = userRepository;
        private readonly IPasswordEncryption _passwordEncryption = passwordEncryption;

        public async Task<ResponseCreateUserDTO> AddUserAsync(RequestCreateUserDTO user)
        {
            UserModel userModel = user.ToUserModelAsUser();
            (userModel.Password, userModel.PasswordKey) = _passwordEncryption.EncryptionPassword(userModel.Password);
            var userId = await _userRepository.InsertUser(userModel).ConfigureAwait(false);
            return new ResponseCreateUserDTO { Id = userId };
        }

        public async Task<ResponseDeleteUserByEmailDTO> DeleteUserByEmailAsync(RequestDeleteUserByEmailDTO userDeleteData)
        {
            ArgumentNullException.ThrowIfNull(userDeleteData);
            await _userRepository.DeleteUserByEmail(userDeleteData.Email).ConfigureAwait(false);
            return new ResponseDeleteUserByEmailDTO { };
        }
    }
}