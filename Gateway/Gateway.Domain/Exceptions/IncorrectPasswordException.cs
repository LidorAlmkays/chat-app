using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gateway.Domain.Exceptions
{
    public class IncorrectPasswordException : Exception
    {
        private const string DefaultPrefix = "Authentication failed, given password didn't match in the database";
        public IncorrectPasswordException()
            : base(DefaultPrefix) { }

        public IncorrectPasswordException(string message) : base(message)
        {
        }

        public IncorrectPasswordException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}