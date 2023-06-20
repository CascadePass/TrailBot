namespace CascadePass.TrailBot
{
    public class Range
    {
        public Range() { }

        public Range(int minimum, int maximum) {
            this.Minimum = minimum;
            this.Maximum = maximum;
        }

        public int Minimum { get; set; }

        public int Maximum { get; set; }

        public int Coalesce(int value)
        {
            if (value < this.Minimum)
            {
                return this.Minimum;
            }

            if (value > this.Maximum)
            {
                return this.Maximum;
            }

            return value;
        }
    }
}
