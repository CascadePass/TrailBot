using CascadePass.TrailBot.DataAccess.DTO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace TrailBot.DataAccess.Tests.DtoTests
{
    [TestClass]
    public class ProviderStatisticsTests
    {
        [TestMethod]
        public void CanCreateInstance()
        {
            _ = new ProviderStatistics();
        }

        #region Get/set access same value

        [TestMethod]
        public void ID_GetSetAccessSameValue()
        {
            long expectedValue = new Random().Next();
            ProviderStatistics testStatistics = new()
            {
                ID = expectedValue
            };

            Assert.AreEqual(expectedValue, testStatistics.ID);
        }

        [TestMethod]
        public void MatchesFound_GetSetAccessSameValue()
        {
            long expectedValue = new Random().Next();
            ProviderStatistics testStatistics = new()
            {
                MatchesFound = expectedValue
            };

            Assert.AreEqual(expectedValue, testStatistics.MatchesFound);
        }

        [TestMethod]
        public void TotalRequestsMade_GetSetAccessSameValue()
        {
            long expectedValue = new Random().Next();
            ProviderStatistics testStatistics = new()
            {
                TotalRequestsMade = expectedValue
            };

            Assert.AreEqual(expectedValue, testStatistics.TotalRequestsMade);
        }

        [TestMethod]
        public void FailedRequests_GetSetAccessSameValue()
        {
            long expectedValue = new Random().Next();
            ProviderStatistics testStatistics = new()
            {
                FailedRequests = expectedValue
            };

            Assert.AreEqual(expectedValue, testStatistics.FailedRequests);
        }

        [TestMethod]
        public void SleepTimeInMS_GetSetAccessSameValue()
        {
            long expectedValue = new Random().Next();
            ProviderStatistics testStatistics = new()
            {
                SleepTimeInMS = expectedValue
            };

            Assert.AreEqual(expectedValue, testStatistics.SleepTimeInMS);
        }

        #endregion

        #region SleepTimeInMS <-> SleepTime

        [TestMethod]
        public void SleepTime_ReturnsCorrectValue()
        {
            long expectedValue = new Random().Next();
            ProviderStatistics testStatistics = new()
            {
                SleepTimeInMS = expectedValue
            };

            Assert.AreEqual(expectedValue, (long)testStatistics.SleepTime.TotalMilliseconds);
        }

        [TestMethod]
        public void SleepTime_SetsCorrectValue()
        {
            long expectedValue = new Random().Next();
            ProviderStatistics testStatistics = new()
            {
                SleepTime = TimeSpan.FromMilliseconds(expectedValue),
            };

            Assert.AreEqual(expectedValue, (long)testStatistics.SleepTimeInMS);
        }

        #endregion
    }
}
