using CascadePass.TrailBot.DataAccess;
using CascadePass.TrailBot.DataAccess.DTO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Text;

namespace TrailBot.DataAccess.Tests.IntegrationTests
{
    [TestClass]
    public class WtaTripReportTests : SqliteIntegrationTestClass
    {
        #region Constructor

        public WtaTripReportTests()
        {
            this.DatabaseFilename = "C:\\Users\\User\\Documents\\TrailBot\\TrailBot-test.db";
            Database.ConnectionString = $"Data Source={this.DatabaseFilename}";
        }

        #endregion

        #region CRUD calls

        [TestMethod]
        public void AddWtaTripReport_UnknownUrl()
        {
            this.AssertRequirements();
            Database.QueryProvider = new SqliteQueryProvider();

            WtaTripReport tripReport = this.GetRandomTripReport();

            Database.AddWtaTripReport(tripReport);

            // Was it actually saved?
            var validate = Database.GetWtaTripReport(tripReport.ID);
            Assert.IsNotNull(validate);

            this.AssertSameTripReport(tripReport, validate);

            // Cleanup
            Database.DeleteWtaTripReport(tripReport.ID);
        }

        [TestMethod]
        public void AddWtaTripReport_ExistingUrl()
        {
            this.AssertRequirements();
            Database.QueryProvider = new SqliteQueryProvider();

            Url testUrl = new() { Address = $"https://test.com/test/url/{Guid.NewGuid()}" };
            Database.AddUrl(testUrl);

            WtaTripReport tripReport = this.GetRandomTripReport();
            tripReport.Url = testUrl;

            Database.AddWtaTripReport(tripReport);

            // Was it actually saved?
            var validate = Database.GetWtaTripReport(tripReport.ID);
            Assert.IsNotNull(validate);

            this.AssertSameTripReport(tripReport, validate);

            // Cleanup
            Database.DeleteWtaTripReport(tripReport.ID);
        }

        [TestMethod]
        public void AddWtaTripReport_ExistingUrlWithoutID()
        {
            this.AssertRequirements();
            Database.QueryProvider = new SqliteQueryProvider();

            Url testUrl = new() { Address = $"https://test.com/test/url/{Guid.NewGuid()}" };
            Database.AddUrl(testUrl);

            WtaTripReport tripReport = this.GetRandomTripReport();
            tripReport.Url.Address = testUrl.Address;

            Database.AddWtaTripReport(tripReport);

            // Was it actually saved?
            var validate = Database.GetWtaTripReport(tripReport.ID);
            Assert.IsNotNull(validate);

            this.AssertSameTripReport(tripReport, validate);

            // Cleanup
            Database.DeleteWtaTripReport(tripReport.ID);
        }

        [TestMethod]
        public void UpdateWtaTripReport_ExceptUrl()
        {
            this.AssertRequirements();
            Database.QueryProvider = new SqliteQueryProvider();

            WtaTripReport
                originalTR = this.GetRandomTripReport(),
                updatedValuesTR = this.GetRandomTripReport();

            // Add some values to the database
            Database.AddWtaTripReport(originalTR);

            // Update those values to other random ones
            updatedValuesTR.ID = originalTR.ID;
            updatedValuesTR.Url = originalTR.Url;
            Database.UpdateWtaTripReport(updatedValuesTR);

            // See what's actually stored
            WtaTripReport storedInTableTR = Database.GetWtaTripReport(updatedValuesTR.ID);

            this.AssertSameTripReport(updatedValuesTR, storedInTableTR);

            // Cleanup
            Database.DeleteWtaTripReport(updatedValuesTR.ID);
        }

        [TestMethod]
        public void UpdateWtaTripReport()
        {
            this.AssertRequirements();
            Database.QueryProvider = new SqliteQueryProvider();

            WtaTripReport
                originalTR = this.GetRandomTripReport(),
                updatedValuesTR = this.GetRandomTripReport();

            // Add some values to the database
            Database.AddWtaTripReport(originalTR);

            // Update those values to other random ones
            updatedValuesTR.ID = originalTR.ID;
            updatedValuesTR.Url.ID = originalTR.Url.ID;
            Database.UpdateWtaTripReport(updatedValuesTR);

            // See what's actually stored
            WtaTripReport storedInTableTR = Database.GetWtaTripReport(updatedValuesTR.ID);

            this.AssertSameTripReport(updatedValuesTR, storedInTableTR);

            // Cleanup
            Database.DeleteWtaTripReport(updatedValuesTR.ID);
        }

        [TestMethod]
        public void UpdateWtaTripReport_NewUrl()
        {
            this.AssertRequirements();
            Database.QueryProvider = new SqliteQueryProvider();

            WtaTripReport
                originalTR = this.GetRandomTripReport(),
                updatedValuesTR = this.GetRandomTripReport();

            // Add some values to the database
            Database.AddWtaTripReport(originalTR);

            // Update those values to other random ones
            updatedValuesTR.ID = originalTR.ID;
            updatedValuesTR.Url.ID = 0;
            Database.UpdateWtaTripReport(updatedValuesTR);

            // See what's actually stored
            WtaTripReport storedInTableTR = Database.GetWtaTripReport(updatedValuesTR.ID);

            this.AssertSameTripReport(updatedValuesTR, storedInTableTR);

            Assert.IsTrue(updatedValuesTR.Url.ID > 0, "Url.ID should have been assigned");
            var validateUrlWasAdded = Database.GetUrl(updatedValuesTR.Url.ID);
            Assert.IsNotNull(validateUrlWasAdded, "Can't find generated url");

            // Cleanup
            Database.DeleteWtaTripReport(updatedValuesTR.ID);
            Database.DeleteUrl(validateUrlWasAdded.ID);
        }

        [TestMethod]
        public void DeleteWtaTripReport_ByID()
        {
            this.AssertRequirements();
            Database.QueryProvider = new SqliteQueryProvider();

            // Create a trip report to delete
            WtaTripReport tripReport = this.GetRandomTripReport();

            Database.AddWtaTripReport(tripReport);

            // Now delete it
            Database.DeleteWtaTripReport(tripReport.ID);

            // Make sure it's really been deleted
            // (no longer exists, can't be loaded by ID)
            var validate = Database.GetWtaTripReport(tripReport.ID);
            Assert.IsNull(validate);
        }

        [TestMethod]
        public void DeleteWtaTripReport_ByDTO()
        {
            this.AssertRequirements();
            Database.QueryProvider = new SqliteQueryProvider();

            // Create a trip report to delete
            WtaTripReport tripReport = this.GetRandomTripReport();

            Database.AddWtaTripReport(tripReport);

            // Now delete it
            Database.DeleteWtaTripReport(tripReport);

            // Make sure it's really been deleted
            // (no longer exists, can't be loaded by ID)
            var validate = Database.GetWtaTripReport(tripReport.ID);
            Assert.IsNull(validate);
        }

        [TestMethod]
        public void GetWtaTripReport()
        {
            this.AssertRequirements();
            Database.QueryProvider = new SqliteQueryProvider();

            WtaTripReport tripReport = this.GetRandomTripReport();

            // Save, to be able to load it
            Database.AddWtaTripReport(tripReport);

            // Make sure it loads
            var validate = Database.GetWtaTripReport(tripReport.ID);
            Assert.IsNotNull(validate);

            // Make sure the loaded values are correct
            this.AssertSameTripReport(tripReport, validate);

            // Cleanup
            Database.DeleteWtaTripReport(tripReport.ID);
        }

        #endregion

        #region Private utility methods

        private WtaTripReport GetRandomTripReport()
        {
            Random random = new();
            StringBuilder reportText = new();

            for (int line = 0; line < random.Next(1, 200); line++)
            {
                for (int wordBurst = 0; wordBurst < random.Next(1, 8); wordBurst++)
                {
                    reportText.Append($"{wordBurst} ");
                }

                reportText.AppendLine();
            }

            return new()
            {
                Url = new() { Address = $"https://test.com/trip_reports/{Guid.NewGuid()}" },
                Title = $"Trip Report {Guid.NewGuid()}",
                Region = Guid.NewGuid().ToString(),
                HikeType = Guid.NewGuid().ToString(),
                Author = Guid.NewGuid().ToString(),
                TripDate = this.GetRandomDateTime(),
                PublishedDate = this.GetRandomDateTime(),
                ProcessedDate = this.GetRandomDateTime(),
                ReportText = reportText.ToString(),
            };
        }

        private void AssertSameTripReport(WtaTripReport tripReport, WtaTripReport validate)
        {
            Assert.AreEqual(tripReport.Url.ID, validate.Url.ID);
            Assert.AreEqual(tripReport.Url.Address, validate.Url.Address);
            Assert.AreEqual(tripReport.Title, validate.Title);
            Assert.AreEqual(tripReport.Region, validate.Region);
            Assert.AreEqual(tripReport.HikeType, validate.HikeType);
            Assert.AreEqual(tripReport.Author, validate.Author);
            Assert.IsTrue(this.AreDatesSame(tripReport.TripDate, validate.TripDate));
            Assert.IsTrue(this.AreDatesSame(tripReport.PublishedDate, validate.PublishedDate));
            Assert.IsTrue(this.AreDatesSame(tripReport.ProcessedDate, validate.ProcessedDate));
            Assert.AreEqual(tripReport.ReportText, validate.ReportText);
        }

        #endregion
    }
}
