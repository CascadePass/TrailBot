using CascadePass.TrailBot.DataAccess;
using CascadePass.TrailBot.DataAccess.DTO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace TrailBot.DataAccess.Tests
{
    [TestClass]
    public class DatabaseTests
    {
        [TestMethod]
        public void CanAccessStaticClass()
        {
            _ = Database.ConnectionString;

            // A static object still needs to be constructed,
            // this test is intended to catch any problems
            // creating the object.
        }

        #region Property Correctness (get/set access same backing object)

        [TestMethod]
        public void ConnectionString_GetSetAccessSameValue()
        {
            string current = Database.ConnectionString;
            string expected = Database.ConnectionString = Guid.NewGuid().ToString();
            string actual = Database.ConnectionString;

            bool pass = string.Equals(expected, actual);

            Database.ConnectionString = current;

            Assert.IsTrue(pass, $"Database.ConnectionString returned {actual}, {expected} was expected.");
        }

        [TestMethod]
        public void QueryProvider_GetSetAccessSameValue()
        {
            ITrailBotQueryProvider current = Database.QueryProvider;
            ITrailBotQueryProvider expected = Database.QueryProvider = new SqliteQueryProvider();
            ITrailBotQueryProvider actual = Database.QueryProvider;

            bool pass = actual == expected;

            Database.QueryProvider = current;

            Assert.IsTrue(pass, $"Database.QueryProvider returned {actual}, {expected} was expected.");
        }

        #endregion

        #region Exceptions

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void GetConnection_ThrowsWithNoConnectionString()
        {
            Database.ConnectionString = null;
            _ = Database.GetConnection();
        }

        [ExpectedException(typeof(ArgumentNullException))]
        [TestMethod]
        public void DeleteUrl_Null()
        {
            Database.QueryProvider = new SqliteQueryProvider();
            Database.DeleteUrl(null);
        }

        [ExpectedException(typeof(ArgumentNullException))]
        [TestMethod]
        public void UpdateUrl_Null()
        {
            Database.QueryProvider = new SqliteQueryProvider();
            Database.UpdateUrl(null);
        }

        #region AddWtaTripReport

        [ExpectedException(typeof(ArgumentNullException))]
        [TestMethod]
        public void AddWtaTripReport_Null()
        {
            Database.QueryProvider = new SqliteQueryProvider();
            Database.AddWtaTripReport(null);
        }

        [ExpectedException(typeof(ArgumentException))]
        [TestMethod]
        public void AddWtaTripReport_UrlProperty_Null()
        {
            Database.QueryProvider = new SqliteQueryProvider();
            Database.AddWtaTripReport(new() { Url = null });
        }

        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        [TestMethod]
        public void AddWtaTripReport_AlreadyHasID()
        {
            Database.QueryProvider = new SqliteQueryProvider();

            WtaTripReport tr = new() { Url = new() { Address = Guid.NewGuid().ToString() } };
            tr.ID = 1;

            Database.AddWtaTripReport(tr);
        }

        #endregion

        [ExpectedException(typeof(ArgumentNullException))]
        [TestMethod]
        public void UpdateWtaTripReport_Null()
        {
            Database.QueryProvider = new SqliteQueryProvider();
            Database.UpdateWtaTripReport(null);
        }

        [ExpectedException(typeof(ArgumentException))]
        [TestMethod]
        public void UpdateWtaTripReport_UrlProperty_Null()
        {
            Database.QueryProvider = new SqliteQueryProvider();
            Database.UpdateWtaTripReport(new() { Url = null });
        }

        #endregion
    }
}
