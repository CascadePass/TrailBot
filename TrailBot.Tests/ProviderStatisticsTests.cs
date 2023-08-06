using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CascadePass.TrailBot.Tests
{
    [TestClass]
    public class ProviderStatisticsTests
    {
        [TestMethod]
        public void CanCreateInstance()
        {
            ProviderStatistics providerStatistics = new();
        }

        [TestMethod]
        public void SleepTime_NotNullByDefault()
        {
            ProviderStatistics providerStatistics = new();
            Assert.IsNotNull(providerStatistics.SleepTime);
        }

        #region get/set access same value

        [TestMethod]
        public void RequestsMade_GetSetAccessSameValue()
        {
            int expectedValue = new Random().Next(int.MaxValue);
            ProviderStatistics providerStatistics = new();

            providerStatistics.RequestsMade = expectedValue;
            Assert.AreEqual(expectedValue, providerStatistics.RequestsMade);
        }

        [TestMethod]
        public void FailedRequests_GetSetAccessSameValue()
        {
            int expectedValue = new Random().Next(int.MaxValue);
            ProviderStatistics providerStatistics = new();

            providerStatistics.FailedRequests = expectedValue;
            Assert.AreEqual(expectedValue, providerStatistics.FailedRequests);
        }

        [TestMethod]
        public void MatchesFound_GetSetAccessSameValue()
        {
            int expectedValue = new Random().Next(int.MaxValue);
            ProviderStatistics providerStatistics = new();

            providerStatistics.MatchesFound = expectedValue;
            Assert.AreEqual(expectedValue, providerStatistics.MatchesFound);
        }

        [TestMethod]
        public void SleepTime_GetSetAccessSameValue()
        {
            int underlyingValue = new Random().Next(int.MaxValue);
            TimeSpan expectedValue = new((long)underlyingValue);
            ProviderStatistics providerStatistics = new();

            providerStatistics.SleepTime = expectedValue;
            Assert.AreEqual(expectedValue, providerStatistics.SleepTime);
        }

        #endregion
    }
}
