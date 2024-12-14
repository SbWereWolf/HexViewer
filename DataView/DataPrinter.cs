using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Visualization;

namespace DataView
{
    internal class DataPrinter : IProcessing
    {
        private readonly StringBuilder Display;
        private readonly Settings Settings;
        public DataPrinter(Settings settings)
        {
            Display = new StringBuilder();
            this.Settings = settings;
        }
        public void ValidByte(byte b)
        {
            var word = Convert
                .ToString(b, Settings.Basis)
                .ToUpper()
                .PadLeft(Settings.Format, '0') + " ";

            Display.Append(word);
        }
        public void InvalidByte()
        {
            var word = Settings.Placeholder;
            Display.Append(word);
        }
        public void LineEnd()
        {
            Display.AppendLine();
        }
        public string Unload()
        {
            return Display.ToString();
        }
    }

}
