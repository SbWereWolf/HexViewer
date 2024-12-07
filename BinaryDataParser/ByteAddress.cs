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

        public long AlignBackward(long positIon)
        {
            positIon = (long)Math.Ceiling(
                                ((double)positIon) / Offset
                                ) * Offset;
            return positIon;
        }

        public long AlignForward(long position)
        {
            position = (long)Math.Floor(
                                ((double)position) / Offset
                                ) * Offset;
            return position;
        }
    }
}
