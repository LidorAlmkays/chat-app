using Common.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gateway.Application.AuthenticationManager
{
    public interface IAuthenticationManager
    {
        Task<ResponseLoginDTO> Login(RequestLoginDTO loginData);
        Task<ResponseUserRoleFromTokenDTO> GetUserRoleFromToken(string token);
    }
}