using CascadePass.TrailBot.DataAccess.DTO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace TrailBot.DataAccess.Tests.DtoTests
{
    [TestClass]
    public class ProviderTests
    {
        [TestMethod]
        public void CanCreateInstance()
        {
            _ = new Provider();
        }

        #region Get/set access same value

        [TestMethod]
        public void ID_GetSetAccessSameValue()
        {
            long expectedValue = new Random().Next();
            Provider testProvider = new()
            {
                ID = expectedValue
            };

            Assert.AreEqual(expectedValue, testProvider.ID);
        }

        [TestMethod]
        public void Name_GetSetAccessSameValue()
        {
            string expectedValue = Guid.NewGuid().ToString();
            Provider testProvider = new()
            {
                Name = expectedValue
            };

            Assert.AreEqual(expectedValue, testProvider.Name);
        }

        [TestMethod]
        public void TypeName_GetSetAccessSameValue()
        {
            string expectedValue = Guid.NewGuid().ToString();
            Provider testProvider = new()
            {
                TypeName = expectedValue
            };

            Assert.AreEqual(expectedValue, testProvider.TypeName);
        }

        [TestMethod]
        public void PreservationRule_GetSetAccessSameValue()
        {
            int expectedValue = new Random().Next();
            Provider testProvider = new()
            {
                PreservationRule = expectedValue
            };

            Assert.AreEqual(expectedValue, testProvider.PreservationRule);
        }

        [TestMethod]
        public void State_GetSetAccessSameValue()
        {
            int expectedValue = new Random().Next();
            Provider testProvider = new()
            {
                State = expectedValue
            };

            Assert.AreEqual(expectedValue, testProvider.State);
        }

        [TestMethod]
        public void Browser_GetSetAccessSameValue()
        {
            int expectedValue = new Random().Next();
            Provider testProvider = new()
            {
                Browser = expectedValue
            };

            Assert.AreEqual(expectedValue, testProvider.Browser);
        }

        [TestMethod]
        public void Found_GetSetAccessSameValue()
        {
            DateTime expectedValue = DateTime.Now.AddDays(new Random().Next(-100, 100));
            Provider testProvider = new()
            {
                LastTripReportRequest = expectedValue
            };

            Assert.AreEqual(expectedValue, testProvider.LastTripReportRequest);
        }

        [TestMethod]
        public void Found_GetSetAccessSameValue_Null()
        {
            Provider testProvider = new()
            {
                LastTripReportRequest = null
            };

            Assert.AreEqual(null, testProvider.LastTripReportRequest);
        }

        [TestMethod]
        public void LastGetRecentRequest_GetSetAccessSameValue()
        {
            DateTime expectedValue = DateTime.Now.AddDays(new Random().Next(-100, 100));
            Provider testProvider = new()
            {
                LastGetRecentRequest = expectedValue
            };

            Assert.AreEqual(expectedValue, testProvider.LastGetRecentRequest);
        }

        [TestMethod]
        public void LastGetRecentRequest_GetSetAccessSameValue_Null()
        {
            Provider testProvider = new()
            {
                LastGetRecentRequest = null
            };

            Assert.AreEqual(null, testProvider.LastGetRecentRequest);
        }

        [TestMethod]
        public void ProviderXml_GetSetAccessSameValue()
        {
            string expectedValue = Guid.NewGuid().ToString();
            Provider testProvider = new()
            {
                ProviderXml = expectedValue
            };

            Assert.AreEqual(expectedValue, testProvider.ProviderXml);
        }

        #endregion
    }
}
