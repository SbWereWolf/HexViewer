namespace Visualization
{
    public class UnknownFormatException : Exception
    {
        public UnknownFormatException()
        {
        }

        public UnknownFormatException(string message)
            : base(message)
        {
        }

        public UnknownFormatException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
