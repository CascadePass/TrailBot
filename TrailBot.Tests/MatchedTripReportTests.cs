using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CascadePass.TrailBot.Tests
{
    [TestClass]
    public class MatchedTripReportTests
    {
        [TestMethod]
        public void CanCreateEmptyInstance()
        {
            MatchedTripReport matchedTripReport = new();
        }

        [TestMethod]
        public void Topics_NotNullWhenCreated()
        {
            MatchedTripReport matchedTripReport = new();

            Assert.IsNotNull(matchedTripReport.Topics);
        }

        #region Create

        #region Exceptions

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Create_NullTripReportThrows()
        {
            _ = MatchedTripReport.Create(null, new());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Create_NullMatchListThrows()
        {
            _ = MatchedTripReport.Create(new(), null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Create_EmptyMatchListThrows()
        {
            _ = MatchedTripReport.Create(new(), new());
        }

        #endregion

        [TestMethod]
        public void Create_Empty_TripReport()
        {
            TripReport tripReport = new();
            List<MatchInfo> matchInfos = new()
            {
                new()
            };

            MatchedTripReport matchedTripReport = MatchedTripReport.Create(tripReport, matchInfos);

            // Expectation is that no exception is thrown, because Create doesn't make
            // any calls on any property that could be null.
            //
            // For example, TripReport.Url was at one point a Uri, and Create called
            // ToString on it to supply MatchedTripReport with a string version of this
            // data.  This caused a NullReferenceException.  This test is intended to
            // ensure this won't happen and that calling code doesn't need to handle this.
        }

        [TestMethod]
        public void Create_SourceUri_IsCorrect()
        {
            string url = "https://test.domain.com/abc/123";
            TripReport tripReport = new() { Url = url };
            List<MatchInfo> matchInfos = new()
            {
                new()
            };

            MatchedTripReport matchedTripReport = MatchedTripReport.Create(tripReport, matchInfos);

            Assert.IsTrue(string.Equals(url, matchedTripReport.SourceUri, StringComparison.Ordinal));
        }

        [TestMethod]
        public void Create_Title_IsCorrect()
        {
            string title = "Cascade Pass";
            TripReport tripReport = new() { Title = title };
            List<MatchInfo> matchInfos = new()
            {
                new()
            };

            MatchedTripReport matchedTripReport = MatchedTripReport.Create(tripReport, matchInfos);

            Assert.IsTrue(string.Equals(title, matchedTripReport.Title, StringComparison.Ordinal));
        }

        [TestMethod]
        public void Create_Region_IsCorrect()
        {
            string region = "North Cascades";
            WtaTripReport tripReport = new() { Region = region };
            List<MatchInfo> matchInfos = new()
            {
                new()
            };

            MatchedTripReport matchedTripReport = MatchedTripReport.Create(tripReport, matchInfos);

            Assert.IsTrue(string.Equals(region, matchedTripReport.Region, StringComparison.Ordinal));
        }

        [TestMethod]
        public void Create_TripDate_IsCorrect()
        {
            DateTime tripDate = new(1977, 12, 21, 20, 39, 11);
            WtaTripReport tripReport = new() { TripDate = tripDate };
            List<MatchInfo> matchInfos = new()
            {
                new()
            };

            MatchedTripReport matchedTripReport = MatchedTripReport.Create(tripReport, matchInfos);

            Assert.AreEqual(tripDate, matchedTripReport.TripDate);
        }

        [TestMethod]
        public void Create_WordCount_IsCorrect()
        {
            WtaTripReport tripReport = new() { };
            List<MatchInfo> matchInfos = new()
            {
                new() { WordCount = 6 }
            };

            MatchedTripReport matchedTripReport = MatchedTripReport.Create(tripReport, matchInfos);

            Assert.AreEqual(6, matchedTripReport.WordCount);
        }

        [TestMethod]
        public void Create_HikeType_IsCorrect()
        {
            string hikeType = "Snowshoe";
            WtaTripReport tripReport = new() { HikeType = hikeType };
            List<MatchInfo> matchInfos = new()
            {
                new()
            };

            MatchedTripReport matchedTripReport = MatchedTripReport.Create(tripReport, matchInfos);

            Assert.IsTrue(string.Equals(hikeType, matchedTripReport.HikeType, StringComparison.Ordinal));
        }

        [TestMethod]
        public void Create_Topics_IsCorrect()
        {
            Topic crime = new() { Name = "crime", MatchAny = "broken window" };
            Topic snow = new() { Name = "snow", MatchAny = "snow" };
            Topic wildlife = new() { Name = "wildlife", MatchAny = "bear" };
            Topic swimming = new() { Name = "swimming", MatchAny = "summer" };


            WtaTripReport tripReport = new() { ReportText = "There was glass on the ground at the trail head, as if somebody had had a broken window.  We found snow at 3,200 feet." };

            List<MatchInfo> matchInfos = new()
            {
                new() { Topic = crime },
                new() { Topic = snow },
            };

            matchInfos[0].MatchCounts.Add("broken window", 1);
            matchInfos[1].MatchCounts.Add("snow", 1);

            MatchedTripReport matchedTripReport = MatchedTripReport.Create(tripReport, matchInfos);

            Console.WriteLine($"{matchedTripReport.Topics.Count} topics:");
            foreach (string topicName in matchedTripReport.Topics)
            {
                Console.WriteLine($"\t{topicName}");
            }

            Assert.IsTrue(matchedTripReport.Topics.Any(m => string.Equals(m, crime.Name, StringComparison.Ordinal)));

            Assert.IsTrue(matchedTripReport.Topics.Any(m => string.Equals(m, snow.Name, StringComparison.Ordinal)));

            Assert.IsFalse(matchedTripReport.Topics.Any(m => string.Equals(m, wildlife.Name, StringComparison.Ordinal)));

            Assert.IsFalse(matchedTripReport.Topics.Any(m => string.Equals(m, swimming.Name, StringComparison.Ordinal)));
        }

        #endregion

        #region GetMatchCounts

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void GetMatchCounts_null_ThrowsCorrectly()
        {
            _ = MatchedTripReport.GetMatchCounts(null);
        }

        [TestMethod]
        public void GetMatchCounts_EmptyList_ReturnsZero()
        {
            int result = MatchedTripReport.GetMatchCounts(new());
            Assert.AreEqual(0, result);
        }

        [TestMethod]
        public void GetMatchCounts_SimpleCaseIsCorrect()
        {
            List<MatchInfo> matches = new();

            Dictionary<string, int> termCounts = new();
            Topic topic = new();
            MatchInfo matchInfo = new() { MatchCounts = termCounts, Topic = topic };

            termCounts.Add("test", 3);
            matches.Add(matchInfo);

            int result = MatchedTripReport.GetMatchCounts(matches);
            Assert.AreEqual(3, result);
        }

        [TestMethod]
        public void GetMatchCounts_OneTermMultipleTopics()
        {
            List<MatchInfo> matches = new();

            Dictionary<string, int> termCounts1 = new(), termCounts2 = new(), termCounts3 = new();
            Topic t1 = new(), t2 = new(), t3 = new();
            MatchInfo
                m1 = new() { MatchCounts = termCounts1, Topic = t1 },
                m2 = new() { MatchCounts = termCounts2, Topic = t2 },
                m3 = new() { MatchCounts = termCounts3, Topic = t3 };

            termCounts1.Add("test", 1);
            termCounts2.Add("test", 2);
            termCounts3.Add("test", 3);

            matches.Add(m1);
            matches.Add(m2);
            matches.Add(m3);

            int result = MatchedTripReport.GetMatchCounts(matches);
            Assert.AreEqual(6, result);
        }

        #endregion
    }
}
