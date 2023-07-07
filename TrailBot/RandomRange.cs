namespace CascadePass.TrailBot
{
    public class RandomRange
    {
        private int min, max, minAllowed, maxAllowed;

        #region Constructor

        public RandomRange()
        {
            this.LowerLimit = int.MinValue;
            this.UpperLimit = int.MaxValue;
        }

        public RandomRange(int minimum, int maximum)
        {
            this.LowerLimit = int.MinValue;
            this.UpperLimit = int.MaxValue;
            this.Minimum = minimum;
            this.Maximum = maximum;
        }

        public RandomRange(int minimumAllowed, int minimum, int maximum, int maximumAllowed)
        {
            this.LowerLimit = minimumAllowed;
            this.UpperLimit = maximumAllowed;
            this.Minimum = minimum;
            this.Maximum = maximum;
        }

        #endregion

        public int Minimum
        {
            get => this.min;
            set
            {
                this.min = value;
                this.EnforceMinMax();
            }
        }

        public int Maximum
        {
            get => this.max;
            set
            {
                this.max = value;
                this.EnforceMinMax();
            }
        }

        public int LowerLimit
        {
            get => this.minAllowed;
            set
            {
                this.minAllowed = value;
                this.EnforceMinMax();
            }
        }

        public int UpperLimit
        {
            get => this.maxAllowed;
            set
            {
                this.maxAllowed = value;
                this.EnforceMinMax();
            }
        }

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

        private void EnforceMinMax()
        {
            if (this.min < this.minAllowed)
            {
                this.min = this.minAllowed;
            }

            if (this.max < this.minAllowed)
            {
                this.max = this.minAllowed;
            }

            if (this.min > this.maxAllowed)
            {
                this.min = this.maxAllowed;
            }

            if (this.max > this.maxAllowed)
            {
                this.max = this.maxAllowed;
            }

        }
    }
}
