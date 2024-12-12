namespace BinaryDataParser
{
    public class Settings
    {
        private const int HexFormat = 2;
        private const string HexPlaceholder = "   ";
        private const int HexBase = 16;
        private const int DecimalFormat = 3;
        private const string DecimalPlaceholder = "    ";
        private const int DecimalBase = 10;
        private const int BinaryFormat = 8;
        private const string BinaryPlaceholder = "         ";
        private const int BinaryBase = 2;

        public readonly int Basis;
        public readonly int Format;
        public readonly string Placeholder;

        private Settings(int basis, int format, string placeholder)
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
