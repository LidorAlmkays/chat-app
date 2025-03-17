
namespace Domain.Exceptions
{
    public class ConnectionException : Exception
    {
        private const string DefaultPrefix = "Failed to connect to the registry";
        public ConnectionException()
            : base(DefaultPrefix) { }

        public ConnectionException(string message) : base(message)
        {
        }

        public ConnectionException(string message, Exception innerException) : base(message, innerException)
        {
        }

    }
}