
namespace Domain.Exceptions
{
    public class ConnectionException : Exception
    {
        private const string DefaultPrefix = "Failed to connect to the registry";
        public ConnectionException()
            : base(DefaultPrefix) { }

        public ConnectionException(string message) : base(DefaultPrefix + message)
        {
        }

        public ConnectionException(string message, Exception innerException) : base(DefaultPrefix + message, innerException)
        {
        }

    }
}