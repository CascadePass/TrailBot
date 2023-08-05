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

        #region Simple property tests

        [TestMethod]
        public void TripReportSource_IsCorrect()
        {
            WtaDataProvider wtaDataProvider = new();
            Assert.AreEqual(
                SupportedTripReportSource.WashingtonTrailsAssociation,
                wtaDataProvider.TripReportSource
            );
        }

        [TestMethod]
        public void SourceName_IsCorrect()
        {
            WtaDataProvider wtaDataProvider = new();

            Assert.IsTrue(
                string.Equals(wtaDataProvider.SourceName, "WTA", StringComparison.Ordinal) ||
                string.Equals(wtaDataProvider.SourceName, "Washington Trails Association", StringComparison.Ordinal) ||
                string.Equals(wtaDataProvider.SourceName, "Washington Trails", StringComparison.Ordinal)
            );

            // It may be shortened at any point in the future
        }

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

        #endregion

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

        #region Parse methods

        [TestMethod]
        public void ParseTripReport_nulls()
        {
            var result = WtaDataProvider.ParseTripReport(null, null);

            Assert.IsNull(result);
        }

        #region ParseReportDate

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
        public void ParseReportDate_NoPeriodDateDoesNotThrow()
        {
            Assert.AreEqual(
                DateTime.MinValue,
                WtaDataProvider.ParseReportDate(new Uri("https://www.wta.org/go-hiking/trip-reports/trip_report"))
                );

            // WtaDataProvider.ParseReportDate looks for a dot in the url to
            // separate the date from other components.  Make sure a url with
            // no dot won't crash anything.
        }

        [TestMethod]
        public void ParseReportDate_MissingDateDoesNotThrow()
        {
            Assert.AreEqual(
                DateTime.MinValue,
                WtaDataProvider.ParseReportDate(new Uri("https://www.wta.org/go-hiking/trip-reports/trip_report.0501766105"))
                );
        }

        [TestMethod]
        public void ParseReportDate_InvalidDateDoesNotThrow()
        {
            Assert.AreEqual(
                DateTime.MinValue,
                WtaDataProvider.ParseReportDate(new Uri("https://www.wta.org/go-hiking/trip-reports/trip_report.2023-05-33.0501766105"))
                );
        }

        [TestMethod]
        public void ParseReportDate_CorrectDateReturned()
        {
            Assert.AreEqual(
                new(2023, 05, 19),
                WtaDataProvider.ParseReportDate(new Uri("https://www.wta.org/go-hiking/trip-reports/trip_report.2023-05-19.0501766105"))
                );
        }

        #endregion

        #region Helper methods

        [TestMethod]
        public void IsPageNotFound_null()
        {
            var result = WtaDataProvider.IsPageNotFound(null);
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void ParseReportText_null()
        {
            var result = WtaDataProvider.ParseReportText(null);
            Assert.IsNull(result);
        }

        [TestMethod]
        public void ParseReportTitle_null()
        {
            var result = WtaDataProvider.ParseReportTitle(null);
            Assert.IsNull(result);
        }

        [TestMethod]
        public void ParseReportRegion_null()
        {
            var result = WtaDataProvider.ParseReportRegion(null);
            Assert.IsNull(result);
        }

        [TestMethod]
        public void ParseReportAuthor_null()
        {
            var result = WtaDataProvider.ParseReportAuthor(null);
            Assert.IsNull(result);
        }

        [TestMethod]
        public void ParseReportTrails_null()
        {
            var result = WtaDataProvider.ParseReportTrails(null);
            Assert.IsNull(result);
        }

        [TestMethod]
        public void ParseReportConditions_null()
        {
            var result = WtaDataProvider.ParseReportConditions(null);
            Assert.IsNull(result);
        }

        [TestMethod]
        public void ParseReportFeature_null()
        {
            var result = WtaDataProvider.ParseReportFeature(null);
            Assert.IsNull(result);
        }

        #endregion

        #endregion

        #region Find within parsed data methods

        #region FindHikeType

        [TestMethod]
        public void FindHikeType_null()
        {
            var result = WtaDataProvider.FindHikeType(null);
            Assert.IsNull(result);
        }

        [TestMethod]
        public void FindHikeType_EmptyTripReport()
        {
            var result = WtaDataProvider.FindHikeType(new());
            Assert.IsNull(result);
        }

        [TestMethod]
        public void FindHikeType_null_TrailConditions()
        {
            var result = WtaDataProvider.FindHikeType(new() { TrailConditions = null });
            Assert.IsNull(result);
        }

        [TestMethod]
        public void FindHikeType_CorrectResult()
        {
            string expectedValue = Guid.NewGuid().ToString();
            WtaTripReport report = new();
            report.TrailConditions.Add(new() { Title = "TYPE OF HIKE", Description = expectedValue });
            var result = WtaDataProvider.FindHikeType(report);

            Assert.AreEqual(expectedValue, result);
        }

        [TestMethod]
        public void FindHikeType_CorrectResult_CaseInsensitive()
        {
            string expectedValue = Guid.NewGuid().ToString();
            WtaTripReport report = new();
            report.TrailConditions.Add(new() { Title = "Type of hike", Description = expectedValue });
            var result = WtaDataProvider.FindHikeType(report);

            Assert.AreEqual(expectedValue, result);
        }

        #endregion

        #region FindRoadConditions

        [TestMethod]
        public void FindRoadConditions_null()
        {
            var result = WtaDataProvider.FindRoadConditions(null);
            Assert.IsNull(result);
        }

        [TestMethod]
        public void FindRoadConditions_EmptyTripReport()
        {
            var result = WtaDataProvider.FindRoadConditions(new());
            Assert.IsNull(result);
        }

        [TestMethod]
        public void FindRoadConditions_null_TrailConditions()
        {
            var result = WtaDataProvider.FindRoadConditions(new() { TrailConditions = null });
            Assert.IsNull(result);
        }

        [TestMethod]
        public void FindRoadConditions_CorrectResult()
        {
            string expectedValue = Guid.NewGuid().ToString();
            WtaTripReport report = new();
            report.TrailConditions.Add(new() { Title = "ROAD", Description = expectedValue });
            var result = WtaDataProvider.FindRoadConditions(report);

            Assert.AreEqual(expectedValue, result);
        }

        [TestMethod]
        public void FindRoadConditions_CorrectResult_CaseInsensitive()
        {
            string expectedValue = Guid.NewGuid().ToString();
            WtaTripReport report = new();
            report.TrailConditions.Add(new() { Title = "Road", Description = expectedValue });
            var result = WtaDataProvider.FindRoadConditions(report);

            Assert.AreEqual(expectedValue, result);
        }

        #endregion

        #region FindBugConditions

        [TestMethod]
        public void FindBugConditions_null()
        {
            var result = WtaDataProvider.FindBugConditions(null);
            Assert.IsNull(result);
        }

        [TestMethod]
        public void FindBugConditions_EmptyTripReport()
        {
            var result = WtaDataProvider.FindBugConditions(new());
            Assert.IsNull(result);
        }

        [TestMethod]
        public void FindBugConditions_null_TrailConditions()
        {
            var result = WtaDataProvider.FindBugConditions(new() { TrailConditions = null });
            Assert.IsNull(result);
        }

        [TestMethod]
        public void FindBugConditions_CorrectResult()
        {
            string expectedValue = Guid.NewGuid().ToString();
            WtaTripReport report = new();
            report.TrailConditions.Add(new() { Title = "BUGS", Description = expectedValue });
            var result = WtaDataProvider.FindBugConditions(report);

            Assert.AreEqual(expectedValue, result);
        }

        [TestMethod]
        public void FindBugConditions_CorrectResult_CaseInsensitive()
        {
            string expectedValue = Guid.NewGuid().ToString();
            WtaTripReport report = new();
            report.TrailConditions.Add(new() { Title = "Bugs", Description = expectedValue });
            var result = WtaDataProvider.FindBugConditions(report);

            Assert.AreEqual(expectedValue, result);
        }

        #endregion

        #region FindBugConditions

        [TestMethod]
        public void FindSnowConditions_null()
        {
            var result = WtaDataProvider.FindSnowConditions(null);
            Assert.IsNull(result);
        }

        [TestMethod]
        public void FindSnowConditions_EmptyTripReport()
        {
            var result = WtaDataProvider.FindSnowConditions(new());
            Assert.IsNull(result);
        }

        [TestMethod]
        public void FindSnowConditions_null_TrailConditions()
        {
            var result = WtaDataProvider.FindSnowConditions(new() { TrailConditions = null });
            Assert.IsNull(result);
        }

        [TestMethod]
        public void FindSnowConditions_CorrectResult()
        {
            string expectedValue = Guid.NewGuid().ToString();
            WtaTripReport report = new();
            report.TrailConditions.Add(new() { Title = "SNOW", Description = expectedValue });
            var result = WtaDataProvider.FindSnowConditions(report);

            Assert.AreEqual(expectedValue, result);
        }

        [TestMethod]
        public void FindSnowConditions_CorrectResult_CaseInsensitive()
        {
            string expectedValue = Guid.NewGuid().ToString();
            WtaTripReport report = new();
            report.TrailConditions.Add(new() { Title = "Snow", Description = expectedValue });
            var result = WtaDataProvider.FindSnowConditions(report);

            Assert.AreEqual(expectedValue, result);
        }

        #endregion

        #region FindHikeCondition

        [TestMethod]
        public void FindHikeCondition_nulls()
        {
            var result = WtaDataProvider.FindHikeCondition(null, null);
            Assert.IsNull(result);
        }

        [TestMethod]
        public void FindHikeCondition_null_TripReport_Valid_Label()
        {
            var result = WtaDataProvider.FindHikeCondition(null, "SNOW");
            Assert.IsNull(result);
        }

        [TestMethod]
        public void FindHikeCondition_Valid_TripReport_null_Label()
        {
            var result = WtaDataProvider.FindHikeCondition(new(), null);
            Assert.IsNull(result);
        }

        [TestMethod]
        public void FindHikeCondition_Valid_TripReport_Empty_Label()
        {
            var result = WtaDataProvider.FindHikeCondition(new(), string.Empty);
            Assert.IsNull(result);
        }

        [TestMethod]
        public void FindHikeCondition_Valid_TripReport_Whitespace_Label()
        {
            var result = WtaDataProvider.FindHikeCondition(new(), " ");
            Assert.IsNull(result);
        }

        [TestMethod]
        public void FindHikeCondition_EmptyTripReport_Valid_Label()
        {
            var result = WtaDataProvider.FindHikeCondition(new(), "SNOW");
            Assert.IsNull(result);
        }

        [TestMethod]
        public void FindHikeCondition_null_TrailConditions_Valid_Label()
        {
            var result = WtaDataProvider.FindHikeCondition(new() { TrailConditions = null }, "SNOW");
            Assert.IsNull(result);
        }

        [TestMethod]
        public void FindHikeCondition_CorrectResult()
        {
            string expectedValue = Guid.NewGuid().ToString();
            WtaTripReport report = new();
            report.TrailConditions.Add(new() { Title = "SNOW", Description = expectedValue });
            var result = WtaDataProvider.FindHikeCondition(report, "SNOW");

            Assert.AreEqual(expectedValue, result);
        }

        [TestMethod]
        public void FindHikeCondition_CorrectResult_CaseInsensitive()
        {
            string expectedValue = Guid.NewGuid().ToString();
            WtaTripReport report = new();
            report.TrailConditions.Add(new() { Title = "SNOW", Description = expectedValue });
            var result = WtaDataProvider.FindHikeCondition(report, "snow");

            Assert.AreEqual(expectedValue, result);
        }

        #endregion

        #endregion
    }
}
