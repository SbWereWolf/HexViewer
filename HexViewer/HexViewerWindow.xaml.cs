
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace ProbyteEditClient
{
    public partial class HexViewerWindow : Window
    {
        private static readonly int BytesPerLine = 16;// Количество байтов на строку
        private static readonly int NumberOfLines = 16 * 2;
        private static readonly int NumberOfBytes = BytesPerLine * NumberOfLines;
        private readonly byte[]? FileBytes;
        private int BytesRead;
        private readonly FileStream? Source;
        private readonly long FileLength;
        private const string Hex = "Hex";
        private const string Decimal = "Decimal";
        private const string Binary = "Binary";
        private string ViewingMode = Hex;
        private const string NoData = "Нет данных для отображения.";
        private readonly BinaryDataParser.DataReader Reader;

        public HexViewerWindow(string binaryFilePath)
        {
            InitializeComponent();

            Source = new FileStream(binaryFilePath, FileMode.Open);
            FileLength = Source.Length;

            Reader = new BinaryDataParser.DataReader(Source, FileLength);

            FileBytes = new byte[NumberOfBytes];
            BytesRead = Source.Read(FileBytes, 0, NumberOfBytes);

            if (BytesRead == 0 || FileLength == 0)
            {
                AddressTextBox.Text = NoData;
                HexTextBox.Text = NoData;
                AsciiTextBox.Text = NoData;

                HexTextBox.Text = "Файл пустой, в файле нет данных";
            }


            if (BytesRead > 0)
            {
                switch (ViewingMode)
                {
                    case Hex:
                        UpdateHexView(FileBytes, BytesRead);
                        break;
                    case Decimal:
                        UpdateDecimalView(FileBytes, BytesRead);
                        break;
                    case Binary:
                        UpdateBinaryView(FileBytes, BytesRead);
                        break;
                }
            }
        }

        // Метод для обновления HEX представления
        private void UpdateHexView(byte[]? fileBytes, int bytesRead)
        {

            if (bytesRead == 0 || fileBytes == null || Source == null)
            {
                return;
            }

            StringBuilder addressBuilder = new();
            StringBuilder hexBuilder = new();
            StringBuilder asciiBuilder = new();

            for (int i = 0; i < fileBytes.Length; i += BytesPerLine)
            {
                // Адрес строки
                addressBuilder.AppendFormat("{0:X8}\n", i + Source.Position - BytesRead);

                if (i < bytesRead)
                {
                    // Байтовая часть в HEX формате
                    for (int j = 0; j < BytesPerLine; j++)
                    {
                        if (i + j < BytesRead)
                        {
                            hexBuilder.AppendFormat("{0:X2} ", fileBytes[i + j]);
                        }
                        else
                        {
                            hexBuilder.Append("  ");
                        }
                    }
                    hexBuilder.AppendLine();

                    // ASCII представление
                    for (int j = 0; j < BytesPerLine; j++)
                    {
                        if (i + j < BytesRead)
                        {
                            byte b = fileBytes[i + j];
                            char c = b >= 32 && b <= 126 ? (char)b : '•';
                            asciiBuilder.Append(c);
                        }
                        else
                        {
                            asciiBuilder.Append(' ');
                        }
                    }
                    asciiBuilder.AppendLine();
                }
            }

            // Устанавливаем текст в соответствующие TextBox
            AddressTextBox.Text = addressBuilder.ToString();
            HexTextBox.Text = hexBuilder.ToString();
            AsciiTextBox.Text = asciiBuilder.ToString();
        }
        private void UpdateDecimalView(byte[]? fileBytes, int bytesRead)
        {
            if (bytesRead == 0 || fileBytes == null || Source == null)
            {
                return;
            }

            StringBuilder addressBuilder = new();
            StringBuilder decimalBuilder = new();
            StringBuilder asciiBuilder = new();

            for (int i = 0; i < fileBytes.Length; i += BytesPerLine)
            {
                // Адрес строки
                addressBuilder.AppendFormat("{0:X8}\n", i + Source.Position - BytesRead);

                if (i < bytesRead)
                {
                    // Байтовая часть в десятичном представлении
                    for (int j = 0; j < BytesPerLine; j++)
                    {
                        if (i + j < BytesRead)
                        {
                            decimalBuilder.AppendFormat("{0:D3} ", fileBytes[i + j]);
                        }
                        else
                        {
                            decimalBuilder.Append("   ");
                        }
                    }
                    decimalBuilder.AppendLine();

                    // ASCII представление
                    for (int j = 0; j < BytesPerLine; j++)
                    {
                        if (i + j < BytesRead)
                        {
                            byte b = fileBytes[i + j];
                            char c = b >= 32 && b <= 126 ? (char)b : '•';
                            asciiBuilder.Append(c);
                        }
                        else
                        {
                            asciiBuilder.Append(' ');
                        }
                    }

                    asciiBuilder.AppendLine();
                }
            }

            // Устанавливаем текст в TextBox
            AddressTextBox.Text = addressBuilder.ToString();
            HexTextBox.Text = decimalBuilder.ToString();
            AsciiTextBox.Text = asciiBuilder.ToString();

        }
        private void UpdateBinaryView(byte[]? fileBytes, int bytesRead)
        {
            if (bytesRead == 0 || fileBytes == null || Source == null)
            {
                return;
            }

            StringBuilder addressBuilder = new();
            StringBuilder binaryBuilder = new();
            StringBuilder asciiBuilder = new();

            for (int i = 0; i < fileBytes.Length; i += BytesPerLine)
            {
                // Адрес строки
                addressBuilder.AppendFormat("{0:X8}\n", i + Source.Position - BytesRead);

                if (i < bytesRead)
                {
                    // Байтовая часть в бинарном представлении
                    for (int j = 0; j < BytesPerLine; j++)
                    {
                        if (i + j < BytesRead)
                        {
                            binaryBuilder.AppendFormat("{0} ", Convert.ToString(fileBytes[i + j], 2).PadLeft(8, '0'));
                        }
                        else
                        {
                            binaryBuilder.Append(new string(' ', 9));
                        }
                    }

                    binaryBuilder.AppendLine();

                    // ASCII представление
                    for (int j = 0; j < BytesPerLine; j++)
                    {
                        if (i + j < BytesRead)
                        {
                            byte b = fileBytes[i + j];
                            char c = b >= 32 && b <= 126 ? (char)b : '•';
                            asciiBuilder.Append(c);
                        }
                        else
                        {
                            asciiBuilder.Append(' ');
                        }
                    }

                    asciiBuilder.AppendLine();
                }

            }

            // Устанавливаем текст в TextBox
            AddressTextBox.Text = addressBuilder.ToString();
            HexTextBox.Text = binaryBuilder.ToString();
            AsciiTextBox.Text = asciiBuilder.ToString();
        }
        private void ShowHexView_Click(object sender, RoutedEventArgs e)
        {
            ViewingMode = Hex;
            UpdateHexView(FileBytes, BytesRead);
        }

        private void ShowDecimalView_Click(object sender, RoutedEventArgs e)
        {
            ViewingMode = Decimal;
            UpdateDecimalView(FileBytes, BytesRead);
        }

        private void ShowBinaryView_Click(object sender, RoutedEventArgs e)
        {
            ViewingMode = Binary;
            UpdateBinaryView(FileBytes, BytesRead);
        }

        private void ScrollViewer_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            if (sender == AddressScrollViewer)
            {
                HexScrollViewer.ScrollToVerticalOffset(e.VerticalOffset);
                AsciiScrollViewer.ScrollToVerticalOffset(e.VerticalOffset);
            }
            else if (sender == HexScrollViewer)
            {
                AddressScrollViewer.ScrollToVerticalOffset(e.VerticalOffset);
                AsciiScrollViewer.ScrollToVerticalOffset(e.VerticalOffset);
            }
            else if (sender == AsciiScrollViewer)
            {
                AddressScrollViewer.ScrollToVerticalOffset(e.VerticalOffset);
                HexScrollViewer.ScrollToVerticalOffset(e.VerticalOffset);
            }
        }

        // Метод для подсветки всех найденных значений
        public void HighlightFoundValues(List<int> foundIndices, string searchText)
        {
            if (foundIndices == null || foundIndices.Count == 0 || string.IsNullOrEmpty(searchText))
                return;

            // Устанавливаем фокус и выделяем все найденные значения
            HexTextBox.Focus();
            foreach (int foundIndex in foundIndices)
            {
                if (foundIndex >= 0 && foundIndex < HexTextBox.Text.Length)
                {
                    HexTextBox.Select(foundIndex, searchText.Length);
                }
            }
        }

        // Метод для прокрутки к определенному значению, восстановленный
        public void ScrollToFoundValue(int foundIndex, int length)
        {
            // Устанавливаем фокус на TextBox
            HexTextBox.Focus();

            // Выделяем текст от найденного индекса на длину искомого значения
            HexTextBox.Select(foundIndex, length);

            // Прокручиваем TextBox так, чтобы найденное значение было видно
            HexTextBox.Dispatcher.Invoke(() =>
            {
                Rect foundRect = HexTextBox.GetRectFromCharacterIndex(foundIndex);

                if (foundRect != Rect.Empty) // Проверяем, что Rect действителен
                {
                    // Рассчитываем вертикальное смещение, чтобы текст был в центре
                    double targetOffset = foundRect.Top + HexTextBox.VerticalOffset - (HexTextBox.ViewportHeight / 2);
                    HexTextBox.ScrollToVerticalOffset(Math.Max(0, targetOffset));
                }
            }, System.Windows.Threading.DispatcherPriority.ApplicationIdle);
        }

        // Метод для открытия окна поиска
        private void OpenSearchWindow_Click(object sender, RoutedEventArgs e)
        {
            SearchWindow searchWindow = new(HexTextBox.Text, this)
            {
                Owner = this // Устанавливаем родительское окно
            };
            searchWindow.Show(); // Открываем окно без блокировки основного окна
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Source?.Dispose();
        }

        private void Backward_Click(object sender, RoutedEventArgs e)
        {
            if (FileBytes != null)
            {
                var allow = Reader.MayBackward(FileBytes.Length);
                if (!allow)
                {
                    MessageBox.Show("Достигнуто начало файла");
                }

                if (allow)
                {
                    BytesRead = Reader.Backward(FileBytes);

                    switch (ViewingMode)
                    {
                        case Hex:
                            UpdateHexView(FileBytes, BytesRead);
                            break;
                        case Decimal:
                            UpdateDecimalView(FileBytes, BytesRead);
                            break;
                        case Binary:
                            UpdateBinaryView(FileBytes, BytesRead);
                            break;
                    }
                }
            }
        }

        private void Forward_Click(object sender, RoutedEventArgs e)
        {

            if (FileBytes != null)
            {
                var allow = Reader.MayForward(FileLength);
                if (!allow)
                {
                    MessageBox.Show("Достигнут конец файла");
                }

                if (allow)
                {
                    BytesRead = Reader.Forward(FileBytes);

                    switch (ViewingMode)
                    {
                        case Hex:
                            UpdateHexView(FileBytes, BytesRead);
                            break;
                        case Decimal:
                            UpdateDecimalView(FileBytes, BytesRead);
                            break;
                        case Binary:
                            UpdateBinaryView(FileBytes, BytesRead);
                            break;
                    }
                }
            }
        }
    }
}
