using System;
using Visualization;

namespace DataView
{
    internal class PositionСorrector : IProcessing
    {
        private int SelectionStart;
        private int SelectionFinish;

        private long BlockStart;
        private long NeedleStart;
        private long NeedleFinish;

        private int Increment;
        private int CurrentIndex;

        public PositionСorrector(
            Settings settings,
            long foundAddress,
            int needleLength,
            long blockEnd,
            int blockLength
            )
        {
            BlockStart = blockEnd - blockLength;

            CurrentIndex = 0;
            Increment = settings.Placeholder.Length;

            NeedleStart = foundAddress;
            NeedleFinish = foundAddress + needleLength;


        }
        public void ValidByte(byte b, int offset)
        {
            var byteAddress = BlockStart + offset;
            if (byteAddress == NeedleStart)
            {
                SelectionStart = CurrentIndex;
            }
            if (byteAddress == NeedleFinish)
            {
                SelectionFinish = CurrentIndex;

            }

            CurrentIndex += Increment;
        }
        public void InvalidByte()
        {
            CurrentIndex += Increment;
        }
        public void LineEnd()
        {
            CurrentIndex += Environment.NewLine.Length;
        }
        public Selection Unload()
        {
            return new Selection(SelectionStart, SelectionFinish);
        }
    }
}
