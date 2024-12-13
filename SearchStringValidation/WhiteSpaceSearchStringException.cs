namespace DataSearch
{
    public class WhiteSpaceSearchStringException : Exception
    {
        public WhiteSpaceSearchStringException()
        {
        }

        public WhiteSpaceSearchStringException(string message)
            : base(message)
        {
        }

        public WhiteSpaceSearchStringException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
