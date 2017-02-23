namespace UI.Console
{
    using System;

    public abstract class Stock
    {
        public string StockSymbol { get; set; }
        public decimal LastDividend { get; set; }

        public abstract decimal CalculateDividendYield(decimal price);

        public decimal CalculatePricePerEarningsRatio(decimal price)
        {
            if (LastDividend == 0)
                throw new DivideByZeroException("Last dividend is zero so cannot calculate PE ratio");

            return price / LastDividend;
        }

        public override bool Equals(object obj)
        {
            var stock2 = obj as Stock;

            if (stock2 == null) return false;

            if (StockSymbol == null) return false;

            return StockSymbol.Equals(stock2.StockSymbol);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((StockSymbol != null ? StockSymbol.GetHashCode() : 0) * 397) ^ LastDividend.GetHashCode();
            }
        }
    }
}