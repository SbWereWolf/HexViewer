namespace Visualization
{
    public class SettingsFactory
    {
        private const int HexFormat = 2;
        private const string HexPlaceholder = "   ";
        private const int HexBase = 16;
        private const int DecimalFormat = 3;
        private const string DecimalPlaceholder = "    ";
        private const int DecimalBase = 10;
        private const int BinaryFormat = 8;
        private const string BinaryPlaceholder = "         ";
        private const int BinaryBase = 2;

        public static Settings ChooseSuitableSettings(
            string? selectedFormat
            )
        {
            Settings settings = selectedFormat switch
            {
                "Hex" => AsHexadecimal(),
                "Decimal" => AsDecimal(),
                "Binary" => AsBinary(),
                _ => throw new UnknownFormatException(
                    $" The value `{selectedFormat}`"
                    + " is unknown format."
                    + " Valid values is Hex,Decimal,Binary"
                    ),
            };
            return settings;
        }
        public static Settings PickUpSettings(Mode mode)
        {
            Settings? settings = null;
            switch (mode)
            {
                case Mode.Binary:
                    settings = AsBinary();
                    break;
                case Mode.Decimal:
                    settings = AsDecimal();
                    break;
                case Mode.Hexadecimal:
                    settings = AsHexadecimal();
                    break;
            }

            if (settings == null)
            {
                throw new UnknownModeException(
                    $"The view mode `{mode}` is undefined."
                    + $" View mode is MUS one of {Mode.Binary},"
                    + $" {Mode.Decimal}, {Mode.Hexadecimal}");
            }

#pragma warning disable CS8603 // Possible null reference return.
            return settings;
#pragma warning restore CS8603 // Possible null reference return.
        }

        private static Settings AsHexadecimal()
        {
            return new Settings(
                HexBase,
                HexFormat,
                HexPlaceholder,
                "Hexadecimal"
                );
        }

        private static Settings AsDecimal()
        {
            return new Settings(
                DecimalBase,
                DecimalFormat,
                DecimalPlaceholder,
                "Decimal"
                );
        }

        private static Settings AsBinary()
        {
            return new Settings(
                BinaryBase,
                BinaryFormat,
                BinaryPlaceholder,
                "Binary"
                );
        }
    }
}
