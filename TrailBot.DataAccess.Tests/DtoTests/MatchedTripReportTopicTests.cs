using CascadePass.TrailBot.DataAccess.DTO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace TrailBot.DataAccess.Tests.DtoTests
{
    [TestClass]
    public class MatchedTripReportTopicTests
    {
        [TestMethod]
        public void CanCreateInstance()
        {
            _ = new MatchedTripReportTopic();
        }

        #region Get/set access same value

        [TestMethod]
        public void ID_GetSetAccessSameValue()
        {
            long expectedValue = new Random().Next();
            MatchedTripReportTopic testObject = new()
            {
                ID = expectedValue
            };

            Assert.AreEqual(expectedValue, testObject.ID);
        }

        [TestMethod]
        public void TripReportID_GetSetAccessSameValue()
        {
            long expectedValue = new Random().Next();
            MatchedTripReportTopic testObject = new()
            {
                TripReportID = expectedValue
            };

            Assert.AreEqual(expectedValue, testObject.TripReportID);
        }

        [TestMethod]
        public void TopicID_GetSetAccessSameValue()
        {
            long expectedValue = new Random().Next();
            MatchedTripReportTopic testObject = new()
            {
                TopicID = expectedValue
            };

            Assert.AreEqual(expectedValue, testObject.TopicID);
        }


        [TestMethod]
        public void Exerpt_GetSetAccessSameValue()
        {
            string expectedValue = Guid.NewGuid().ToString();
            MatchedTripReportTopic testObject = new()
            {
                Exerpt = expectedValue
            };

            Assert.AreEqual(expectedValue, testObject.Exerpt);
        }

        #endregion    
    }
}
