namespace Visualization
{
    public class Settings
    {
        public readonly int Basis;
        public readonly int Format;
        public readonly string Placeholder;
        public readonly string Name;
        public const string AddressFormat = "0x{0:X8}";

        public Settings(
            int basis,
            int format,
            string placeholder,
            string name
            )
        {
            Basis = basis;
            Format = format;
            Placeholder = placeholder;
            Name = name;

        }
    }
}
