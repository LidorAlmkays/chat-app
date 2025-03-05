namespace Gateway.Domain.Exceptions
{
    public class DeleteUserByEmailException : Exception
    {
        private const string DefaultPrefix = "Failed to delete user from registry";
        public DeleteUserByEmailException() : base(DefaultPrefix)
        {
        }
        public DeleteUserByEmailException(string message) : base(DefaultPrefix + ", " + message)
        {
        }

        public DeleteUserByEmailException(string message, Exception innerException) : base(DefaultPrefix + ", " + message, innerException)
        {
        }

    }
}