namespace Tests.Unit
{
    using System;
    using Moq;
    using NUnit.Framework;
    using UI.Console;

    [TestFixture]
    [Description("Covers requirement 1a.ii")]
    public class StockTests
    {
        [TestCase(5, ExpectedResult = 2.5)]
        [TestCase(0, ExpectedResult = 0)]
        [TestCase(-1, ExpectedResult = -0.5)]
        public decimal CalculatePricePerEarningsRatio_WithPrice_ReturnsExpectedValue(decimal price)
        {
            // Arrange
            var stockMock = new Mock<Stock>();
            stockMock.Object.LastDividend = 2;

            // Act
            var result = stockMock.Object.CalculatePricePerEarningsRatio(price);

            // Assert
            return result;
        }

        [Test]
        public void CalculatePricePerEarningsRatio_WithZeroLastDividend_ThrowsDivideByZeroException()
        {
            // Arrange
            var stockMock = new Mock<Stock>();
            stockMock.Object.LastDividend = 0;

            // Act
            var testDelegate = new TestDelegate(() => stockMock.Object.CalculatePricePerEarningsRatio(12));

            // Assert
            Assert.That(testDelegate, Throws.TypeOf<DivideByZeroException>());
        }

        [Test]
        public void Equals_WithTwoStocksWithSameSymbol_ReturnsTrue()
        {
            // Arrange
            var stock1 = new CommonStock { StockSymbol = "TEA" };
            var stock2 = new CommonStock { StockSymbol = "TEA" };

            // Act
            var areEquals = stock1.Equals(stock2);

            // Assert
            Assert.True(areEquals);
        }

        /// <summary>
        /// Note - assumption made that stock symbol is unique even across different stock types
        /// for the purpose of this exercise.
        /// </summary>
        [Test]
        public void Equals_WithTwoStocksWithSameSymbolOfDifferentSubtypes_ReturnsTrue()
        {
            // Arrange
            var stock1 = new CommonStock { StockSymbol = "TEA" };
            var stock2 = new PreferredStock { StockSymbol = "TEA" };

            // Act
            var areEquals = stock1.Equals(stock2);

            // Assert
            Assert.True(areEquals);
        }

        [Test]
        public void Equals_WithTwoStocksWithDifferentSymbol_ReturnsFalse()
        {
            // Arrange
            var stock1 = new CommonStock { StockSymbol = "TEA" };
            var stock2 = new CommonStock { StockSymbol = "TEB" };

            // Act
            var areEquals = stock1.Equals(stock2);

            // Assert
            Assert.False(areEquals);
        }

        [Test]
        public void Equals_WithOneObjectNotAStock_ReturnsFalse()
        {
            // Arrange
            var stock1 = new CommonStock { StockSymbol = "TEA" };
            var trade = new Trade();

            // Act
            var areEquals = stock1.Equals(trade);

            // Assert
            Assert.False(areEquals);
        }

        [Test]
        public void Equals_IfStockSymbolIsNull_ReturnsFalse()
        {
            // Arrange
            var stock1 = new CommonStock();
            var stock2 = new PreferredStock();

            // Act
            var areEquals = stock1.Equals(stock2);

            // Assert
            Assert.False(areEquals);
        }
    }
}