using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CascadePass.TrailBot.Tests
{
    [TestClass]
    public class RangeTests
    {
        [TestMethod]
        public void CanCreateRange()
        {
            _ = new Range();
        }

        [TestMethod]
        public void CanCreateRangeWithParameters()
        {
            _ = new Range(1, 2);
        }

        [TestMethod]
        public void ConstructorSetsMinimumCorrectly()
        {
            Range range = new(int.MinValue, int.MaxValue);
            Assert.AreEqual(int.MinValue, range.Minimum);
        }

        [TestMethod]
        public void ConstructorSetsMaximumCorrectly()
        {
            Range range = new(int.MinValue, int.MaxValue);
            Assert.AreEqual(int.MaxValue, range.Maximum);
        }

        #region get/set access same value

        [TestMethod]
        public void Minimum()
        {
            Range range = new(1, 2);

            range.Minimum = 3;
            Assert.AreEqual(3, range.Minimum);
        }

        [TestMethod]
        public void Maximum()
        {
            Range range = new(1, 2);

            range.Maximum = 3;
            Assert.AreEqual(3, range.Maximum);
        }

        #endregion

        #region Coalesce

        [TestMethod]
        public void Coalesce_TooSmall()
        {
            Range range = new Range(10, 20);

            int result = range.Coalesce(5);

            Assert.AreEqual(range.Minimum, result);
        }

        [TestMethod]
        public void Coalesce_TooLarge()
        {
            Range range = new Range(10, 20);

            int result = range.Coalesce(50);

            Assert.AreEqual(range.Maximum, result);
        }

        [TestMethod]
        public void Coalesce_WithinRange()
        {
            Range range = new Range(10, 20);

            int result = range.Coalesce(15);

            Assert.AreEqual(15, result);
        }

        #endregion
    }
}
