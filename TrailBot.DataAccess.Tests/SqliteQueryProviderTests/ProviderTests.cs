using CascadePass.TrailBot.DataAccess;
using CascadePass.TrailBot.DataAccess.DTO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Data.Common;

namespace TrailBot.DataAccess.Tests.SqliteQueryProviderTests
{
    [TestClass]
    public class ProviderTests
    {
        #region AddProvider

        #region Validation

        #region Name can't be blank

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void AddProvider_Null()
        {
            SqliteQueryProvider queryProvider = new();
            queryProvider.AddProvider(null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void AddProvider_Url_Null()
        {
            SqliteQueryProvider queryProvider = new();
            queryProvider.AddProvider(new Provider() { Name = null });
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void AddProvider_Url_EmptyString()
        {
            SqliteQueryProvider queryProvider = new();
            queryProvider.AddProvider(new Provider() { Name = string.Empty });
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void AddProvider_Url_WhiteSpace()
        {
            SqliteQueryProvider queryProvider = new();
            queryProvider.AddProvider(new Provider() { Name = " " });
        }

        #endregion

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void AddProvider_HasID()
        {
            SqliteQueryProvider queryProvider = new();
            queryProvider.AddProvider(new Provider() { ID = 25, Name = "test", TypeName = "test" });
        }

        #endregion

        [TestMethod]
        public void AddProvider_ResultIsNotNull()
        {
            SqliteQueryProvider queryProvider = new();
            var result = queryProvider.AddProvider(new() { Name = Guid.NewGuid().ToString(), TypeName = Guid.NewGuid().ToString() });

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void AddProvider_ResultIsCorrectType()
        {
            SqliteQueryProvider queryProvider = new();
            var result = queryProvider.AddProvider(new() { Name = Guid.NewGuid().ToString(), TypeName = Guid.NewGuid().ToString() });

            Console.WriteLine(result.GetType().FullName);
            Assert.IsTrue(result.GetType().Name == "SQLiteCommand");
        }

        [TestMethod]
        public void AddProvider_CommandText()
        {
            SqliteQueryProvider queryProvider = new();
            var result = queryProvider.AddProvider(new() { Name = Guid.NewGuid().ToString(), TypeName = Guid.NewGuid().ToString() });

            Console.WriteLine(result.CommandText);
            Assert.IsTrue(result.CommandText.Trim().ToUpper().StartsWith("INSERT INTO PROVIDER"));
            Assert.IsTrue(result.CommandText.Trim().ToUpper().Contains("VALUES"));
            Assert.IsTrue(result.CommandText.Trim().ToUpper().Contains("SELECT LAST_INSERT_ROWID();"));
        }

        [TestMethod]
        public void AddProvider_QueryIsParameterized()
        {
            string text = Guid.NewGuid().ToString();
            SqliteQueryProvider queryProvider = new();
            var result = queryProvider.AddProvider(new() { Name = text, TypeName = Guid.NewGuid().ToString() });

            Console.WriteLine(result.CommandText);

            foreach (DbParameter parameter in result.Parameters)
            {
                Console.WriteLine(parameter.ParameterName + "\t" + parameter.Value);
            }

            // None of the input values should appear in the text
            Assert.IsFalse(result.CommandText.Trim().ToUpper().Contains(text.ToUpper()));

            // Instead, there should be parameters with these values
            Assert.IsTrue(result.Parameters.Contains("@Name"));
            Assert.AreEqual(text, result.Parameters["@Name"].Value);
        }

        [TestMethod]
        public void AddProvider_NullDatesNullParameters()
        {
            string text = Guid.NewGuid().ToString();
            SqliteQueryProvider queryProvider = new();
            var result = queryProvider.AddProvider(new() { Name = text, TypeName = Guid.NewGuid().ToString() });

            Console.WriteLine(result.CommandText);

            foreach (DbParameter parameter in result.Parameters)
            {
                Console.WriteLine(parameter.ParameterName + "\t" + parameter.Value);
            }

            Assert.IsTrue(result.Parameters.Contains("@LastTripReportRequest"));
            Assert.IsTrue(result.Parameters["@LastTripReportRequest"].Value == null || result.Parameters["@LastTripReportRequest"].Value == Convert.DBNull, $"Expected null, result was {result.Parameters["@LastTripReportRequest"].Value}");

            Assert.IsTrue(result.Parameters.Contains("@LastGetRecentRequest"));
            Assert.IsTrue(result.Parameters["@LastGetRecentRequest"].Value == null || result.Parameters["@LastTripReportRequest"].Value == Convert.DBNull, $"Expected null, result was {result.Parameters["@LastTripReportRequest"].Value}");
        }

        [TestMethod]
        public void AddProvider_ValidDatesValidParameters()
        {
            DateTime now = DateTime.Now;
            string text = Guid.NewGuid().ToString();
            SqliteQueryProvider queryProvider = new();
            var result = queryProvider.AddProvider(new() { Name = text, TypeName = Guid.NewGuid().ToString(), LastGetRecentRequest = now, LastTripReportRequest = now });

            Console.WriteLine(result.CommandText);

            foreach (DbParameter parameter in result.Parameters)
            {
                Console.WriteLine(parameter.ParameterName + "\t" + parameter.Value);
            }

            Assert.IsTrue(result.Parameters.Contains("@LastTripReportRequest"));
            Assert.AreEqual(DataTransferObject.ToUnixDate(now), result.Parameters["@LastTripReportRequest"].Value, $"Expected null, result was {result.Parameters["@LastTripReportRequest"].Value}");

            Assert.IsTrue(result.Parameters.Contains("@LastGetRecentRequest"));
            Assert.AreEqual(DataTransferObject.ToUnixDate(now), result.Parameters["@LastGetRecentRequest"].Value, $"Expected null, result was {result.Parameters["@LastTripReportRequest"].Value}");
        }

        #endregion

        #region UpdateProvider

        #region By DTO

        #region Validation (ID must be in range)

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void UpdateProvider_ByDTO_Null()
        {
            SqliteQueryProvider queryProvider = new();
            queryProvider.UpdateProvider(null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void UpdateProvider_ByDTO_Zero()
        {
            SqliteQueryProvider queryProvider = new();
            queryProvider.UpdateProvider(new() { ID = 0, Name = Guid.NewGuid().ToString() });
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void UpdateProvider_ByDTO_Negative()
        {
            SqliteQueryProvider queryProvider = new();
            queryProvider.UpdateProvider(new() { ID = -30, Name = Guid.NewGuid().ToString() });
        }

        #endregion

        #region Correctness

        [TestMethod]
        public void UpdateProvider_ByDTO_ResultIsNotNull()
        {
            SqliteQueryProvider queryProvider = new();
            var result = queryProvider.UpdateProvider(new() { ID = 100, Name = Guid.NewGuid().ToString() });

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void UpdateProvider_ByDTO_ResultIsCorrectType()
        {
            SqliteQueryProvider queryProvider = new();
            var result = queryProvider.UpdateProvider(new() { ID = 100, Name = Guid.NewGuid().ToString() });

            Console.WriteLine(result.GetType().FullName);
            Assert.IsTrue(result.GetType().Name == "SQLiteCommand");
        }

        [TestMethod]
        public void UpdateProvider_ByDTO_CommandText()
        {
            SqliteQueryProvider queryProvider = new();
            var result = queryProvider.UpdateProvider(new() { ID = 100, Name = Guid.NewGuid().ToString() });

            Console.WriteLine(result.CommandText);
            Assert.IsTrue(result.CommandText.Trim().ToUpper().StartsWith("UPDATE PROVIDER"));
        }

        [TestMethod]
        public void UpdateProvider_ByDTO_QueryIsParameterized()
        {
            SqliteQueryProvider queryProvider = new();
            var result = queryProvider.UpdateProvider(new() { ID = 100, Name = Guid.NewGuid().ToString() });

            Console.WriteLine(result.CommandText);

            foreach (DbParameter parameter in result.Parameters)
            {
                Console.WriteLine(parameter.ParameterName + "\t" + parameter.Value);
            }

            Assert.IsTrue(result.Parameters.Contains("@Name"));
            Assert.IsTrue(result.Parameters.Contains("@ID"));
        }

        [TestMethod]
        public void UpdateProvider_NullDatesNullParameters()
        {
            string text = Guid.NewGuid().ToString();
            SqliteQueryProvider queryProvider = new();
            var result = queryProvider.UpdateProvider(new() { ID = 1, Name = text, TypeName = Guid.NewGuid().ToString() });

            Console.WriteLine(result.CommandText);

            foreach (DbParameter parameter in result.Parameters)
            {
                Console.WriteLine(parameter.ParameterName + "\t" + parameter.Value);
            }

            Assert.IsTrue(result.Parameters.Contains("@LastTripReportRequest"));
            Assert.IsTrue(result.Parameters["@LastTripReportRequest"].Value == null || result.Parameters["@LastTripReportRequest"].Value == Convert.DBNull, $"Expected null, result was {result.Parameters["@LastTripReportRequest"].Value}");

            Assert.IsTrue(result.Parameters.Contains("@LastGetRecentRequest"));
            Assert.IsTrue(result.Parameters["@LastGetRecentRequest"].Value == null || result.Parameters["@LastTripReportRequest"].Value == Convert.DBNull, $"Expected null, result was {result.Parameters["@LastTripReportRequest"].Value}");
        }

        [TestMethod]
        public void UpdateProvider_ValidDatesValidParameters()
        {
            DateTime now = DateTime.Now;
            string text = Guid.NewGuid().ToString();
            SqliteQueryProvider queryProvider = new();
            var result = queryProvider.UpdateProvider(new() { ID = 100, Name = text, TypeName = Guid.NewGuid().ToString(), LastGetRecentRequest = now, LastTripReportRequest = now });

            Console.WriteLine(result.CommandText);

            foreach (DbParameter parameter in result.Parameters)
            {
                Console.WriteLine(parameter.ParameterName + "\t" + parameter.Value);
            }

            Assert.IsTrue(result.Parameters.Contains("@LastTripReportRequest"));
            Assert.AreEqual(DataTransferObject.ToUnixDate(now), result.Parameters["@LastTripReportRequest"].Value, $"Expected null, result was {result.Parameters["@LastTripReportRequest"].Value}");

            Assert.IsTrue(result.Parameters.Contains("@LastGetRecentRequest"));
            Assert.AreEqual(DataTransferObject.ToUnixDate(now), result.Parameters["@LastGetRecentRequest"].Value, $"Expected null, result was {result.Parameters["@LastTripReportRequest"].Value}");
        }

        #endregion

        #endregion

        #endregion

        #region DeleteProvider

        #region By ID

        #region ID must be in range

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void DeleteProvider_ByID_Zero()
        {
            SqliteQueryProvider queryProvider = new();
            queryProvider.DeleteProvider(0);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void DeleteProvider_ByID_Negative()
        {
            SqliteQueryProvider queryProvider = new();
            queryProvider.DeleteProvider(-25);
        }

        #endregion

        [TestMethod]
        public void DeleteProvider_ByID_ResultIsNotNull()
        {
            SqliteQueryProvider queryProvider = new();
            var result = queryProvider.DeleteProvider(100);

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void DeleteProvider_ByID_ResultIsCorrectType()
        {
            SqliteQueryProvider queryProvider = new();
            var result = queryProvider.DeleteProvider(100);

            Console.WriteLine(result.GetType().FullName);
            Assert.IsTrue(result.GetType().Name == "SQLiteCommand");
        }

        [TestMethod]
        public void DeleteProvider_ByID_CommandText()
        {
            SqliteQueryProvider queryProvider = new();
            var result = queryProvider.DeleteProvider(100);

            Console.WriteLine(result.CommandText);
            Assert.IsTrue(result.CommandText.Trim().ToUpper().StartsWith("DELETE FROM PROVIDER"));
        }

        [TestMethod]
        public void DeleteProvider_ByID_QueryIsParameterized()
        {
            SqliteQueryProvider queryProvider = new();
            var result = queryProvider.DeleteProvider(int.MaxValue);

            Console.WriteLine(result.CommandText);

            foreach (DbParameter parameter in result.Parameters)
            {
                Console.WriteLine(parameter.ParameterName + "\t" + parameter.Value);
            }

            Assert.IsTrue(result.Parameters.Contains("@ID"));
            Assert.IsFalse(result.CommandText.Contains(int.MaxValue.ToString()));
        }

        #endregion

        #region By DTO

        #region Validation

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void DeleteProvider_ByDTO_Null()
        {
            SqliteQueryProvider queryProvider = new();
            queryProvider.DeleteProvider(null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void DeleteProvider_ByDTO_Zero()
        {
            SqliteQueryProvider queryProvider = new();
            queryProvider.DeleteProvider(new Provider() { ID = 0 });
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void DeleteProvider_ByDTO_Negative()
        {
            SqliteQueryProvider queryProvider = new();
            queryProvider.DeleteProvider(new Provider() { ID = -25 });
        }

        #endregion

        [TestMethod]
        public void DeleteProvider_ByDTO_ResultIsNotNull()
        {
            SqliteQueryProvider queryProvider = new();
            var result = queryProvider.DeleteProvider(new Provider() { ID = 1 });

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void DeleteProvider_ByDTO_ResultIsCorrectType()
        {
            SqliteQueryProvider queryProvider = new();
            var result = queryProvider.DeleteProvider(new Provider() { ID = 80 });

            Console.WriteLine(result.GetType().FullName);
            Assert.IsTrue(result.GetType().Name == "SQLiteCommand");
        }

        [TestMethod]
        public void DeleteProvider_ByDTO_CommandText()
        {
            SqliteQueryProvider queryProvider = new();
            var result = queryProvider.DeleteProvider(new Provider() { ID = 100 });

            Console.WriteLine(result.CommandText);
            Assert.IsTrue(result.CommandText.Trim().ToUpper().StartsWith("DELETE FROM PROVIDER"));
        }

        [TestMethod]
        public void DeleteProvider_ByDTO_QueryIsParameterized()
        {
            SqliteQueryProvider queryProvider = new();
            var result = queryProvider.DeleteProvider(new Provider() { ID = 25000 });

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

        #region GetProvider

        #region ID must be in range

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void GetProviderZero()
        {
            SqliteQueryProvider queryProvider = new();
            queryProvider.GetProvider(0);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void GetProviderNegative()
        {
            SqliteQueryProvider queryProvider = new();
            queryProvider.GetProvider(-25);
        }

        #endregion

        [TestMethod]
        public void GetProviderResultIsNotNull()
        {
            SqliteQueryProvider queryProvider = new();
            var result = queryProvider.GetProvider(100);

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void GetProviderResultIsCorrectType()
        {
            SqliteQueryProvider queryProvider = new();
            var result = queryProvider.GetProvider(100);

            Console.WriteLine(result.GetType().FullName);
            Assert.IsTrue(result.GetType().Name == "SQLiteCommand");
        }

        [TestMethod]
        public void GetProviderCommandText()
        {
            SqliteQueryProvider queryProvider = new();
            var result = queryProvider.GetProvider(100);

            Console.WriteLine(result.CommandText);
            Assert.IsTrue(result.CommandText.Trim().ToUpper().StartsWith("SELECT"));
            Assert.IsTrue(result.CommandText.Trim().ToUpper().Contains("FROM PROVIDER"));
        }

        [TestMethod]
        public void GetProviderQueryIsParameterized()
        {
            SqliteQueryProvider queryProvider = new();
            var result = queryProvider.GetProvider(int.MaxValue);

            Console.WriteLine(result.CommandText);

            foreach (DbParameter parameter in result.Parameters)
            {
                Console.WriteLine(parameter.ParameterName + "\t" + parameter.Value);
            }

            Assert.IsTrue(result.Parameters.Contains("@ID"));
            Assert.IsFalse(result.CommandText.Contains(int.MaxValue.ToString()));
        }

        #endregion

        #region GetProviders

        [TestMethod]
        public void GetProvidersResultIsNotNull()
        {
            SqliteQueryProvider queryProvider = new();
            var result = queryProvider.GetProviders();

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void GetProvidersResultIsCorrectType()
        {
            SqliteQueryProvider queryProvider = new();
            var result = queryProvider.GetProviders();

            Console.WriteLine(result.GetType().FullName);
            Assert.IsTrue(result.GetType().Name == "SQLiteCommand");
        }

        [TestMethod]
        public void GetProvidersCommandText()
        {
            SqliteQueryProvider queryProvider = new();
            var result = queryProvider.GetProviders();

            Console.WriteLine(result.CommandText);
            Assert.IsTrue(result.CommandText.Trim().ToUpper().StartsWith("SELECT"));
            Assert.IsTrue(result.CommandText.Trim().ToUpper().Contains("FROM PROVIDER"));
        }

        #endregion
    }
}
