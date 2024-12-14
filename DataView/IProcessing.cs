
using System.Text;
using Visualization;

namespace DataView
{
    internal interface IProcessing
    {
        void ValidByte(byte b);
        void InvalidByte();
        void LineEnd();
    }
}
