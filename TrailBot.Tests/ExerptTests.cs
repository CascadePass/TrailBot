using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace CascadePass.TrailBot.Tests
{
    [TestClass]
    public class ExerptTests
    {
        [TestMethod]
        public void CanCreateExerpt()
        {
            _ = new Exerpt();
        }

        #region get/set access same value

        [TestMethod]
        public void Topic_GetSetAccessSameValue()
        {
            Exerpt exerpt = new();
            Topic topic = new();

            exerpt.Topic = topic;
            Assert.AreEqual(topic, exerpt.Topic);
        }

        [TestMethod]
        public void Quote_GetSetAccessSameValue()
        {
            Exerpt exerpt = new();
            string quote = Guid.NewGuid().ToString();

            exerpt.Quote = quote;
            Assert.AreEqual(quote, exerpt.Quote);
        }

        #endregion
    }
}
