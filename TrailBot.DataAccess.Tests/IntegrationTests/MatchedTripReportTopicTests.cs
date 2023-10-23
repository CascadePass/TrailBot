using CascadePass.TrailBot.DataAccess;
using CascadePass.TrailBot.DataAccess.DTO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace TrailBot.DataAccess.Tests.IntegrationTests
{
    [TestClass]
    public class MatchedTripReportTopicTests : SqliteIntegrationTestClass
    {
        #region Constructor

        public MatchedTripReportTopicTests()
        {
            this.DatabaseFilename = "C:\\Users\\User\\Documents\\TrailBot\\TrailBot-test.db";
            Database.ConnectionString = $"Data Source={this.DatabaseFilename}";
        }

        #endregion

        [TestMethod]
        public void AddMatchedTripReportTopic()
        {
            this.AssertRequirements();
            Database.QueryProvider = new SqliteQueryProvider();

            var matchedTopic = this.GetRandomMatchedTripReportTopic();

            Database.Add(matchedTopic);

            // Was it actually saved?
            var validate = Database.GetMatchedTripReportTopic(matchedTopic.ID);
            Assert.IsNotNull(validate);

            this.AssertSameMatchedTripReportTopic(matchedTopic, validate);

            // Cleanup
            Database.DeleteMatchedTripReportTopic(matchedTopic.ID);
        }

        [TestMethod]
        public void AddMatchedTripReportTopic_RawItemsOverload()
        {
            this.AssertRequirements();
            Database.QueryProvider = new SqliteQueryProvider();

            Url url = new() { Address = $"https://test.com/{Guid.NewGuid()}" };
            Topic topic = new() { Name = Guid.NewGuid().ToString() };
            WtaTripReport tripReport = new() { Author = Guid.NewGuid().ToString(), Url = url };
            string expert = Guid.NewGuid().ToString();

            Database.Add(topic);
            Database.Add(tripReport);

            long generatedID = Database.Add(tripReport, topic, expert);

            Console.WriteLine($"Generated MatchedTripReportTopic ID {generatedID}");

            // Was it actually saved?
            var validate = Database.GetMatchedTripReportTopic(generatedID);
            Assert.IsNotNull(validate);

            Assert.AreEqual(topic.ID, validate.TopicID);
            Assert.AreEqual(tripReport.ID, validate.TripReportID);

            // Cleanup
            Database.DeleteMatchedTripReportTopic(generatedID);
        }

        [TestMethod]
        public void DeleteMatchedTripReportTopic_ByID()
        {
            this.AssertRequirements();
            Database.QueryProvider = new SqliteQueryProvider();

            // Create a trip report to delete
            MatchedTripReportTopic matchedTopic = this.GetRandomMatchedTripReportTopic();

            var addRowCount = Database.Add(matchedTopic);
            Console.WriteLine($"Added {addRowCount} matchedTopic rows.");

            if (addRowCount == 0)
            {
                Assert.Inconclusive("Unable to add a matchedTopic.");
            }

            // Now delete it
            Database.DeleteMatchedTripReportTopic(matchedTopic.ID);

            // Make sure it's really been deleted
            // (no longer exists, can't be loaded by ID)
            var validate = Database.GetMatchedTripReportTopic(matchedTopic.ID);
            Assert.IsNull(validate);
        }

        [TestMethod]
        public void DeleteMatchedTripReportTopic_ByDTO()
        {
            this.AssertRequirements();
            Database.QueryProvider = new SqliteQueryProvider();

            // Create a trip report to delete
            MatchedTripReportTopic matchedTopic = this.GetRandomMatchedTripReportTopic();

            var addRowCount = Database.Add(matchedTopic);
            Console.WriteLine($"Added {addRowCount} matchedTopic rows.");

            if (addRowCount == 0)
            {
                Assert.Inconclusive("Unable to add a matchedTopic.");
            }

            // Now delete it
            Database.Delete(matchedTopic);

            // Make sure it's really been deleted
            // (no longer exists, can't be loaded by ID)
            var validate = Database.GetMatchedTripReportTopic(matchedTopic.ID);
            Assert.IsNull(validate);
        }

        [TestMethod]
        public void GetMatchedTripReportTopic()
        {
            this.AssertRequirements();
            Database.QueryProvider = new SqliteQueryProvider();

            var matchedTopic = this.GetRandomMatchedTripReportTopic();

            // Save, to be able to load it
            Database.Add(matchedTopic);

            // Make sure it loads
            var validate = Database.GetMatchedTripReportTopic(matchedTopic.ID);
            Assert.IsNotNull(validate);

            // Make sure the loaded values are correct
            this.AssertSameMatchedTripReportTopic(matchedTopic, validate);

            // Cleanup
            Database.DeleteMatchedTripReportTopic(matchedTopic.ID);
        }

        #region Private utility methods

        private MatchedTripReportTopic GetRandomMatchedTripReportTopic()
        {
            return this.GetRandomMatchedTripReportTopic(0);
        }

        private MatchedTripReportTopic GetRandomMatchedTripReportTopic(int id)
        {
            Random random = new();

            return new()
            {
                ID = id,
                TripReportID = random.Next(),
                TopicID = random.Next(),
                Exerpt = Guid.NewGuid().ToString(),
            };
        }

        private void AssertSameMatchedTripReportTopic(MatchedTripReportTopic matchedTopic1, MatchedTripReportTopic matchedTopic2)
        {
            Assert.AreEqual(matchedTopic1.ID, matchedTopic2.ID, "ID doesn't match");
            Assert.AreEqual(matchedTopic1.TripReportID, matchedTopic2.TripReportID, "TripReportID doesn't match");
            Assert.AreEqual(matchedTopic1.TopicID, matchedTopic2.TopicID, "TopicID doesn't match");
            Assert.AreEqual(matchedTopic1.Exerpt, matchedTopic2.Exerpt, "Exerpt doesn't match");
        }

        #endregion
    }
}
