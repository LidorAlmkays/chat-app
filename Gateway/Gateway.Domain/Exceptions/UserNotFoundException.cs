using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gateway.Domain.Exceptions
{
    public class UserNotFoundException : Exception
    {

        private const string DefaultPrefix = "User not found inside registry";
        public UserNotFoundException()
            : base(DefaultPrefix) { }

        public UserNotFoundException(string message) : base(DefaultPrefix + ", " + message)
        {
        }

        public UserNotFoundException(string message, Exception innerException) : base(DefaultPrefix + ", " + message, innerException)
        {
        }
    }
}