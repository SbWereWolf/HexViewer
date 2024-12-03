using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace ProbyteEdit_Client
{
    public partial class HexViewerWindow : Window
    {
        private readonly byte[] fileBytes;
        private readonly int bytesPerLine = 16; // Количество байтов на строку        

        public HexViewerWindow(byte[] fileBytes)
        {
            InitializeComponent();
            this.fileBytes = fileBytes;

            // Вызов метода для отображения данных в HEX представлении
            UpdateHexView();
        }

        // Метод для обновления HEX представления
        private void UpdateHexView()
        {
            if (fileBytes == null || fileBytes.Length == 0)
            {
                AddressTextBox.Text = "Нет данных для отображения.";
                HexTextBox.Text = "Нет данных для отображения.";
                AsciiTextBox.Text = "Нет данных для отображения.";
                return;
            }

            StringBuilder addressBuilder = new StringBuilder();
            StringBuilder hexBuilder = new StringBuilder();
            StringBuilder asciiBuilder = new StringBuilder();

            for (int i = 0; i < fileBytes.Length; i += bytesPerLine)
            {
                // Адрес строки
                addressBuilder.AppendFormat("{0:X8}\n", i);

                // Байтовая часть в HEX формате
                for (int j = 0; j < bytesPerLine; j++)
                {
                    if (i + j < fileBytes.Length)
                    {
                        hexBuilder.AppendFormat("{0:X2} ", fileBytes[i + j]);
                    }
                    else
                    {
                        hexBuilder.Append("   ");
                    }
                }
                hexBuilder.AppendLine();

                // ASCII представление
                for (int j = 0; j < bytesPerLine; j++)
                {
                    if (i + j < fileBytes.Length)
                    {
                        byte b = fileBytes[i + j];
                        char c = b >= 32 && b <= 126 ? (char)b : '.';
                        asciiBuilder.Append(c);
                    }
                    else
                    {
                        asciiBuilder.Append(' ');
                    }
                }
                asciiBuilder.AppendLine();
            }

            // Устанавливаем текст в соответствующие TextBox
            AddressTextBox.Text = addressBuilder.ToString();
            HexTextBox.Text = hexBuilder.ToString();
            AsciiTextBox.Text = asciiBuilder.ToString();
        }
        private void UpdateDecimalView()
        {
            if (fileBytes == null || fileBytes.Length == 0)
            {
                HexTextBox.Text = "Нет данных для отображения.";
                return;
            }

            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < fileBytes.Length; i += bytesPerLine)
            {
                // Адрес строки
                sb.AppendFormat("{0:X8}  || ", i);

                // Байтовая часть в десятичном представлении
                for (int j = 0; j < bytesPerLine; j++)
                {
                    if (i + j < fileBytes.Length)
                    {
                        sb.AppendFormat("{0:D3} ", fileBytes[i + j]);
                    }
                    else
                    {
                        sb.Append("    ");
                    }
                }

                // Разделитель между Decimal и ASCII
                sb.Append("|| ");

                // ASCII представление
                for (int j = 0; j < bytesPerLine; j++)
                {
                    if (i + j < fileBytes.Length)
                    {
                        byte b = fileBytes[i + j];
                        char c = b >= 32 && b <= 126 ? (char)b : '.';
                        sb.Append(c);
                    }
                    else
                    {
                        sb.Append(' ');
                    }
                }

                sb.AppendLine();
            }

            // Устанавливаем текст в TextBox
            HexTextBox.Text = sb.ToString();
        }
        private void UpdateBinaryView()
        {
            if (fileBytes == null || fileBytes.Length == 0)
            {
                HexTextBox.Text = "Нет данных для отображения.";
                return;
            }

            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < fileBytes.Length; i += bytesPerLine)
            {
                // Адрес строки
                sb.AppendFormat("{0:X8}  || ", i);

                // Байтовая часть в бинарном представлении
                for (int j = 0; j < bytesPerLine; j++)
                {
                    if (i + j < fileBytes.Length)
                    {
                        sb.AppendFormat("{0} ", Convert.ToString(fileBytes[i + j], 2).PadLeft(8, '0'));
                    }
                    else
                    {
                        sb.Append(new string(' ', 9));
                    }
                }

                // Разделитель между Binary и ASCII
                sb.Append("|| ");

                // ASCII представление
                for (int j = 0; j < bytesPerLine; j++)
                {
                    if (i + j < fileBytes.Length)
                    {
                        byte b = fileBytes[i + j];
                        char c = b >= 32 && b <= 126 ? (char)b : '.';
                        sb.Append(c);
                    }
                    else
                    {
                        sb.Append(' ');
                    }
                }

                sb.AppendLine();
            }

            // Устанавливаем текст в TextBox
            HexTextBox.Text = sb.ToString();
        }
        private void ShowHexView_Click(object sender, RoutedEventArgs e)
        {
            UpdateHexView();
        }

        private void ShowDecimalView_Click(object sender, RoutedEventArgs e)
        {
            UpdateDecimalView();
        }

        private void ShowBinaryView_Click(object sender, RoutedEventArgs e)
        {
            UpdateBinaryView();
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
    }
}
