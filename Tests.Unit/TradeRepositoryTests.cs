namespace Tests.Unit
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Moq;
    using Moq.Protected;
    using NUnit.Framework;
    using UI.Console;

    [TestFixture]
    [Description("Covers requirement 1a.iii")]
    public class TradeRepositoryTests
    {
        [Test]
        public void AddTrade_WithDetails_SuccessfullyAddsTradeToList()
        {
            // Arrange
            var dateTimeProvider = new Mock<IDateTimeProvider>();
            var localTimestamp = new DateTime(2017, 2, 17, 22, 14, 44);

            dateTimeProvider.Setup(x => x.Now).Returns(localTimestamp);
            var tradeRepository = new TradeRepository(dateTimeProvider.Object);
            var teaStock = new CommonStock { StockSymbol = "TEA" };

            // Act
            var trade = tradeRepository.AddTrade(teaStock, 100, TransactionType.Buy, 1.23M);

            // Assert
            Assert.AreEqual(1, tradeRepository.TradeCount);
            Assert.AreEqual(100, trade.Quantity);
            Assert.AreEqual(TransactionType.Buy, trade.TransactionType);
            Assert.AreEqual(1.23, trade.TradePrice);
            Assert.AreEqual(localTimestamp, trade.LocalTimestamp);
            Assert.AreEqual(teaStock, trade.Stock);
        }

        [Test]
        public void GetTrades_ForACertainStock_ReturnsTheCorrectTradesForThatStock()
        {
            // Arrange
            var dateTimeProvider = new Mock<IDateTimeProvider>();
            var tradeRepository = new Mock<TradeRepository>(dateTimeProvider.Object);
            var teaStock = new CommonStock { StockSymbol = "TEA" };
            var popStock = new CommonStock { StockSymbol = "POP" };
            var aleStock = new CommonStock { StockSymbol = "ALE" };

            tradeRepository.Protected().Setup<List<Trade>>("Trades").Returns(
                new List<Trade>
                {
                    new Trade { Stock = teaStock, Quantity = 1 },
                    new Trade { Stock = popStock, Quantity = 2 },
                    new Trade { Stock = aleStock, Quantity = 3 },
                    new Trade { Stock = aleStock, Quantity = 4 },
                    new Trade { Stock = teaStock, Quantity = 5 },
                    new Trade { Stock = teaStock, Quantity = 6 },
                    new Trade { Stock = aleStock, Quantity = 7 }
                });

            // Act
            var trades = tradeRepository.Object.GetTrades(new CommonStock { StockSymbol = "TEA" });

            // Assert
            Assert.AreEqual(3, trades.Count);
            var quantities = trades.Select(x => x.Quantity).ToList();
            Assert.That(quantities, Contains.Item(1));
            Assert.That(quantities, Contains.Item(5));
            Assert.That(quantities, Contains.Item(6));
        }
    }
}