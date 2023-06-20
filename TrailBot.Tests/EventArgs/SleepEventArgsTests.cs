using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace CascadePass.TrailBot.Tests
{
    [TestClass]
    public class SleepEventArgsTests
    {
        [TestMethod]
        public void CanCreateInstance()
        {
            _ = new SleepEventArgs();
        }

        [TestMethod]
        public void MatchedTripReport_GetSetAccessSameValue()
        {
            SleepEventArgs e = new();
            TimeSpan timeSpan = TimeSpan.FromMilliseconds(new Random().NextDouble() * 10000);

            e.Duration = timeSpan;
            Assert.AreEqual(timeSpan, e.Duration);
        }
    }
}
