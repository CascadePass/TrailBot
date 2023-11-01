using CascadePass.TrailBot.DataAccess;
using CascadePass.TrailBot.DataAccess.DTO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Data.Common;

namespace TrailBot.DataAccess.Tests.SqliteQueryProviderTests
{
    [TestClass]
    public class TopicTests
    {
        #region AddTopic

        #region Validation

        #region Name can't be blank

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void AddTopic_Null()
        {
            SqliteQueryProvider queryProvider = new();
            queryProvider.AddTopic(null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void AddTopic_Url_Null()
        {
            SqliteQueryProvider queryProvider = new();
            queryProvider.AddTopic(new Topic() { Name = null });
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void AddTopic_Url_EmptyString()
        {
            SqliteQueryProvider queryProvider = new();
            queryProvider.AddTopic(new Topic() { Name = string.Empty });
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void AddTopic_Url_WhiteSpace()
        {
            SqliteQueryProvider queryProvider = new();
            queryProvider.AddTopic(new Topic() { Name = " " });
        }

        #endregion

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void AddTopic_HasID()
        {
            SqliteQueryProvider queryProvider = new();
            queryProvider.AddTopic(new Topic() { ID = 25, Name = "test" });
        }

        #endregion

        [TestMethod]
        public void AddTopic_ResultIsNotNull()
        {
            SqliteQueryProvider queryProvider = new();
            var result = queryProvider.AddTopic(new() { Name = Guid.NewGuid().ToString() });

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void AddTopic_ResultIsCorrectType()
        {
            SqliteQueryProvider queryProvider = new();
            var result = queryProvider.AddTopic(new() { Name = Guid.NewGuid().ToString() });

            Console.WriteLine(result.GetType().FullName);
            Assert.IsTrue(result.GetType().Name == "SQLiteCommand");
        }

        [TestMethod]
        public void AddTopic_CommandText()
        {
            SqliteQueryProvider queryProvider = new();
            var result = queryProvider.AddTopic(new() { Name = Guid.NewGuid().ToString() });

            Console.WriteLine(result.CommandText);
            Assert.IsTrue(result.CommandText.Trim().ToUpper().StartsWith("INSERT INTO TOPIC"));
            Assert.IsTrue(result.CommandText.Trim().ToUpper().Contains("VALUES"));
            Assert.IsTrue(result.CommandText.Trim().ToUpper().Contains("SELECT LAST_INSERT_ROWID();"));
        }

        [TestMethod]
        public void AddTopic_QueryIsParameterized()
        {
            string text = Guid.NewGuid().ToString();
            SqliteQueryProvider queryProvider = new();
            var result = queryProvider.AddTopic(new() { Name = text });

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

        #endregion

        #region UpdateTopic

        #region By ID

        #region Validation

        #region (ID must be in range)

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void UpdateTopic_ByID_Zero()
        {
            SqliteQueryProvider queryProvider = new();
            queryProvider.UpdateTopic(0, Guid.NewGuid().ToString());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void UpdateTopic_ByID_Negative()
        {
            SqliteQueryProvider queryProvider = new();
            queryProvider.UpdateTopic(-25, Guid.NewGuid().ToString());
        }

        #endregion

        #region Name

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void UpdateTopic_ByID_Name_Null()
        {
            SqliteQueryProvider queryProvider = new();
            queryProvider.UpdateTopic(123, null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void UpdateTopic_ByID_Name_Empty()
        {
            SqliteQueryProvider queryProvider = new();
            queryProvider.UpdateTopic(123, string.Empty);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void UpdateTopic_ByID_Name_Space()
        {
            SqliteQueryProvider queryProvider = new();
            queryProvider.UpdateTopic(123, " ");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void UpdateTopic_ByID_Name_NewLines()
        {
            SqliteQueryProvider queryProvider = new();
            queryProvider.UpdateTopic(123, "\r\n");
        }

        #endregion

        #endregion

        #region Correctness

        [TestMethod]
        public void UpdateTopic_ByID_ResultIsNotNull()
        {
            SqliteQueryProvider queryProvider = new();
            var result = queryProvider.UpdateTopic(100, Guid.NewGuid().ToString());

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void UpdateTopic_ByID_ResultIsCorrectType()
        {
            SqliteQueryProvider queryProvider = new();
            var result = queryProvider.UpdateTopic(100, Guid.NewGuid().ToString());

            Console.WriteLine(result.GetType().FullName);
            Assert.IsTrue(result.GetType().Name == "SQLiteCommand");
        }

        [TestMethod]
        public void UpdateTopic_ByID_CommandText()
        {
            SqliteQueryProvider queryProvider = new();
            var result = queryProvider.UpdateTopic(100, Guid.NewGuid().ToString());

            Console.WriteLine(result.CommandText);
            Assert.IsTrue(result.CommandText.Trim().ToUpper().StartsWith("UPDATE TOPIC"));
        }

        [TestMethod]
        public void UpdateTopic_ByID_QueryIsParameterized()
        {
            SqliteQueryProvider queryProvider = new();
            var result = queryProvider.UpdateTopic(100, Guid.NewGuid().ToString());

            Console.WriteLine(result.CommandText);

            foreach (DbParameter parameter in result.Parameters)
            {
                Console.WriteLine(parameter.ParameterName + "\t" + parameter.Value);
            }

            Assert.IsTrue(result.Parameters.Contains("@Name"));
            Assert.IsTrue(result.Parameters.Contains("@ID"));
        }

        #endregion

        #endregion

        #region By DTO

        #region Validation (ID must be in range)

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void UpdateTopic_ByDTO_Null()
        {
            SqliteQueryProvider queryProvider = new();
            queryProvider.UpdateTopic(null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void UpdateTopic_ByDTO_Zero()
        {
            SqliteQueryProvider queryProvider = new();
            queryProvider.UpdateTopic(new () { ID = 0, Name = Guid.NewGuid().ToString() });
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void UpdateTopic_ByDTO_Negative()
        {
            SqliteQueryProvider queryProvider = new();
            queryProvider.UpdateTopic(new() { ID = -30, Name = Guid.NewGuid().ToString() });
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void UpdateTopic_ByDTO_Name_EmptyString()
        {
            SqliteQueryProvider queryProvider = new();
            queryProvider.UpdateTopic(new() { ID = -30, Name = string.Empty });
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void UpdateTopic_ByDTO_Name_Null()
        {
            SqliteQueryProvider queryProvider = new();
            queryProvider.UpdateTopic(new() { ID = -30, Name = string.Empty });
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void UpdateTopic_ByDTO_Name_WhiteSpace()
        {
            SqliteQueryProvider queryProvider = new();
            queryProvider.UpdateTopic(new() { ID = -30, Name = " " });
        }

        #endregion

        #region Correctness

        [TestMethod]
        public void UpdateTopic_ByDTO_ResultIsNotNull()
        {
            SqliteQueryProvider queryProvider = new();
            var result = queryProvider.UpdateTopic(new() { ID = 100, Name = Guid.NewGuid().ToString() });

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void UpdateTopic_ByDTO_ResultIsCorrectType()
        {
            SqliteQueryProvider queryProvider = new();
            var result = queryProvider.UpdateTopic(new() { ID = 100, Name = Guid.NewGuid().ToString() });

            Console.WriteLine(result.GetType().FullName);
            Assert.IsTrue(result.GetType().Name == "SQLiteCommand");
        }

        [TestMethod]
        public void UpdateTopic_ByDTO_CommandText()
        {
            SqliteQueryProvider queryProvider = new();
            var result = queryProvider.UpdateTopic(new() { ID = 100, Name = Guid.NewGuid().ToString() });

            Console.WriteLine(result.CommandText);
            Assert.IsTrue(result.CommandText.Trim().ToUpper().StartsWith("UPDATE TOPIC"));
        }

        [TestMethod]
        public void UpdateTopic_ByDTO_QueryIsParameterized()
        {
            SqliteQueryProvider queryProvider = new();
            var result = queryProvider.UpdateTopic(new() { ID = 100, Name = Guid.NewGuid().ToString() });

            Console.WriteLine(result.CommandText);

            foreach (DbParameter parameter in result.Parameters)
            {
                Console.WriteLine(parameter.ParameterName + "\t" + parameter.Value);
            }

            Assert.IsTrue(result.Parameters.Contains("@Name"));
            Assert.IsTrue(result.Parameters.Contains("@ID"));
        }

        #endregion

        #endregion

        #endregion

        #region DeleteTopic

        #region By ID

        #region ID must be in range

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void DeleteTopic_ByID_Zero()
        {
            SqliteQueryProvider queryProvider = new();
            queryProvider.DeleteTopic(0);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void DeleteTopic_ByID_Negative()
        {
            SqliteQueryProvider queryProvider = new();
            queryProvider.DeleteTopic(-25);
        }

        #endregion

        [TestMethod]
        public void DeleteTopic_ByID_ResultIsNotNull()
        {
            SqliteQueryProvider queryProvider = new();
            var result = queryProvider.DeleteTopic(100);

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void DeleteTopic_ByID_ResultIsCorrectType()
        {
            SqliteQueryProvider queryProvider = new();
            var result = queryProvider.DeleteTopic(100);

            Console.WriteLine(result.GetType().FullName);
            Assert.IsTrue(result.GetType().Name == "SQLiteCommand");
        }

        [TestMethod]
        public void DeleteTopic_ByID_CommandText()
        {
            SqliteQueryProvider queryProvider = new();
            var result = queryProvider.DeleteTopic(100);

            Console.WriteLine(result.CommandText);
            Assert.IsTrue(result.CommandText.Trim().ToUpper().StartsWith("DELETE FROM TOPIC"));
        }

        [TestMethod]
        public void DeleteTopic_ByID_QueryIsParameterized()
        {
            SqliteQueryProvider queryProvider = new();
            var result = queryProvider.DeleteTopic(int.MaxValue);

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
        public void DeleteTopic_ByDTO_Null()
        {
            SqliteQueryProvider queryProvider = new();
            queryProvider.DeleteTopic(null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void DeleteTopic_ByDTO_Zero()
        {
            SqliteQueryProvider queryProvider = new();
            queryProvider.DeleteTopic(new Topic() { ID = 0 });
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void DeleteTopic_ByDTO_Negative()
        {
            SqliteQueryProvider queryProvider = new();
            queryProvider.DeleteTopic(new Topic() { ID = -25 });
        }

        #endregion

        [TestMethod]
        public void DeleteTopic_ByDTO_ResultIsNotNull()
        {
            SqliteQueryProvider queryProvider = new();
            var result = queryProvider.DeleteTopic(new Topic() { ID = 1 });

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void DeleteTopic_ByDTO_ResultIsCorrectType()
        {
            SqliteQueryProvider queryProvider = new();
            var result = queryProvider.DeleteTopic(new Topic() { ID = 80 });

            Console.WriteLine(result.GetType().FullName);
            Assert.IsTrue(result.GetType().Name == "SQLiteCommand");
        }

        [TestMethod]
        public void DeleteTopic_ByDTO_CommandText()
        {
            SqliteQueryProvider queryProvider = new();
            var result = queryProvider.DeleteTopic(new Topic() { ID = 100 });

            Console.WriteLine(result.CommandText);
            Assert.IsTrue(result.CommandText.Trim().ToUpper().StartsWith("DELETE FROM TOPIC"));
        }

        [TestMethod]
        public void DeleteTopic_ByDTO_QueryIsParameterized()
        {
            SqliteQueryProvider queryProvider = new();
            var result = queryProvider.DeleteTopic(new Topic() { ID = 25000 });

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

        #region GetTopic

        #region ID must be in range

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void GetTopicZero()
        {
            SqliteQueryProvider queryProvider = new();
            queryProvider.GetTopic(0);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void GetTopicNegative()
        {
            SqliteQueryProvider queryProvider = new();
            queryProvider.GetTopic(-25);
        }

        #endregion

        [TestMethod]
        public void GetTopicResultIsNotNull()
        {
            SqliteQueryProvider queryProvider = new();
            var result = queryProvider.GetTopic(100);

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void GetTopicResultIsCorrectType()
        {
            SqliteQueryProvider queryProvider = new();
            var result = queryProvider.GetTopic(100);

            Console.WriteLine(result.GetType().FullName);
            Assert.IsTrue(result.GetType().Name == "SQLiteCommand");
        }

        [TestMethod]
        public void GetTopicCommandText()
        {
            SqliteQueryProvider queryProvider = new();
            var result = queryProvider.GetTopic(100);

            Console.WriteLine(result.CommandText);
            Assert.IsTrue(result.CommandText.Trim().ToUpper().StartsWith("SELECT"));
            Assert.IsTrue(result.CommandText.Trim().ToUpper().Contains("FROM TOPIC"));
        }

        [TestMethod]
        public void GetTopicQueryIsParameterized()
        {
            SqliteQueryProvider queryProvider = new();
            var result = queryProvider.GetTopic(int.MaxValue);

            Console.WriteLine(result.CommandText);

            foreach (DbParameter parameter in result.Parameters)
            {
                Console.WriteLine(parameter.ParameterName + "\t" + parameter.Value);
            }

            Assert.IsTrue(result.Parameters.Contains("@ID"));
            Assert.IsFalse(result.CommandText.Contains(int.MaxValue.ToString()));
        }

        #endregion

        #region GetTopics

        [TestMethod]
        public void GetTopicsResultIsNotNull()
        {
            SqliteQueryProvider queryProvider = new();
            var result = queryProvider.GetTopics();

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void GetTopicsResultIsCorrectType()
        {
            SqliteQueryProvider queryProvider = new();
            var result = queryProvider.GetTopics();

            Console.WriteLine(result.GetType().FullName);
            Assert.IsTrue(result.GetType().Name == "SQLiteCommand");
        }

        [TestMethod]
        public void GetTopicsCommandText()
        {
            SqliteQueryProvider queryProvider = new();
            var result = queryProvider.GetTopics();

            Console.WriteLine(result.CommandText);
            Assert.IsTrue(result.CommandText.Trim().ToUpper().StartsWith("SELECT"));
            Assert.IsTrue(result.CommandText.Trim().ToUpper().Contains("FROM TOPIC"));
        }

        #endregion
    }
}
