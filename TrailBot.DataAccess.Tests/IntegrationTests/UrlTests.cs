using CascadePass.TrailBot.DataAccess;
using CascadePass.TrailBot.DataAccess.DTO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;

namespace TrailBot.DataAccess.Tests.IntegrationTests
{
    [TestClass]
    public class UrlTests : SqliteIntegrationTestClass
    {
        private static readonly List<long> urlIDs;
        private static readonly List<string> generatedUrls;

        #region Constructor

        public UrlTests()
        {
            this.DatabaseFilename = "C:\\Users\\User\\Documents\\TrailBot\\TrailBot-test.db";
            Database.ConnectionString = $"Data Source={this.DatabaseFilename}";
        }

        static UrlTests()
        {
            UrlTests.urlIDs = new();
            UrlTests.generatedUrls = new();
        }

        #endregion

        #region Validation

        [ExpectedException(typeof(ArgumentNullException))]
        [TestMethod]
        public void UpdateUrl_ByDTO_Null()
        {
            this.AssertRequirements();
            Database.QueryProvider = new SqliteQueryProvider();
            Database.UpdateUrl(null);
        }

        [ExpectedException(typeof(ArgumentNullException))]
        [TestMethod]
        public void DeleteUrl_ByDTO_Null()
        {
            this.AssertRequirements();
            Database.QueryProvider = new SqliteQueryProvider();
            Database.UpdateUrl(null);
        }

        #endregion

        [TestMethod]
        public void AddUrl_ByDTO_AddressOnly()
        {
            this.AssertRequirements();
            Database.QueryProvider = new SqliteQueryProvider();

            string address = $"http://{Guid.NewGuid()}";

            Console.WriteLine(address);

            var url = Url.Create(address);
            long id = Database.AddUrl(url);

            this.AddGeneratedUrl(id, address);

            Console.WriteLine(id);

            Assert.IsTrue(id > 0);
        }

        [TestMethod]
        public void AddUrl_ByDTO_AddressAndFoundDate()
        {
            this.AssertRequirements();
            Database.QueryProvider = new SqliteQueryProvider();

            string address = $"http://{Guid.NewGuid()}";

            Console.WriteLine(address);

            var url = Url.Create(address, DateTime.UtcNow);
            long id = Database.AddUrl(url);

            this.AddGeneratedUrl(id, address);

            Console.WriteLine(id);

            Assert.IsTrue(id > 0);
        }

        [TestMethod]
        public void AddUrl_ByDTO_AddressAndFoundAndCollectedDates()
        {
            this.AssertRequirements();
            Database.QueryProvider = new SqliteQueryProvider();

            string address = $"http://{Guid.NewGuid()}";

            Console.WriteLine(address);

            var url = Url.Create(address, DateTime.UtcNow, DateTime.UtcNow, null);
            long id = Database.AddUrl(url);

            this.AddGeneratedUrl(id, address);

            Console.WriteLine(id);

            Assert.IsTrue(id > 0);
        }

        [TestMethod]
        public void AddUrl_ByDTO_AddressAndFoundAndLockedDates()
        {
            this.AssertRequirements();
            Database.QueryProvider = new SqliteQueryProvider();

            string address = $"http://{Guid.NewGuid()}";

            Console.WriteLine(address);

            var url = Url.Create(address, DateTime.UtcNow, null, DateTime.UtcNow);
            long id = Database.AddUrl(url);

            this.AddGeneratedUrl(id, address);

            Console.WriteLine(id);

            Assert.IsTrue(id > 0);
        }

        [TestMethod]
        public void GetUrl_ByAddress()
        {
            this.AssertRequirements();
            Database.QueryProvider = new SqliteQueryProvider();

            Console.WriteLine($"Searching for {UrlTests.generatedUrls[0]}");
            Url url = Database.GetUrl(UrlTests.generatedUrls[0]);

            Assert.IsNotNull(url);

            Console.WriteLine(UrlTests.generatedUrls[0]);
            Console.WriteLine(url.Address);

            Assert.AreEqual(UrlTests.generatedUrls[0], url.Address);
        }

        [TestMethod]
        public void GetUrl_ByID()
        {
            this.AssertRequirements();
            Database.QueryProvider = new SqliteQueryProvider();

            Console.WriteLine($"Searching for {UrlTests.urlIDs[0]}");
            Url url = Database.GetUrl(UrlTests.urlIDs[0]);

            Assert.IsNotNull(url);

            Console.WriteLine(UrlTests.generatedUrls[0]);
            Console.WriteLine(url.Address);

            Assert.AreEqual(UrlTests.generatedUrls[0], url.Address);
        }

        [TestMethod]
        public void UpdateUrl_ByDTO()
        {
            this.AssertRequirements();
            Database.QueryProvider = new SqliteQueryProvider();
            DateTime now = DateTime.Now;

            Console.WriteLine($"Searching for {UrlTests.generatedUrls[0]}");
            Url url = Database.GetUrl(UrlTests.generatedUrls[0]);

            Assert.IsNotNull(url);

            Console.WriteLine($"Update Collected to {now}");
            url.Collected = now;
            Database.UpdateUrl(url);

            Console.WriteLine($"Update IntentLocked to {now.AddSeconds(200)}");
            url.IntentLocked = now.AddSeconds(200);
            Database.UpdateUrl(url);

            // Verify that the stored data can be read as expected
            var validate = Database.GetUrl(url.ID);

            Assert.IsTrue(this.AreDatesSame(now, validate.Collected.Value));
            Assert.IsTrue(this.AreDatesSame(now.AddSeconds(200), validate.IntentLocked.Value));
        }

        [TestMethod]
        public void UpdateUrl_ByDTO_NotFound()
        {
            this.AssertRequirements();
            Database.QueryProvider = new SqliteQueryProvider();

            Console.WriteLine($"Searching for {int.MaxValue}");
            Url url = Database.GetUrl(int.MaxValue);

            if (url != null)
            {
                Assert.Inconclusive();
            }

            // The purpose of this test is to validate the if(dataReader.Read()) logic
        }

        [TestMethod]
        public void UpdateUrl_ByID_CollectedDateOnly()
        {
            this.AssertRequirements();
            Database.QueryProvider = new SqliteQueryProvider();

            Database.UpdateUrl(UrlTests.urlIDs[0], DateTime.Now.AddDays(1), null);
        }

        [TestMethod]
        public void UpdateUrl_ByID_IntentLockedDateOnly()
        {
            this.AssertRequirements();
            Database.QueryProvider = new SqliteQueryProvider();

            Database.UpdateUrl(UrlTests.urlIDs[0], null, DateTime.Now.AddDays(1));
        }

        [TestMethod]
        public void UpdateUrl_ByID_AllDatesFullValues()
        {
            this.AssertRequirements();
            Database.QueryProvider = new SqliteQueryProvider();

            Database.UpdateUrl(UrlTests.urlIDs[0], DateTime.Now.AddDays(2), DateTime.Now.AddDays(2));
        }

        [TestMethod]
        public void UpdateUrl_ByID_AllDatesNull()
        {
            this.AssertRequirements();
            Database.QueryProvider = new SqliteQueryProvider();

            Database.UpdateUrl(UrlTests.urlIDs[0], null, null);
        }

        [TestMethod]
        public void CheckUrlExistance_True()
        {
            this.AssertRequirements();
            Database.QueryProvider = new SqliteQueryProvider();

            Console.WriteLine($"Database.CheckUrlExistance({UrlTests.generatedUrls[0]})");
            var result = Database.CheckUrlExistance(UrlTests.generatedUrls[0]);

            Console.WriteLine(result);
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void CheckUrlExistance_False()
        {
            this.AssertRequirements();
            Database.QueryProvider = new SqliteQueryProvider();

            Console.WriteLine($"Database.CheckUrlExistance({UrlTests.generatedUrls[0]}/not/found)");
            var result = Database.CheckUrlExistance($"{UrlTests.generatedUrls[0]}/not/found");

            Console.WriteLine(result);
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void zDeleteUrl_ByID()
        {
            this.AssertRequirements();

            if (UrlTests.urlIDs.Count == 0)
            {
                Assert.Inconclusive("Need more URL data to test against.");
            }

            Database.QueryProvider = new SqliteQueryProvider();

            var resultCount = Database.DeleteUrl(UrlTests.urlIDs[0]);

            Console.WriteLine($"Deleted {resultCount} rows");

            // Verify that it's actually been deleted
            var verifySearchByID = Database.GetUrl(UrlTests.urlIDs[0]);
            var verifySearchByUrl = Database.GetUrl(UrlTests.generatedUrls[0]);

            Assert.IsNull(verifySearchByID);
            Assert.IsNull(verifySearchByUrl);

            Console.WriteLine($"{UrlTests.generatedUrls[0]} (UrlIntegrationTests.urlIDs[0]) not found in database.");

            // Remove for future tests
            UrlTests.urlIDs.RemoveAt(0);
            UrlTests.generatedUrls.RemoveAt(0);
        }

        [TestMethod]
        public void zDeleteUrl_ByDTO()
        {
            this.AssertRequirements();

            if (UrlTests.urlIDs.Count == 0)
            {
                Assert.Inconclusive("Need more URL data to test against.");
            }

            Database.QueryProvider = new SqliteQueryProvider();

            var urlDto = Database.GetUrl(UrlTests.urlIDs[0]);

            var resultCount = Database.DeleteUrl(urlDto);

            Console.WriteLine($"Deleted {resultCount} rows");

            // Verify that it's actually been deleted
            var verifySearchByID = Database.GetUrl(UrlTests.urlIDs[0]);
            var verifySearchByUrl = Database.GetUrl(UrlTests.generatedUrls[0]);

            Assert.IsNull(verifySearchByID);
            Assert.IsNull(verifySearchByUrl);

            Console.WriteLine($"{UrlTests.generatedUrls[0]} (UrlIntegrationTests.urlIDs[0]) not found in database.");

            // Remove for future tests
            UrlTests.urlIDs.RemoveAt(0);
            UrlTests.generatedUrls.RemoveAt(0);
        }

        private void AddGeneratedUrl(long id, string address)
        {
            UrlTests.urlIDs.Add(id);
            UrlTests.generatedUrls.Add(address);
        }
    }
}
