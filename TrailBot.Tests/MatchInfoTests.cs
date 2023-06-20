using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace CascadePass.TrailBot.Tests
{
    [TestClass]
    public class MatchInfoTests
    {
        [TestMethod]
        public void CanCreateEmptyInstance()
        {
            MatchInfo matchInfo = new();
        }

        #region Defaults

        [TestMethod]
        public void NewInstanceIsEmpty()
        {
            MatchInfo matchInfo = new();
            Assert.IsTrue(matchInfo.IsEmpty);
        }

        [TestMethod]
        public void NewInstanceCountIsZero()
        {
            MatchInfo matchInfo = new();
            Assert.AreEqual(0, matchInfo.Count);
        }

        #endregion

        [TestMethod]
        public void MatchCountMakesIsEmptyFalse()
        {
            MatchInfo matchInfo = new();

            matchInfo.MatchCounts.Add(Guid.NewGuid().ToString(), 1);

            Assert.IsFalse(matchInfo.IsEmpty);
        }

        [TestMethod]
        public void CountReflectsMatchingWordsNotSumOfCounts()
        {
            MatchInfo matchInfo = new();

            matchInfo.MatchCounts.Add(Guid.NewGuid().ToString(), 100);
            matchInfo.MatchCounts.Add(Guid.NewGuid().ToString(), 200);
            matchInfo.MatchCounts.Add(Guid.NewGuid().ToString(), 300);

            Assert.AreEqual(3, matchInfo.Count);
        }
    }
}
