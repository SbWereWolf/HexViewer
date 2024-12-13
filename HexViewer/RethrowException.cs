namespace ProbyteEditClient
{
    internal class RethrowException : System.Exception
    {
        public RethrowException()
        {
        }

        public RethrowException(string message) : base(message)
        {
            if (string.IsNullOrEmpty(message))
            {
                message = "Rethrow exception";
            }
        }

        public RethrowException(string message, System.Exception inner)
            : base(message, inner)
        {
            if (string.IsNullOrEmpty(message))
            {
                message = "Rethrow exception";
            }
        }
    }
}
