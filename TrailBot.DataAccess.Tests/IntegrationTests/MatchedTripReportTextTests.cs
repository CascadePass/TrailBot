using CascadePass.TrailBot.DataAccess;
using CascadePass.TrailBot.DataAccess.DTO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace TrailBot.DataAccess.Tests.IntegrationTests
{
    [TestClass]
    public class MatchedTripReportTextTests : SqliteIntegrationTestClass
    {
        #region Constructor

        public MatchedTripReportTextTests()
        {
            this.DatabaseFilename = "C:\\Users\\User\\Documents\\TrailBot\\TrailBot-test.db";
            Database.ConnectionString = $"Data Source={this.DatabaseFilename}";
        }

        #endregion

        [TestMethod]
        public void AddMatchedTripReportText()
        {
            this.AssertRequirements();
            Database.QueryProvider = new SqliteQueryProvider();

            var matchedText = this.GetRandomMatchedTripReportText();

            Database.Add(matchedText);

            // Was it actually saved?
            var validate = Database.GetMatchedTripReportText(matchedText.ID);
            Assert.IsNotNull(validate);

            this.AssertSameMatchedTripReportText(matchedText, validate);

            // Cleanup
            Database.DeleteMatchedTripReportText(matchedText.ID);
        }

        [TestMethod]
        public void DeleteMatchedTripReportText_ByID()
        {
            this.AssertRequirements();
            Database.QueryProvider = new SqliteQueryProvider();

            // Create a trip report to delete
            MatchedTripReportText matchedText = this.GetRandomMatchedTripReportText();

            var addRowCount = Database.Add(matchedText);
            Console.WriteLine($"Added {addRowCount} matchedText rows.");

            if (addRowCount == 0)
            {
                Assert.Inconclusive("Unable to add a matchedText.");
            }

            // Now delete it
            Database.DeleteMatchedTripReportText(matchedText.ID);

            // Make sure it's really been deleted
            // (no longer exists, can't be loaded by ID)
            var validate = Database.GetMatchedTripReportText(matchedText.ID);
            Assert.IsNull(validate);
        }

        [TestMethod]
        public void DeleteMatchedTripReportText_ByDTO()
        {
            this.AssertRequirements();
            Database.QueryProvider = new SqliteQueryProvider();

            // Create a trip report to delete
            MatchedTripReportText matchedText = this.GetRandomMatchedTripReportText();

            var addRowCount = Database.Add(matchedText);
            Console.WriteLine($"Added {addRowCount} matchedText rows.");

            if (addRowCount == 0)
            {
                Assert.Inconclusive("Unable to add a matchedText.");
            }

            // Now delete it
            Database.Delete(matchedText);

            // Make sure it's really been deleted
            // (no longer exists, can't be loaded by ID)
            var validate = Database.GetMatchedTripReportText(matchedText.ID);
            Assert.IsNull(validate);
        }

        [TestMethod]
        public void GetMatchedTripReportText()
        {
            this.AssertRequirements();
            Database.QueryProvider = new SqliteQueryProvider();

            var matchedText = this.GetRandomMatchedTripReportText();

            // Save, to be able to load it
            Database.Add(matchedText);

            // Make sure it loads
            var validate = Database.GetMatchedTripReportText(matchedText.ID);
            Assert.IsNotNull(validate);

            // Make sure the loaded values are correct
            this.AssertSameMatchedTripReportText(matchedText, validate);

            // Cleanup
            Database.DeleteMatchedTripReportText(matchedText.ID);
        }

        #region Private utility methods

        private MatchedTripReportText GetRandomMatchedTripReportText()
        {
            return this.GetRandomMatchedTripReportText(0);
        }

        private MatchedTripReportText GetRandomMatchedTripReportText(int id)
        {
            Random random = new();

            return new()
            {
                ID = id,
                TripReportID = random.Next(),
                TextID = random.Next(),
            };
        }

        private void AssertSameMatchedTripReportText(MatchedTripReportText matchedText1, MatchedTripReportText matchedText2)
        {
            Assert.AreEqual(matchedText1.ID, matchedText2.ID, "ID doesn't match");
            Assert.AreEqual(matchedText1.TripReportID, matchedText2.TripReportID, "TripReportID doesn't match");
            Assert.AreEqual(matchedText1.TextID, matchedText2.TextID, "TextID doesn't match");
        }

        #endregion
    }
}
