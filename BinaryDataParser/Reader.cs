
using System.IO;

namespace BinaryDataParser
{
    public class Reader
    {
        private readonly FileStream Source;
        private readonly long FileLength = 0;

        public long Position
        {
            get =>  this.Source.Position;
        }

        public Reader(FileStream source, long length)
        {
            Source = source;
            FileLength = length;
        }

        public bool MayBackward(long dataFrameSize)
        {
            var allow = !(Source.Position == dataFrameSize);

            return allow;
        }

        public bool MayForward()
        {
            var allow = !(Source.Position == FileLength);

            return allow;
        }

        public int Backward(byte[] dataFrame)
        {
            int bytesRead = 0;
            if (Source != null)
            {
                var mayBackward = Source.Position >= dataFrame.Length * 2;
                long position = 0;
                if (mayBackward)
                {
                    position = -2 * dataFrame.LongLength;
                }
                if (!mayBackward)
                {
                    position = -1 * Source.Position;
                }

                Source.Seek(position, SeekOrigin.Current);
                bytesRead = Source.Read(dataFrame);
            }

            return bytesRead;
        }

        public int Forward(byte[] dataFrame)
        {
            int bytesRead = 0;
            if (Source != null)
            {
                bytesRead = Source.Read(dataFrame);
            }

            return bytesRead;
        }

    }
}
