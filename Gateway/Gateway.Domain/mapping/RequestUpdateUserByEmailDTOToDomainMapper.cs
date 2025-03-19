using Common.DTOs;
using Gateway.Domain.models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gateway.Domain.mapping
{
    public static class RequestUpdateUserByEmailDTOToDomainMapper
    {
        public static UpdateUserModel ToUpdateUserModel(this RequestUpdateUserByEmailDTO dto)
        {
            ArgumentNullException.ThrowIfNull(dto);
            return new UpdateUserModel
            {
                Username = dto.Username,
                Birthday = dto.Birthday,
                NewEmail = dto.NewEmail,
                Password = dto?.Password,
                Role = dto?.Role
            };
        }
    }
}