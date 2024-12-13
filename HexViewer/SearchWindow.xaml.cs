using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Linq;
using BinaryDataParser;
using System.IO;
using static BinaryDataParser.TemplateEngine;

namespace ProbyteEditClient
{
    public partial class SearchWindow : Window
    {

        // Класс для представления результатов поиска
        private class SearchResult
        {
            public SearchResult(long index = 0, long address = 0, string patern = "")
            {
                Index = index;
                Address = address;
                Needle = patern;
            }

            public long Index { get; }
            public long Address { get; }
            public string Needle { get; }
        }

        private readonly HexViewerWindow parentWindow;
        private readonly FileStream Source;

        public SearchWindow(HexViewerWindow parent, FileStream source)
        {
            InitializeComponent();

            this.parentWindow = parent;
            Source = source;
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            string searchText = SearchBox.Text;
            searchText = searchText.Replace(" ", string.Empty);
            if (string.IsNullOrEmpty(searchText))
            {
                MessageBox.Show("Введите значение для поиска.");
                return;
            }

            // Получение выбранного формата поиска
            string? selectedFormat = (SearchFormatComboBox.SelectedItem as ComboBoxItem)?.Content.ToString();

            // Определяем, в каком формате производить поиск
            Settings settings;
            switch (selectedFormat)
            {
                case "Hex":
                    settings = Settings.AsHex();
                    break;

                case "Decimal":
                    settings = Settings.AsDecamal();
                    break;

                case "Binary":
                    settings = Settings.AsBinary();
                    break;

                default:
                    MessageBox.Show("Выберите формат поиска.");
                    return;
            }

            int length = searchText.Length;
            var stringRemainder = length % settings.Format;
            if (stringRemainder != 0)
            {
                MessageBox.Show($"Строка не соответствует выбранному формату поиска. Количество цифр {length} должно быть пропорционально {settings.Format}.");
                return;
            }

            SearchResultsDataGrid.ItemsSource = new List<SearchResult>();

            var digits = Enumerable
                .Range(0, length / settings.Format)
                .Select(
                i => searchText
                .Substring(i * settings.Format, settings.Format)
                );

            var pattern = string.Join(" ", digits);
            var numbers = new List<byte>();
            foreach (var digit in digits)
            {
                var number = Convert.ToByte(digit, settings.Basis);
                numbers.Add(number);
            }

            List<SearchResult> searchResults = new();
            long foundIndex = 0;
            long count = 1;

            var needle = numbers.ToArray();

            Source.Seek(0, SeekOrigin.Begin);

            var needleLength = needle.Length;
            var haystack = new byte[2 * needleLength];

            var haystackSize = haystack.Length;

            var validBytes = Source.Read(haystack);

            while (validBytes >= needleLength)
            {
                var limit = validBytes - needleLength;
                var i = 0;
                for (i = 0; i <= limit; i++)
                {
                    var isFound = FindBytes(haystack, i, needle);
                    if (isFound)
                    {
                        foundIndex = Source.Position - validBytes + i;
                        searchResults.Add(new SearchResult(count++, foundIndex, pattern));

                        i += needleLength - 1;
                    }
                }

                var bytesRemainder = validBytes - i;
                Array.Copy(haystack, i, haystack, 0, bytesRemainder);

                var portion = new byte[2* needleLength- bytesRemainder];
                var bytesRead = Source.Read(portion);
                Array.Copy(portion, 0, haystack, bytesRemainder, bytesRead);

                validBytes = bytesRemainder + bytesRead;
            }

            if (searchResults.Count != 0)
            {
                SearchResultsDataGrid.ItemsSource = searchResults;
            }
            if (searchResults.Count == 0)
            {
                MessageBox.Show("Значение не найдено.");
            }
        }

        private static bool FindBytes(byte[] haystack, int i, byte[] needle)
        {
            var needleLength = needle.Length;
            var example = new byte[needleLength];
            Array.Copy(haystack, i, example, 0, needleLength);
            var isFound = needle.SequenceEqual(example);

            return isFound;
        }

        private void SearchResultsDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (SearchResultsDataGrid.SelectedItem is SearchResult selectedResult)
            {
                parentWindow.ScrollToFoundValue(selectedResult.Address);
                parentWindow.SelectFoundValue(selectedResult.Needle);
            }
        }

        // Метод для закрытия окна при нажатии на кнопку "Закрыть"
        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close(); // Закрывает текущее окно поиска
        }

        private void SearchResultsDataGrid_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (SearchResultsDataGrid.SelectedItem is SearchResult selectedResult)
            {
                parentWindow.ScrollToFoundValue(selectedResult.Address, selectedResult.DataView);
            }
        }
    }
}
