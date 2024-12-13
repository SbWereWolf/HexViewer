
using System.IO;
using DataView;
using System.Windows;
using Visualization;

namespace ProbyteEditClient
{
    public partial class HexViewerWindow : Window
    {
        private static readonly int BytesPerLine = 16;
        private static readonly int NumberOfLines = 16 * 2;
        private static readonly int NumberOfBytes = BytesPerLine * NumberOfLines;
        private readonly byte[] FileBytes;
        private int BytesRead;
        private readonly FileStream Source;
        private readonly long FileLength;
        private const string NoData = "Нет данных для отображения.";
        private readonly Reader Reader;
        private readonly TemplateEngine ViewTemplate;
        private readonly ByteAddress Position;
        Mode ViewMode = Mode.Hexadecimal;

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
            DataScrollBar.SmallChange = BytesPerLine;
            DataScrollBar.LargeChange = NumberOfBytes;
            DataScrollBar.ViewportSize = NumberOfBytes;

            /* init reader and template */
            Reader = new Reader(Source, FileLength);
            ViewTemplate = new TemplateEngine(
                BytesPerLine
                );
            Position = new ByteAddress(BytesPerLine);

            /* initial read forward */
            FileBytes = new byte[NumberOfBytes];
            DataScrollBar.Value = NumberOfBytes;
        }
        private void ShowHexView_Click(
            object sender,
            RoutedEventArgs e
            )
        {
            ViewMode = Mode.Hexadecimal;
            RenderDisplay();
        }
        private void ShowDecimalView_Click(
            object sender,
            RoutedEventArgs e
            )
        {
            ViewMode = Mode.Decimal;
            RenderDisplay();
        }
        private void ShowBinaryView_Click(
            object sender,
            RoutedEventArgs e
            )
        {
            ViewMode = Mode.Binary;
            RenderDisplay();
        }
        private void RenderBytes()
        {
            var view = ViewTemplate.Render(
                FileBytes,
                BytesRead,
                Reader.Position,
                ViewMode
                );

            AddressTextBox.Text = view.Address;
            DataTextBox.Text = view.Display;
            AsciiTextBox.Text = view.Ascii;
        }
        private void RenderDisplay()
        {
            var view = ViewTemplate.Render(
                FileBytes,
                BytesRead,
                Reader.Position,
                ViewMode
                );

            DataTextBox.Text = view.Display;
        }
        private void Window_Closing(
            object sender,
            System.ComponentModel.CancelEventArgs e
            )
        {
            Source.Dispose();
        }
        private void DataTextBox_MouseWheel(
            object sender,
            System.Windows.Input.MouseWheelEventArgs e
            )
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
        private void DataScrollBar_ValueChanged(
            object sender,
            RoutedPropertyChangedEventArgs<double> e
            )
        {
            var newPosition = (long)DataScrollBar.Value;
            var alignedPosition = Position.Align(newPosition);

            BytesRead = Reader.ReadAt(alignedPosition, FileBytes);
            RenderBytes();

            PositionTextBlock.Text = Reader.Position.ToString();
        }
        public void ScrollToFoundValue(long foundIndex, string pattern)
        {
            var position = foundIndex
                + BytesPerLine
                + NumberOfBytes / 2;
            if (position < 0)
            {
                position = 0;
            }
            DataScrollBar.Value = position;
            this.Activate();
            DataTextBox.Focus();
            var index = DataTextBox.Text.IndexOf(pattern, 0);
            if (index != -1)
            {
                DataTextBox.Select(index, pattern.Length);
            }
        }
        // Метод для открытия окна поиска
        private void OpenSearchWindow_Click(
            object sender,
            RoutedEventArgs e
            )
        {
            // Устанавливаем родительское окно
            SearchWindow searchWindow = new(this, Source)
            {
                Owner = this
            };
            // Открываем окно без блокировки основного окна
            searchWindow.Show();
        }
    }
}
