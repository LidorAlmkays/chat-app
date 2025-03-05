
namespace Domain.Exceptions
{
    public class CreateUserEmailTakenException : Exception
    {
        private const string DefaultPrefix = "Failed to create user in registry because user email taken";

        public CreateUserEmailTakenException() : base(DefaultPrefix)
        {
        }

        public CreateUserEmailTakenException(string message) : base(DefaultPrefix + ", " + message)
        {
        }

        public CreateUserEmailTakenException(string message, Exception innerException) : base(DefaultPrefix + ", " + message, innerException)
        {
        }
    }
}