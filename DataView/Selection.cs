
namespace DataView
{
    public class Selection
    {
        public readonly int Start;
        public readonly int Finish;

        public Selection(int start, int finish)
        {
            this.Start = start;
            this.Finish = finish;
        }
    }
}
