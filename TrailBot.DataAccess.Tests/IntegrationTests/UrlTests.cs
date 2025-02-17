using CascadePass.TrailBot.DataAccess;
using CascadePass.TrailBot.DataAccess.DTO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace TrailBot.DataAccess.Tests.IntegrationTests
{
    [TestClass]
    public class UrlTests : SqliteIntegrationTestClass
    {
        #region Validation

        [ExpectedException(typeof(ArgumentNullException))]
        [TestMethod]
        public void UpdateUrl_ByDTO_Null()
        {
            this.AssertRequirements();
            Database.QueryProvider = new SqliteQueryProvider();
            Database.Update((Url)null);
        }

        [ExpectedException(typeof(ArgumentNullException))]
        [TestMethod]
        public void DeleteUrl_ByDTO_Null()
        {
            this.AssertRequirements();
            Database.QueryProvider = new SqliteQueryProvider();
            Database.Update((Url)null);
        }

        #endregion

        #region AddUrl

        #region Use implicitly managed connection

        [TestMethod]
        public void AddUrl_AddressOnly()
        {
            this.AssertRequirements();
            Database.QueryProvider = new SqliteQueryProvider();

            string address = $"https://UrlIntegrationTest.net/{Guid.NewGuid()}";

            Console.WriteLine(address);

            var url = Url.Create(address);
            long id = Database.Add(url);

            Console.WriteLine(id);

            Assert.IsTrue(id > 0);

            var validate = Database.GetUrl(id);
            Assert.IsNotNull(validate);

            this.AssertSameUrl(url, validate);

            Database.DeleteUrl(id);
        }

        [TestMethod]
        public void AddUrl_AddressAndFoundDate()
        {
            this.AssertRequirements();
            Database.QueryProvider = new SqliteQueryProvider();

            string address = $"https://UrlIntegrationTest.net/{Guid.NewGuid()}";

            Console.WriteLine(address);

            var url = Url.Create(address, DateTime.Now);
            long id = Database.Add(url);

            Console.WriteLine(id);

            Assert.IsTrue(id > 0);

            var validate = Database.GetUrl(id);
            Assert.IsNotNull(validate);

            this.AssertSameUrl(url, validate);

            Database.DeleteUrl(id);
        }

        [TestMethod]
        public void AddUrl_AddressAndFoundAndCollectedDates()
        {
            this.AssertRequirements();
            Database.QueryProvider = new SqliteQueryProvider();

            string address = $"https://UrlIntegrationTest.net/{Guid.NewGuid()}";

            Console.WriteLine(address);

            var url = Url.Create(address, DateTime.Now, DateTime.Now, null);
            long id = Database.Add(url);

            Console.WriteLine(id);

            Assert.IsTrue(id > 0);

            var validate = Database.GetUrl(id);
            Assert.IsNotNull(validate);

            this.AssertSameUrl(url, validate);

            Database.DeleteUrl(id);
        }

        [TestMethod]
        public void AddUrl_AddressAndFoundAndLockedDates()
        {
            this.AssertRequirements();
            Database.QueryProvider = new SqliteQueryProvider();

            string address = $"https://UrlIntegrationTest.net/{Guid.NewGuid()}";

            Console.WriteLine(address);

            var url = Url.Create(address, DateTime.Now, null, DateTime.Now);
            long id = Database.Add(url);

            Console.WriteLine(id);

            Assert.IsTrue(id > 0);

            var validate = Database.GetUrl(id);
            Assert.IsNotNull(validate);

            this.AssertSameUrl(url, validate);

            Database.DeleteUrl(id);
        }

        #endregion

        #region Shared connection

        #region AddUrl still behaves as normal

        [TestMethod]
        public void AddUrl_SharedConnection_AddressOnly()
        {
            this.AssertRequirements();
            Database.QueryProvider = new SqliteQueryProvider();

            string address = $"https://UrlIntegrationTest.net/{Guid.NewGuid()}";

            Console.WriteLine(address);

            using var connection = Database.GetConnection();
            var url = Url.Create(address);
            long id = Database.Add(url, connection);

            Console.WriteLine(id);

            Assert.IsTrue(id > 0);

            var validate = Database.GetUrl(id);
            Assert.IsNotNull(validate);

            this.AssertSameUrl(url, validate);

            Database.DeleteUrl(id);
        }

        [TestMethod]
        public void AddUrl_SharedConnection_AddressAndFoundDate()
        {
            this.AssertRequirements();
            Database.QueryProvider = new SqliteQueryProvider();

            string address = $"https://UrlIntegrationTest.net/{Guid.NewGuid()}";

            Console.WriteLine(address);

            using var connection = Database.GetConnection();
            var url = Url.Create(address, DateTime.Now);
            long id = Database.Add(url, connection);

            Console.WriteLine(id);

            Assert.IsTrue(id > 0);

            var validate = Database.GetUrl(id);
            Assert.IsNotNull(validate);

            this.AssertSameUrl(url, validate);

            Database.DeleteUrl(id);
        }

        [TestMethod]
        public void AddUrl_SharedConnection_AddressAndFoundAndCollectedDates()
        {
            this.AssertRequirements();
            Database.QueryProvider = new SqliteQueryProvider();

            string address = $"https://UrlIntegrationTest.net/{Guid.NewGuid()}";

            Console.WriteLine(address);

            using var connection = Database.GetConnection();
            var url = Url.Create(address, DateTime.Now, DateTime.Now, null);
            long id = Database.Add(url, connection);

            Console.WriteLine(id);

            Assert.IsTrue(id > 0);

            var validate = Database.GetUrl(id);
            Assert.IsNotNull(validate);

            this.AssertSameUrl(url, validate);

            Database.DeleteUrl(id);
        }

        [TestMethod]
        public void AddUrl_SharedConnection_AddressAndFoundAndLockedDates()
        {
            this.AssertRequirements();
            Database.QueryProvider = new SqliteQueryProvider();

            string address = $"https://UrlIntegrationTest.net/{Guid.NewGuid()}";

            Console.WriteLine(address);

            using var connection = Database.GetConnection();
            var url = Url.Create(address, DateTime.Now, null, DateTime.Now);
            long id = Database.Add(url, connection);

            Console.WriteLine(id);

            Assert.IsTrue(id > 0);

            var validate = Database.GetUrl(id);
            Assert.IsNotNull(validate);

            this.AssertSameUrl(url, validate);

            Database.DeleteUrl(id);
        }

        #endregion

        #region Connection handled properly

        [TestMethod]
        public void AddUrl_SharedConnection_NotClosed()
        {
            this.AssertRequirements();
            Database.QueryProvider = new SqliteQueryProvider();

            string address = $"https://UrlIntegrationTest.net/{Guid.NewGuid()}";

            Console.WriteLine(address);

            using var connection = Database.GetConnection();
            var originalConnectionState = connection.State;

            Console.WriteLine($"Connection started {connection.State}");

            var url = Url.Create(address);
            long id = Database.Add(url, connection);

            Console.WriteLine($"Generated ID={id}");
            Console.WriteLine($"Connection is {connection.State} after AddUrl call");

            Assert.IsTrue(id > 0);
            Assert.AreEqual(originalConnectionState, connection.State);

            var validate = Database.GetUrl(id);
            Assert.IsNotNull(validate);

            this.AssertSameUrl(url, validate);

            Database.DeleteUrl(id);
        }

        #endregion

        #endregion

        #endregion

        [TestMethod]
        public void GetUrl_ByAddress()
        {
            this.AssertRequirements();
            Database.QueryProvider = new SqliteQueryProvider();

            string address = $"https://UrlIntegrationTest.net/{Guid.NewGuid()}";
            var created = Url.Create(address);
            Database.Add(created);

            Console.WriteLine($"Searching for {created.Address}");
            Url url = Database.GetUrl(created.Address);

            Assert.IsNotNull(url);

            Assert.AreEqual(address, url.Address);

            Database.DeleteUrl(url.ID);
        }

        [TestMethod]
        public void GetUrl_ByID()
        {
            this.AssertRequirements();
            Database.QueryProvider = new SqliteQueryProvider();

            string address = $"https://UrlIntegrationTest.net/{Guid.NewGuid()}";
            var created = Url.Create(address);
            Database.Add(created);

            Console.WriteLine($"Searching for {created.ID}");
            Url url = Database.GetUrl(created.ID);

            Assert.IsNotNull(url);

            Assert.AreEqual(address, url.Address);
            Assert.AreEqual(created.ID, url.ID);

            Database.DeleteUrl(url.ID);

        }

        [TestMethod]
        public void UpdateUrl_ByDTO()
        {
            this.AssertRequirements();
            Database.QueryProvider = new SqliteQueryProvider();
            DateTime now = DateTime.Now;

            string address = $"https://UrlIntegrationTest.net/{Guid.NewGuid()}";

            Console.WriteLine(address);

            var originalCreated = Url.Create(address);
            long id = Database.Add(originalCreated);

            Console.WriteLine($"Searching for {originalCreated.Address}");
            Url url = Database.GetUrl(originalCreated.Address);

            Assert.IsNotNull(url);

            Console.WriteLine($"Update Collected to {now}");
            url.Collected = now;
            Database.Update(url);

            Console.WriteLine($"Update IntentLocked to {now.AddSeconds(200)}");
            url.IntentLocked = now.AddSeconds(200);
            Database.Update(url);

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

            string address = $"https://UrlIntegrationTest.net/{Guid.NewGuid()}";

            Console.WriteLine(address);

            var originalCreated = Url.Create(address);
            long id = Database.Add(originalCreated);

            var collectedDate = DateTime.Now.AddDays(1);

            Database.UpdateUrl(originalCreated.ID, collectedDate, null);

            var validate = Database.GetUrl(originalCreated.ID);
            Assert.IsNotNull(validate);

            Assert.IsTrue(this.AreDatesSame(collectedDate, validate.Collected));
            Assert.IsNull(validate.IntentLocked);
        }

        [TestMethod]
        public void UpdateUrl_ByID_IntentLockedDateOnly()
        {
            this.AssertRequirements();
            Database.QueryProvider = new SqliteQueryProvider();

            string address = $"https://UrlIntegrationTest.net/{Guid.NewGuid()}";
            Console.WriteLine(address);

            var originalCreated = Url.Create(address);
            long id = Database.Add(originalCreated);

            var intentLockedDate = DateTime.Now.AddDays(1);

            Database.UpdateUrl(originalCreated.ID, null, intentLockedDate);

            var validate = Database.GetUrl(originalCreated.ID);
            Assert.IsNotNull(validate);

            Assert.IsTrue(this.AreDatesSame(intentLockedDate, validate.IntentLocked));
            Assert.IsNull(validate.Collected);
        }

        [TestMethod]
        public void UpdateUrl_ByID_AllDatesFullValues()
        {
            this.AssertRequirements();
            Database.QueryProvider = new SqliteQueryProvider();

            string address = $"https://UrlIntegrationTest.net/{Guid.NewGuid()}";
            Console.WriteLine(address);

            var originalCreated = Url.Create(address);
            long id = Database.Add(originalCreated);

            var collectedDate = DateTime.Now.AddDays(2);
            var intentLockedDate = DateTime.Now.AddDays(3);

            Database.UpdateUrl(originalCreated.ID, collectedDate, intentLockedDate);

            var validate = Database.GetUrl(originalCreated.ID);
            Assert.IsNotNull(validate);

            Assert.IsTrue(this.AreDatesSame(intentLockedDate, validate.IntentLocked));
            Assert.IsTrue(this.AreDatesSame(collectedDate, validate.Collected));
        }

        [TestMethod]
        public void UpdateUrl_ByID_AllDatesNull()
        {
            this.AssertRequirements();
            Database.QueryProvider = new SqliteQueryProvider();

            string address = $"https://UrlIntegrationTest.net/{Guid.NewGuid()}";
            Console.WriteLine(address);

            var originalCreated = Url.Create(address);
            long id = Database.Add(originalCreated);

            Database.UpdateUrl(originalCreated.ID, null, null);

            var validate = Database.GetUrl(originalCreated.ID);
            Assert.IsNotNull(validate);
            Assert.IsNull(validate.Collected);
            Assert.IsNull(validate.IntentLocked);

            Database.DeleteUrl(originalCreated.ID);
        }

        [TestMethod]
        public void CheckUrlExistance_True()
        {
            this.AssertRequirements();
            Database.QueryProvider = new SqliteQueryProvider();

            string address = $"https://UrlIntegrationTest.net/{Guid.NewGuid()}";
            var originalCreated = Url.Create(address);
            long id = Database.Add(originalCreated);

            Console.WriteLine($"Database.CheckUrlExistance({address})");
            var result = Database.CheckUrlExistance(address);

            Console.WriteLine(result);
            Assert.IsTrue(result);

            Database.DeleteUrl(id);
        }

        [TestMethod]
        public void CheckUrlExistance_False()
        {
            this.AssertRequirements();
            Database.QueryProvider = new SqliteQueryProvider();

            string badAddress = $"https://negative.test/CheckUrlExistance_False/{Guid.NewGuid()}/not/found";

            Console.WriteLine($"Database.CheckUrlExistance({badAddress})");
            var result = Database.CheckUrlExistance(badAddress);

            Console.WriteLine(result);
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void DeleteUrl_ByID()
        {
            this.AssertRequirements();
            Database.QueryProvider = new SqliteQueryProvider();

            string address = $"https://UrlIntegrationTest.net/{Guid.NewGuid()}";
            var originalCreated = Url.Create(address);
            long id = Database.Add(originalCreated);

            var resultCount = Database.DeleteUrl(id);

            Console.WriteLine($"Deleted {resultCount} rows");

            // Verify that it's actually been deleted
            var verifySearchByID = Database.GetUrl(id);
            var verifySearchByUrl = Database.GetUrl(originalCreated.Address);

            Assert.IsNull(verifySearchByID);
            Assert.IsNull(verifySearchByUrl);

            Console.WriteLine($"{id} not found in database.");
        }

        [TestMethod]
        public void DeleteUrl_ByDTO()
        {
            this.AssertRequirements();
            Database.QueryProvider = new SqliteQueryProvider();

            string address = $"https://UrlIntegrationTest.net/{Guid.NewGuid()}";
            var originalCreated = Url.Create(address);
            long id = Database.Add(originalCreated);

            var urlDto = Database.GetUrl(id);

            var resultCount = Database.Delete(urlDto);

            Console.WriteLine($"Deleted {resultCount} rows");

            // Verify that it's actually been deleted
            var verifySearchByID = Database.GetUrl(id);
            var verifySearchByUrl = Database.GetUrl(originalCreated.Address);

            Assert.IsNull(verifySearchByID);
            Assert.IsNull(verifySearchByUrl);

            Console.WriteLine($"{id} not found in database.");
        }

        #region Private utility methods

        private Url GetRandomUrl()
        {
            return this.GetRandomUrl(0);
        }

        private Url GetRandomUrl(int id)
        {
            Random random = new();

            return new()
            {
                ID = id,
                Address = $"https://UrlIntegrationTest.net/{Guid.NewGuid()}",
                Found = this.GetRandomDateTime(),
                Collected = this.GetRandomDateTime(),
                IntentLocked = this.GetRandomDateTime(),
            };
        }

        private void AssertSameUrl(Url url1, Url url2)
        {
            Assert.AreEqual(url1.ID, url2.ID, "ID doesn't match");
            Assert.AreEqual(url1.Address, url2.Address, "Name doesn't match");

            // Found gets a default value of "now"
            if (url1.Found.HasValue && url2.Found.HasValue)
            {
                Assert.IsTrue(this.AreDatesSame(url1.Found, url2.Found), $"Found doesn't match: '{url1.Found}' vs '{url2.Found}'");
            }

            Assert.IsTrue(this.AreDatesSame(url1.Collected, url2.Collected), $"Collected doesn't match: '{url1.Collected}' vs '{url2.Collected}'");
            Assert.IsTrue(this.AreDatesSame(url1.IntentLocked, url2.IntentLocked), $"IntentLocked doesn't match: '{url1.IntentLocked}' vs '{url2.IntentLocked}'");
        }

        #endregion
    }
}
