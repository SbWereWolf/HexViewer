
using System.IO;

namespace BinaryDataParser
{
    public class Reader
    {
        private readonly FileStream Source;

        public long Position
        {
            get => this.Source.Position;
        }

        public Reader(FileStream source, long length)
        {
            Source = source;
        }

        public int ReadAt(long position, byte[] dataFrame)
        {

            position -= dataFrame.LongLength;
            if (position < 0)
            {
                position = 0;
            }

            Source.Seek(position, SeekOrigin.Begin);
            int bytesRead = Source.Read(dataFrame);

            return bytesRead;
        }
    }
}
