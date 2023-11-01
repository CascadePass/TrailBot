using CascadePass.TrailBot.DataAccess;
using CascadePass.TrailBot.DataAccess.DTO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Data.Common;

namespace TrailBot.DataAccess.Tests.SqliteQueryProviderTests
{
    [TestClass]
    public class WtaTripReportTests
    {
        #region AddWtaTripReport

        #region Validation

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void AddWtaTripReport_Null()
        {
            SqliteQueryProvider queryProvider = new();
            queryProvider.AddWtaTripReport(null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void AddWtaTripReport_HasID()
        {
            SqliteQueryProvider queryProvider = new();
            WtaTripReport tripReport = new() { Url = new(), ID = 12 };
            queryProvider.AddWtaTripReport(tripReport);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void AddWtaTripReport_Url_Null()
        {
            SqliteQueryProvider queryProvider = new();
            WtaTripReport tripReport = new() { Url = null };
            queryProvider.AddWtaTripReport(tripReport);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void AddWtaTripReport_UrlID_Zero()
        {
            SqliteQueryProvider queryProvider = new();
            WtaTripReport tripReport = new() { Url = new() { ID = 0 } };
            queryProvider.AddWtaTripReport(tripReport);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void AddWtaTripReport_UrlID_Negative()
        {
            SqliteQueryProvider queryProvider = new();
            WtaTripReport tripReport = new() { Url = new() { ID = -1 } };
            queryProvider.AddWtaTripReport(tripReport);
        }

        #endregion

        [TestMethod]
        public void AddWtaTripReport_ResultIsNotNull()
        {
            SqliteQueryProvider queryProvider = new();
            WtaTripReport tripReport = new() { Url = new() { ID = 1 } };
            var result = queryProvider.AddWtaTripReport(tripReport);

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void AddWtaTripReport_ResultIsCorrectType()
        {
            SqliteQueryProvider queryProvider = new();
            WtaTripReport tripReport = new() { Url = new() { ID = 1 } };
            var result = queryProvider.AddWtaTripReport(tripReport);

            Console.WriteLine(result.GetType().FullName);
            Assert.IsTrue(result.GetType().Name == "SQLiteCommand");
        }

        [TestMethod]
        public void AddWtaTripReport_CommandText()
        {
            SqliteQueryProvider queryProvider = new();
            var result = queryProvider.AddWtaTripReport(new() { Url = new() { ID = 1 } });

            Console.WriteLine(result.CommandText);
            Assert.IsTrue(result.CommandText.Trim().ToUpper().StartsWith("INSERT INTO WTATRIPREPORT"));
            Assert.IsTrue(result.CommandText.Trim().ToUpper().Contains("VALUES"));
            Assert.IsTrue(result.CommandText.Trim().ToUpper().Contains("SELECT LAST_INSERT_ROWID();"));
        }

        [TestMethod]
        public void AddWtaTripReport_QueryIsParameterized()
        {
            SqliteQueryProvider queryProvider = new();
            WtaTripReport tripReport = new() { Url = new() { ID = 1, Address = "https://test/" } };
            var result = queryProvider.AddWtaTripReport(tripReport);

            Console.WriteLine(result.CommandText);

            foreach (DbParameter parameter in result.Parameters)
            {
                Console.WriteLine(parameter.ParameterName + "\t" + parameter.Value);
            }

            // None of the input values should appear in the text
            Assert.IsFalse(result.CommandText.Trim().ToUpper().Contains("HTTPS://TEST/"));

            // Instead, there should be parameters with these values
            Assert.IsTrue(result.Parameters.Contains("@UrlID"));
            Assert.AreEqual(1L, result.Parameters["@UrlID"].Value);

            // And these, too
            Assert.IsTrue(result.Parameters.Contains("@Title"));
            Assert.IsTrue(result.Parameters.Contains("@Region"));
            Assert.IsTrue(result.Parameters.Contains("@HikeType"));
            Assert.IsTrue(result.Parameters.Contains("@Author"));
            Assert.IsTrue(result.Parameters.Contains("@TripDate"));
            Assert.IsTrue(result.Parameters.Contains("@PublishedDate"));
            Assert.IsTrue(result.Parameters.Contains("@ReportText"));

            // Not testing @ProcessedDate, because this may get
            // moved to a different table in order to store the
            // complete history.  Validating all of the other
            // parameters should be enough.
        }

        [TestMethod]
        public void AddWtaTripReport_AllDatesAreUnixTimestamps()
        {
            SqliteQueryProvider queryProvider = new();
            WtaTripReport tripReport = new() {
                Url = new() { ID = 1 },
                TripDate = DateTime.Now,
                PublishedDate = DateTime.Now,
                ProcessedDate = DateTime.Now,
            };
            var result = queryProvider.AddWtaTripReport(tripReport);

            Console.WriteLine(result.CommandText);

            foreach (DbParameter parameter in result.Parameters)
            {
                Console.WriteLine(parameter.ParameterName + "\t" + parameter.Value);
            }

            Assert.IsTrue(result.Parameters["@TripDate"].Value is long, $"@TripDate is {result.Parameters["@TripDate"].Value.GetType()}");
            Assert.IsTrue(result.Parameters["@PublishedDate"].Value is long, $"@PublishedDate is {result.Parameters["@PublishedDate"].Value.GetType()}");
            Assert.IsTrue(result.Parameters["@ProcessedDate"].Value is long, $"@ProcessedDate is {result.Parameters["@ProcessedDate"].Value.GetType()}");
        }

        #endregion

        #region UpdateWtaTripReport

        #region Validation

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void UpdateWtaTripReport_Null()
        {
            SqliteQueryProvider queryProvider = new();
            queryProvider.UpdateWtaTripReport(null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void UpdateWtaTripReport_HasNoID()
        {
            SqliteQueryProvider queryProvider = new();
            WtaTripReport tripReport = new() { Url = new() };
            queryProvider.UpdateWtaTripReport(tripReport);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void UpdateWtaTripReport_Url_Null()
        {
            SqliteQueryProvider queryProvider = new();
            WtaTripReport tripReport = new() { Url = null, ID = 1 };
            queryProvider.UpdateWtaTripReport(tripReport);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void UpdateWtaTripReport_UrlID_Zero()
        {
            SqliteQueryProvider queryProvider = new();
            WtaTripReport tripReport = new() { Url = new() { ID = 0 }, ID = 1 };
            queryProvider.UpdateWtaTripReport(tripReport);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void UpdateWtaTripReport_UrlID_Negative()
        {
            SqliteQueryProvider queryProvider = new();
            WtaTripReport tripReport = new() { Url = new() { ID = -1 }, ID = 1 };
            queryProvider.UpdateWtaTripReport(tripReport);
        }

        #endregion

        [TestMethod]
        public void UpdateWtaTripReport_ResultIsNotNull()
        {
            SqliteQueryProvider queryProvider = new();
            WtaTripReport tripReport = new() { Url = new() { ID = 1 }, ID = 1 };
            var result = queryProvider.UpdateWtaTripReport(tripReport);

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void UpdateWtaTripReport_ResultIsCorrectType()
        {
            SqliteQueryProvider queryProvider = new();
            WtaTripReport tripReport = new() { Url = new() { ID = 1 }, ID = 1 };
            var result = queryProvider.UpdateWtaTripReport(tripReport);

            Console.WriteLine(result.GetType().FullName);
            Assert.IsTrue(result.GetType().Name == "SQLiteCommand");
        }

        [TestMethod]
        public void UpdateWtaTripReport_CommandText()
        {
            SqliteQueryProvider queryProvider = new();
            var result = queryProvider.UpdateWtaTripReport(new() { Url = new() { ID = 1 }, ID = 1 });

            Console.WriteLine(result.CommandText);
            Assert.IsTrue(result.CommandText.Trim().ToUpper().StartsWith("UPDATE WTATRIPREPORT"));
            Assert.IsTrue(result.CommandText.Trim().ToUpper().Contains("WHERE WTATRIPREPORTID"));
        }

        [TestMethod]
        public void UpdateWtaTripReport_QueryIsParameterized()
        {
            SqliteQueryProvider queryProvider = new();
            WtaTripReport tripReport = new() { Url = new() { ID = 1, Address = "https://test/" }, ID = 1 };
            var result = queryProvider.UpdateWtaTripReport(tripReport);

            Console.WriteLine(result.CommandText);

            foreach (DbParameter parameter in result.Parameters)
            {
                Console.WriteLine(parameter.ParameterName + "\t" + parameter.Value);
            }

            // None of the input values should appear in the text
            Assert.IsFalse(result.CommandText.Trim().ToUpper().Contains("HTTPS://TEST/"));

            // Instead, there should be parameters with these values
            Assert.IsTrue(result.Parameters.Contains("@UrlID"));
            Assert.AreEqual(1L, result.Parameters["@UrlID"].Value);

            // And these, too
            Assert.IsTrue(result.Parameters.Contains("@Title"));
            Assert.IsTrue(result.Parameters.Contains("@Region"));
            Assert.IsTrue(result.Parameters.Contains("@HikeType"));
            Assert.IsTrue(result.Parameters.Contains("@Author"));
            Assert.IsTrue(result.Parameters.Contains("@TripDate"));
            Assert.IsTrue(result.Parameters.Contains("@PublishedDate"));
            Assert.IsTrue(result.Parameters.Contains("@ReportText"));

            // Not testing @ProcessedDate, because this may get
            // moved to a different table in order to store the
            // complete history.  Validating all of the other
            // parameters should be enough.
        }

        [TestMethod]
        public void UpdateWtaTripReport_UsesCorrectID()
        {
            SqliteQueryProvider queryProvider = new();
            WtaTripReport tripReport = new() {
                ID = 222,
                Url = new() { ID = 333, Address = "https://test/" },
            };
            var result = queryProvider.UpdateWtaTripReport(tripReport);

            Console.WriteLine(result.CommandText);

            foreach (DbParameter parameter in result.Parameters)
            {
                Console.WriteLine(parameter.ParameterName + "\t" + parameter.Value);
            }

            Assert.AreEqual(222L, result.Parameters["@WtaTripReportID"].Value, "Expected TripReportID = 222, not TripReport.Url.ID");

            Assert.AreEqual(333L, result.Parameters["@UrlID"].Value, "Expected UrlID = 333, not TripReport.ID");
        }

        [TestMethod]
        public void UpdateWtaTripReport_AllDatesAreUnixTimestamps()
        {
            SqliteQueryProvider queryProvider = new();
            WtaTripReport tripReport = new()
            {
                ID = 1,
                Url = new() { ID = 1 },
                TripDate = DateTime.Now,
                PublishedDate = DateTime.Now,
                ProcessedDate = DateTime.Now,
            };
            var result = queryProvider.UpdateWtaTripReport(tripReport);

            Console.WriteLine(result.CommandText);

            foreach (DbParameter parameter in result.Parameters)
            {
                Console.WriteLine(parameter.ParameterName + "\t" + parameter.Value);
            }

            Assert.IsTrue(result.Parameters["@TripDate"].Value is long, $"@TripDate is {result.Parameters["@TripDate"].Value.GetType()}");
            Assert.IsTrue(result.Parameters["@PublishedDate"].Value is long, $"@PublishedDate is {result.Parameters["@PublishedDate"].Value.GetType()}");
            Assert.IsTrue(result.Parameters["@ProcessedDate"].Value is long, $"@ProcessedDate is {result.Parameters["@ProcessedDate"].Value.GetType()}");
        }

        #endregion

        #region DeleteWtaTripReport

        #region By DTO

        #region Validation

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void DeleteWtaTripReport_ByDto_Null()
        {
            SqliteQueryProvider queryProvider = new();
            queryProvider.DeleteWtaTripReport(null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void DeleteWtaTripReport_ByDto_HasNoID()
        {
            SqliteQueryProvider queryProvider = new();
            WtaTripReport tripReport = new() { Url = new() };
            queryProvider.DeleteWtaTripReport(tripReport);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void DeleteWtaTripReport_ByDto_NegativeID()
        {
            SqliteQueryProvider queryProvider = new();
            WtaTripReport tripReport = new() { Url = new(), ID = -27 };
            queryProvider.DeleteWtaTripReport(tripReport);
        }

        #endregion

        [TestMethod]
        public void DeleteWtaTripReport_ByDto_ResultIsNotNull()
        {
            SqliteQueryProvider queryProvider = new();
            WtaTripReport tripReport = new() { Url = new() { ID = 1 }, ID = 1 };
            var result = queryProvider.DeleteWtaTripReport(tripReport);

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void DeleteWtaTripReport_ByDto_ResultIsCorrectType()
        {
            SqliteQueryProvider queryProvider = new();
            WtaTripReport tripReport = new() { Url = new() { ID = 1 }, ID = 1 };
            var result = queryProvider.DeleteWtaTripReport(tripReport);

            Console.WriteLine(result.GetType().FullName);
            Assert.IsTrue(result.GetType().Name == "SQLiteCommand");
        }

        [TestMethod]
        public void DeleteWtaTripReport_ByDto_CommandText()
        {
            SqliteQueryProvider queryProvider = new();
            WtaTripReport tripReport = new() { Url = new() { ID = 1 }, ID = 1 };
            var result = queryProvider.DeleteWtaTripReport(tripReport);

            Console.WriteLine(result.CommandText);
            Assert.IsTrue(result.CommandText.Trim().ToUpper().StartsWith("DELETE FROM WTATRIPREPORT"));
            Assert.IsTrue(result.CommandText.Trim().ToUpper().Contains("WHERE WTATRIPREPORTID"));
        }

        [TestMethod]
        public void DeleteWtaTripReport_ByDto_QueryIsParameterized()
        {
            SqliteQueryProvider queryProvider = new();
            WtaTripReport tripReport = new() { Url = new() { ID = 1, Address = "https://test/" }, ID = 2 };
            var result = queryProvider.DeleteWtaTripReport(tripReport);

            Console.WriteLine(result.CommandText);

            foreach (DbParameter parameter in result.Parameters)
            {
                Console.WriteLine(parameter.ParameterName + "\t" + parameter.Value);
            }

            // None of the input values should appear in the text
            Assert.IsFalse(result.CommandText.Trim().ToUpper().Contains("HTTPS://TEST/"));

            // Instead, there should be parameters with these values
            Assert.IsTrue(result.Parameters.Contains("@WtaTripReportID"));
            Assert.AreEqual(2L, result.Parameters["@WtaTripReportID"].Value);
        }

        [TestMethod]
        public void DeleteWtaTripReport_ByDto_UsesCorrectID()
        {
            SqliteQueryProvider queryProvider = new();
            WtaTripReport tripReport = new()
            {
                ID = 222,
                Url = new() { ID = 333, Address = "https://test/" },
            };
            var result = queryProvider.DeleteWtaTripReport(tripReport);

            Console.WriteLine(result.CommandText);

            foreach (DbParameter parameter in result.Parameters)
            {
                Console.WriteLine(parameter.ParameterName + "\t" + parameter.Value);
            }

            Assert.AreEqual(222L, result.Parameters["@WtaTripReportID"].Value, "Expected TripReportID = 222, not TripReport.Url.ID");
        }


        #endregion

        #region By ID

        #region Validation

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void DeleteWtaTripReport_ByID_Zero()
        {
            SqliteQueryProvider queryProvider = new();
            queryProvider.DeleteWtaTripReport(0);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void DeleteWtaTripReport_ByID_Negative()
        {
            SqliteQueryProvider queryProvider = new();
            queryProvider.DeleteWtaTripReport(-12);
        }

        #endregion

        [TestMethod]
        public void DeleteWtaTripReport_ByID_ResultIsNotNull()
        {
            SqliteQueryProvider queryProvider = new();
            var result = queryProvider.DeleteWtaTripReport(2);

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void DeleteWtaTripReport_ByID_ResultIsCorrectType()
        {
            SqliteQueryProvider queryProvider = new();
            var result = queryProvider.DeleteWtaTripReport(1);

            Console.WriteLine(result.GetType().FullName);
            Assert.IsTrue(result.GetType().Name == "SQLiteCommand");
        }

        [TestMethod]
        public void DeleteWtaTripReport_ByID_CommandText()
        {
            SqliteQueryProvider queryProvider = new();
            var result = queryProvider.DeleteWtaTripReport(38);

            Console.WriteLine(result.CommandText);
            Assert.IsTrue(result.CommandText.Trim().ToUpper().StartsWith("DELETE FROM WTATRIPREPORT"));
            Assert.IsTrue(result.CommandText.Trim().ToUpper().Contains("WHERE WTATRIPREPORTID"));
        }

        [TestMethod]
        public void DeleteWtaTripReport_ByID_QueryIsParameterized()
        {
            SqliteQueryProvider queryProvider = new();
            var result = queryProvider.DeleteWtaTripReport(492);

            Console.WriteLine(result.CommandText);

            foreach (DbParameter parameter in result.Parameters)
            {
                Console.WriteLine(parameter.ParameterName + "\t" + parameter.Value);
            }

            // None of the input values should appear in the text
            Assert.IsFalse(result.CommandText.Trim().ToUpper().Contains("HTTPS://TEST/"));

            // Instead, there should be parameters with these values
            Assert.IsTrue(result.Parameters.Contains("@WtaTripReportID"));
            Assert.AreEqual(492L, result.Parameters["@WtaTripReportID"].Value);
        }

        #endregion

        #endregion


        #region GetWtaTripReport (by ID)

        #region ID must be in range

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void GetWtaTripReport_Zero()
        {
            SqliteQueryProvider queryProvider = new();
            queryProvider.GetWtaTripReport(0);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void GetWtaTripReport_Negative()
        {
            SqliteQueryProvider queryProvider = new();
            queryProvider.GetWtaTripReport(-25);
        }

        #endregion

        [TestMethod]
        public void GetWtaTripReport_ResultIsNotNull()
        {
            SqliteQueryProvider queryProvider = new();
            var result = queryProvider.GetWtaTripReport(100);

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void GetWtaTripReport_ResultIsCorrectType()
        {
            SqliteQueryProvider queryProvider = new();
            var result = queryProvider.GetWtaTripReport(100);

            Console.WriteLine(result.GetType().FullName);
            Assert.IsTrue(result.GetType().Name == "SQLiteCommand");
        }

        [TestMethod]
        public void GetWtaTripReport_CommandText()
        {
            SqliteQueryProvider queryProvider = new();
            var result = queryProvider.GetWtaTripReport(100);

            Console.WriteLine(result.CommandText);
            Assert.IsTrue(result.CommandText.Trim().ToUpper().StartsWith("SELECT"));
            Assert.IsTrue(result.CommandText.Trim().ToUpper().Contains("FROM WTATRIPREPORT"));
        }

        [TestMethod]
        public void GetWtaTripReport_QueryIsParameterized()
        {
            SqliteQueryProvider queryProvider = new();
            var result = queryProvider.GetWtaTripReport(int.MaxValue);

            Console.WriteLine(result.CommandText);

            foreach (DbParameter parameter in result.Parameters)
            {
                Console.WriteLine(parameter.ParameterName + "\t" + parameter.Value);
            }

            Assert.IsTrue(result.Parameters.Contains("@WtaTripReportID"));
            Assert.IsFalse(result.CommandText.Contains(int.MaxValue.ToString()));
        }

        #endregion

    }
}
