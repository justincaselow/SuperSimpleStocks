namespace UI.Console
{
    using System;

    public class PreferredStock : Stock
    {
        public decimal FixedDividend { get; set; }
        public decimal ParValue { get; set; }

        public override decimal CalculateDividendYield(decimal price)
        {
            if (price == 0)
                throw new ArgumentException("Price cannot be zero");

            return (FixedDividend * ParValue) / price;
        }
    }
}