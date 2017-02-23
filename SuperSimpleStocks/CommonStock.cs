namespace UI.Console
{
    using System;

    public class CommonStock : Stock
    {
        public override decimal CalculateDividendYield(decimal price)
        {
            if (price == 0)
                throw new ArgumentException("Price cannot be zero");

            return LastDividend / price;
        }
    }
}