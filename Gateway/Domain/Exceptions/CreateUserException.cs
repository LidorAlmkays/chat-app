
namespace Domain.Exceptions
{
    public class UsernameTakenException : Exception
    {
        private const string DefaultPrefix = "Failed to create user in registry because user name taken";

        public UsernameTakenException() : base(DefaultPrefix)
        {
        }

        public UsernameTakenException(string message) : base(message)
        {
        }

        public UsernameTakenException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}