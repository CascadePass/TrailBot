using CascadePass.TrailBot.DataAccess.DTO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace TrailBot.DataAccess.Tests.DtoTests
{
    [TestClass]
    public class WtaTripReportImageTests
    {
        [TestMethod]
        public void CanCreateInstance()
        {
            _ = new ImageUrl();
        }

        #region Get/set access same value

        [TestMethod]
        public void ID_GetSetAccessSameValue()
        {
            long expectedValue = new Random().Next();
            WtaTripReportImage testWtaTripReportImage = new()
            {
                ID = expectedValue
            };

            Assert.AreEqual(expectedValue, testWtaTripReportImage.ID);
        }

        [TestMethod]
        public void WtaTripReportID_GetSetAccessSameValue()
        {
            long expectedValue = new Random().Next();
            WtaTripReportImage testWtaTripReportImage = new()
            {
                WtaTripReportID = expectedValue
            };

            Assert.AreEqual(expectedValue, testWtaTripReportImage.WtaTripReportID);
        }

        [TestMethod]
        public void ImageID_GetSetAccessSameValue()
        {
            long expectedValue = new Random().Next();
            WtaTripReportImage testWtaTripReportImage = new()
            {
                ImageID = expectedValue
            };

            Assert.AreEqual(expectedValue, testWtaTripReportImage.ImageID);
        }

        #endregion
    }
}
