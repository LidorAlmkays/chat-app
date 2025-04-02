using Gateway.Application.Encryption;
using Gateway.Domain.mapping;
using Gateway.Domain.models;
using Common.DTOs;
using Gateway.Infrastructure.UserRepository;

namespace Gateway.Application.UserManager
{

    //NOTE THIS IS PURE TEST CLASS FOR ADMIN USER IT ISNT FOR NORMAL USERS TO ACCESS AND CALL
    //AND MOSTLY I WANTED TO LEARN HOW TO WRITE CLEAN CODE
    public class UserRepositoryManager(IUserRepository userRepository, IPasswordEncryption passwordEncryption) : IUserManager
    {
        private readonly IUserRepository _userRepository = userRepository;
        private readonly IPasswordEncryption _passwordEncryption = passwordEncryption;

        public async Task<ResponseCreateUserDTO> AddUserAsync(RequestCreateUserDTO user)
        {
            UserModel userModel = user.ToUserModel();
            (userModel.Password, userModel.PasswordKey) = _passwordEncryption.EncryptionPassword(userModel.Password);
            var userId = await _userRepository.InsertUserAsync(userModel).ConfigureAwait(false);
            return new ResponseCreateUserDTO { Id = userId };
        }

        public async Task<ResponseDeleteUserByEmailDTO> DeleteUserByEmailAsync(RequestDeleteUserByEmailDTO userDeleteData)
        {
            ArgumentNullException.ThrowIfNull(userDeleteData);
            await _userRepository.DeleteUserByEmailAsync(userDeleteData.Email).ConfigureAwait(false);
            return new ResponseDeleteUserByEmailDTO { };
        }

        public async Task<ResponseGetUserByEmailDTO> GetUserByEmailAsync(RequestGetUserByEmailDTO userGetData)
        {
            ArgumentNullException.ThrowIfNull(userGetData);
            var user = await _userRepository.GetUserByEmailAsync(userGetData.Email).ConfigureAwait(false);
            return user.ToResponseGetUserByEmailDTO();
        }

        public async Task<ResponseUpdateUserByEmailDTO> UpdateUserByEmailAsync(RequestUpdateUserByEmailDTO data)
        {
            ArgumentNullException.ThrowIfNull(data);
            UpdateUserModel UpdateUserModel = data.ToUpdateUserModel();
            if (data.Password != null)
                (UpdateUserModel.Password, UpdateUserModel.PasswordKey) = _passwordEncryption.EncryptionPassword(data.Password);

            await _userRepository.UpdateByEmailAsync(data.Email, UpdateUserModel).ConfigureAwait(false);
            return new ResponseUpdateUserByEmailDTO { };
        }
    }
}