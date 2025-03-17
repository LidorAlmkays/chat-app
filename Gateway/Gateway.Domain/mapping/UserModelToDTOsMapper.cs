using Common.DTOs;
using Gateway.Domain.models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gateway.Domain.mapping
{
    public static class UserModelToDTOsMapper
    {
        public static ResponseGetUserByEmailDTO ToResponseGetUserByEmailDTO(this UserModel user)
        {
            ArgumentNullException.ThrowIfNull(user);
            return new ResponseGetUserByEmailDTO
            {
                Username = user.Username,
                Birthday = user.Birthday,
                Email = user.Email,
                Password = user.Password,
                Role = user.Role,
                CreatedAt = user.CreatedAt,
                PasswordKey = user.PasswordKey,
                Id = user.Id
            };
        }

    }
}