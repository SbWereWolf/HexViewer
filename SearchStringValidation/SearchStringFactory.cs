
using Visualization;

namespace DataSearch
{
    public class SearchStringFactory
    {
        public readonly string Search;

        public string SelectedFormat { get; }

        public SearchStringFactory(
            string search,
            string selectedFormat
            )
        {
            Search = search;
            SelectedFormat = selectedFormat;
        }
        public SearchString Make()
        {
            var search = Search.Replace(" ", "").ToUpper();
            var settings = this.Validate(search, SelectedFormat);

            var digits = this.SplitToDigits(search, settings);
            var pattern = string.Join(" ", digits);

            var needle = this.MakeNeedle(settings, digits);

            return new SearchString(pattern, needle);
        }

        private Settings Validate(string search, string selectedFormat)
        {
            if (string.IsNullOrWhiteSpace(search))
            {
                throw new WhiteSpaceSearchStringException(
                    "Строка поиска пустая"
                    + ", задайте строку поиска"
                    + " - какие байты надо найти ?"
                    );
            }

            // Определяем, в каком формате производить поиск
            var settings = Settings.SetupSettings(selectedFormat);

            var length = search.Length;
            var stringRemainder = length % settings.Format;
            if (stringRemainder != 0)
            {
                throw new InadequateSearchStringException(
                    "Строка поиска не соответствует"
                    + $" выбранному формату поиска ({settings.Name})."
                    + $" Количество цифр {length} в строке поиска"
                    + " должно быть пропорционально количеству"
                    + $" цифр в одном слове - {settings.Format}."
                    );
            }

            return settings;
        }
        private string[] SplitToDigits(string search, Settings settings)
        {
            var digits = Enumerable
                .Range(0, search.Length / settings.Format)
                .Select(
                i => search
                .Substring(i * settings.Format, settings.Format)
                );

            return digits.ToArray<string>();
        }
        private byte[] MakeNeedle(Settings settings, string[] digits)
        {
            var numbers = new List<byte>();
            foreach (var digit in digits)
            {
                var number = Convert.ToByte(digit, settings.Basis);
                numbers.Add(number);
            }

            var needle = numbers.ToArray();

            return needle;
        }
    }

}
