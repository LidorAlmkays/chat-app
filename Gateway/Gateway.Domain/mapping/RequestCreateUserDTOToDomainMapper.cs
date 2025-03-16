using Gateway.Domain.models;
using Common.DTOs;
using Common.Enums;

namespace Gateway.Domain.mapping
{
    public static class RequestCreateUserDTOToDomainMapper
    {
        public static UserModel ToUserModelAsUser(this RequestCreateUserDTO userDTO)
        {
            ArgumentNullException.ThrowIfNull(userDTO);
            return new UserModel
            {
                Username = userDTO.Username,
                Birthday = userDTO.Birthday,
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
                Birthday = userDTO.Birthday,
                Email = userDTO.Email,
                Password = userDTO.Password,
                Role = nameof(Role.Admin),
                PasswordKey = ""
            };
        }
    }
}