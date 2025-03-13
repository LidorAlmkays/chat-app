using Gateway.Domain.models;
using DTOs;
using Enums;

namespace Gateway.Application.mapping
{
    public static class UserDtoToDomainMapper
    {
        public static UserModel ToUserModelAsUser(this RequestCreateUserDTO userDTO)
        {
            ArgumentNullException.ThrowIfNull(userDTO);
            return new UserModel
            {
                Username = userDTO.Username,
                Age = userDTO.Age,
                Email = userDTO.Email,
                Password = userDTO.Password,
                Role = nameof(Role.User),
                PasswordKey = ""
            };
        }
        public static UserModel ToUserModelAsAdmin(this RequestCreateUserDTO userDTO)
        {
            ArgumentNullException.ThrowIfNull(userDTO);
            return new UserModel
            {
                Username = userDTO.Username,
                Age = userDTO.Age,
                Email = userDTO.Email,
                Password = userDTO.Password,
                Role = nameof(Role.Admin),
                PasswordKey = ""
            };
        }
    }
}