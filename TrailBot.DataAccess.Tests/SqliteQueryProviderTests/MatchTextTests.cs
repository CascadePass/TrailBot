using CascadePass.TrailBot.DataAccess;
using CascadePass.TrailBot.DataAccess.DTO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Data.Common;

namespace TrailBot.DataAccess.Tests.SqliteQueryProviderTests
{
    [TestClass]
    public class MatchTextTests
    {
        #region AddMatchText

        #region Validation

        #region Name can't be blank

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void AddMatchText_ByText_Null()
        {
            SqliteQueryProvider queryProvider = new();
            queryProvider.AddMatchText(null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void AddMatchText_ByDTO_Text_Null()
        {
            SqliteQueryProvider queryProvider = new();
            queryProvider.AddMatchText(new MatchText() { Text = null });
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void AddMatchText_ByDTO_Text_EmptyString()
        {
            SqliteQueryProvider queryProvider = new();
            queryProvider.AddMatchText(new MatchText() { Text = string.Empty });
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void AddMatchText_ByDTO_Text_WhiteSpace()
        {
            SqliteQueryProvider queryProvider = new();
            queryProvider.AddMatchText(new MatchText() { Text = " " });
        }

        #endregion

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void AddMatchText_ByDTO_HasID()
        {
            SqliteQueryProvider queryProvider = new();
            queryProvider.AddMatchText(new MatchText() { ID = 25, Text = "test" });
        }

        #endregion

        [TestMethod]
        public void AddMatchText_ByDTO_ResultIsNotNull()
        {
            SqliteQueryProvider queryProvider = new();
            var result = queryProvider.AddMatchText(new() { Text = Guid.NewGuid().ToString() });

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void AddMatchText_ByDTO_ResultIsCorrectType()
        {
            SqliteQueryProvider queryProvider = new();
            var result = queryProvider.AddMatchText(new() { Text = Guid.NewGuid().ToString() });

            Console.WriteLine(result.GetType().FullName);
            Assert.IsTrue(result.GetType().Name == "SQLiteCommand");
        }

        [TestMethod]
        public void AddMatchText_ByDTO_CommandText()
        {
            SqliteQueryProvider queryProvider = new();
            var result = queryProvider.AddMatchText(new() { Text = Guid.NewGuid().ToString() });

            Console.WriteLine(result.CommandText);
            Assert.IsTrue(result.CommandText.Trim().ToUpper().StartsWith("INSERT INTO MATCHTEXT"));
            Assert.IsTrue(result.CommandText.Trim().ToUpper().Contains("VALUES"));
            Assert.IsTrue(result.CommandText.Trim().ToUpper().Contains("SELECT LAST_INSERT_ROWID();"));
        }

        [TestMethod]
        public void AddMatchText_ByDTO_QueryIsParameterized()
        {
            string text = Guid.NewGuid().ToString();
            SqliteQueryProvider queryProvider = new();
            var result = queryProvider.AddMatchText(new() { Text = text });

            Console.WriteLine(result.CommandText);

            foreach (DbParameter parameter in result.Parameters)
            {
                Console.WriteLine(parameter.ParameterName + "\t" + parameter.Value);
            }

            // None of the input values should appear in the text
            Assert.IsFalse(result.CommandText.Trim().ToUpper().Contains(text.ToUpper()));

            // Instead, there should be parameters with these values
            Assert.IsTrue(result.Parameters.Contains("@Text"));
            Assert.AreEqual(text, result.Parameters["@Text"].Value);
        }

        #endregion

        #region UpdateMatchText

        #region By ID (2 parameters: ID, Text)

        #region Validation

        #region ID must be in range

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void UpdateMatchText_ByID_Zero()
        {
            SqliteQueryProvider queryProvider = new();
            queryProvider.UpdateMatchText(0, Guid.NewGuid().ToString());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void UpdateMatchText_ByID_Negative()
        {
            SqliteQueryProvider queryProvider = new();
            queryProvider.UpdateMatchText(-25, Guid.NewGuid().ToString());
        }

        #endregion

        #region Text

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void UpdateMatchText_ByID_Text_Null()
        {
            SqliteQueryProvider queryProvider = new();
            queryProvider.UpdateMatchText(0, null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void UpdateMatchText_ByID_Text_Empty()
        {
            SqliteQueryProvider queryProvider = new();
            queryProvider.UpdateMatchText(-25, string.Empty);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void UpdateMatchText_ByID_Text_WhiteSpace()
        {
            SqliteQueryProvider queryProvider = new();
            queryProvider.UpdateMatchText(-25, " \t \r\n");
        }

        #endregion

        #endregion

        #region Correctness

        [TestMethod]
        public void UpdateMatchText_ByID_ResultIsNotNull()
        {
            SqliteQueryProvider queryProvider = new();
            var result = queryProvider.UpdateMatchText(100, Guid.NewGuid().ToString());

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void UpdateMatchText_ByID_ResultIsCorrectType()
        {
            SqliteQueryProvider queryProvider = new();
            var result = queryProvider.UpdateMatchText(100, Guid.NewGuid().ToString());

            Console.WriteLine(result.GetType().FullName);
            Assert.IsTrue(result.GetType().Name == "SQLiteCommand");
        }

        [TestMethod]
        public void UpdateMatchText_ByID_CommandText()
        {
            SqliteQueryProvider queryProvider = new();
            var result = queryProvider.UpdateMatchText(100, Guid.NewGuid().ToString());

            Console.WriteLine(result.CommandText);
            Assert.IsTrue(result.CommandText.Trim().ToUpper().StartsWith("UPDATE MATCHTEXT"));
            Assert.IsTrue(result.CommandText.Trim().ToUpper().Contains("WHERE"));
        }

        [TestMethod]
        public void UpdateMatchText_ByID_QueryIsParameterized()
        {
            SqliteQueryProvider queryProvider = new();
            var result = queryProvider.UpdateMatchText(100, Guid.NewGuid().ToString());

            Console.WriteLine(result.CommandText);

            foreach (DbParameter parameter in result.Parameters)
            {
                Console.WriteLine(parameter.ParameterName + "\t" + parameter.Value);
            }

            Assert.IsTrue(result.Parameters.Contains("@Text"));
            Assert.IsTrue(result.Parameters.Contains("@MatchTextID"));
        }

        [TestMethod]
        public void UpdateMatchText_ByID_DoesNotUpdateParentID()
        {
            SqliteQueryProvider queryProvider = new();
            var result = queryProvider.UpdateMatchText(100, Guid.NewGuid().ToString());

            Console.WriteLine(result.CommandText);

            foreach (DbParameter parameter in result.Parameters)
            {
                Console.WriteLine(parameter.ParameterName + "\t" + parameter.Value);
            }

            Assert.IsTrue(result.Parameters.Contains("@Text"));
            Assert.IsTrue(result.Parameters.Contains("@MatchTextID"));
            Assert.IsFalse(result.Parameters.Contains("@FalseMatchParentID"));
        }
        #endregion

        #endregion

        #region By ID (3 parameters: ID, ParentID, Text)

        #region Validation

        #region ID must be in range

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void UpdateMatchText_ByID_3Params_Zero()
        {
            SqliteQueryProvider queryProvider = new();
            queryProvider.UpdateMatchText(0, 0, Guid.NewGuid().ToString());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void UpdateMatchText_ByID_3Params_Negative()
        {
            SqliteQueryProvider queryProvider = new();
            queryProvider.UpdateMatchText(-25, 0, Guid.NewGuid().ToString());
        }

        #endregion

        #region Text

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void UpdateMatchText_ByID_3Params_Text_Null()
        {
            SqliteQueryProvider queryProvider = new();
            queryProvider.UpdateMatchText(100, 100, null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void UpdateMatchText_ByID_3Params_Text_Empty()
        {
            SqliteQueryProvider queryProvider = new();
            queryProvider.UpdateMatchText(100, 100, string.Empty);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void UpdateMatchText_ByID_3Params_Text_WhiteSpace()
        {
            SqliteQueryProvider queryProvider = new();
            queryProvider.UpdateMatchText(100, 100, " \t \r\n");
        }

        #endregion

        #region Correctness

        [TestMethod]
        public void UpdateMatchText_ByID_3Params_ResultIsNotNull()
        {
            SqliteQueryProvider queryProvider = new();
            var result = queryProvider.UpdateMatchText(100, 200, Guid.NewGuid().ToString());

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void UpdateMatchText_ByID_3Params_ResultIsCorrectType()
        {
            SqliteQueryProvider queryProvider = new();
            var result = queryProvider.UpdateMatchText(100, 300, Guid.NewGuid().ToString());

            Console.WriteLine(result.GetType().FullName);
            Assert.IsTrue(result.GetType().Name == "SQLiteCommand");
        }

        [TestMethod]
        public void UpdateMatchText_ByID_3Params_CommandText()
        {
            SqliteQueryProvider queryProvider = new();
            var result = queryProvider.UpdateMatchText(100, 400, Guid.NewGuid().ToString());

            Console.WriteLine(result.CommandText);
            Assert.IsTrue(result.CommandText.Trim().ToUpper().StartsWith("UPDATE MATCHTEXT"));
            Assert.IsTrue(result.CommandText.Trim().ToUpper().Contains("WHERE"));
        }

        [TestMethod]
        public void UpdateMatchText_ByID_3Params_QueryIsParameterized()
        {
            SqliteQueryProvider queryProvider = new();
            var result = queryProvider.UpdateMatchText(100, 500, Guid.NewGuid().ToString());

            Console.WriteLine(result.CommandText);

            foreach (DbParameter parameter in result.Parameters)
            {
                Console.WriteLine(parameter.ParameterName + "\t" + parameter.Value);
            }

            Assert.IsTrue(result.Parameters.Contains("@Text"));
            Assert.IsTrue(result.Parameters.Contains("@MatchTextID"));
        }

        [TestMethod]
        public void UpdateMatchText_ByID_3Params_UpdatesParentID_NormalValue()
        {
            SqliteQueryProvider queryProvider = new();
            var result = queryProvider.UpdateMatchText(100, 500, Guid.NewGuid().ToString());

            Console.WriteLine(result.CommandText);

            foreach (DbParameter parameter in result.Parameters)
            {
                Console.WriteLine(parameter.ParameterName + "\t" + parameter.Value);
            }

            Assert.IsTrue(result.Parameters.Contains("@Text"));
            Assert.IsTrue(result.Parameters.Contains("@MatchTextID"));
            Assert.IsTrue(result.Parameters.Contains("@FalseMatchParentID"));

            Assert.AreEqual(500L, result.Parameters["@FalseMatchParentID"].Value);
        }

        [TestMethod]
        public void UpdateMatchText_ByID_3Params_UpdatesParentID_Null()
        {
            SqliteQueryProvider queryProvider = new();
            var result = queryProvider.UpdateMatchText(100, null, Guid.NewGuid().ToString());

            Console.WriteLine(result.CommandText);

            foreach (DbParameter parameter in result.Parameters)
            {
                Console.WriteLine(parameter.ParameterName + "\t" + parameter.Value);
            }

            Assert.IsTrue(result.Parameters.Contains("@Text"));
            Assert.IsTrue(result.Parameters.Contains("@MatchTextID"));
            Assert.IsTrue(result.Parameters.Contains("@FalseMatchParentID"));

            Assert.AreEqual(null, result.Parameters["@FalseMatchParentID"].Value);
        }

        #endregion

        #endregion

        #region By DTO

        #region Validation

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void UpdateMatchText_ByDTO_Null()
        {
            SqliteQueryProvider queryProvider = new();
            queryProvider.UpdateMatchText(null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void UpdateMatchText_ByDTO_Zero()
        {
            SqliteQueryProvider queryProvider = new();
            queryProvider.UpdateMatchText(new() { ID = 0, Text = Guid.NewGuid().ToString() });
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void UpdateMatchText_ByDTO_Negative()
        {
            SqliteQueryProvider queryProvider = new();
            queryProvider.UpdateMatchText(new() { ID = -30, Text = Guid.NewGuid().ToString() });
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void UpdateMatchText_ByDTO_ParentID_Negative()
        {
            SqliteQueryProvider queryProvider = new();
            queryProvider.UpdateMatchText(new() { ID = 30, ParentID = -27, Text = Guid.NewGuid().ToString() });
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void UpdateMatchText_ByDTO_Text_Null()
        {
            SqliteQueryProvider queryProvider = new();
            queryProvider.UpdateMatchText(new() { ID = 0, Text = null });
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void UpdateMatchText_ByDTO_Text_Empty()
        {
            SqliteQueryProvider queryProvider = new();
            queryProvider.UpdateMatchText(new() { ID = 0, Text = string.Empty });
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void UpdateMatchText_ByDTO_Text_WhiteSpace()
        {
            SqliteQueryProvider queryProvider = new();
            queryProvider.UpdateMatchText(new() { ID = 0, Text = "\t" });
        }

        #endregion

        #region Correctness

        [TestMethod]
        public void UpdateMatchText_ByDTO_ResultIsNotNull()
        {
            SqliteQueryProvider queryProvider = new();
            var result = queryProvider.UpdateMatchText(new() { ID = 100, Text = Guid.NewGuid().ToString() });

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void UpdateMatchText_ByDTO_ResultIsCorrectType()
        {
            SqliteQueryProvider queryProvider = new();
            var result = queryProvider.UpdateMatchText(new() { ID = 100, Text = Guid.NewGuid().ToString() });

            Console.WriteLine(result.GetType().FullName);
            Assert.IsTrue(result.GetType().Name == "SQLiteCommand");
        }

        [TestMethod]
        public void UpdateMatchText_ByDTO_CommandText()
        {
            SqliteQueryProvider queryProvider = new();
            var result = queryProvider.UpdateMatchText(new() { ID = 100, Text = Guid.NewGuid().ToString() });

            Console.WriteLine(result.CommandText);
            Assert.IsTrue(result.CommandText.Trim().ToUpper().StartsWith("UPDATE MATCHTEXT"));
            Assert.IsTrue(result.CommandText.Trim().ToUpper().Contains("WHERE"));
        }

        [TestMethod]
        public void UpdateMatchText_ByDTO_QueryIsParameterized()
        {
            SqliteQueryProvider queryProvider = new();
            var result = queryProvider.UpdateMatchText(new() { ID = 100, Text = Guid.NewGuid().ToString() });

            Console.WriteLine(result.CommandText);

            foreach (DbParameter parameter in result.Parameters)
            {
                Console.WriteLine(parameter.ParameterName + "\t" + parameter.Value);
            }

            Assert.IsTrue(result.Parameters.Contains("@Text"));
            Assert.IsTrue(result.Parameters.Contains("@MatchTextID"));
        }

        #endregion

        #endregion

        #endregion

        #endregion

        #region DeleteMatchText

        #region By ID

        #region ID must be in range

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void DeleteMatchText_ByID_Zero()
        {
            SqliteQueryProvider queryProvider = new();
            queryProvider.DeleteMatchText(0);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void DeleteMatchText_ByID_Negative()
        {
            SqliteQueryProvider queryProvider = new();
            queryProvider.DeleteMatchText(-25);
        }

        #endregion

        [TestMethod]
        public void DeleteMatchText_ByID_ResultIsNotNull()
        {
            SqliteQueryProvider queryProvider = new();
            var result = queryProvider.DeleteMatchText(100);

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void DeleteMatchText_ByID_ResultIsCorrectType()
        {
            SqliteQueryProvider queryProvider = new();
            var result = queryProvider.DeleteMatchText(100);

            Console.WriteLine(result.GetType().FullName);
            Assert.IsTrue(result.GetType().Name == "SQLiteCommand");
        }

        [TestMethod]
        public void DeleteMatchText_ByID_CommandText()
        {
            SqliteQueryProvider queryProvider = new();
            var result = queryProvider.DeleteMatchText(100);

            Console.WriteLine(result.CommandText);
            Assert.IsTrue(result.CommandText.Trim().ToUpper().StartsWith("DELETE FROM MATCHTEXT"));
        }

        [TestMethod]
        public void DeleteMatchText_ByID_QueryIsParameterized()
        {
            SqliteQueryProvider queryProvider = new();
            var result = queryProvider.DeleteMatchText(int.MaxValue);

            Console.WriteLine(result.CommandText);

            foreach (DbParameter parameter in result.Parameters)
            {
                Console.WriteLine(parameter.ParameterName + "\t" + parameter.Value);
            }

            Assert.IsTrue(result.Parameters.Contains("@MatchTextID"));
            Assert.IsFalse(result.CommandText.Contains(int.MaxValue.ToString()));
        }

        #endregion

        #region By DTO

        #region Validation

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void DeleteMatchText_ByDTO_Null()
        {
            SqliteQueryProvider queryProvider = new();
            queryProvider.DeleteMatchText(null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void DeleteMatchText_ByDTO_Zero()
        {
            SqliteQueryProvider queryProvider = new();
            queryProvider.DeleteMatchText(new MatchText() { ID = 0 });
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void DeleteMatchText_ByDTO_Negative()
        {
            SqliteQueryProvider queryProvider = new();
            queryProvider.DeleteMatchText(new MatchText() { ID = -25 });
        }

        #endregion

        [TestMethod]
        public void DeleteMatchText_ByDTO_ResultIsNotNull()
        {
            SqliteQueryProvider queryProvider = new();
            var result = queryProvider.DeleteMatchText(new MatchText() { ID = 1 });

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void DeleteMatchText_ByDTO_ResultIsCorrectType()
        {
            SqliteQueryProvider queryProvider = new();
            var result = queryProvider.DeleteMatchText(new MatchText() { ID = 80 });

            Console.WriteLine(result.GetType().FullName);
            Assert.IsTrue(result.GetType().Name == "SQLiteCommand");
        }

        [TestMethod]
        public void DeleteMatchText_ByDTO_CommandText()
        {
            SqliteQueryProvider queryProvider = new();
            var result = queryProvider.DeleteMatchText(new MatchText() { ID = 100 });

            Console.WriteLine(result.CommandText);
            Assert.IsTrue(result.CommandText.Trim().ToUpper().StartsWith("DELETE FROM MATCHTEXT"));
        }

        [TestMethod]
        public void DeleteMatchText_ByDTO_QueryIsParameterized()
        {
            SqliteQueryProvider queryProvider = new();
            var result = queryProvider.DeleteMatchText(new MatchText() { ID = 25000 });

            Console.WriteLine(result.CommandText);

            foreach (DbParameter parameter in result.Parameters)
            {
                Console.WriteLine(parameter.ParameterName + "\t" + parameter.Value);
            }

            Assert.IsTrue(result.Parameters.Contains("@MatchTextID"));
            Assert.IsFalse(result.CommandText.Contains(int.MaxValue.ToString()));
        }

        #endregion

        #endregion

        #region GetMatchText

        #region ID must be in range

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void GetMatchTextZero()
        {
            SqliteQueryProvider queryProvider = new();
            queryProvider.GetMatchText(0);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void GetMatchTextNegative()
        {
            SqliteQueryProvider queryProvider = new();
            queryProvider.GetMatchText(-25);
        }

        #endregion

        #region Correctness

        [TestMethod]
        public void GetMatchTextResultIsNotNull()
        {
            SqliteQueryProvider queryProvider = new();
            var result = queryProvider.GetMatchText(100);

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void GetMatchTextResultIsCorrectType()
        {
            SqliteQueryProvider queryProvider = new();
            var result = queryProvider.GetMatchText(100);

            Console.WriteLine(result.GetType().FullName);
            Assert.IsTrue(result.GetType().Name == "SQLiteCommand");
        }

        [TestMethod]
        public void GetMatchTextCommandText()
        {
            SqliteQueryProvider queryProvider = new();
            var result = queryProvider.GetMatchText(100);

            Console.WriteLine(result.CommandText);
            Assert.IsTrue(result.CommandText.Trim().ToUpper().StartsWith("SELECT"));
            Assert.IsTrue(result.CommandText.Trim().ToUpper().Contains("FROM MATCHTEXT"));
        }

        [TestMethod]
        public void GetMatchTextQueryIsParameterized()
        {
            SqliteQueryProvider queryProvider = new();
            var result = queryProvider.GetMatchText(int.MaxValue);

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

        #region GetMatchTextByTopic

        #region Validation

        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        [TestMethod]
        public void GetTopicTextByTopic_Zero()
        {
            SqliteQueryProvider queryProvider = new();
            queryProvider.GetMatchTextByTopic(0);
        }

        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        [TestMethod]
        public void GetTopicTextByTopic_Negative()
        {
            SqliteQueryProvider queryProvider = new();
            queryProvider.GetMatchTextByTopic(-37);
        }

        #endregion

        #region Correctness

        [TestMethod]
        public void GetMatchTextByTopicResultIsNotNull()
        {
            SqliteQueryProvider queryProvider = new();
            var result = queryProvider.GetMatchTextByTopic(100);

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void GetMatchTextByTopicResultIsCorrectType()
        {
            SqliteQueryProvider queryProvider = new();
            var result = queryProvider.GetMatchTextByTopic(100);

            Console.WriteLine(result.GetType().FullName);
            Assert.IsTrue(result.GetType().Name == "SQLiteCommand");
        }

        [TestMethod]
        public void GetMatchTextByTopicCommandText()
        {
            SqliteQueryProvider queryProvider = new();
            var result = queryProvider.GetMatchTextByTopic(100);

            Console.WriteLine(result.CommandText);
            Assert.IsTrue(result.CommandText.Trim().ToUpper().StartsWith("SELECT"));
            Assert.IsTrue(result.CommandText.Trim().ToUpper().Contains("FROM MATCHTEXT"));
        }

        [TestMethod]
        public void GetMatchTextByTopicQueryIsParameterized()
        {
            SqliteQueryProvider queryProvider = new();
            var result = queryProvider.GetMatchTextByTopic(int.MaxValue);

            Console.WriteLine(result.CommandText);

            foreach (DbParameter parameter in result.Parameters)
            {
                Console.WriteLine(parameter.ParameterName + "\t" + parameter.Value);
            }

            Assert.IsTrue(result.Parameters.Contains("@TopicID"));
            Assert.IsFalse(result.CommandText.Contains(int.MaxValue.ToString()));
        }

        #endregion

        #endregion
    }
}
