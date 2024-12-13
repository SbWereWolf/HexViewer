
namespace DataSearch
{
    public class SearchString
    {
        public readonly string Pattern;
        public readonly byte[] Needle;
        public SearchString(string pattern, byte[] needle)
        {
            this.Pattern = pattern;
            this.Needle = needle;
        }
    }
}
