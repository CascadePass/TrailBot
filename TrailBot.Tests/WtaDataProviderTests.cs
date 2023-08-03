using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using System;

namespace CascadePass.TrailBot.Tests
{
    [TestClass]
    public class WtaDataProviderTests
    {
        [TestMethod]
        public void CanCreateInstance()
        {
            WtaDataProvider wtaDataProvider = new();
        }

        [TestMethod]
        public void TripReportSource()
        {
            WtaDataProvider wtaDataProvider = new();
            Assert.AreEqual(
                SupportedTripReportSource.WashingtonTrailsAssociation,
                wtaDataProvider.TripReportSource
            );
        }

        #region get/set access same value

        [TestMethod]
        public void LastTripReportRequest_GetSetAccessSameValue()
        {
            DateTime testValue = DateTime.Now.AddYears(-10);
            WtaDataProvider wtaDataProvider = new();

            wtaDataProvider.LastTripReportRequest = testValue;
            Assert.AreEqual(testValue, wtaDataProvider.LastTripReportRequest);
        }

        [TestMethod]
        public void LastGetRecentRequest_GetSetAccessSameValue()
        {
            DateTime testValue = DateTime.Now.AddYears(-10);
            WtaDataProvider wtaDataProvider = new();

            wtaDataProvider.LastGetRecentRequest = testValue;
            Assert.AreEqual(testValue, wtaDataProvider.LastGetRecentRequest);
        }

        #endregion


        [TestMethod]
        public void AgeOfLastTripReportRequest_IsCorrect()
        {
            WtaDataProvider wtaDataProvider = new() { LastTripReportRequest = DateTime.Now };
            double elapsed = wtaDataProvider.AgeOfLastTripReportRequest.TotalMilliseconds;

            Console.WriteLine($"wtaDataProvider.AgeOfLastTripReportRequest == {elapsed} ms");
            Assert.IsTrue(elapsed < 10);
        }

        [TestMethod]
        public void AgeOfLastRecentReportsRequest_IsCorrect()
        {
            WtaDataProvider wtaDataProvider = new() { LastGetRecentRequest = DateTime.Now };
            double elapsed = wtaDataProvider.AgeOfLastRecentReportsRequest.TotalMilliseconds;

            Console.WriteLine($"wtaDataProvider.AgeOfLastRecentReportsRequest == {elapsed} ms");
            Assert.IsTrue(elapsed < 10);
        }


        [TestMethod]
        public void ParseReportDate_Url_null()
        {
            Assert.AreEqual(DateTime.MinValue, WtaDataProvider.ParseReportDate((Uri)null));
        }

        [TestMethod]
        public void ParseReportDate_Page_null()
        {
            Assert.AreEqual(DateTime.MinValue, WtaDataProvider.ParseReportDate((WebDriver)null));
        }

        [TestMethod]
        public void ParseReportDate_CorrectDateReturned()
        {
            Assert.AreEqual(
                new(2023, 05, 19),
                WtaDataProvider.ParseReportDate(new Uri("https://www.wta.org/go-hiking/trip-reports/trip_report.2023-05-19.0501766105"))
                );
        }
    }
}
