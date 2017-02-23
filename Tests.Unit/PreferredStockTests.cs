namespace Tests.Unit
{
    using NUnit.Framework;
    using UI.Console;

    [TestFixture]
    [Description("Covers requirement 1a.i")]
    public class PreferredStockTests
    {
        [TestCase(2, 100, ExpectedResult = 25)]
        [TestCase(-4, 180, ExpectedResult = -22.5)]
        public decimal CalculateDividendYield_WithNonZeroPriceAndPositiveDividend_CorrectlyCalculates(decimal price, decimal parValue)
        {
            // Arrange
            var stock = new PreferredStock
            {
                FixedDividend = 0.5M,
                ParValue = parValue
            };

            // Act
            var result = stock.CalculateDividendYield(price);

            // Assert
            return result;
        }

        [Test]
        public void CalculateDividendYield_WithZeroPrice_ThrowsArgumentException()
        {
            // Arrange
            var stock = new PreferredStock
            {
                FixedDividend = 0.5M,
                ParValue = 100
            };

            // Act
            var testDelegate = new TestDelegate(() => stock.CalculateDividendYield(0));

            // Assert
            Assert.That(testDelegate, Throws.ArgumentException);
        }
    }
}