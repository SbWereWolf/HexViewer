using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BinaryDataParser
{
    internal class Settings
    {
        private const string HexFormat = "{0:00} ";
        private const string HexPlaceholder = "   ";
        private const int HexBase = 16;
        private const string DecimalFormat = "{0:000} ";
        private const string DecimalPlaceholder = "    ";
        private const int DecimalBase = 10;
        private const string BinaryFormat = "{0:00000000} ";
        private const string BinaryPlaceholder = "         ";
        private const int BinaryBase = 2;

        public readonly int Basis;
        public readonly string Format;
        public readonly string Placeholder;

        private Settings(int basis, string format, string placeholder)
        {
            Basis = basis;
            Format = format;
            Placeholder = placeholder;
        }

        public static Settings AsHex()
        {
            return new Settings(HexBase, HexFormat, HexPlaceholder);
        }

        public static Settings AsDecamal()
        {
            return new Settings(DecimalBase, DecimalFormat, DecimalPlaceholder);
        }

        public static Settings AsBinary()
        {
            return new Settings(BinaryBase, BinaryFormat, BinaryPlaceholder);
        }
    }
}
