
namespace DataView
{
    public class View
    {
        public readonly string Address ;
        public readonly string Display ;
        public readonly string Ascii;

        public View(string address, string display, string ascii)
        {
            Address = address;
            Display = display;
            Ascii = ascii;
        }
    }
}
