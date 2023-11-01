using CascadePass.TrailBot.DataAccess;
using CascadePass.TrailBot.DataAccess.DTO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Data.Common;

namespace TrailBot.DataAccess.Tests.SqliteQueryProviderTests
{
    [TestClass]
    public class ProviderStatisticsTests
    {
        #region AddProviderStatistics

        #region Validation

        #region ProviderStatistics can't be blank

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void AddProviderStatistics_Null()
        {
            SqliteQueryProvider queryProvider = new();
            queryProvider.AddProviderStatistics(null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void AddProviderStatistics_ProviderStatistics_ID_Zero()
        {
            SqliteQueryProvider queryProvider = new();
            var statistics = this.CreateStatistics();
            statistics.ID = 0;
            queryProvider.AddProviderStatistics(statistics);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void AddProviderStatistics_ProviderStatistics_ID_Negative()
        {
            SqliteQueryProvider queryProvider = new();
            var statistics = this.CreateStatistics();
            statistics.ID = -23;
            queryProvider.AddProviderStatistics(statistics);
        }

        #endregion

        #endregion

        [TestMethod]
        public void AddProviderStatistics_ResultIsNotNull()
        {
            SqliteQueryProvider queryProvider = new();
            var statistics = this.CreateStatistics();
            var result = queryProvider.AddProviderStatistics(statistics);

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void AddProviderStatistics_ResultIsCorrectType()
        {
            SqliteQueryProvider queryProvider = new();
            var statistics = this.CreateStatistics();
            var result = queryProvider.AddProviderStatistics(statistics);

            Console.WriteLine(result.GetType().FullName);
            Assert.IsTrue(result.GetType().Name == "SQLiteCommand");
        }

        [TestMethod]
        public void AddProviderStatistics_CommandText()
        {
            SqliteQueryProvider queryProvider = new();
            var statistics = this.CreateStatistics();
            var result = queryProvider.AddProviderStatistics(statistics);

            Console.WriteLine(result.CommandText);
            Assert.IsTrue(result.CommandText.Trim().ToUpper().StartsWith("INSERT INTO PROVIDERSTATISTICS"));
            Assert.IsTrue(result.CommandText.Trim().ToUpper().Contains("VALUES"));
            Assert.IsTrue(result.CommandText.Trim().ToUpper().Contains("SELECT LAST_INSERT_ROWID();"));
        }

        [TestMethod]
        public void AddProviderStatistics_QueryIsParameterized()
        {
            SqliteQueryProvider queryProvider = new();
            var statistics = this.CreateStatistics();
            var result = queryProvider.AddProviderStatistics(statistics);

            Console.WriteLine(result.CommandText);

            foreach (DbParameter parameter in result.Parameters)
            {
                Console.WriteLine(parameter.ParameterName + "\t" + parameter.Value);
            }

            // None of the input values should appear in the text
            Assert.IsFalse(result.CommandText.Trim().ToUpper().Contains("HTTPS://TEST/"));

            // Instead, there should be parameters with these values
            Assert.IsTrue(result.Parameters.Contains("@ProviderID"));
            Assert.AreEqual(statistics.ID, result.Parameters["@ProviderID"].Value);
        }

        #endregion

        #region UpdateProviderStatistics

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void UpdateProviderStatistics_Null()
        {
            SqliteQueryProvider queryProvider = new();
            var result = queryProvider.UpdateProviderStatistics(null);
        }

        #region ID must be in range

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void UpdateProviderStatistics_Zero()
        {
            SqliteQueryProvider queryProvider = new();
            var statistics = this.CreateStatistics();
            statistics.ID = 0;
            var result = queryProvider.UpdateProviderStatistics(statistics);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void UpdateProviderStatistics_Negative()
        {
            SqliteQueryProvider queryProvider = new();
            var statistics = this.CreateStatistics();
            statistics.ID = -25;
            var result = queryProvider.UpdateProviderStatistics(statistics);
        }

        #endregion

        [TestMethod]
        public void UpdateProviderStatistics_ResultIsNotNull()
        {
            SqliteQueryProvider queryProvider = new();
            var statistics = this.CreateStatistics();
            var result = queryProvider.UpdateProviderStatistics(statistics);

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void UpdateProviderStatistics_ResultIsCorrectType()
        {
            SqliteQueryProvider queryProvider = new();
            var statistics = this.CreateStatistics();
            var result = queryProvider.UpdateProviderStatistics(statistics);

            Console.WriteLine(result.GetType().FullName);
            Assert.IsTrue(result.GetType().Name == "SQLiteCommand");
        }

        [TestMethod]
        public void UpdateProviderStatistics_CommandText()
        {
            SqliteQueryProvider queryProvider = new();
            var statistics = this.CreateStatistics();
            var result = queryProvider.UpdateProviderStatistics(statistics);

            Console.WriteLine(result.CommandText);
            Assert.IsTrue(result.CommandText.Trim().ToUpper().StartsWith("UPDATE PROVIDERSTATISTICS"));
        }

        [TestMethod]
        public void UpdateProviderStatistics_QueryIsParameterized()
        {
            SqliteQueryProvider queryProvider = new();
            var statistics = this.CreateStatistics();
            var result = queryProvider.UpdateProviderStatistics(statistics);

            Console.WriteLine(result.CommandText);

            foreach (DbParameter parameter in result.Parameters)
            {
                Console.WriteLine(parameter.ParameterName + "\t" + parameter.Value);
            }

            Assert.IsTrue(result.Parameters.Contains("@ProviderID"));
            Assert.IsTrue(result.Parameters.Contains("@RequestsMade"));
            Assert.IsTrue(result.Parameters.Contains("@FailedRequests"));
            Assert.IsTrue(result.Parameters.Contains("@MatchesFound"));
            Assert.IsTrue(result.Parameters.Contains("@SleepTimeInMS"));
        }

        [TestMethod]
        public void UpdateProviderStatistics_CannotEditProviderStatistics()
        {
            SqliteQueryProvider queryProvider = new();
            var statistics = this.CreateStatistics();
            var result = queryProvider.UpdateProviderStatistics(statistics);

            Console.WriteLine(result.CommandText);

            foreach (DbParameter parameter in result.Parameters)
            {
                Console.WriteLine(parameter.ParameterName + "\t" + parameter.Value);
            }

            Assert.IsFalse(result.Parameters.Contains("@ProviderStatistics"));
        }

        [TestMethod]
        public void UpdateProviderStatistics_CannotEditFoundDate()
        {
            SqliteQueryProvider queryProvider = new();
            var statistics = this.CreateStatistics();
            var result = queryProvider.UpdateProviderStatistics(statistics);

            Console.WriteLine(result.CommandText);

            foreach (DbParameter parameter in result.Parameters)
            {
                Console.WriteLine(parameter.ParameterName + "\t" + parameter.Value);
            }

            Assert.IsFalse(result.Parameters.Contains("@FoundDate"));
        }

        #endregion

        #region DeleteProviderStatistics

        #region By ID

        #region ID must be in range

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void DeleteProviderStatistics_id_Zero()
        {
            SqliteQueryProvider queryProvider = new();
            queryProvider.DeleteProviderStatistics(0);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void DeleteProviderStatistics_id_Negative()
        {
            SqliteQueryProvider queryProvider = new();
            queryProvider.DeleteProviderStatistics(-25);
        }

        #endregion

        [TestMethod]
        public void DeleteProviderStatistics_ResultIsNotNull()
        {
            SqliteQueryProvider queryProvider = new();
            var result = queryProvider.DeleteProviderStatistics(100);

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void DeleteProviderStatistics_ResultIsCorrectType()
        {
            SqliteQueryProvider queryProvider = new();
            var result = queryProvider.DeleteProviderStatistics(100);

            Console.WriteLine(result.GetType().FullName);
            Assert.IsTrue(result.GetType().Name == "SQLiteCommand");
        }

        [TestMethod]
        public void DeleteProviderStatistics_CommandText()
        {
            SqliteQueryProvider queryProvider = new();
            var result = queryProvider.DeleteProviderStatistics(100);

            Console.WriteLine(result.CommandText);
            Assert.IsTrue(result.CommandText.Trim().ToUpper().StartsWith("DELETE FROM PROVIDERSTATISTICS"));
        }

        [TestMethod]
        public void DeleteProviderStatistics_QueryIsParameterized()
        {
            SqliteQueryProvider queryProvider = new();
            var result = queryProvider.DeleteProviderStatistics(int.MaxValue);

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

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void DeleteProviderStatistics_ByDTO_Null()
        {
            SqliteQueryProvider queryProvider = new();
            queryProvider.DeleteProviderStatistics(null);
        }

        [TestMethod]
        public void DeleteProviderStatistics_ByDTO_ResultIsNotNull()
        {
            SqliteQueryProvider queryProvider = new();
            var result = queryProvider.DeleteProviderStatistics(new ProviderStatistics() { ID = 25 });

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void DeleteProviderStatistics_ByDTO_ResultIsCorrectType()
        {
            SqliteQueryProvider queryProvider = new();
            var result = queryProvider.DeleteProviderStatistics(new ProviderStatistics() { ID = 25 });

            Console.WriteLine(result.GetType().FullName);
            Assert.IsTrue(result.GetType().Name == "SQLiteCommand");
        }

        [TestMethod]
        public void DeleteProviderStatistics_ByDTO_CommandText()
        {
            SqliteQueryProvider queryProvider = new();
            var result = queryProvider.DeleteProviderStatistics(new ProviderStatistics() { ID = 25 });

            Console.WriteLine(result.CommandText);
            Assert.IsTrue(result.CommandText.Trim().ToUpper().StartsWith("DELETE FROM PROVIDERSTATISTICS"));
        }

        [TestMethod]
        public void DeleteProviderStatistics_ByDTO_QueryIsParameterized()
        {
            SqliteQueryProvider queryProvider = new();
            var result = queryProvider.DeleteProviderStatistics(new ProviderStatistics() { ID = int.MaxValue });

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

        #region GetProviderStatistics (by ID)

        #region ID must be in range

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void GetProviderStatisticsByID_Zero()
        {
            SqliteQueryProvider queryProvider = new();
            queryProvider.GetProviderStatistics(0);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void GetProviderStatisticsByID_Negative()
        {
            SqliteQueryProvider queryProvider = new();
            queryProvider.GetProviderStatistics(-25);
        }

        #endregion

        [TestMethod]
        public void GetProviderStatisticsByID_ResultIsNotNull()
        {
            SqliteQueryProvider queryProvider = new();
            var result = queryProvider.GetProviderStatistics(100);

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void GetProviderStatisticsByID_ResultIsCorrectType()
        {
            SqliteQueryProvider queryProvider = new();
            var result = queryProvider.GetProviderStatistics(100);

            Console.WriteLine(result.GetType().FullName);
            Assert.IsTrue(result.GetType().Name == "SQLiteCommand");
        }

        [TestMethod]
        public void GetProviderStatisticsByID_CommandText()
        {
            SqliteQueryProvider queryProvider = new();
            var result = queryProvider.GetProviderStatistics(100);

            Console.WriteLine(result.CommandText);
            Assert.IsTrue(result.CommandText.Trim().ToUpper().StartsWith("SELECT"));
            Assert.IsTrue(result.CommandText.Trim().ToUpper().Contains("FROM PROVIDERSTATISTICS"));
        }

        [TestMethod]
        public void GetProviderStatisticsByID_QueryIsParameterized()
        {
            SqliteQueryProvider queryProvider = new();
            var result = queryProvider.GetProviderStatistics(int.MaxValue);

            Console.WriteLine(result.CommandText);

            foreach (DbParameter parameter in result.Parameters)
            {
                Console.WriteLine(parameter.ParameterName + "\t" + parameter.Value);
            }

            Assert.IsTrue(result.Parameters.Contains("@ID"));
            Assert.IsFalse(result.CommandText.Contains(int.MaxValue.ToString()));
        }

        #endregion

        #region IncrementProviderStatistics

        #region Validation

        #region ID must be in range

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void IncrementProviderStatistics_ID_Zero()
        {
            SqliteQueryProvider queryProvider = new();
            var statistics = this.CreateStatistics();
            statistics.ID = 0;
            var result = queryProvider.IncrementProviderStatistics(0, 1, 2, 3, 4);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void IncrementProviderStatistics_ID_Negative()
        {
            SqliteQueryProvider queryProvider = new();
            var statistics = this.CreateStatistics();
            statistics.ID = -25;
            var result = queryProvider.IncrementProviderStatistics(-1, 2, 3, 4, 5);
        }

        #endregion

        #endregion

        [TestMethod]
        public void IncrementProviderStatistics_ResultIsNotNull()
        {
            SqliteQueryProvider queryProvider = new();
            var result = queryProvider.IncrementProviderStatistics(1, 2, 3, 4, 5);

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void IncrementProviderStatistics_ResultIsCorrectType()
        {
            SqliteQueryProvider queryProvider = new();
            var result = queryProvider.IncrementProviderStatistics(1, 2, 3, 4, 5);

            Console.WriteLine(result.GetType().FullName);
            Assert.IsTrue(result.GetType().Name == "SQLiteCommand");
        }

        [TestMethod]
        public void IncrementProviderStatistics_CommandText()
        {
            SqliteQueryProvider queryProvider = new();
            var result = queryProvider.IncrementProviderStatistics(1, 2, 3, 4, 5);

            Console.WriteLine(result.CommandText);
            Assert.IsTrue(result.CommandText.Trim().ToUpper().StartsWith("UPDATE PROVIDERSTATISTICS"));
        }

        [TestMethod]
        public void IncrementProviderStatistics_QueryIsParameterized()
        {
            SqliteQueryProvider queryProvider = new();
            var result = queryProvider.IncrementProviderStatistics(1, 2, 3, 4, 5);

            Console.WriteLine(result.CommandText);

            foreach (DbParameter parameter in result.Parameters)
            {
                Console.WriteLine(parameter.ParameterName + "\t" + parameter.Value);
            }

            Assert.IsTrue(result.Parameters.Contains("@ProviderID"));
            Assert.IsTrue(result.Parameters.Contains("@RequestsMade"));
            Assert.IsTrue(result.Parameters.Contains("@FailedRequests"));
            Assert.IsTrue(result.Parameters.Contains("@MatchesFound"));
            Assert.IsTrue(result.Parameters.Contains("@SleepTimeInMS"));
        }

        #endregion

        private ProviderStatistics CreateStatistics()
        {
            return new()
            {
                ID = 12,
                FailedRequests = 1,
                TotalRequestsMade = 2,
                MatchesFound = 3,
                SleepTime = TimeSpan.FromSeconds(20),
            };
        }
    }
}
