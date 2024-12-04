using System;
using System.Text;

namespace BinaryDataParser
{
    public class TemplateEngine
    {
        private readonly int BytesPerLine = 16;// Количество байтов на строку
        private Mode ViewingMode = Mode.Hex;
        private const string NoData = "Нет данных для отображения.";
        private const char WrongAsciiSymbol = '•';
        private const char AsciiSymbolPlaceholder = ' ';
        private const string AddressFormat = "{0:X8}";
        private readonly StringBuilder Address = new();
        private readonly StringBuilder Display = new();
        private readonly StringBuilder Ascii = new();
        private readonly Settings Hex;
        private readonly Settings Dec;
        private readonly Settings Bin;

        public enum Mode
        {
            Binary,
            Decimal,
            Hex
        }

        public TemplateEngine(int bytesPerLine, Mode mode)
        {
            ViewingMode = mode;
            BytesPerLine = bytesPerLine;
            Hex = Settings.AsHex();
            Dec = Settings.AsDecamal();
            Bin = Settings.AsBinary();
        }

        // Метод для обновления HEX представления
        public View Render(byte[] fileBytes, int bytesRead, long position)
        {
            var display = RenderData(fileBytes, bytesRead);
            var address = RenderAddress(fileBytes, bytesRead, position);
            var ascii = RenderAsciiView(fileBytes, bytesRead);

            var view = new View(address, display, ascii);

            return view;
        }

        private string RenderAsciiView(byte[] fileBytes, int bytesRead)
        {
            Ascii.Clear();
            for (int i = 0; i < fileBytes.Length; i += BytesPerLine)
            {
                if (i < bytesRead)
                {
                    // ASCII представление
                    for (int j = 0; j < BytesPerLine; j++)
                    {
                        if (i + j < bytesRead)
                        {
                            byte b = fileBytes[i + j];
                            char c = b >= 32 && b <= 126 ? (char)b : WrongAsciiSymbol;
                            Ascii.Append(c);
                        }
                        else
                        {
                            Ascii.Append(AsciiSymbolPlaceholder);
                        }
                    }
                    Ascii.AppendLine();
                }
            }

            var result = Ascii.ToString();  

            return result;
        }

        private string RenderAddress(byte[] fileBytes, int bytesRead, long position)
        {
            Address.Clear();
            for (int i = 0; i < fileBytes.Length; i += BytesPerLine)
            {
                // Адрес строки
                Address.AppendFormat(AddressFormat, i + position - bytesRead);
                Address.AppendLine();
            }

            var result = Address.ToString();
            return result;
        }

        private string RenderData(byte[] fileBytes, int bytesRead)
        {
            Settings settings = Hex;
            switch (ViewingMode)
            {
                case Mode.Binary:
                    settings = Bin;
                    break;
                case Mode.Decimal:
                    settings = Dec;
                    break;
                case Mode.Hex:
                    settings = Hex;
                    break;
            }

            Display.Clear();
            var word = "";
            for (int i = 0; i < fileBytes.Length; i += BytesPerLine)
            {
                if (i < bytesRead)
                {
                    for (int j = 0; j < BytesPerLine; j++)
                    {
                        var byteIndex = i + j;
                        var isValidByte = byteIndex < bytesRead;
                        if (isValidByte)
                        {
                            word = Convert
                                .ToString(fileBytes[i + j], settings.Basis)
                                .ToUpper()
                                .PadLeft(settings.Format,'0') + " ";
                        }
                        if (!isValidByte)
                        {
                            word = settings.Placeholder;
                        }

                        Display.Append(word);
                    }
                    Display.AppendLine();
                }
            }

            return Display.ToString();
        }

        public void AsHex()
        {
            ViewingMode = Mode.Hex;
        }

        public void AsDecimal()
        {
            ViewingMode = Mode.Decimal;
        }

        public void AsBinary()
        {
            ViewingMode = Mode.Binary;
        }
    }
}
