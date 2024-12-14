using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using Visualization;
using System.IO;
using DataSearch;

namespace ProbyteEditClient
{
    public partial class SearchWindow : Window
    {
        private class SearchResult
        {
            public SearchResult(
                long index = 0,
                long address = 0,
                string bytesView = ""
                )
            {
                Index = index;
                Address = address;
                AddressView = string.Format(
                    Settings.AddressFormat,
                    address
                    );
                BytesView = bytesView;
            }

            public long Index { get; }
            public long Address { get; }
            public string AddressView { get; }
            public string BytesView { get; }
        }

        private readonly HexViewerWindow parentWindow;
        private readonly SearchEngine searchEngine;

        public SearchWindow(HexViewerWindow parent, FileStream source)
        {
            InitializeComponent();

            this.parentWindow = parent;
            searchEngine = new SearchEngine(source);
        }

        private void SearchButton_Click(
            object sender,
            RoutedEventArgs e
            )
        {
            string searchText = SearchBox.Text;
            string selectedFormat =
                (SearchFormatComboBox.SelectedItem as ComboBoxItem)?
                .Content.ToString() ?? string.Empty;

            SearchString? search = null;
            try
            {
                var fastory = new SearchStringFactory(
                    searchText,
                    selectedFormat
                    );
                search = fastory.Make();
            }
            catch (Exception ex)
            {
                if (
                    ex is WhiteSpaceSearchStringException
                    || ex is InadequateSearchStringException
                    || ex is UnknownFormatException
                    )
                {
                    MessageBox.Show(ex.Message);
                    return;
                }

                throw new RethrowException("", ex);
            }

            if (search == null)
            {
                return;
            }

            var foundAddresses =
                searchEngine.FindAllOccurrences(search.Needle);

            SearchResultsDataGrid.ItemsSource =
                MakeSearchResult(foundAddresses, search.Pattern);
        }

        private List<SearchResult> MakeSearchResult(
            long[] found,
            string pattern
            )
        {
            List<SearchResult> searchResults = new();
            long count = 1;
            foreach (var address in found)
            {
                searchResults
                    .Add(new SearchResult(count++, address, pattern));
            }

            if (searchResults.Count == 0)
            {
                MessageBox.Show("Значение не найдено.");
            }

            return searchResults;
        }
        private void CloseButton_Click(
            object sender,
            RoutedEventArgs e
            )
        {
            this.Close();
        }
        private void SearchResultsDataGrid_MouseDoubleClick(
            object sender,
            System.Windows.Input.MouseButtonEventArgs e
            )
        {
            if (SearchResultsDataGrid.SelectedItem is
                SearchResult selectedResult)
            {
                parentWindow.ScrollToFoundValue(
                    selectedResult.Address,
                    selectedResult.BytesView
                    );
            }
        }
    }
}
