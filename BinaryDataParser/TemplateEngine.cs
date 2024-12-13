using System;
using System.Text;
using Visualization;

namespace BinaryDataParser
{
    public class TemplateEngine
    {
        private readonly int BytesPerLine;
        private const char WrongAsciiSymbol = '•';
        private const char AsciiSymbolPlaceholder = ' ';
        private const string AddressFormat = "{0:X8}";

        private readonly StringBuilder Address = new();
        private readonly StringBuilder Display = new();
        private readonly StringBuilder Ascii = new();

        public TemplateEngine(int bytesPerLine)
        {
            BytesPerLine = bytesPerLine;
        }

        // Метод для обновления HEX представления
        public View Render(
            byte[] fileBytes, 
            int bytesRead, 
            long position, 
            Mode mode
            )
        {
            var display = RenderData(fileBytes, bytesRead, mode);
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

        private string RenderData(byte[] fileBytes, int bytesRead, Mode mode)
        {
            var settings = Settings.ChooseSettings(mode);

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
                                .ToString(fileBytes[byteIndex], settings.Basis)
                                .ToUpper()
                                .PadLeft(settings.Format, '0') + " ";
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
    }
}
