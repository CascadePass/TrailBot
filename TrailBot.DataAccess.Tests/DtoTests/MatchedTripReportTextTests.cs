using CascadePass.TrailBot.DataAccess.DTO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace TrailBot.DataAccess.Tests.DtoTests
{
    [TestClass]
    public class MatchedTripReportTextTests
    {
        [TestMethod]
        public void CanCreateInstance()
        {
            _ = new MatchedTripReportText();
        }

        #region Get/set access same value

        [TestMethod]
        public void ID_GetSetAccessSameValue()
        {
            long expectedValue = new Random().Next();
            MatchedTripReportText testObject = new()
            {
                ID = expectedValue
            };

            Assert.AreEqual(expectedValue, testObject.ID);
        }

        [TestMethod]
        public void TripReportID_GetSetAccessSameValue()
        {
            long expectedValue = new Random().Next();
            MatchedTripReportText testObject = new()
            {
                TripReportID = expectedValue
            };

            Assert.AreEqual(expectedValue, testObject.TripReportID);
        }

        [TestMethod]
        public void TextID_GetSetAccessSameValue()
        {
            long expectedValue = new Random().Next();
            MatchedTripReportText testObject = new()
            {
                TextID = expectedValue
            };

            Assert.AreEqual(expectedValue, testObject.TextID);
        }

        #endregion
    }
}
