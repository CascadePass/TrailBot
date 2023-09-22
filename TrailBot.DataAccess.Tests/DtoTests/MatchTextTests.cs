using CascadePass.TrailBot.DataAccess.DTO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrailBot.DataAccess.Tests.DtoTests
{
    [TestClass]
    public class MatchTextTests
    {
        [TestMethod]
        public void CanCreateInstance()
        {
            _ = new MatchText();
        }

        #region Get/set access same value

        [TestMethod]
        public void ID_GetSetAccessSameValue()
        {
            long expectedValue = new Random().Next();
            MatchText testObject = new()
            {
                ID = expectedValue
            };

            Assert.AreEqual(expectedValue, testObject.ID);
        }

        [TestMethod]
        public void ParentID_GetSetAccessSameValue()
        {
            long expectedValue = new Random().Next();
            MatchText testObject = new()
            {
                ParentID = expectedValue
            };

            Assert.AreEqual(expectedValue, testObject.ParentID);
        }

        [TestMethod]
        public void ParentID_GetSetAccessSameValue_Null()
        {
            long expectedValue = new Random().Next();
            MatchText testObject = new()
            {
                ParentID = null
            };

            Assert.AreEqual(null, testObject.ParentID);
        }

        [TestMethod]
        public void Text_GetSetAccessSameValue()
        {
            string expectedValue = Guid.NewGuid().ToString();
            MatchText testObject = new()
            {
                Text = expectedValue
            };

            Assert.AreEqual(expectedValue, testObject.Text);
        }

        #endregion
    }
}
