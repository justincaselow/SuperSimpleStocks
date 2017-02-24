namespace UI.Console
{
    using System;
    using System.Linq;

    public class TradeCalculator
    {
        private readonly ITradeRepository _tradeRepository;
        private readonly IDateTimeProvider _dateTimeProvider;

        public TradeCalculator(ITradeRepository tradeRepository, IDateTimeProvider dateTimeProvider)
        {
            tradeRepository.CheckForNull("tradeRepository");
            dateTimeProvider.CheckForNull("dateTimeProvider");

            _tradeRepository = tradeRepository;
            _dateTimeProvider = dateTimeProvider;
        }

        public decimal CalculateVolumeWeightedStockPrice(Stock stock)
        {
            stock.CheckForNull("stock");

            var stockTrades = _tradeRepository.GetTrades(stock);

            if (stockTrades == null || !stockTrades.Any())
                return 0;

            var fifteenMinutesAgo = _dateTimeProvider.Now.AddMinutes(-15);

            var tradesWithinFifteenMinutes = stockTrades.Where(x => x.LocalTimestamp > fifteenMinutesAgo).ToList();

            var vwap = tradesWithinFifteenMinutes.Sum(x => x.Quantity * x.TradePrice) /
                       tradesWithinFifteenMinutes.Sum(x => x.Quantity);

            return vwap;
        }

        /*
         * Please note my interpretation of how to calculate the "GBCE All Share Index"
         * is to use the prices from all the trades given the sample data in Table 1 didn't appear to
         * suggest that stocks had a "Price" property
         */
        public double CalculateGbceAllShareIndex()
        {
            var allTrades = _tradeRepository.GetTrades();

            if (allTrades == null || !allTrades.Any())
                return 0;

            var allPrices = allTrades.Select(x => x.TradePrice);

            var allPricesProduct = (double)allPrices.Aggregate((p1, p2) => p1 * p2);

            var result = Math.Pow(allPricesProduct, 1.0 / allTrades.Count);

            return result;
        }
    }
}