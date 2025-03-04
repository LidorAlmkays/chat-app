
namespace Domain.Exceptions
{
    public class UserEmailTakenException : Exception
    {
        private const string DefaultPrefix = "Failed to create user in registry because user email taken";

        public UserEmailTakenException() : base(DefaultPrefix)
        {
        }

        public UserEmailTakenException(string message) : base(message)
        {
        }

        public UserEmailTakenException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}