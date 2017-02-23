namespace Tests.Unit
{
    using System;
    using System.Collections.Generic;
    using Moq;
    using NUnit.Framework;
    using UI.Console;

    [TestFixture]
    [Description("Covers requirements 1a.iv and 1b")]
    public class TradeCalculatorTests
    {
        private Mock<ITradeRepository> tradeRepositoryMock;
        private TradeCalculator tradeCalculator;
        private CommonStock teaStock;
        private CommonStock ginStock;
        private CommonStock aleStock;

        [SetUp]
        public void Setup()
        {
            tradeRepositoryMock = new Mock<ITradeRepository>();
            var now = new DateTime(2017, 2, 17, 22, 22, 34);
            var dateTimeProvider = new Mock<IDateTimeProvider>();
            dateTimeProvider.Setup(x => x.Now).Returns(now);

            tradeCalculator = new TradeCalculator(tradeRepositoryMock.Object, dateTimeProvider.Object);
            teaStock = new CommonStock { StockSymbol = "TEA" };
            ginStock = new CommonStock { StockSymbol = "GIN" };
            aleStock = new CommonStock { StockSymbol = "ALE" };
        }

        [Test]
        public void CalculateVolumeWeightedStockPrice_WithSomeTradesBefore15Minutes_ReturnsExpectedValue()
        {
            // Arrange
            tradeRepositoryMock.Setup(x => x.GetTrades(It.IsAny<Stock>())).Returns(new List<Trade>
            {
                // The following should be included within the last 15 minutes
                new Trade { Stock = teaStock, LocalTimestamp = new DateTime(2017, 2, 17, 22, 22, 34), TradePrice = 5,  Quantity = 200 }, // TP * Q = 1000
                new Trade { Stock = teaStock, LocalTimestamp = new DateTime(2017, 2, 17, 22, 22, 33), TradePrice = 10, Quantity = 200 }, // TP * Q = 2000
                new Trade { Stock = teaStock, LocalTimestamp = new DateTime(2017, 2, 17, 22, 21, 00), TradePrice = 20, Quantity = 100 }, // TP * Q = 2000
                new Trade { Stock = teaStock, LocalTimestamp = new DateTime(2017, 2, 17, 22, 20, 00), TradePrice = 10, Quantity = 50  }, // TP * Q = 500
                new Trade { Stock = teaStock, LocalTimestamp = new DateTime(2017, 2, 17, 22, 15, 00), TradePrice = 20, Quantity = 300 }, // TP * Q = 6000
                new Trade { Stock = teaStock, LocalTimestamp = new DateTime(2017, 2, 17, 22, 07, 35), TradePrice = 10, Quantity = 100 }, // TP * Q = 1000
                // The following should NOT be included as they are outside of 15 minutes
                new Trade { Stock = teaStock, LocalTimestamp = new DateTime(2017, 2, 17, 22, 07, 34), TradePrice = 15, Quantity = 400 },
                new Trade { Stock = teaStock, LocalTimestamp = new DateTime(2017, 2, 17, 22, 07, 33), TradePrice = 10, Quantity = 800 },
                new Trade { Stock = teaStock, LocalTimestamp = new DateTime(2017, 2, 16, 22, 08, 33), TradePrice = 10, Quantity = 50 },
            });

            // Act
            var result = tradeCalculator.CalculateVolumeWeightedStockPrice(teaStock);

            // Assert (12500 / 950 = 13.1579)
            Assert.That(result, Is.EqualTo(13.1579).Within(0.0001));
        }

        [Test]
        public void CalculateVolumeWeightedStockPrice_WithNullStock_ThrowsArgumentNullException()
        {
            // Arrange
            tradeRepositoryMock.Setup(x => x.GetTrades(It.IsAny<Stock>())).Returns(new List<Trade>());

            // Act
            var testDelegate = new TestDelegate(() => tradeCalculator.CalculateVolumeWeightedStockPrice(null));

            // Assert
            Assert.That(testDelegate, Throws.ArgumentNullException);
        }

        [Test]
        public void CalculateVolumeWeightedStockPrice_WithNullTrades_ReturnsZero()
        {
            // Arrange
            tradeRepositoryMock.Setup(x => x.GetTrades(It.IsAny<Stock>())).Returns((List<Trade>)null);

            // Act
            var result = tradeCalculator.CalculateVolumeWeightedStockPrice(teaStock);

            // Assert
            Assert.AreEqual(0, result);
        }

        [Test]
        public void CalculateVolumeWeightedStockPrice_WithNoTrades_ReturnsZero()
        {
            // Arrange
            tradeRepositoryMock.Setup(x => x.GetTrades(It.IsAny<Stock>())).Returns(new List<Trade>());

            // Act
            var result = tradeCalculator.CalculateVolumeWeightedStockPrice(teaStock);

            // Assert
            Assert.AreEqual(0, result);
        }

        [Test]
        public void CalculateGbceAllShareIndex_WithSomePrices_ReturnsExpectedValue()
        {
            // Arrange
            tradeRepositoryMock.Setup(x => x.GetTrades()).Returns(new List<Trade>
            {
                new Trade { Stock = teaStock, TradePrice = 5,  Quantity = 200 },
                new Trade { Stock = teaStock, TradePrice = 2, Quantity = 200 },
                new Trade { Stock = ginStock, TradePrice = 15, Quantity = 100 },
                new Trade { Stock = ginStock, TradePrice = 4, Quantity = 100 },
                new Trade { Stock = ginStock, TradePrice = 5, Quantity = 400 },
                new Trade { Stock = aleStock, TradePrice = 8, Quantity = 50 },
            });

            // Act
            var result = tradeCalculator.CalculateGbceAllShareIndex();

            // Assert (5 * 2 * 15 * 4 * 5 * 8) = 24,000
            Assert.That(result, Is.EqualTo(5.3708).Within(0.0001));
        }

        [Test]
        public void CalculateGbceAllShareIndex_WithNullTrades_ReturnsZero()
        {
            // Arrange
            tradeRepositoryMock.Setup(x => x.GetTrades()).Returns((List<Trade>)null);

            // Act
            var result = tradeCalculator.CalculateGbceAllShareIndex();

            // Assert
            Assert.AreEqual(0, result);
        }

        [Test]
        public void CalculateGbceAllShareIndex_WithNoTrades_ReturnsZero()
        {
            // Arrange
            tradeRepositoryMock.Setup(x => x.GetTrades()).Returns(new List<Trade>());

            // Act
            var result = tradeCalculator.CalculateGbceAllShareIndex();

            // Assert
            Assert.AreEqual(0, result);
        }
    }
}