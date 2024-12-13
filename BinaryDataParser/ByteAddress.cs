using System;

namespace DataView
{
    public class ByteAddress
    {
        public readonly int Offset;

        public ByteAddress(int offset)
        {
            Offset = offset;
        }

        public long Align(long position)
        {
            position = (long)Math.Ceiling(
                                ((double)position) / Offset
                                ) * Offset;
            return position;
        }
    }
}
