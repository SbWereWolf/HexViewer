using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Linq;

namespace ProbyteEditClient
{
    public partial class SearchWindow : Window
    {

        // Класс для представления результатов поиска
        private class HexSearchResult
        {
            public HexSearchResult(int index = 0, string address = "", string hexValue = "")
            {
                Index = index;
                Address = address;
                HexValue = hexValue;
            }

            public int Index { get; }
            public string Address { get; }
            public string HexValue { get; }
        }

        private readonly string hexData = "";
        private readonly HexViewerWindow parentWindow;


        public SearchWindow(string hexData, HexViewerWindow parent)
        {
            InitializeComponent();
            this.hexData = hexData;
            this.parentWindow = parent;
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            string searchText = SearchBox.Text;
            if (string.IsNullOrEmpty(searchText))
            {
                MessageBox.Show("Введите значение для поиска.");
                return;
            }

            // Получение выбранного формата поиска
            string? selectedFormat = (SearchFormatComboBox.SelectedItem as ComboBoxItem)?.Content.ToString();

            List<HexSearchResult> searchResults = new();
            int startIndex = 0;
            int foundIndex;
            int count = 1;

            // Определяем, в каком формате производить поиск
            switch (selectedFormat)
            {
                case "Hex":
                    // Поиск в шестнадцатеричном представлении
                    while ((foundIndex = hexData.IndexOf(searchText, startIndex, StringComparison.OrdinalIgnoreCase)) != -1)
                    {
                        searchResults.Add(new HexSearchResult(count++, $"0x{foundIndex:X8}", searchText));

                        // Прокручиваем к найденному значению и выделяем его в основном окне
                        parentWindow.ScrollToFoundValue(foundIndex, searchText.Length);

                        startIndex = foundIndex + searchText.Length;
                    }
                    break;

                case "Decimal":
                    // Форматируем строку для поиска
                    string[] decimalValues = searchText.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                    // Проверяем, что все значения корректные
                    bool allValuesValid = true;
                    foreach (string value in decimalValues)
                    {
                        if (!int.TryParse(value, out int _))
                        {
                            MessageBox.Show($"Введите корректное десятичное значение для поиска: '{value}' некорректно.");
                            allValuesValid = false;
                            break;
                        }
                    }
                    if (!allValuesValid)
                    {
                        return;
                    }

                    // Создаем строку поиска в виде "129 007 140 162" для TextBox
                    string decimalSearchString = string.Join(" ", decimalValues.Select(v => int.Parse(v).ToString("D3")));

                    // Поиск в десятичном представлении
                    startIndex = 0;
                    while ((foundIndex = FindDecimalInTextBox(decimalSearchString, startIndex)) != -1)
                    {
                        searchResults.Add(new HexSearchResult(count++, $"0x{foundIndex:X8}", decimalSearchString));

                        // Прокручиваем к найденному значению и выделяем его в основном окне
                        parentWindow.ScrollToFoundValue(foundIndex, decimalSearchString.Length);

                        startIndex = foundIndex + decimalSearchString.Length;
                    }

                    break;

                case "Binary":
                    // Разделяем строку поиска на отдельные бинарные значения
                    string[] binaryValues = searchText.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                    // Проверяем, что все значения корректные (только 0 и 1)
                    if (binaryValues.All(value => System.Text.RegularExpressions.Regex.IsMatch(value, "^[01]{8}$")))
                    {
                        // Создаем строку поиска в виде "01001000 01100101 01101100" для TextBox
                        string binarySearchString = string.Join(" ", binaryValues.Select(v => v.PadLeft(8, '0')));

                        // Поиск в бинарном представлении
                        startIndex = 0;
                        while ((foundIndex = FindBinaryInTextBox(binarySearchString, startIndex)) != -1)
                        {
                            searchResults.Add(new HexSearchResult(count++, $"0x{foundIndex:X8}", binarySearchString));

                            // Прокручиваем к найденному значению и выделяем его в основном окне
                            parentWindow.ScrollToFoundValue(foundIndex, binarySearchString.Length);

                            startIndex = foundIndex + binarySearchString.Length;
                        }
                    }
                    else
                    {
                        MessageBox.Show("Введите корректное бинарное значение для поиска (8 бит, только 0 и 1, например '01100101').");
                        return;
                    }
                    break;

                default:
                    MessageBox.Show("Выберите формат поиска.");
                    return;
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

        // Метод для поиска бинарных значений в TextBox
        private int FindBinaryInTextBox(string binaryString, int startIndex)
        {
            // Поиск точного соответствия бинарной строки в отображаемом тексте
            string text = parentWindow.HexTextBox.Text;
            return text.IndexOf(binaryString, startIndex, StringComparison.Ordinal);
        }

        // Метод для поиска десятичных значений в TextBox
        private int FindDecimalInTextBox(string decimalString, int startIndex)
        {
            // Поиск точного соответствия трехзначному числу в отображаемом тексте
            string text = parentWindow.HexTextBox.Text;
            return text.IndexOf(decimalString, startIndex, StringComparison.Ordinal);
        }

        private void SearchResultsDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (SearchResultsDataGrid.SelectedItem is HexSearchResult selectedResult)
            {
                // Извлекаем индекс и прокручиваем основное окно к найденному значению
                if (int.TryParse(selectedResult.Address.Substring(2), System.Globalization.NumberStyles.HexNumber, null, out int selectedIndex))
                {
                    parentWindow.ScrollToFoundValue(selectedIndex, SearchBox.Text.Length);
                }
            }
        }

        // Метод для закрытия окна при нажатии на кнопку "Закрыть"
        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close(); // Закрывает текущее окно поиска
        }
    }
}
