using Application.mapping;
using Domain.models;
using DTOs;
using Enums;
using Infrastructure.UserRepository;

namespace Application.UserManager
{
    public class SaltAndPepperUserManager(IUserRepository userRepository) : IUserManager
    {
        private readonly IUserRepository _userRepository = userRepository;

        public Task<int> AddUserAsync(RequestCreateUserDTO user)
        {
            UserModel userModel = user.ToUserModel();
            return _userRepository.InsertUser(userModel);
        }
    }
}