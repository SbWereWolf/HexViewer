using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Visualization;

namespace DataView
{
    internal class AsciiPrinter : IProcessing
    {
        private readonly StringBuilder Ascii;
        private readonly Settings Settings;
        public AsciiPrinter(Settings settings)
        {
            Ascii = new StringBuilder();
            this.Settings = settings;
        }
        public void ValidByte(byte b)
        {
            char c =
                b >= 32 && b <= 126
                ? (char)b
                : Settings.WrongAsciiSymbol;
            Ascii.Append(c);
        }
        public void InvalidByte()
        {
            char c = Settings.AsciiSymbolPlaceholder;
            Ascii.Append(c);
        }
        public void LineEnd()
        {
            Ascii.AppendLine();
        }
        public string Unload()
        {
            return Ascii.ToString();
        }
    }
}
