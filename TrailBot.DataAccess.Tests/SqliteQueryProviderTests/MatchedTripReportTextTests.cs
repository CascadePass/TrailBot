using CascadePass.TrailBot.DataAccess;
using CascadePass.TrailBot.DataAccess.DTO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Data.Common;

namespace TrailBot.DataAccess.Tests.SqliteQueryProviderTests
{
    [TestClass]
    public class MatchedTripReportTextTests
    {
        #region AddMatchedTripReportText

        #region By DTO

        #region Validation

        #region Name can't be blank

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void AddMatchedTripReportText_ByDTO_Null()
        {
            SqliteQueryProvider queryProvider = new();
            queryProvider.AddMatchedTripReportText(null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void AddMatchedTripReportText_ByDTO_TextID_Zero()
        {
            SqliteQueryProvider queryProvider = new();
            queryProvider.AddMatchedTripReportText(new MatchedTripReportText() { TextID = 0 });
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void AddMatchedTripReportText_ByDTO_TextID_Negative()
        {
            SqliteQueryProvider queryProvider = new();
            queryProvider.AddMatchedTripReportText(new MatchedTripReportText() { TextID = -56 });
        }

        #endregion

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void AddMatchedTripReportText_ByDTO_HasID()
        {
            SqliteQueryProvider queryProvider = new();
            queryProvider.AddMatchedTripReportText(new MatchedTripReportText() { ID = 25, TextID = 1, TripReportID = 2 });
        }

        #endregion

        [TestMethod]
        public void AddMatchedTripReportText_ByDTO_ResultIsNotNull()
        {
            SqliteQueryProvider queryProvider = new();
            var result = queryProvider.AddMatchedTripReportText(new() { TextID = 1, TripReportID = 2 });

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void AddMatchedTripReportText_ByDTO_ResultIsCorrectType()
        {
            SqliteQueryProvider queryProvider = new();
            var result = queryProvider.AddMatchedTripReportText(new() { TextID = 1, TripReportID = 2 });

            Console.WriteLine(result.GetType().FullName);
            Assert.IsTrue(result.GetType().Name == "SQLiteCommand");
        }

        [TestMethod]
        public void AddMatchedTripReportText_ByDTO_CommandText()
        {
            SqliteQueryProvider queryProvider = new();
            var result = queryProvider.AddMatchedTripReportText(new() { TextID = 1, TripReportID = 2 });

            Console.WriteLine(result.CommandText);
            Assert.IsTrue(result.CommandText.Trim().ToUpper().StartsWith("INSERT INTO MATCHEDTRIPREPORTTEXT"));
            Assert.IsTrue(result.CommandText.Trim().ToUpper().Contains("VALUES"));
            Assert.IsTrue(result.CommandText.Trim().ToUpper().Contains("SELECT LAST_INSERT_ROWID();"));
        }

        [TestMethod]
        public void AddMatchedTripReportText_ByDTO_QueryIsParameterized()
        {
            string text = Guid.NewGuid().ToString();
            SqliteQueryProvider queryProvider = new();
            var result = queryProvider.AddMatchedTripReportText(new() { TextID = 1, TripReportID = 2 });

            Console.WriteLine(result.CommandText);

            foreach (DbParameter parameter in result.Parameters)
            {
                Console.WriteLine(parameter.ParameterName + "\t" + parameter.Value);
            }

            // None of the input values should appear in the text
            Assert.IsFalse(result.CommandText.Trim().ToUpper().Contains(text.ToUpper()));

            // Instead, there should be parameters with these values
            Assert.IsTrue(result.Parameters.Contains("@MatchTextID"));
            Assert.AreEqual(2L, result.Parameters["@TripReportID"].Value);
        }

        #endregion

        #region By Multi DTO

        #region Validation

        #region Name can't be blank

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void AddMatchedTripReportText_ByMultipleDTOs_Null_TripReport()
        {
            SqliteQueryProvider queryProvider = new();
            queryProvider.AddMatchedTripReportText(null, new());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void AddMatchedTripReportText_ByMultipleDTOs_Null_MatchText()
        {
            SqliteQueryProvider queryProvider = new();
            queryProvider.AddMatchedTripReportText(new(), null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void AddMatchedTripReportText_ByMultipleDTOs_TextID_Zero()
        {
            SqliteQueryProvider queryProvider = new();

            WtaTripReport tripReport = new();
            MatchText matchText = new();
            queryProvider.AddMatchedTripReportText(tripReport, matchText);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void AddMatchedTripReportText_ByMultipleDTOs_TextID_Negative()
        {
            SqliteQueryProvider queryProvider = new();

            WtaTripReport tripReport = new() { ID = -23 };
            MatchText matchText = new() { ID = -1 };
            queryProvider.AddMatchedTripReportText(tripReport, matchText);
        }

        #endregion

        #endregion

        [TestMethod]
        public void AddMatchedTripReportText_ByMultipleDTOs_ResultIsNotNull()
        {
            SqliteQueryProvider queryProvider = new();
            WtaTripReport tripReport = new() { ID = 1 };
            MatchText matchText = new() { ID = 1 };
            var result = queryProvider.AddMatchedTripReportText(tripReport, matchText);

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void AddMatchedTripReportText_ByMultipleDTOs_ResultIsCorrectType()
        {
            SqliteQueryProvider queryProvider = new();
            WtaTripReport tripReport = new() { ID = 1 };
            MatchText matchText = new() { ID = 1 };
            var result = queryProvider.AddMatchedTripReportText(tripReport, matchText);

            Console.WriteLine(result.GetType().FullName);
            Assert.IsTrue(result.GetType().Name == "SQLiteCommand");
        }

        [TestMethod]
        public void AddMatchedTripReportText_ByMultipleDTOs_CommandText()
        {
            SqliteQueryProvider queryProvider = new();
            WtaTripReport tripReport = new() { ID = 1 };
            MatchText matchText = new() { ID = 1 };
            var result = queryProvider.AddMatchedTripReportText(tripReport, matchText);

            Console.WriteLine(result.CommandText);
            Assert.IsTrue(result.CommandText.Trim().ToUpper().StartsWith("INSERT INTO MATCHEDTRIPREPORTTEXT"));
            Assert.IsTrue(result.CommandText.Trim().ToUpper().Contains("VALUES"));
            Assert.IsTrue(result.CommandText.Trim().ToUpper().Contains("SELECT LAST_INSERT_ROWID();"));
        }

        [TestMethod]
        public void AddMatchedTripReportText_ByMultipleDTOs_QueryIsParameterized()
        {
            string text = Guid.NewGuid().ToString();
            SqliteQueryProvider queryProvider = new();
            WtaTripReport tripReport = new() { ID = 1 };
            MatchText matchText = new() { ID = 1 };
            var result = queryProvider.AddMatchedTripReportText(tripReport, matchText);

            Console.WriteLine(result.CommandText);

            foreach (DbParameter parameter in result.Parameters)
            {
                Console.WriteLine(parameter.ParameterName + "\t" + parameter.Value);
            }

            // None of the input values should appear in the text
            Assert.IsFalse(result.CommandText.Trim().ToUpper().Contains(text.ToUpper()));

            // Instead, there should be parameters with these values
            Assert.IsTrue(result.Parameters.Contains("@MatchTextID"));
            Assert.AreEqual(1L, result.Parameters["@TripReportID"].Value);
        }

        #endregion

        #region By IDs

        #region Validation

        #region ID

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void AddMatchedTripReportText_ByIDs_TextID_Zero_TripReport()
        {
            SqliteQueryProvider queryProvider = new();

            queryProvider.AddMatchedTripReportText(0, 1);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void AddMatchedTripReportText_ByIDs_TextID_Zero_MatchText()
        {
            SqliteQueryProvider queryProvider = new();

            queryProvider.AddMatchedTripReportText(1, 0);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void AddMatchedTripReportText_ByIDs_TextID_Negative()
        {
            SqliteQueryProvider queryProvider = new();

            queryProvider.AddMatchedTripReportText(-1, -2);
        }

        #endregion

        #endregion

        [TestMethod]
        public void AddMatchedTripReportText_ByIDs_ResultIsNotNull()
        {
            SqliteQueryProvider queryProvider = new();
            var result = queryProvider.AddMatchedTripReportText(1, 2);

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void AddMatchedTripReportText_ByIDs_ResultIsCorrectType()
        {
            SqliteQueryProvider queryProvider = new();
            var result = queryProvider.AddMatchedTripReportText(2, 3);

            Console.WriteLine(result.GetType().FullName);
            Assert.IsTrue(result.GetType().Name == "SQLiteCommand");
        }

        [TestMethod]
        public void AddMatchedTripReportText_ByIDs_CommandText()
        {
            SqliteQueryProvider queryProvider = new();
            var result = queryProvider.AddMatchedTripReportText(4, 5);

            Console.WriteLine(result.CommandText);
            Assert.IsTrue(result.CommandText.Trim().ToUpper().StartsWith("INSERT INTO MATCHEDTRIPREPORTTEXT"));
            Assert.IsTrue(result.CommandText.Trim().ToUpper().Contains("VALUES"));
            Assert.IsTrue(result.CommandText.Trim().ToUpper().Contains("SELECT LAST_INSERT_ROWID();"));
        }

        [TestMethod]
        public void AddMatchedTripReportText_ByIDs_QueryIsParameterized()
        {
            string text = Guid.NewGuid().ToString();
            SqliteQueryProvider queryProvider = new();
            var result = queryProvider.AddMatchedTripReportText(5, 6);

            Console.WriteLine(result.CommandText);

            foreach (DbParameter parameter in result.Parameters)
            {
                Console.WriteLine(parameter.ParameterName + "\t" + parameter.Value);
            }

            // None of the input values should appear in the text
            Assert.IsFalse(result.CommandText.Trim().ToUpper().Contains(text.ToUpper()));

            // Instead, there should be parameters with these values
            Assert.IsTrue(result.Parameters.Contains("@MatchTextID"));
            Assert.AreEqual(5L, result.Parameters["@TripReportID"].Value);
        }

        #endregion

        #endregion

        #region DeleteMatchedTripReportText

        #region By ID

        #region ID must be in range

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void DeleteMatchedTripReportText_ByID_Zero()
        {
            SqliteQueryProvider queryProvider = new();
            queryProvider.DeleteMatchedTripReportText(0);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void DeleteMatchedTripReportText_ByID_Negative()
        {
            SqliteQueryProvider queryProvider = new();
            queryProvider.DeleteMatchedTripReportText(-25);
        }

        #endregion

        [TestMethod]
        public void DeleteMatchedTripReportText_ByID_ResultIsNotNull()
        {
            SqliteQueryProvider queryProvider = new();
            var result = queryProvider.DeleteMatchedTripReportText(100);

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void DeleteMatchedTripReportText_ByID_ResultIsCorrectType()
        {
            SqliteQueryProvider queryProvider = new();
            var result = queryProvider.DeleteMatchedTripReportText(100);

            Console.WriteLine(result.GetType().FullName);
            Assert.IsTrue(result.GetType().Name == "SQLiteCommand");
        }

        [TestMethod]
        public void DeleteMatchedTripReportText_ByID_CommandText()
        {
            SqliteQueryProvider queryProvider = new();
            var result = queryProvider.DeleteMatchedTripReportText(100);

            Console.WriteLine(result.CommandText);
            Assert.IsTrue(result.CommandText.Trim().ToUpper().StartsWith("DELETE FROM MATCHEDTRIPREPORTTEXT"));
        }

        [TestMethod]
        public void DeleteMatchedTripReportText_ByID_QueryIsParameterized()
        {
            SqliteQueryProvider queryProvider = new();
            var result = queryProvider.DeleteMatchedTripReportText(int.MaxValue);

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
        public void DeleteMatchedTripReportText_ByDTO_Null()
        {
            SqliteQueryProvider queryProvider = new();
            queryProvider.DeleteMatchedTripReportText(null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void DeleteMatchedTripReportText_ByDTO_Zero()
        {
            SqliteQueryProvider queryProvider = new();
            queryProvider.DeleteMatchedTripReportText(new MatchedTripReportText() { ID = 0 });
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void DeleteMatchedTripReportText_ByDTO_Negative()
        {
            SqliteQueryProvider queryProvider = new();
            queryProvider.DeleteMatchedTripReportText(new MatchedTripReportText() { ID = -25 });
        }

        #endregion

        [TestMethod]
        public void DeleteMatchedTripReportText_ByDTO_ResultIsNotNull()
        {
            SqliteQueryProvider queryProvider = new();
            var result = queryProvider.DeleteMatchedTripReportText(new MatchedTripReportText() { ID = 1 });

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void DeleteMatchedTripReportText_ByDTO_ResultIsCorrectType()
        {
            SqliteQueryProvider queryProvider = new();
            var result = queryProvider.DeleteMatchedTripReportText(new MatchedTripReportText() { ID = 80 });

            Console.WriteLine(result.GetType().FullName);
            Assert.IsTrue(result.GetType().Name == "SQLiteCommand");
        }

        [TestMethod]
        public void DeleteMatchedTripReportText_ByDTO_CommandText()
        {
            SqliteQueryProvider queryProvider = new();
            var result = queryProvider.DeleteMatchedTripReportText(new MatchedTripReportText() { ID = 100 });

            Console.WriteLine(result.CommandText);
            Assert.IsTrue(result.CommandText.Trim().ToUpper().StartsWith("DELETE FROM MATCHEDTRIPREPORTTEXT"));
        }

        [TestMethod]
        public void DeleteMatchedTripReportText_ByDTO_QueryIsParameterized()
        {
            SqliteQueryProvider queryProvider = new();
            var result = queryProvider.DeleteMatchedTripReportText(new MatchedTripReportText() { ID = int.MaxValue });

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

        #region GetMatchedTripReportText

        #region ID must be in range

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void GetMatchedTripReportTextZero()
        {
            SqliteQueryProvider queryProvider = new();
            queryProvider.GetMatchedTripReportText(0);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void GetMatchedTripReportTextNegative()
        {
            SqliteQueryProvider queryProvider = new();
            queryProvider.GetMatchedTripReportText(-25);
        }

        #endregion

        [TestMethod]
        public void GetMatchedTripReportTextResultIsNotNull()
        {
            SqliteQueryProvider queryProvider = new();
            var result = queryProvider.GetMatchedTripReportText(100);

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void GetMatchedTripReportTextResultIsCorrectType()
        {
            SqliteQueryProvider queryProvider = new();
            var result = queryProvider.GetMatchedTripReportText(100);

            Console.WriteLine(result.GetType().FullName);
            Assert.IsTrue(result.GetType().Name == "SQLiteCommand");
        }

        [TestMethod]
        public void GetMatchedTripReportTextCommandText()
        {
            SqliteQueryProvider queryProvider = new();
            var result = queryProvider.GetMatchedTripReportText(100);

            Console.WriteLine(result.CommandText);
            Assert.IsTrue(result.CommandText.Trim().ToUpper().StartsWith("SELECT"));
            Assert.IsTrue(result.CommandText.Trim().ToUpper().Contains("FROM MATCHEDTRIPREPORTTEXT"));
        }

        [TestMethod]
        public void GetMatchedTripReportTextQueryIsParameterized()
        {
            SqliteQueryProvider queryProvider = new();
            var result = queryProvider.GetMatchedTripReportText(int.MaxValue);

            Console.WriteLine(result.CommandText);

            foreach (DbParameter parameter in result.Parameters)
            {
                Console.WriteLine(parameter.ParameterName + "\t" + parameter.Value);
            }

            Assert.IsTrue(result.Parameters.Contains("@ID"));
            Assert.IsFalse(result.CommandText.Contains(int.MaxValue.ToString()));
        }

        #endregion
    }
}
