namespace Visualization
{
    internal class UnknownModeException : Exception
    {
        public UnknownModeException()
        {
        }

        public UnknownModeException(string message)
            : base(message)
        {
        }

        public UnknownModeException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
