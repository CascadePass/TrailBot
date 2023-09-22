using CascadePass.TrailBot.DataAccess;
using CascadePass.TrailBot.DataAccess.DTO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Data.Common;

namespace TrailBot.DataAccess.Tests.SqliteQueryProviderTests
{
    [TestClass]
    public class MatchedTripReportTopicTests
    {
        #region AddMatchedTripReportTopic

        #region By DTO

        #region Validation

        #region Name can't be blank

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void AddMatchedTripReportTopic_ByDTO_Null()
        {
            SqliteQueryProvider queryProvider = new();
            queryProvider.AddMatchedTripReportTopic(null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void AddMatchedTripReportTopic_ByDTO_TopicID_Zero()
        {
            SqliteQueryProvider queryProvider = new();
            queryProvider.AddMatchedTripReportTopic(new MatchedTripReportTopic() { TopicID = 0 });
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void AddMatchedTripReportTopic_ByDTO_TopicID_Negative()
        {
            SqliteQueryProvider queryProvider = new();
            queryProvider.AddMatchedTripReportTopic(new MatchedTripReportTopic() { TopicID = -56 });
        }

        #endregion

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void AddMatchedTripReportTopic_ByDTO_HasID()
        {
            SqliteQueryProvider queryProvider = new();
            queryProvider.AddMatchedTripReportTopic(new MatchedTripReportTopic() { ID = 25, TopicID = 1, TripReportID = 2 });
        }

        #endregion

        [TestMethod]
        public void AddMatchedTripReportTopic_ByDTO_ResultIsNotNull()
        {
            SqliteQueryProvider queryProvider = new();
            var result = queryProvider.AddMatchedTripReportTopic(new() { TopicID = 1, TripReportID = 2 });

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void AddMatchedTripReportTopic_ByDTO_ResultIsCorrectType()
        {
            SqliteQueryProvider queryProvider = new();
            var result = queryProvider.AddMatchedTripReportTopic(new() { TopicID = 1, TripReportID = 2 });

            Console.WriteLine(result.GetType().FullName);
            Assert.IsTrue(result.GetType().Name == "SQLiteCommand");
        }

        [TestMethod]
        public void AddMatchedTripReportTopic_ByDTO_CommandText()
        {
            SqliteQueryProvider queryProvider = new();
            var result = queryProvider.AddMatchedTripReportTopic(new() { TopicID = 1, TripReportID = 2 });

            Console.WriteLine(result.CommandText);
            Assert.IsTrue(result.CommandText.Trim().ToUpper().StartsWith("INSERT INTO MATCHEDTRIPREPORTTOPIC"));
            Assert.IsTrue(result.CommandText.Trim().ToUpper().Contains("VALUES"));
            Assert.IsTrue(result.CommandText.Trim().ToUpper().Contains("SELECT LAST_INSERT_ROWID();"));
        }

        [TestMethod]
        public void AddMatchedTripReportTopic_ByDTO_QueryIsParameterized()
        {
            string text = Guid.NewGuid().ToString();
            SqliteQueryProvider queryProvider = new();
            var result = queryProvider.AddMatchedTripReportTopic(new() { TopicID = 1, TripReportID = 2 });

            Console.WriteLine(result.CommandText);

            foreach (DbParameter parameter in result.Parameters)
            {
                Console.WriteLine(parameter.ParameterName + "\t" + parameter.Value);
            }

            // None of the input values should appear in the text
            Assert.IsFalse(result.CommandText.Trim().ToUpper().Contains(text.ToUpper()));

            // Instead, there should be parameters with these values
            Assert.IsTrue(result.Parameters.Contains("@TopicID"));
            Assert.AreEqual(2L, result.Parameters["@TripReportID"].Value);
        }

        #endregion

        #region By Multi DTO

        #region Validation

        #region Name can't be blank

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void AddMatchedTripReportTopic_ByMultipleDTOs_Null_TripReport()
        {
            SqliteQueryProvider queryProvider = new();
            queryProvider.AddMatchedTripReportTopic(null, new(), Guid.NewGuid().ToString());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void AddMatchedTripReportTopic_ByMultipleDTOs_Null_MatchTopic()
        {
            SqliteQueryProvider queryProvider = new();
            queryProvider.AddMatchedTripReportTopic(new(), null, Guid.NewGuid().ToString());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void AddMatchedTripReportTopic_ByMultipleDTOs_TopicID_Zero()
        {
            SqliteQueryProvider queryProvider = new();

            WtaTripReport tripReport = new();
            Topic topic = new();
            queryProvider.AddMatchedTripReportTopic(tripReport, topic, Guid.NewGuid().ToString());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void AddMatchedTripReportTopic_ByMultipleDTOs_TopicID_Negative()
        {
            SqliteQueryProvider queryProvider = new();

            WtaTripReport tripReport = new() { ID = -23 };
            Topic topic = new() { ID = -1 };
            queryProvider.AddMatchedTripReportTopic(tripReport, topic, Guid.NewGuid().ToString());
        }

        #endregion

        #endregion

        [TestMethod]
        public void AddMatchedTripReportTopic_ByMultipleDTOs_ResultIsNotNull()
        {
            SqliteQueryProvider queryProvider = new();
            WtaTripReport tripReport = new() { ID = 1 };
            Topic topic = new() { ID = 1 };
            var result = queryProvider.AddMatchedTripReportTopic(tripReport, topic, Guid.NewGuid().ToString());

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void AddMatchedTripReportTopic_ByMultipleDTOs_ResultIsCorrectType()
        {
            SqliteQueryProvider queryProvider = new();
            WtaTripReport tripReport = new() { ID = 1 };
            Topic topic = new() { ID = 1 };
            var result = queryProvider.AddMatchedTripReportTopic(tripReport, topic, Guid.NewGuid().ToString());

            Console.WriteLine(result.GetType().FullName);
            Assert.IsTrue(result.GetType().Name == "SQLiteCommand");
        }

        [TestMethod]
        public void AddMatchedTripReportTopic_ByMultipleDTOs_CommandText()
        {
            SqliteQueryProvider queryProvider = new();
            WtaTripReport tripReport = new() { ID = 1 };
            Topic topic = new() { ID = 1 };
            var result = queryProvider.AddMatchedTripReportTopic(tripReport, topic, Guid.NewGuid().ToString());

            Console.WriteLine(result.CommandText);
            Assert.IsTrue(result.CommandText.Trim().ToUpper().StartsWith("INSERT INTO MATCHEDTRIPREPORTTOPIC"));
            Assert.IsTrue(result.CommandText.Trim().ToUpper().Contains("VALUES"));
            Assert.IsTrue(result.CommandText.Trim().ToUpper().Contains("SELECT LAST_INSERT_ROWID();"));
        }

        [TestMethod]
        public void AddMatchedTripReportTopic_ByMultipleDTOs_QueryIsParameterized()
        {
            string text = Guid.NewGuid().ToString();
            SqliteQueryProvider queryProvider = new();
            WtaTripReport tripReport = new() { ID = 1 };
            Topic topic = new() { ID = 1 };
            var result = queryProvider.AddMatchedTripReportTopic(tripReport, topic, Guid.NewGuid().ToString());

            Console.WriteLine(result.CommandText);

            foreach (DbParameter parameter in result.Parameters)
            {
                Console.WriteLine(parameter.ParameterName + "\t" + parameter.Value);
            }

            // None of the input values should appear in the text
            Assert.IsFalse(result.CommandText.Trim().ToUpper().Contains(text.ToUpper()));

            // Instead, there should be parameters with these values
            Assert.IsTrue(result.Parameters.Contains("@TopicID"));
            Assert.AreEqual(1L, result.Parameters["@TripReportID"].Value);
        }

        #endregion

        #region By IDs

        #region Validation

        #region ID

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void AddMatchedTripReportTopic_ByIDs_TopicID_Zero_TripReport()
        {
            SqliteQueryProvider queryProvider = new();

            queryProvider.AddMatchedTripReportTopic(0, 1, Guid.NewGuid().ToString());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void AddMatchedTripReportTopic_ByIDs_TopicID_Zero_MatchTopic()
        {
            SqliteQueryProvider queryProvider = new();

            queryProvider.AddMatchedTripReportTopic(1, 0, Guid.NewGuid().ToString());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void AddMatchedTripReportTopic_ByIDs_TopicID_Negative()
        {
            SqliteQueryProvider queryProvider = new();

            queryProvider.AddMatchedTripReportTopic(-1, -2, Guid.NewGuid().ToString());
        }

        #endregion

        #endregion

        [TestMethod]
        public void AddMatchedTripReportTopic_ByIDs_ResultIsNotNull()
        {
            SqliteQueryProvider queryProvider = new();
            var result = queryProvider.AddMatchedTripReportTopic(1, 2, Guid.NewGuid().ToString());

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void AddMatchedTripReportTopic_ByIDs_ResultIsCorrectType()
        {
            SqliteQueryProvider queryProvider = new();
            var result = queryProvider.AddMatchedTripReportTopic(2, 3, Guid.NewGuid().ToString());

            Console.WriteLine(result.GetType().FullName);
            Assert.IsTrue(result.GetType().Name == "SQLiteCommand");
        }

        [TestMethod]
        public void AddMatchedTripReportTopic_ByIDs_CommandText()
        {
            SqliteQueryProvider queryProvider = new();
            var result = queryProvider.AddMatchedTripReportTopic(4, 5, Guid.NewGuid().ToString());

            Console.WriteLine(result.CommandText);
            Assert.IsTrue(result.CommandText.Trim().ToUpper().StartsWith("INSERT INTO MATCHEDTRIPREPORTTOPIC"));
            Assert.IsTrue(result.CommandText.Trim().ToUpper().Contains("VALUES"));
            Assert.IsTrue(result.CommandText.Trim().ToUpper().Contains("SELECT LAST_INSERT_ROWID();"));
        }

        [TestMethod]
        public void AddMatchedTripReportTopic_ByIDs_QueryIsParameterized()
        {
            string text = Guid.NewGuid().ToString();
            SqliteQueryProvider queryProvider = new();
            var result = queryProvider.AddMatchedTripReportTopic(5, 6, Guid.NewGuid().ToString());

            Console.WriteLine(result.CommandText);

            foreach (DbParameter parameter in result.Parameters)
            {
                Console.WriteLine(parameter.ParameterName + "\t" + parameter.Value);
            }

            // None of the input values should appear in the text
            Assert.IsFalse(result.CommandText.Trim().ToUpper().Contains(text.ToUpper()));

            // Instead, there should be parameters with these values
            Assert.IsTrue(result.Parameters.Contains("@TopicID"));
            Assert.AreEqual(5L, result.Parameters["@TripReportID"].Value);
        }

        #endregion

        #endregion

        #region DeleteMatchedTripReportTopic

        #region By ID

        #region ID must be in range

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void DeleteMatchedTripReportTopic_ByID_Zero()
        {
            SqliteQueryProvider queryProvider = new();
            queryProvider.DeleteMatchedTripReportTopic(0);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void DeleteMatchedTripReportTopic_ByID_Negative()
        {
            SqliteQueryProvider queryProvider = new();
            queryProvider.DeleteMatchedTripReportTopic(-25);
        }

        #endregion

        [TestMethod]
        public void DeleteMatchedTripReportTopic_ByID_ResultIsNotNull()
        {
            SqliteQueryProvider queryProvider = new();
            var result = queryProvider.DeleteMatchedTripReportTopic(100);

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void DeleteMatchedTripReportTopic_ByID_ResultIsCorrectType()
        {
            SqliteQueryProvider queryProvider = new();
            var result = queryProvider.DeleteMatchedTripReportTopic(100);

            Console.WriteLine(result.GetType().FullName);
            Assert.IsTrue(result.GetType().Name == "SQLiteCommand");
        }

        [TestMethod]
        public void DeleteMatchedTripReportTopic_ByID_CommandText()
        {
            SqliteQueryProvider queryProvider = new();
            var result = queryProvider.DeleteMatchedTripReportTopic(100);

            Console.WriteLine(result.CommandText);
            Assert.IsTrue(result.CommandText.Trim().ToUpper().StartsWith("DELETE FROM MATCHEDTRIPREPORTTOPIC"));
        }

        [TestMethod]
        public void DeleteMatchedTripReportTopic_ByID_QueryIsParameterized()
        {
            SqliteQueryProvider queryProvider = new();
            var result = queryProvider.DeleteMatchedTripReportTopic(int.MaxValue);

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
        public void DeleteMatchedTripReportTopic_ByDTO_Null()
        {
            SqliteQueryProvider queryProvider = new();
            queryProvider.DeleteMatchedTripReportTopic(null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void DeleteMatchedTripReportTopic_ByDTO_Zero()
        {
            SqliteQueryProvider queryProvider = new();
            queryProvider.DeleteMatchedTripReportTopic(new MatchedTripReportTopic() { ID = 0 });
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void DeleteMatchedTripReportTopic_ByDTO_Negative()
        {
            SqliteQueryProvider queryProvider = new();
            queryProvider.DeleteMatchedTripReportTopic(new MatchedTripReportTopic() { ID = -25 });
        }

        #endregion

        [TestMethod]
        public void DeleteMatchedTripReportTopic_ByDTO_ResultIsNotNull()
        {
            SqliteQueryProvider queryProvider = new();
            var result = queryProvider.DeleteMatchedTripReportTopic(new MatchedTripReportTopic() { ID = 1 });

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void DeleteMatchedTripReportTopic_ByDTO_ResultIsCorrectType()
        {
            SqliteQueryProvider queryProvider = new();
            var result = queryProvider.DeleteMatchedTripReportTopic(new MatchedTripReportTopic() { ID = 80 });

            Console.WriteLine(result.GetType().FullName);
            Assert.IsTrue(result.GetType().Name == "SQLiteCommand");
        }

        [TestMethod]
        public void DeleteMatchedTripReportTopic_ByDTO_CommandText()
        {
            SqliteQueryProvider queryProvider = new();
            var result = queryProvider.DeleteMatchedTripReportTopic(new MatchedTripReportTopic() { ID = 100 });

            Console.WriteLine(result.CommandText);
            Assert.IsTrue(result.CommandText.Trim().ToUpper().StartsWith("DELETE FROM MATCHEDTRIPREPORTTOPIC"));
        }

        [TestMethod]
        public void DeleteMatchedTripReportTopic_ByDTO_QueryIsParameterized()
        {
            SqliteQueryProvider queryProvider = new();
            var result = queryProvider.DeleteMatchedTripReportTopic(new MatchedTripReportTopic() { ID = int.MaxValue });

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

        #region GetMatchedTripReportTopic

        #region ID must be in range

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void GetMatchedTripReportTopicZero()
        {
            SqliteQueryProvider queryProvider = new();
            queryProvider.GetMatchedTripReportTopic(0);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void GetMatchedTripReportTopicNegative()
        {
            SqliteQueryProvider queryProvider = new();
            queryProvider.GetMatchedTripReportTopic(-25);
        }

        #endregion

        [TestMethod]
        public void GetMatchedTripReportTopicResultIsNotNull()
        {
            SqliteQueryProvider queryProvider = new();
            var result = queryProvider.GetMatchedTripReportTopic(100);

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void GetMatchedTripReportTopicResultIsCorrectType()
        {
            SqliteQueryProvider queryProvider = new();
            var result = queryProvider.GetMatchedTripReportTopic(100);

            Console.WriteLine(result.GetType().FullName);
            Assert.IsTrue(result.GetType().Name == "SQLiteCommand");
        }

        [TestMethod]
        public void GetMatchedTripReportTopicCommandText()
        {
            SqliteQueryProvider queryProvider = new();
            var result = queryProvider.GetMatchedTripReportTopic(100);

            Console.WriteLine(result.CommandText);
            Assert.IsTrue(result.CommandText.Trim().ToUpper().StartsWith("SELECT"));
            Assert.IsTrue(result.CommandText.Trim().ToUpper().Contains("FROM MATCHEDTRIPREPORTTOPIC"));
        }

        [TestMethod]
        public void GetMatchedTripReportTopicQueryIsParameterized()
        {
            SqliteQueryProvider queryProvider = new();
            var result = queryProvider.GetMatchedTripReportTopic(int.MaxValue);

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
