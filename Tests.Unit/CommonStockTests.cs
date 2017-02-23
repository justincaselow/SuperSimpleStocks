namespace Tests.Unit
{
    using NUnit.Framework;
    using UI.Console;

    [TestFixture]
    [Description("Covers requirement 1a.i")]
    public class CommonStockTests
    {
        [TestCase(2, ExpectedResult = 5)]
        [TestCase(-4, ExpectedResult = -2.5)]
        public decimal CalculateDividendYield_WithNonZeroPriceAndPositiveDividend_CorrectlyCalculates(decimal price)
        {
            // Arrange
            var stock = new CommonStock
            {
                LastDividend = 10
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
            var stock = new CommonStock
            {
                LastDividend = 20
            };

            // Act
            var testDelegate = new TestDelegate(() => stock.CalculateDividendYield(0));

            // Assert
            Assert.That(testDelegate, Throws.ArgumentException);
        }
    }
}