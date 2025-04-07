using Gateway.Domain.models;
using Common.DTOs;
using Common.Enums;
using Gateway.Domain.Exceptions;

namespace Gateway.Domain.mapping
{
    public static class RequestCreateUserDTOToDomainMapper
    {
        public static UserModel ToUserModel(this RequestCreateUserDTO userDTO)
        {
            ArgumentNullException.ThrowIfNull(userDTO);
            if (!Enum.TryParse(typeof(Role), userDTO.Role ?? nameof(Role.User), true, out _))
            {
                throw new IncorrectUserRoleException("Incorrect role was givin in request to create user dto");
            }
            return new UserModel
            {
                Username = userDTO.Username,
                Birthday = userDTO.Birthday,
                Email = userDTO.Email,
                Password = userDTO.Password,
                Role = userDTO.Role ?? nameof(Role.User),
                PasswordKey = ""
            };
        }
    }
}