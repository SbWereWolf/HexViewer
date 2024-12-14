using System;
using System.Text;
using Visualization;

namespace DataView
{
    public class TemplateEngine
    {
        private readonly int BytesPerLine;
        private readonly StringBuilder Address = new();
        public TemplateEngine(int bytesPerLine)
        {
            BytesPerLine = bytesPerLine;
        }
        public View Render(
            byte[] blockBytes,
            int bytesRead,
            long position,
            Mode mode
            )
        {
            var settings = SettingsFactory.PickUpSettings(mode);

            var dataPrinter = new DataPrinter(settings);
            RenderView(blockBytes, bytesRead, dataPrinter);
            var display = dataPrinter.Unload();


            var asciiPrinter = new AsciiPrinter(settings);
            RenderView(blockBytes, bytesRead, asciiPrinter);
            var ascii  = asciiPrinter.Unload();

            var address = RenderAddress(
                blockBytes,
                bytesRead,
                position
                );

            var view = new View(address, display, ascii);

            return view;
        }
        private void RenderView(
            byte[] blockBytes,
            int bytesRead,
            IProcessing printer
            )
        {
            for (int i = 0; i < blockBytes.Length; i += BytesPerLine)
            {
                if (i < bytesRead)
                {
                    for (int j = 0; j < BytesPerLine; j++)
                    {
                        var byteIndex = i + j;
                        var isValidByte = byteIndex < bytesRead;
                        if (isValidByte)
                        {
                            byte b = blockBytes[byteIndex];
                            printer.ValidByte(b);
                        }
                        if (!isValidByte)
                        {
                            printer.InvalidByte();
                        }
                    }
                    printer.LineEnd();
                }
            }
        }

        private string RenderAddress(
            byte[] blockBytes,
            int bytesRead,
            long position
            )
        {
            Address.Clear();
            for (int i = 0; i < blockBytes.Length; i += BytesPerLine)
            {
                Address.AppendFormat(
                    Settings.AddressFormat,
                    i + position - bytesRead
                    );
                Address.AppendLine();
            }

            var result = Address.ToString();
            return result;
        }
    }
}
