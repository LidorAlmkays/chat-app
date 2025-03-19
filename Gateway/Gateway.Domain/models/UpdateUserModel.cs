using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gateway.Domain.models
{
    public class UpdateUserModel
    {
        public string? NewEmail { get; set; }
        public string? Username { get; set; }
        public DateTime? Birthday { get; set; }
        public string? Password { get; set; }
        public string? Role { get; set; }
        public string? PasswordKey { get; set; }

    }
}