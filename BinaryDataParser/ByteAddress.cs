using System;

namespace BinaryDataParser
{
    public class ByteAddress
    {
        public readonly int Offset;

        public ByteAddress(int offset)
        {
            Offset = offset;
        }

        public long Align(long positIon)
        {
            positIon = (long)Math.Ceiling(
                                ((double)positIon) / Offset
                                ) * Offset;
            return positIon;
        }
    }
}
