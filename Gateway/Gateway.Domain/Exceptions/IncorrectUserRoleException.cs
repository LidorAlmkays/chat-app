using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gateway.Domain.Exceptions
{
    public class IncorrectUserRoleException : Exception
    {
        public IncorrectUserRoleException(string message) : base(message)
        {
        }

        public IncorrectUserRoleException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}