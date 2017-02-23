namespace UI.Console
{
    using System.Collections.Generic;
    using System.Linq;

    public interface ITradeRepository
    {
        IReadOnlyList<Trade> GetTrades(Stock stock);
        IReadOnlyList<Trade> GetTrades();
    }

    public class TradeRepository : ITradeRepository
    {
        public IDateTimeProvider DateTimeProvider { get; private set; }
        protected virtual List<Trade> Trades { get; set; }

        public TradeRepository(IDateTimeProvider dateTimeProvider)
        {
            dateTimeProvider.CheckForNull("dateTimeProvider");

            DateTimeProvider = dateTimeProvider;
            Trades = new List<Trade>();
        }

        public double TradeCount
        {
            get { return Trades.Count; }
        }

        public Trade AddTrade(Stock stock, int quantity, TransactionType transactionType, decimal tradePrice)
        {
            var trade = new Trade
            {
                Stock = stock,
                Quantity = quantity,
                TransactionType = transactionType,
                TradePrice = tradePrice,
                LocalTimestamp = DateTimeProvider.Now
            };

            Trades.Add(trade);

            return trade;
        }

        public IReadOnlyList<Trade> GetTrades()
        {
            return Trades.AsReadOnly();
        }

        public IReadOnlyList<Trade> GetTrades(Stock stock)
        {
            return Trades.Where(x => x.Stock.Equals(stock)).ToList().AsReadOnly();
        }
    }
}