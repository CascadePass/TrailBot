using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace CascadePass.TrailBot.Tests
{
    [TestClass]
    public class RandomRangeTests
    {
        #region Constructor tests

        [TestMethod]
        public void CanCreateRange()
        {
            _ = new RandomRange();
        }

        [TestMethod]
        public void CanCreateRangeWith2Parameters()
        {
            _ = new RandomRange(1, 2);
        }

        [TestMethod]
        public void CanCreateRangeWith4Parameters()
        {
            _ = new RandomRange(100, 3, 4, 200);
        }

        [TestMethod]
        public void Cctor2ParamsGoToRightProperties()
        {
            RandomRange range = new(int.MinValue, int.MaxValue);

            Assert.AreEqual(int.MinValue, range.Minimum);
            Assert.AreEqual(int.MaxValue, range.Maximum);
        }

        [TestMethod]
        public void Cctor4ParamsGoToRightProperties()
        {
            RandomRange range = new(int.MinValue, 100, 200, int.MaxValue);

            Assert.AreEqual(int.MinValue, range.LowerLimit);
            Assert.AreEqual(int.MaxValue, range.UpperLimit);
            Assert.AreEqual(100, range.Minimum);
            Assert.AreEqual(200, range.Maximum);
        }

        [TestMethod]
        public void ConstructorSetsMinimumCorrectly()
        {
            RandomRange range = new(int.MinValue, int.MaxValue);
            Assert.AreEqual(int.MinValue, range.Minimum);
        }

        [TestMethod]
        public void ConstructorSetsMaximumCorrectly()
        {
            RandomRange range = new(int.MinValue, int.MaxValue);
            Assert.AreEqual(int.MaxValue, range.Maximum);
        }

        #endregion

        #region Properties: get/set access same value

        [TestMethod]
        public void Minimum_GetSetAccessSameValue()
        {
            RandomRange range = new(1, 2);

            range.Minimum = 3;
            Assert.AreEqual(3, range.Minimum);
        }

        [TestMethod]
        public void Maximum_GetSetAccessSameValue()
        {
            RandomRange range = new(1, 2);

            range.Maximum = 3;
            Assert.AreEqual(3, range.Maximum);
        }

        [TestMethod]
        public void LowerLimit_GetSetAccessSameValue()
        {
            RandomRange range = new(1, 2);

            range.LowerLimit = 3;
            Assert.AreEqual(3, range.LowerLimit);
        }

        [TestMethod]
        public void UpperLimit_GetSetAccessSameValue()
        {
            RandomRange range = new(1, 2);

            range.UpperLimit = 3;
            Assert.AreEqual(3, range.UpperLimit);
        }

        #endregion

        #region Properties: state consistency

        #region Minimum

        [TestMethod]
        public void Minimum_SetBelowLowerLimit()
        {
            RandomRange range = new()
            {
                LowerLimit = 1000,
                UpperLimit = 2000
            };

            range.Minimum = 500;

            Console.WriteLine($"Limits: {range.LowerLimit} - {range.UpperLimit}");
            Console.WriteLine($"Min/Max: {range.Minimum} - {range.Maximum}");

            Assert.IsFalse(range.Minimum == 500);
            Assert.IsFalse(range.Minimum < range.LowerLimit);
        }

        [TestMethod]
        public void Minimum_SetAboveUpperLimit()
        {
            RandomRange range = new()
            {
                LowerLimit = 1000,
                UpperLimit = 2000
            };

            range.Minimum = 2500;

            Console.WriteLine($"Limits: {range.LowerLimit} - {range.UpperLimit}");
            Console.WriteLine($"Min/Max: {range.Minimum} - {range.Maximum}");

            Assert.IsFalse(range.Minimum == 2500);
            Assert.IsFalse(range.Minimum > range.UpperLimit);
        }

        [TestMethod]
        public void Minimum_SetValidValue()
        {
            RandomRange range = new()
            {
                LowerLimit = 1000,
                UpperLimit = 2000
            };

            range.Minimum = 1500;

            Console.WriteLine($"Limits: {range.LowerLimit} - {range.UpperLimit}");
            Console.WriteLine($"Min/Max: {range.Minimum} - {range.Maximum}");

            Assert.IsTrue(range.Minimum == 1500);
        }

        #endregion

        #region Minimum

        [TestMethod]
        public void Maximum_SetBelowLowerLimit()
        {
            RandomRange range = new()
            {
                LowerLimit = 1000,
                UpperLimit = 2000
            };

            range.Maximum = 500;

            Console.WriteLine($"Limits: {range.LowerLimit} - {range.UpperLimit}");
            Console.WriteLine($"Min/Max: {range.Minimum} - {range.Maximum}");

            Assert.IsFalse(range.Maximum == 500);
            Assert.IsFalse(range.Maximum < range.LowerLimit);
        }

        [TestMethod]
        public void Maximum_SetAboveUpperLimit()
        {
            RandomRange range = new()
            {
                LowerLimit = 1000,
                UpperLimit = 2000
            };

            range.Maximum = 2500;

            Console.WriteLine($"Limits: {range.LowerLimit} - {range.UpperLimit}");
            Console.WriteLine($"Min/Max: {range.Minimum} - {range.Maximum}");

            Assert.IsFalse(range.Maximum == 2500);
            Assert.IsFalse(range.Maximum > range.UpperLimit);
        }

        [TestMethod]
        public void Maximum_SetValidValue()
        {
            RandomRange range = new()
            {
                LowerLimit = 1000,
                UpperLimit = 2000
            };

            range.Maximum = 1500;

            Console.WriteLine($"Limits: {range.LowerLimit} - {range.UpperLimit}");
            Console.WriteLine($"Min/Max: {range.Minimum} - {range.Maximum}");

            Assert.IsTrue(range.Maximum == 1500);
        }

        #endregion

        #endregion

        #region Coalesce

        [TestMethod]
        public void Coalesce_TooSmall()
        {
            RandomRange range = new RandomRange(10, 20);

            int result = range.Coalesce(5);

            Assert.AreEqual(range.Minimum, result);
        }

        [TestMethod]
        public void Coalesce_TooLarge()
        {
            RandomRange range = new RandomRange(10, 20);

            int result = range.Coalesce(50);

            Assert.AreEqual(range.Maximum, result);
        }

        [TestMethod]
        public void Coalesce_WithinRange()
        {
            RandomRange range = new RandomRange(10, 20);

            int result = range.Coalesce(15);

            Assert.AreEqual(15, result);
        }

        #endregion
    }
}
