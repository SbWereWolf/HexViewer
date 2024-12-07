
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;

namespace ProbyteEditClient
{
    public partial class HexViewerWindow : Window
    {
        private static readonly int BytesPerLine = 16;// Количество байтов на строку
        private static readonly int NumberOfLines = 16 * 2;
        private static readonly int NumberOfBytes = BytesPerLine * NumberOfLines;
        private readonly byte[] FileBytes;
        private int BytesRead;
        private readonly FileStream Source;
        private readonly long FileLength;
        private const string NoData = "Нет данных для отображения.";
        private readonly BinaryDataParser.Reader Reader;
        private readonly BinaryDataParser.TemplateEngine ViewTemplate;
        private readonly BinaryDataParser.ByteAddress Position;

        public HexViewerWindow(string binaryFilePath)
        {
            InitializeComponent();

            /* init file stream */
            Source = new FileStream(binaryFilePath, FileMode.Open);
            FileLength = Source.Length;

            if (FileLength == 0)
            {
                AddressTextBox.Text = NoData;
                DataTextBox.Text = NoData;
                AsciiTextBox.Text = NoData;

                DataTextBox.Text = "Файл пустой, в файле нет данных";
            }

            /* init position indicator */
            DataScrollBar.Maximum = FileLength;
            DataScrollBar.Minimum = 0;
            DataScrollBar.Value = 0;
            DataScrollBar.SmallChange = BytesPerLine;
            DataScrollBar.LargeChange = NumberOfBytes;
            DataScrollBar.ViewportSize = NumberOfBytes;

            /* init reader and template */
            Reader = new BinaryDataParser.Reader(Source, FileLength);
            ViewTemplate = new BinaryDataParser.TemplateEngine(
                BytesPerLine,
                BinaryDataParser.TemplateEngine.Mode.Hex
                );
            Position = new BinaryDataParser.ByteAddress(BytesPerLine);

            /* initial read forward */
            FileBytes = new byte[NumberOfBytes];
            DataScrollBar.Value = NumberOfBytes;
        }
        private void ShowHexView_Click(object sender, RoutedEventArgs e)
        {
            ViewTemplate.AsHex();
            RenderDisplay();
        }

        private void ShowDecimalView_Click(object sender, RoutedEventArgs e)
        {
            ViewTemplate.AsDecimal();
            RenderDisplay();
        }

        private void ShowBinaryView_Click(object sender, RoutedEventArgs e)
        {
            ViewTemplate.AsBinary();
            RenderDisplay();
        }

        // Метод для подсветки всех найденных значений
        public void HighlightFoundValues(List<int> foundIndices, string searchText)
        {
            if (foundIndices == null || foundIndices.Count == 0 || string.IsNullOrEmpty(searchText))
                return;

            // Устанавливаем фокус и выделяем все найденные значения
            DataTextBox.Focus();
            foreach (int foundIndex in foundIndices)
            {
                if (foundIndex >= 0 && foundIndex < DataTextBox.Text.Length)
                {
                    DataTextBox.Select(foundIndex, searchText.Length);
                }
            }
        }

        // Метод для прокрутки к определенному значению, восстановленный
        public void ScrollToFoundValue(int foundIndex, int length)
        {
            // Устанавливаем фокус на TextBox
            DataTextBox.Focus();

            // Выделяем текст от найденного индекса на длину искомого значения
            DataTextBox.Select(foundIndex, length);

            // Прокручиваем TextBox так, чтобы найденное значение было видно
            DataTextBox.Dispatcher.Invoke(() =>
            {
                Rect foundRect = DataTextBox.GetRectFromCharacterIndex(foundIndex);

                if (foundRect != Rect.Empty) // Проверяем, что Rect действителен
                {
                    // Рассчитываем вертикальное смещение, чтобы текст был в центре
                    double targetOffset = foundRect.Top + DataTextBox.VerticalOffset - (DataTextBox.ViewportHeight / 2);
                    DataTextBox.ScrollToVerticalOffset(Math.Max(0, targetOffset));
                }
            }, System.Windows.Threading.DispatcherPriority.ApplicationIdle);
        }

        // Метод для открытия окна поиска
        private void OpenSearchWindow_Click(object sender, RoutedEventArgs e)
        {
            SearchWindow searchWindow = new(DataTextBox.Text, this)
            {
                Owner = this // Устанавливаем родительское окно
            };
            searchWindow.Show(); // Открываем окно без блокировки основного окна
        }

        private void RenderBytes()
        {
            var view = ViewTemplate.Render(FileBytes, BytesRead, Reader.Position);

            AddressTextBox.Text = view.Address;
            DataTextBox.Text = view.Display;
            AsciiTextBox.Text = view.Ascii;
        }

        private void RenderDisplay()
        {
            var view = ViewTemplate.Render(FileBytes, BytesRead, Reader.Position);

            DataTextBox.Text = view.Display;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Source.Dispose();
        }
        private void DataTextBox_MouseWheel(object sender, System.Windows.Input.MouseWheelEventArgs e)
        {
            var currentPosition = (long)DataScrollBar.Value;

            var moveBackward = e.Delta > 0;
            long newPosition = 0;
            if (moveBackward)
            {
                newPosition = currentPosition - BytesPerLine;
            }
            if (!moveBackward)
            {
                newPosition = currentPosition + BytesPerLine;
            }

            DataScrollBar.Value = newPosition;
        }

        private void DataScrollBar_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            var newPosition = (long)DataScrollBar.Value;
            var alignedPosition = Position.Align(newPosition);

            BytesRead = Reader.ReadAt(alignedPosition, FileBytes);
            RenderBytes();

            PositionTextBlock.Text = Reader.Position.ToString();
        }
    }
}
