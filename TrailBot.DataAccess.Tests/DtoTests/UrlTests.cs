using CascadePass.TrailBot.DataAccess.DTO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace TrailBot.DataAccess.Tests.DtoTests
{
    [TestClass]
    public class UrlTests
    {
        [TestMethod]
        public void CanCreateInstance()
        {
            _ = new Url();
        }

        #region Get/set access same value

        [TestMethod]
        public void ID_GetSetAccessSameValue()
        {
            long expectedValue = new Random().Next();
            Url testUrl = new()
            {
                ID = expectedValue
            };

            Assert.AreEqual(expectedValue, testUrl.ID);
        }

        [TestMethod]
        public void Text_GetSetAccessSameValue()
        {
            string expectedValue = Guid.NewGuid().ToString();
            Url testUrl = new()
            {
                Address = expectedValue
            };

            Assert.AreEqual(expectedValue, testUrl.Address);
        }

        [TestMethod]
        public void Found_GetSetAccessSameValue()
        {
            DateTime expectedValue = DateTime.Now.AddDays(new Random().Next(-100, 100));
            Url testUrl = new()
            {
                Found = expectedValue
            };

            Assert.AreEqual(expectedValue, testUrl.Found);
        }

        [TestMethod]
        public void Found_GetSetAccessSameValue_Null()
        {
            Url testUrl = new()
            {
                Found = null
            };

            Assert.AreEqual(null, testUrl.Found);
        }

        [TestMethod]
        public void Collected_GetSetAccessSameValue()
        {
            DateTime expectedValue = DateTime.Now.AddDays(new Random().Next(-100, 100));
            Url testUrl = new()
            {
                Collected = expectedValue
            };

            Assert.AreEqual(expectedValue, testUrl.Collected);
        }

        [TestMethod]
        public void Collected_GetSetAccessSameValue_Null()
        {
            Url testUrl = new()
            {
                Collected = null
            };

            Assert.AreEqual(null, testUrl.Collected);
        }

        [TestMethod]
        public void IntentLocked_GetSetAccessSameValue()
        {
            DateTime expectedValue = DateTime.Now.AddDays(new Random().Next(-100, 100));
            Url testUrl = new()
            {
                IntentLocked = expectedValue
            };

            Assert.AreEqual(expectedValue, testUrl.IntentLocked);
        }

        [TestMethod]
        public void IntentLocked_GetSetAccessSameValue_Null()
        {
            Url testUrl = new()
            {
                IntentLocked = null
            };

            Assert.AreEqual(null, testUrl.IntentLocked);
        }

        #endregion

        #region Static creation methods

        [TestMethod]
        public void CreateUrlByAddress_NotNull()
        {
            Url testUrl = Url.Create(Guid.NewGuid().ToString());
            Assert.IsNotNull(testUrl);
        }

        [TestMethod]
        public void CreateUrlByAddress_CorrectUrl()
        {
            string expectedValue = Guid.NewGuid().ToString();
            Url testUrl = Url.Create(expectedValue);
            Assert.IsNotNull(testUrl);
            Assert.AreEqual(expectedValue, testUrl.Address);
        }

        #endregion
    }
}
