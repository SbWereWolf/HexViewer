namespace DataSearch
{
    public class InadequateSearchStringException : Exception
    {
        public InadequateSearchStringException()
        {
        }

        public InadequateSearchStringException(string message)
            : base(message)
        {
        }

        public InadequateSearchStringException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
