using Domain.models;
using DTOs;
using Enums;

namespace Application.mapping
{
    public static class UserDtoToDomainMapper
    {
        public static UserModel ToUserModel(this RequestCreateUserDTO userDTO)
        {
            ArgumentNullException.ThrowIfNull(userDTO);
            return new UserModel
            {
                UserName = userDTO.UserName,
                Age = userDTO.Age,
                Email = userDTO.Email,
                Password = userDTO.Password,
                Role = nameof(Role.User)
            };
        }
    }
}