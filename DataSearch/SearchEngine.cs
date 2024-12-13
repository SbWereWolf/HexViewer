
using System.IO;

namespace DataSearch
{
    public class SearchEngine
    {
        private readonly FileStream Source;

        public SearchEngine(FileStream source)
        {
            Source = source;
        }

        public long[] FindAllOccurrences(byte[] needle)
        {
            var found = new List<long>();

            var needleLength = needle.Length;
            var haystack = new byte[2 * needleLength];

            Source.Seek(0, SeekOrigin.Begin);
            var validBytes = Source.Read(haystack);
            while (validBytes >= needleLength)
            {
                var limit = validBytes - needleLength;
                var i = 0;
                for (i = 0; i <= limit; i++)
                {
                    var isFound = FindBytes(haystack, i, needle);
                    if (isFound)
                    {
                        var address = Source.Position - validBytes + i;
                        found.Add(address);

                        i += needleLength - 1;
                    }
                }

                var bytesRemainder = validBytes - i;
                Array.Copy(haystack, i, haystack, 0, bytesRemainder);

                var portion = new byte[2 * needleLength - bytesRemainder];
                var bytesRead = Source.Read(portion);
                Array.Copy(portion, 0, haystack, bytesRemainder, bytesRead);

                validBytes = bytesRemainder + bytesRead;
            }

            var foundAddresses = found.ToArray();
            return foundAddresses;
        }

        private static bool FindBytes(byte[] haystack, int i, byte[] needle)
        {
            var needleLength = needle.Length;
            var example = new byte[needleLength];
            Array.Copy(haystack, i, example, 0, needleLength);
            var isFound = needle.SequenceEqual(example);

            return isFound;
        }
    }
}
