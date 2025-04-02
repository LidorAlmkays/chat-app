using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Common.DTOs
{
    // public class RequestUserRoleDTO
    // {
    // } the token needs to be inside the Authorization field header

    public record ResponseUserRoleFromTokenDTO
    {
        public string Role { get; set; }
    }
}