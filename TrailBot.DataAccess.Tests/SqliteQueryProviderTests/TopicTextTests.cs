using CascadePass.TrailBot.DataAccess;
using CascadePass.TrailBot.DataAccess.DTO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Data.Common;

namespace TrailBot.DataAccess.Tests.SqliteQueryProviderTests
{
    [TestClass]
    public class TopicTextTests
    {
        #region AddTopicText

        #region Validation

        #region Null

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void AddTopicText_ByText_Null()
        {
            SqliteQueryProvider queryProvider = new();
            queryProvider.AddTopicText(null);
        }

        #endregion

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void AddTopicText_ByDTO_HasID()
        {
            SqliteQueryProvider queryProvider = new();
            queryProvider.AddTopicText(new() { ID = 25, TextID = 1, TopicID = 2 });
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void AddTopicText_ByDTO_TextID_Zero()
        {
            SqliteQueryProvider queryProvider = new();
            queryProvider.AddTopicText(new() { TextID = 0, TopicID = 2 });
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void AddTopicText_ByDTO_TextID_Negative()
        {
            SqliteQueryProvider queryProvider = new();
            queryProvider.AddTopicText(new() { TextID = -1, TopicID = 2 });
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void AddTopicText_ByDTO_TopicID_Zero()
        {
            SqliteQueryProvider queryProvider = new();
            queryProvider.AddTopicText(new() { TextID = 12, TopicID = -2 });
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void AddTopicText_ByDTO_TopicID_Negative()
        {
            SqliteQueryProvider queryProvider = new();
            queryProvider.AddTopicText(new() { TextID = 17, TopicID = -3 });
        }

        #endregion

        [TestMethod]
        public void AddTopicText_ByDTO_ResultIsNotNull()
        {
            SqliteQueryProvider queryProvider = new();
            var result = queryProvider.AddTopicText(new() { TextID = 1, TopicID = 2 });

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void AddTopicText_ByDTO_ResultIsCorrectType()
        {
            SqliteQueryProvider queryProvider = new();
            var result = queryProvider.AddTopicText(new() { TextID = 1, TopicID = 2 });

            Console.WriteLine(result.GetType().FullName);
            Assert.IsTrue(result.GetType().Name == "SQLiteCommand");
        }

        [TestMethod]
        public void AddTopicText_ByDTO_CommandText()
        {
            SqliteQueryProvider queryProvider = new();
            var result = queryProvider.AddTopicText(new() { TextID = 1, TopicID = 2 });

            Console.WriteLine(result.CommandText);
            Assert.IsTrue(result.CommandText.Trim().ToUpper().StartsWith("INSERT INTO TOPICTEXT"));
            Assert.IsTrue(result.CommandText.Trim().ToUpper().Contains("VALUES"));
            Assert.IsTrue(result.CommandText.Trim().ToUpper().Contains("SELECT LAST_INSERT_ROWID();"));
        }

        [TestMethod]
        public void AddTopicText_ByDTO_QueryIsParameterized()
        {
            SqliteQueryProvider queryProvider = new();
            var result = queryProvider.AddTopicText(new() { TextID = 1111, TopicID = 2222 });

            Console.WriteLine(result.CommandText);

            foreach (DbParameter parameter in result.Parameters)
            {
                Console.WriteLine(parameter.ParameterName + "\t" + parameter.Value);
            }

            // None of the input values should appear in the text
            Assert.IsFalse(result.CommandText.Contains("1111"));
            Assert.IsFalse(result.CommandText.Contains("2222"));

            // Instead, there should be parameters with these values
            Assert.IsTrue(result.Parameters.Contains("@TopicID"));
            Assert.AreEqual(2222L, result.Parameters["@TopicID"].Value);
        }

        #endregion

        #region UpdateTopicText

        #region By ID (3 parameters: ID, ParentID, Text)

        #region Validation (ID must be in range)

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void UpdateTopicText_ByID_ID_Zero()
        {
            SqliteQueryProvider queryProvider = new();
            queryProvider.UpdateTopicText(0, 1, 1);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void UpdateTopicText_ByID_ID_Negative()
        {
            SqliteQueryProvider queryProvider = new();
            queryProvider.UpdateTopicText(-25, 1, 1);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void UpdateTopicText_ByID_TopicID_Zero()
        {
            SqliteQueryProvider queryProvider = new();
            queryProvider.UpdateTopicText(1, 0, 1);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void UpdateTopicText_ByID_TopicID_Negative()
        {
            SqliteQueryProvider queryProvider = new();
            queryProvider.UpdateTopicText(25, -1, 1);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void UpdateTopicText_ByID_TextID_Zero()
        {
            SqliteQueryProvider queryProvider = new();
            queryProvider.UpdateTopicText(1, 1, 0);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void UpdateTopicText_ByID_TextID_Negative()
        {
            SqliteQueryProvider queryProvider = new();
            queryProvider.UpdateTopicText(5, 1, -10);
        }

        #endregion

        #region Correctness

        [TestMethod]
        public void UpdateTopicText_ByID_3Params_ResultIsNotNull()
        {
            SqliteQueryProvider queryProvider = new();
            var result = queryProvider.UpdateTopicText(100, 200, 300);

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void UpdateTopicText_ByID_3Params_ResultIsCorrectType()
        {
            SqliteQueryProvider queryProvider = new();
            var result = queryProvider.UpdateTopicText(100, 200, 300);

            Console.WriteLine(result.GetType().FullName);
            Assert.IsTrue(result.GetType().Name == "SQLiteCommand");
        }

        [TestMethod]
        public void UpdateTopicText_ByID_3Params_CommandText()
        {
            SqliteQueryProvider queryProvider = new();
            var result = queryProvider.UpdateTopicText(100, 200, 300);

            Console.WriteLine(result.CommandText);
            Assert.IsTrue(result.CommandText.Trim().ToUpper().StartsWith("UPDATE TOPICTEXT"));
            Assert.IsTrue(result.CommandText.Trim().ToUpper().Contains("WHERE"));
        }

        [TestMethod]
        public void UpdateTopicText_ByID_3Params_QueryIsParameterized()
        {
            SqliteQueryProvider queryProvider = new();
            var result = queryProvider.UpdateTopicText(100, 500, 1000);

            Console.WriteLine(result.CommandText);

            foreach (DbParameter parameter in result.Parameters)
            {
                Console.WriteLine(parameter.ParameterName + "\t" + parameter.Value);
            }

            Assert.IsTrue(result.Parameters.Contains("@MatchTextID"));
            Assert.IsTrue(result.Parameters.Contains("@TopicID"));
        }

        #endregion

        #endregion

        #region By DTO

        #region Validation (ID must be in range)

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void UpdateTopicText_ByDTO_Null()
        {
            SqliteQueryProvider queryProvider = new();
            queryProvider.UpdateTopicText(null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void UpdateTopicText_ByDTO_ID_Zero()
        {
            SqliteQueryProvider queryProvider = new();
            queryProvider.UpdateTopicText(new() { ID = 0, TopicID = 1, TextID = 2 });
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void UpdateTopicText_ByDTO_ID_Negative()
        {
            SqliteQueryProvider queryProvider = new();
            queryProvider.UpdateTopicText(new() { ID = -45, TopicID = 1, TextID = 2 });
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void UpdateTopicText_ByDTO_TopicID_Zero()
        {
            SqliteQueryProvider queryProvider = new();
            queryProvider.UpdateTopicText(new() { ID = 10, TopicID = 0, TextID = 2 });
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void UpdateTopicText_ByDTO_TopicID_Negative()
        {
            SqliteQueryProvider queryProvider = new();
            queryProvider.UpdateTopicText(new() { ID = 45, TopicID = -1, TextID = 2 });
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void UpdateTopicText_ByDTO_TextID_Zero()
        {
            SqliteQueryProvider queryProvider = new();
            queryProvider.UpdateTopicText(new() { ID = 10, TopicID = 20, TextID = 0 });
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void UpdateTopicText_ByDTO_TextID_Negative()
        {
            SqliteQueryProvider queryProvider = new();
            queryProvider.UpdateTopicText(new() { ID = 45, TopicID = 1, TextID = -2 });
        }

        #endregion

        #region Correctness

        [TestMethod]
        public void UpdateTopicText_ByDTO_ResultIsNotNull()
        {
            SqliteQueryProvider queryProvider = new();
            var result = queryProvider.UpdateTopicText(new() { ID = 100, TextID = 200, TopicID = 300 });

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void UpdateTopicText_ByDTO_ResultIsCorrectType()
        {
            SqliteQueryProvider queryProvider = new();
            var result = queryProvider.UpdateTopicText(new() { ID = 100, TextID = 200, TopicID = 300 });

            Console.WriteLine(result.GetType().FullName);
            Assert.IsTrue(result.GetType().Name == "SQLiteCommand");
        }

        [TestMethod]
        public void UpdateTopicText_ByDTO_CommandText()
        {
            SqliteQueryProvider queryProvider = new();
            var result = queryProvider.UpdateTopicText(new() { ID = 100, TextID = 200, TopicID = 300 });

            Console.WriteLine(result.CommandText);
            Assert.IsTrue(result.CommandText.Trim().ToUpper().StartsWith("UPDATE TOPICTEXT"));
            Assert.IsTrue(result.CommandText.Trim().ToUpper().Contains("WHERE"));
        }

        [TestMethod]
        public void UpdateTopicText_ByDTO_QueryIsParameterized()
        {
            SqliteQueryProvider queryProvider = new();
            var result = queryProvider.UpdateTopicText(new() { ID = 100, TextID = 200, TopicID = 300 });

            Console.WriteLine(result.CommandText);

            foreach (DbParameter parameter in result.Parameters)
            {
                Console.WriteLine(parameter.ParameterName + "\t" + parameter.Value);
            }

            Assert.IsTrue(result.Parameters.Contains("@MatchTextID"));
            Assert.IsTrue(result.Parameters.Contains("@TopicID"));
        }

        #endregion

        #endregion

        #endregion

        #region DeleteTopicText

        #region By ID

        #region ID must be in range

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void DeleteTopicText_ByID_Zero()
        {
            SqliteQueryProvider queryProvider = new();
            queryProvider.DeleteTopicText(0);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void DeleteTopicText_ByID_Negative()
        {
            SqliteQueryProvider queryProvider = new();
            queryProvider.DeleteTopicText(-25);
        }

        #endregion

        [TestMethod]
        public void DeleteTopicText_ByID_ResultIsNotNull()
        {
            SqliteQueryProvider queryProvider = new();
            var result = queryProvider.DeleteTopicText(100);

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void DeleteTopicText_ByID_ResultIsCorrectType()
        {
            SqliteQueryProvider queryProvider = new();
            var result = queryProvider.DeleteTopicText(100);

            Console.WriteLine(result.GetType().FullName);
            Assert.IsTrue(result.GetType().Name == "SQLiteCommand");
        }

        [TestMethod]
        public void DeleteTopicText_ByID_CommandText()
        {
            SqliteQueryProvider queryProvider = new();
            var result = queryProvider.DeleteTopicText(100);

            Console.WriteLine(result.CommandText);
            Assert.IsTrue(result.CommandText.Trim().ToUpper().StartsWith("DELETE FROM TOPICTEXT"));
        }

        [TestMethod]
        public void DeleteTopicText_ByID_QueryIsParameterized()
        {
            SqliteQueryProvider queryProvider = new();
            var result = queryProvider.DeleteTopicText(int.MaxValue);

            Console.WriteLine(result.CommandText);

            foreach (DbParameter parameter in result.Parameters)
            {
                Console.WriteLine(parameter.ParameterName + "\t" + parameter.Value);
            }

            Assert.IsTrue(result.Parameters.Contains("@TopicTextID"));
            Assert.IsFalse(result.CommandText.Contains(int.MaxValue.ToString()));
        }

        #endregion

        #region By DTO

        #region Validation

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void DeleteTopicText_ByDTO_Null()
        {
            SqliteQueryProvider queryProvider = new();
            queryProvider.DeleteTopicText(null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void DeleteTopicText_ByDTO_Zero()
        {
            SqliteQueryProvider queryProvider = new();
            queryProvider.DeleteTopicText(new TopicText() { ID = 0 });
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void DeleteTopicText_ByDTO_Negative()
        {
            SqliteQueryProvider queryProvider = new();
            queryProvider.DeleteTopicText(new TopicText() { ID = -25 });
        }

        #endregion

        [TestMethod]
        public void DeleteTopicText_ByDTO_ResultIsNotNull()
        {
            SqliteQueryProvider queryProvider = new();
            var result = queryProvider.DeleteTopicText(new TopicText() { ID = 1 });

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void DeleteTopicText_ByDTO_ResultIsCorrectType()
        {
            SqliteQueryProvider queryProvider = new();
            var result = queryProvider.DeleteTopicText(new TopicText() { ID = 80 });

            Console.WriteLine(result.GetType().FullName);
            Assert.IsTrue(result.GetType().Name == "SQLiteCommand");
        }

        [TestMethod]
        public void DeleteTopicText_ByDTO_CommandText()
        {
            SqliteQueryProvider queryProvider = new();
            var result = queryProvider.DeleteTopicText(new TopicText() { ID = 100 });

            Console.WriteLine(result.CommandText);
            Assert.IsTrue(result.CommandText.Trim().ToUpper().StartsWith("DELETE FROM TOPICTEXT"));
        }

        [TestMethod]
        public void DeleteTopicText_ByDTO_QueryIsParameterized()
        {
            SqliteQueryProvider queryProvider = new();
            var result = queryProvider.DeleteTopicText(new TopicText() { ID = 25000 });

            Console.WriteLine(result.CommandText);

            foreach (DbParameter parameter in result.Parameters)
            {
                Console.WriteLine(parameter.ParameterName + "\t" + parameter.Value);
            }

            Assert.IsTrue(result.Parameters.Contains("@TopicTextID"));
            Assert.IsFalse(result.CommandText.Contains(int.MaxValue.ToString()));
        }

        #endregion

        #endregion

        #region GetTopicText

        #region ID must be in range

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void GetTopicTextZero()
        {
            SqliteQueryProvider queryProvider = new();
            queryProvider.GetTopicText(0);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void GetTopicTextNegative()
        {
            SqliteQueryProvider queryProvider = new();
            queryProvider.GetTopicText(-25);
        }

        #endregion

        #region Correctness

        [TestMethod]
        public void GetTopicTextResultIsNotNull()
        {
            SqliteQueryProvider queryProvider = new();
            var result = queryProvider.GetTopicText(100);

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void GetTopicTextResultIsCorrectType()
        {
            SqliteQueryProvider queryProvider = new();
            var result = queryProvider.GetTopicText(100);

            Console.WriteLine(result.GetType().FullName);
            Assert.IsTrue(result.GetType().Name == "SQLiteCommand");
        }

        [TestMethod]
        public void GetTopicTextCommandText()
        {
            SqliteQueryProvider queryProvider = new();
            var result = queryProvider.GetTopicText(100);

            Console.WriteLine(result.CommandText);
            Assert.IsTrue(result.CommandText.Trim().ToUpper().StartsWith("SELECT"));
            Assert.IsTrue(result.CommandText.Trim().ToUpper().Contains("FROM TOPICTEXT"));
        }

        [TestMethod]
        public void GetTopicTextQueryIsParameterized()
        {
            SqliteQueryProvider queryProvider = new();
            var result = queryProvider.GetTopicText(int.MaxValue);

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

        #region GetTopicTextByTopic

        #region Validation

        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        [TestMethod]
        public void GetTopicTextByTopic_Zero()
        {
            SqliteQueryProvider queryProvider = new();
            queryProvider.GetTopicTextByTopic(0);
        }

        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        [TestMethod]
        public void GetTopicTextByTopic_Negative()
        {
            SqliteQueryProvider queryProvider = new();
            queryProvider.GetTopicTextByTopic(-37);
        }

        #endregion

        #region Correctness

        [TestMethod]
        public void GetTopicTextByTopicByTopicResultIsNotNull()
        {
            SqliteQueryProvider queryProvider = new();
            var result = queryProvider.GetTopicTextByTopic(100);

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void GetTopicTextByTopicByTopicResultIsCorrectType()
        {
            SqliteQueryProvider queryProvider = new();
            var result = queryProvider.GetTopicTextByTopic(100);

            Console.WriteLine(result.GetType().FullName);
            Assert.IsTrue(result.GetType().Name == "SQLiteCommand");
        }

        [TestMethod]
        public void GetTopicTextByTopicByTopicCommandText()
        {
            SqliteQueryProvider queryProvider = new();
            var result = queryProvider.GetTopicTextByTopic(100);

            Console.WriteLine(result.CommandText);
            Assert.IsTrue(result.CommandText.Trim().ToUpper().StartsWith("SELECT"));
            Assert.IsTrue(result.CommandText.Trim().ToUpper().Contains("FROM TOPICTEXT"));
        }

        [TestMethod]
        public void GetTopicTextByTopicByTopicQueryIsParameterized()
        {
            SqliteQueryProvider queryProvider = new();
            var result = queryProvider.GetTopicTextByTopic(int.MaxValue);

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
    }
}
