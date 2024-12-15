
using System.Text;
using Visualization;

namespace DataView
{
    internal interface IProcessing
    {
        void ValidByte(byte b, int offset);
        void InvalidByte();
        void LineEnd();
    }
}
