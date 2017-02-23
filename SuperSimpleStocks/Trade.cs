namespace UI.Console
{
    using System;

    public class Trade
    {
        public Stock Stock { get; set; }
        public decimal Quantity { get; set; }
        public TransactionType TransactionType { get; set; }
        public decimal TradePrice { get; set; }
        public DateTime LocalTimestamp { get; set; }
    }
}