using CascadePass.TrailBot.DataAccess;
using CascadePass.TrailBot.DataAccess.DTO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace TrailBot.DataAccess.Tests.IntegrationTests
{
    [TestClass]
    public class ProviderStatisticsTests : SqliteIntegrationTestClass
    {
        #region Constructor

        public ProviderStatisticsTests()
        {
            this.DatabaseFilename = "C:\\Users\\User\\Documents\\TrailBot\\TrailBot-test.db";
            Database.ConnectionString = $"Data Source={this.DatabaseFilename}";
        }

        #endregion

        [TestMethod]
        public void AddProviderStatistics()
        {
            this.AssertRequirements();
            Database.QueryProvider = new SqliteQueryProvider();

            var statistics = this.GetRandomProviderStatistics();

            Database.Add(statistics);

            // Was it actually saved?
            var validate = Database.GetProviderStatistics(statistics.ID);
            Assert.IsNotNull(validate);

            this.AssertSameProviderStatistics(statistics, validate);

            // Cleanup
            Database.DeleteProviderStatistics(statistics.ID);
        }

        [TestMethod]
        public void UpdateProviderStatistics()
        {
            this.AssertRequirements();
            Database.QueryProvider = new SqliteQueryProvider();

            ProviderStatistics
                original = this.GetRandomProviderStatistics(),
                updatedValues = this.GetRandomProviderStatistics();

            // Add some values to the database
            Database.Add(original);

            // Update those values to other random ones
            updatedValues.ID = original.ID;
            Database.Update(updatedValues);

            // See what's actually stored
            ProviderStatistics storedInTable = Database.GetProviderStatistics(updatedValues.ID);

            Assert.IsNotNull(storedInTable, $"ProviderStatistics {updatedValues.ID} not found in database");
            this.AssertSameProviderStatistics(updatedValues, storedInTable);

            // Cleanup
            Database.DeleteProviderStatistics(updatedValues.ID);
        }

        [TestMethod]
        public void DeleteProviderStatistics_ByID()
        {
            this.AssertRequirements();
            Database.QueryProvider = new SqliteQueryProvider();

            // Create a trip report to delete
            ProviderStatistics statistics = this.GetRandomProviderStatistics();

            var addRowCount = Database.Add(statistics);
            Console.WriteLine($"Added {addRowCount} statistics rows.");

            if (addRowCount == 0)
            {
                Assert.Inconclusive("Unable to add a statistics.");
            }

            // Now delete it
            Database.DeleteProviderStatistics(statistics.ID);

            // Make sure it's really been deleted
            // (no longer exists, can't be loaded by ID)
            var validate = Database.GetProviderStatistics(statistics.ID);
            Assert.IsNull(validate);
        }

        [TestMethod]
        public void DeleteProviderStatistics_ByDTO()
        {
            this.AssertRequirements();
            Database.QueryProvider = new SqliteQueryProvider();

            // Create a trip report to delete
            ProviderStatistics statistics = this.GetRandomProviderStatistics();

            var addRowCount = Database.Add(statistics);
            Console.WriteLine($"Added {addRowCount} statistics rows.");

            if (addRowCount == 0)
            {
                Assert.Inconclusive("Unable to add a statistics.");
            }

            // Now delete it
            Database.Delete(statistics);

            // Make sure it's really been deleted
            // (no longer exists, can't be loaded by ID)
            var validate = Database.GetProviderStatistics(statistics.ID);
            Assert.IsNull(validate);
        }

        [TestMethod]
        public void GetProviderStatistics()
        {
            this.AssertRequirements();
            Database.QueryProvider = new SqliteQueryProvider();

            var statistics = this.GetRandomProviderStatistics();

            // Save, to be able to load it
            Database.Add(statistics);

            // Make sure it loads
            var validate = Database.GetProviderStatistics(statistics.ID);
            Assert.IsNotNull(validate);

            // Make sure the loaded values are correct
            this.AssertSameProviderStatistics(statistics, validate);

            // Cleanup
            Database.DeleteProviderStatistics(statistics.ID);
        }

        [TestMethod]
        public void IncrementProviderStatistics()
        {
            this.AssertRequirements();
            Database.QueryProvider = new SqliteQueryProvider();

            ProviderStatistics
                original = this.GetRandomProviderStatistics(),
                updatedValues = this.GetRandomProviderStatistics();

            // Add some values to the database
            Database.Add(original);

            // Update those values to other random ones
            Database.IncrementProviderStatistics(
                original.ID,
                updatedValues.TotalRequestsMade,
                updatedValues.FailedRequests,
                updatedValues.MatchesFound,
                updatedValues.SleepTimeInMS
            );

            // See what's actually stored
            ProviderStatistics storedInTable = Database.GetProviderStatistics(updatedValues.ID);

            Assert.IsNotNull(storedInTable, $"ProviderStatistics {updatedValues.ID} not found in database");

            Assert.AreEqual(original.TotalRequestsMade + updatedValues.TotalRequestsMade, storedInTable.TotalRequestsMade, "TotalRequestsMade");
            Assert.AreEqual(original.FailedRequests + updatedValues.FailedRequests, storedInTable.FailedRequests, "FailedRequests");
            Assert.AreEqual(original.MatchesFound + updatedValues.MatchesFound, storedInTable.MatchesFound, "MatchesFound");
            Assert.AreEqual(original.SleepTimeInMS + updatedValues.SleepTimeInMS, storedInTable.SleepTimeInMS, "SleepTimeInMS");

            // Cleanup
            Database.DeleteProviderStatistics(updatedValues.ID);
        }

        #region Private utility methods

        private ProviderStatistics GetRandomProviderStatistics()
        {
            Random random = new();

            return new()
            {
                ID = random.Next(1, int.MaxValue),
                MatchesFound = random.Next(0, 10000),
                TotalRequestsMade = random.Next(0, 100000),
                FailedRequests = random.Next(0, 100000),
                SleepTimeInMS = random.Next(0, 100000),
            };
        }

        private void AssertSameProviderStatistics(ProviderStatistics statistics1, ProviderStatistics statistics2)
        {
            Assert.AreEqual(statistics1.ID, statistics2.ID, "ID doesn't match");
            Assert.AreEqual(statistics1.MatchesFound, statistics2.MatchesFound, "MatchesFound doesn't match");
            Assert.AreEqual(statistics1.TotalRequestsMade, statistics2.TotalRequestsMade, "TotalRequestsMade doesn't match");
            Assert.AreEqual(statistics1.FailedRequests, statistics2.FailedRequests, "FailedRequests doesn't match");
            Assert.AreEqual(statistics1.SleepTimeInMS, statistics2.SleepTimeInMS, "SleepTimeInMS doesn't match");
        }

        #endregion
    }
}
