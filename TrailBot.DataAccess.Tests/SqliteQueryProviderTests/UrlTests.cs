using CascadePass.TrailBot.DataAccess;
using CascadePass.TrailBot.DataAccess.DTO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Data.Common;

namespace TrailBot.DataAccess.Tests.SqliteQueryProviderTests
{
    [TestClass]
    public class UrlTests
    {
        #region AddUrl

        #region Validation

        #region Url can't be blank

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void AddUrl_Null()
        {
            SqliteQueryProvider queryProvider = new();
            queryProvider.AddUrl(null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void AddUrl_Url_Null()
        {
            SqliteQueryProvider queryProvider = new();
            queryProvider.AddUrl(Url.Create(null, DateTime.Now, DateTime.Now, DateTime.Now));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void AddUrl_Url_EmptyString()
        {
            SqliteQueryProvider queryProvider = new();
            queryProvider.AddUrl(Url.Create(string.Empty, DateTime.Now, DateTime.Now, DateTime.Now));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void AddUrl_Url_WhiteSpace()
        {
            SqliteQueryProvider queryProvider = new();
            queryProvider.AddUrl(Url.Create(" ", DateTime.Now, DateTime.Now, DateTime.Now));
        }

        #endregion

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void AddUrl_HasID()
        {
            SqliteQueryProvider queryProvider = new();
            queryProvider.AddUrl(new Url() { ID = 25, Address = "https://test/" });
        }

        #endregion

        [TestMethod]
        public void AddUrl_ResultIsNotNull()
        {
            SqliteQueryProvider queryProvider = new();
            var result = queryProvider.AddUrl(Url.Create("https://test/", DateTime.Now, DateTime.Now, DateTime.Now));

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void AddUrl_ResultIsCorrectType()
        {
            SqliteQueryProvider queryProvider = new();
            var result = queryProvider.AddUrl(Url.Create("https://test/", DateTime.Now, DateTime.Now, DateTime.Now));

            Console.WriteLine(result.GetType().FullName);
            Assert.IsTrue(result.GetType().Name == "SQLiteCommand");
        }

        [TestMethod]
        public void AddUrl_CommandText()
        {
            SqliteQueryProvider queryProvider = new();
            var result = queryProvider.AddUrl(Url.Create("https://test/", DateTime.Now, DateTime.Now, DateTime.Now));

            Console.WriteLine(result.CommandText);
            Assert.IsTrue(result.CommandText.Trim().ToUpper().StartsWith("INSERT INTO URL"));
            Assert.IsTrue(result.CommandText.Trim().ToUpper().Contains("VALUES"));
            Assert.IsTrue(result.CommandText.Trim().ToUpper().Contains("SELECT LAST_INSERT_ROWID();"));
        }

        [TestMethod]
        public void AddUrl_QueryIsParameterized()
        {
            SqliteQueryProvider queryProvider = new();
            var result = queryProvider.AddUrl(Url.Create("https://test/", DateTime.Now, DateTime.Now, DateTime.Now));

            Console.WriteLine(result.CommandText);

            foreach (DbParameter parameter in result.Parameters)
            {
                Console.WriteLine(parameter.ParameterName + "\t" + parameter.Value);
            }

            // None of the input values should appear in the text
            Assert.IsFalse(result.CommandText.Trim().ToUpper().Contains("HTTPS://TEST/"));

            // Instead, there should be parameters with these values
            Assert.IsTrue(result.Parameters.Contains("@Url"));
            Assert.AreEqual("https://test/", result.Parameters["@Url"].Value);
        }

        [TestMethod]
        public void AddUrl_NullFoundDateDefaultsToNow()
        {
            SqliteQueryProvider queryProvider = new();
            var result = queryProvider.AddUrl(Url.Create("https://test/", null, DateTime.Now, DateTime.Now));

            Console.WriteLine(result.CommandText);

            foreach (DbParameter parameter in result.Parameters)
            {
                Console.WriteLine(parameter.ParameterName + "\t" + parameter.Value);
            }

            long linuxTimestamp = (long)result.Parameters["@FoundDate"].Value;
            DateTime timestamp = DataTransferObject.ToDateTime(linuxTimestamp);
            TimeSpan elapsed = DateTime.Now - timestamp;

            Assert.IsTrue(elapsed.Seconds < 2);
        }

        [TestMethod]
        public void AddUrl_AllDatesAreUnixTimestamps_Explicit()
        {
            SqliteQueryProvider queryProvider = new();
            var result = queryProvider.AddUrl(Url.Create("https://test/", DateTime.Now, DateTime.Now, DateTime.Now));

            Console.WriteLine(result.CommandText);

            foreach (DbParameter parameter in result.Parameters)
            {
                Console.WriteLine(parameter.ParameterName + "\t" + parameter.Value);
            }

            Assert.IsTrue(result.Parameters["@FoundDate"].Value is long, $"@FoundDate is {result.Parameters["@FoundDate"].Value.GetType()}");
            Assert.IsTrue(result.Parameters["@CollectedDate"].Value is long, $"@CollectedDate is {result.Parameters["@CollectedDate"].Value.GetType()}");
            Assert.IsTrue(result.Parameters["@IntentLockedDate"].Value is long, $"@IntentLockedDate is {result.Parameters["@IntentLockedDate"].Value.GetType()}");
        }

        [TestMethod]
        public void AddUrl_AllDatesAreUnixTimestamps_Default()
        {
            SqliteQueryProvider queryProvider = new();
            var result = queryProvider.AddUrl(Url.Create("https://test/", null, null, null));

            Console.WriteLine(result.CommandText);

            foreach (DbParameter parameter in result.Parameters)
            {
                Console.WriteLine(parameter.ParameterName + "\t" + parameter.Value);
            }

            Assert.IsTrue(result.Parameters["@FoundDate"].Value is long, $"@FoundDate is {result.Parameters["@FoundDate"].Value.GetType()}");
            Assert.IsTrue(result.Parameters["@CollectedDate"].Value == Convert.DBNull, $"@CollectedDate is {result.Parameters["@CollectedDate"].Value.GetType()}");
            Assert.IsTrue(result.Parameters["@IntentLockedDate"].Value == Convert.DBNull, $"@IntentLockedDate is {result.Parameters["@IntentLockedDate"].Value.GetType()}");

        }

        #endregion

        #region UpdateUrl

        #region ID must be in range

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void UpdateUrl_Zero()
        {
            SqliteQueryProvider queryProvider = new();
            queryProvider.UpdateUrl(0, DateTime.Now, DateTime.Now);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void UpdateUrl_Negative()
        {
            SqliteQueryProvider queryProvider = new();
            queryProvider.UpdateUrl(-25, DateTime.Now, DateTime.Now);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void UpdateUrl_Null()
        {
            SqliteQueryProvider queryProvider = new();
            queryProvider.UpdateUrl(null);
        }

        #endregion

        [TestMethod]
        public void UpdateUrl_ResultIsNotNull()
        {
            SqliteQueryProvider queryProvider = new();
            var result = queryProvider.UpdateUrl(100, DateTime.Now, DateTime.Now);

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void UpdateUrl_ResultIsCorrectType()
        {
            SqliteQueryProvider queryProvider = new();
            var result = queryProvider.UpdateUrl(100, DateTime.Now, DateTime.Now);

            Console.WriteLine(result.GetType().FullName);
            Assert.IsTrue(result.GetType().Name == "SQLiteCommand");
        }

        [TestMethod]
        public void UpdateUrl_CommandText()
        {
            SqliteQueryProvider queryProvider = new();
            var result = queryProvider.UpdateUrl(100, DateTime.Now, DateTime.Now);

            Console.WriteLine(result.CommandText);
            Assert.IsTrue(result.CommandText.Trim().ToUpper().StartsWith("UPDATE URL"));
        }

        [TestMethod]
        public void UpdateUrl_QueryIsParameterized()
        {
            SqliteQueryProvider queryProvider = new();
            var result = queryProvider.UpdateUrl(100, DateTime.Now, DateTime.Now);

            Console.WriteLine(result.CommandText);

            foreach (DbParameter parameter in result.Parameters)
            {
                Console.WriteLine(parameter.ParameterName + "\t" + parameter.Value);
            }

            Assert.IsTrue(result.Parameters.Contains("@CollectedDate"));
            Assert.IsTrue(result.Parameters.Contains("@IntentLockedDate"));
        }

        [TestMethod]
        public void UpdateUrl_CannotEditUrl()
        {
            SqliteQueryProvider queryProvider = new();
            var result = queryProvider.UpdateUrl(100, DateTime.Now, DateTime.Now);

            Console.WriteLine(result.CommandText);

            foreach (DbParameter parameter in result.Parameters)
            {
                Console.WriteLine(parameter.ParameterName + "\t" + parameter.Value);
            }

            Assert.IsFalse(result.Parameters.Contains("@Url"));
        }

        [TestMethod]
        public void UpdateUrl_CannotEditFoundDate()
        {
            SqliteQueryProvider queryProvider = new();
            var result = queryProvider.UpdateUrl(100, DateTime.Now, DateTime.Now);

            Console.WriteLine(result.CommandText);

            foreach (DbParameter parameter in result.Parameters)
            {
                Console.WriteLine(parameter.ParameterName + "\t" + parameter.Value);
            }

            Assert.IsFalse(result.Parameters.Contains("@FoundDate"));
        }

        [TestMethod]
        public void UpdateUrl_AllDatesAreUnixTimestamps_Explicit()
        {
            SqliteQueryProvider queryProvider = new();
            var result = queryProvider.UpdateUrl(500, DateTime.Now, DateTime.Now);

            Console.WriteLine(result.CommandText);

            foreach (DbParameter parameter in result.Parameters)
            {
                Console.WriteLine(parameter.ParameterName + "\t" + parameter.Value);
            }

            Assert.IsTrue(result.Parameters["@CollectedDate"].Value is long, $"@CollectedDate is {result.Parameters["@CollectedDate"].Value.GetType()}");
            Assert.IsTrue(result.Parameters["@IntentLockedDate"].Value is long, $"@IntentLockedDate is {result.Parameters["@IntentLockedDate"].Value.GetType()}");
        }

        [TestMethod]
        public void UpdateUrl_AllDatesAreUnixTimestamps_Default()
        {
            SqliteQueryProvider queryProvider = new();
            var result = queryProvider.UpdateUrl(500, null, null);

            Console.WriteLine(result.CommandText);

            foreach (DbParameter parameter in result.Parameters)
            {
                Console.WriteLine(parameter.ParameterName + "\t" + parameter.Value);
            }

            Assert.IsTrue(result.Parameters["@CollectedDate"].Value == Convert.DBNull, $"@CollectedDate is {result.Parameters["@CollectedDate"].Value.GetType()}");
            Assert.IsTrue(result.Parameters["@IntentLockedDate"].Value == Convert.DBNull, $"@IntentLockedDate is {result.Parameters["@IntentLockedDate"].Value.GetType()}");

        }

        #endregion

        #region DeleteUrl

        #region By ID

        #region Validation (ID must be in range)

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void DeleteUrl_ID_Zero()
        {
            SqliteQueryProvider queryProvider = new();
            queryProvider.DeleteUrl(0);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void DeleteUrl_ID_Negative()
        {
            SqliteQueryProvider queryProvider = new();
            queryProvider.DeleteUrl(-25);
        }

        #endregion

        #region Correctness

        [TestMethod]
        public void DeleteUrl_ResultIsNotNull()
        {
            SqliteQueryProvider queryProvider = new();
            var result = queryProvider.DeleteUrl(100);

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void DeleteUrl_ResultIsCorrectType()
        {
            SqliteQueryProvider queryProvider = new();
            var result = queryProvider.DeleteUrl(100);

            Console.WriteLine(result.GetType().FullName);
            Assert.IsTrue(result.GetType().Name == "SQLiteCommand");
        }

        [TestMethod]
        public void DeleteUrl_CommandText()
        {
            SqliteQueryProvider queryProvider = new();
            var result = queryProvider.DeleteUrl(100);

            Console.WriteLine(result.CommandText);
            Assert.IsTrue(result.CommandText.Trim().ToUpper().StartsWith("DELETE FROM URL"));
        }

        [TestMethod]
        public void DeleteUrl_QueryIsParameterized()
        {
            SqliteQueryProvider queryProvider = new();
            var result = queryProvider.DeleteUrl(int.MaxValue);

            Console.WriteLine(result.CommandText);

            foreach (DbParameter parameter in result.Parameters)
            {
                Console.WriteLine(parameter.ParameterName + "\t" + parameter.Value);
            }

            Assert.IsTrue(result.Parameters.Contains("@ID"));
            Assert.IsFalse(result.CommandText.Contains(int.MaxValue.ToString()));
        }

        #endregion

        #endregion

        #region By DTO

        #region Validation

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void DeleteUrl_ByDTO_null()
        {
            SqliteQueryProvider queryProvider = new();
            queryProvider.DeleteUrl(null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void DeleteUrl_ByDTO_ID_Zero()
        {
            SqliteQueryProvider queryProvider = new();
            queryProvider.DeleteUrl(new Url() { ID = 0 });
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void DeleteUrl_ByDTO_ID_Negative()
        {
            SqliteQueryProvider queryProvider = new();
            queryProvider.DeleteUrl(new Url() { ID = -389247 });
        }
        #endregion

        #region Correctness

        [TestMethod]
        public void DeleteUrl_ByDTO_ResultIsNotNull()
        {
            SqliteQueryProvider queryProvider = new();
            var result = queryProvider.DeleteUrl(new Url() { ID = 100 });

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void DeleteUrl_ByDTO_ResultIsCorrectType()
        {
            SqliteQueryProvider queryProvider = new();
            var result = queryProvider.DeleteUrl(new Url() { ID = 100 });

            Console.WriteLine(result.GetType().FullName);
            Assert.IsTrue(result.GetType().Name == "SQLiteCommand");
        }

        [TestMethod]
        public void DeleteUrl_ByDTO_CommandText()
        {
            SqliteQueryProvider queryProvider = new();
            var result = queryProvider.DeleteUrl(new Url() { ID = 100 });

            Console.WriteLine(result.CommandText);
            Assert.IsTrue(result.CommandText.Trim().ToUpper().StartsWith("DELETE FROM URL"));
        }

        [TestMethod]
        public void DeleteUrl_ByDTO_QueryIsParameterized()
        {
            SqliteQueryProvider queryProvider = new();
            var result = queryProvider.DeleteUrl(new Url() { ID = int.MaxValue });

            Console.WriteLine(result.CommandText);

            foreach (DbParameter parameter in result.Parameters)
            {
                Console.WriteLine(parameter.ParameterName + "\t" + parameter.Value);
            }

            Assert.IsTrue(result.Parameters.Contains("@ID"));
            Assert.IsFalse(result.CommandText.Contains(int.MaxValue.ToString()));
        }

        #endregion

        #endregion

        #endregion

        #region GetUrl (by ID)

        #region ID must be in range

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void GetUrlByID_Zero()
        {
            SqliteQueryProvider queryProvider = new();
            queryProvider.GetUrl(0);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void GetUrlByID_Negative()
        {
            SqliteQueryProvider queryProvider = new();
            queryProvider.GetUrl(-25);
        }

        #endregion

        [TestMethod]
        public void GetUrlByID_ResultIsNotNull()
        {
            SqliteQueryProvider queryProvider = new();
            var result = queryProvider.GetUrl(100);

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void GetUrlByID_ResultIsCorrectType()
        {
            SqliteQueryProvider queryProvider = new();
            var result = queryProvider.GetUrl(100);

            Console.WriteLine(result.GetType().FullName);
            Assert.IsTrue(result.GetType().Name == "SQLiteCommand");
        }

        [TestMethod]
        public void GetUrlByID_CommandText()
        {
            SqliteQueryProvider queryProvider = new();
            var result = queryProvider.GetUrl(100);

            Console.WriteLine(result.CommandText);
            Assert.IsTrue(result.CommandText.Trim().ToUpper().StartsWith("SELECT"));
            Assert.IsTrue(result.CommandText.Trim().ToUpper().Contains("FROM URL"));
        }

        [TestMethod]
        public void GetUrlByID_QueryIsParameterized()
        {
            SqliteQueryProvider queryProvider = new();
            var result = queryProvider.GetUrl(int.MaxValue);

            Console.WriteLine(result.CommandText);

            foreach (DbParameter parameter in result.Parameters)
            {
                Console.WriteLine(parameter.ParameterName + "\t" + parameter.Value);
            }

            Assert.IsTrue(result.Parameters.Contains("@ID"));
            Assert.IsFalse(result.CommandText.Contains(int.MaxValue.ToString()));
        }

        #endregion

        #region GetUrl (by text/url)

        #region Url can't be blank

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void GetUrlByText_id_Null()
        {
            SqliteQueryProvider queryProvider = new();
            queryProvider.GetUrl(null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void GetUrlByText_id_EmptyString()
        {
            SqliteQueryProvider queryProvider = new();
            queryProvider.GetUrl(string.Empty);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void GetUrlByText_id_WhiteSpace()
        {
            SqliteQueryProvider queryProvider = new();
            queryProvider.GetUrl("\t");
        }

        #endregion

        [TestMethod]
        public void GetUrlByText_ResultIsNotNull()
        {
            SqliteQueryProvider queryProvider = new();
            var result = queryProvider.GetUrl("https://test/");

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void GetUrlByText_ResultIsCorrectType()
        {
            SqliteQueryProvider queryProvider = new();
            var result = queryProvider.GetUrl("https://test/");

            Console.WriteLine(result.GetType().FullName);
            Assert.IsTrue(result.GetType().Name == "SQLiteCommand");
        }

        [TestMethod]
        public void GetUrlByText_CommandText()
        {
            SqliteQueryProvider queryProvider = new();
            var result = queryProvider.GetUrl("https://test/");

            Console.WriteLine(result.CommandText);
            Assert.IsTrue(result.CommandText.Trim().ToUpper().StartsWith("SELECT"));
            Assert.IsTrue(result.CommandText.Trim().ToUpper().Contains("FROM URL"));
        }

        [TestMethod]
        public void GetUrlByText_QueryIsParameterized()
        {
            SqliteQueryProvider queryProvider = new();
            var result = queryProvider.GetUrl("https://test/");

            Console.WriteLine(result.CommandText);

            foreach (DbParameter parameter in result.Parameters)
            {
                Console.WriteLine(parameter.ParameterName + "\t" + parameter.Value);
            }

            Assert.IsTrue(result.Parameters.Contains("@Url"));
            Assert.IsFalse(result.CommandText.Contains("https://test/"));
        }

        #endregion
    }
}
