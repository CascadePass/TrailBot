using CascadePass.TrailBot.DataAccess;
using CascadePass.TrailBot.DataAccess.DTO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Data.Common;

namespace TrailBot.DataAccess.Tests.SqliteQueryProviderTests
{
    [TestClass]
    public class ImageUrlTests
    {
        #region AddImageUrl

        #region By url (string)

        #region Validation (Address can't be blank)

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void AddImageUrl_Null()
        {
            SqliteQueryProvider queryProvider = new();
            queryProvider.AddImageUrl((string)null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void AddImageUrl_Url_EmptyString()
        {
            SqliteQueryProvider queryProvider = new();
            queryProvider.AddImageUrl(string.Empty);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void AddImageUrl_Url_WhiteSpace()
        {
            SqliteQueryProvider queryProvider = new();
            queryProvider.AddImageUrl(" ");
        }

        #endregion

        [TestMethod]
        public void AddImageUrl_ByString_ResultIsNotNull()
        {
            SqliteQueryProvider queryProvider = new();
            var result = queryProvider.AddImageUrl($"https://test/{Guid.NewGuid()}");

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void AddImageUrl_ByString_ResultIsCorrectType()
        {
            SqliteQueryProvider queryProvider = new();
            var result = queryProvider.AddImageUrl($"https://test/{Guid.NewGuid().ToString()}");

            Console.WriteLine(result.GetType().FullName);
            Assert.IsTrue(result.GetType().Name == "SQLiteCommand");
        }

        [TestMethod]
        public void AddImageUrl_ByString_CommandText()
        {
            SqliteQueryProvider queryProvider = new();
            var result = queryProvider.AddImageUrl($"https://test/{Guid.NewGuid().ToString()}");

            Console.WriteLine(result.CommandText);
            Assert.IsTrue(result.CommandText.Trim().ToUpper().StartsWith("INSERT INTO IMAGEURL"));
            Assert.IsTrue(result.CommandText.Trim().ToUpper().Contains("VALUES"));
            Assert.IsTrue(result.CommandText.Trim().ToUpper().Contains("SELECT LAST_INSERT_ROWID();"));
        }

        [TestMethod]
        public void AddImageUrl_ByString_QueryIsParameterized()
        {
            string text = $"https://test/{Guid.NewGuid()}";
            SqliteQueryProvider queryProvider = new();
            var result = queryProvider.AddImageUrl(text);

            Console.WriteLine(result.CommandText);

            foreach (DbParameter parameter in result.Parameters)
            {
                Console.WriteLine(parameter.ParameterName + "\t" + parameter.Value);
            }

            // None of the input values should appear in the text
            Assert.IsFalse(result.CommandText.Trim().ToUpper().Contains(text.ToUpper()));

            // Instead, there should be parameters with these values
            Assert.IsTrue(result.Parameters.Contains("@ImageUrl"));
            Assert.AreEqual(text, result.Parameters["@ImageUrl"].Value);
        }

        #endregion

        #region By DTO

        #region Validation

        #region Address can't be blank

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void AddImageUrl_By_DTO_Null()
        {
            SqliteQueryProvider queryProvider = new();
            queryProvider.AddImageUrl((ImageUrl)null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void AddImageUrl_By_DTO_Url_Null()
        {
            SqliteQueryProvider queryProvider = new();
            queryProvider.AddImageUrl(new ImageUrl() { Address = null });
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void AddImageUrl_By_DTO_Url_EmptyString()
        {
            SqliteQueryProvider queryProvider = new();
            queryProvider.AddImageUrl(new ImageUrl() { Address = string.Empty });
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void AddImageUrl_By_DTO_Url_WhiteSpace()
        {
            SqliteQueryProvider queryProvider = new();
            queryProvider.AddImageUrl(new ImageUrl() { Address = " " });
        }

        #endregion

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void AddImageUrl_By_DTO_HasID()
        {
            SqliteQueryProvider queryProvider = new();
            queryProvider.AddImageUrl(new ImageUrl() { ID = 25, Address = "test" });
        }

        #endregion

        [TestMethod]
        public void AddImageUrl_By_DTO_ResultIsNotNull()
        {
            SqliteQueryProvider queryProvider = new();
            var result = queryProvider.AddImageUrl(new ImageUrl() { Address = Guid.NewGuid().ToString() });

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void AddImageUrl_By_DTO_ResultIsCorrectType()
        {
            SqliteQueryProvider queryProvider = new();
            var result = queryProvider.AddImageUrl(new ImageUrl() { Address = Guid.NewGuid().ToString() });

            Console.WriteLine(result.GetType().FullName);
            Assert.IsTrue(result.GetType().Name == "SQLiteCommand");
        }

        [TestMethod]
        public void AddImageUrl_By_DTO_CommandText()
        {
            SqliteQueryProvider queryProvider = new();
            var result = queryProvider.AddImageUrl(new ImageUrl() { Address = Guid.NewGuid().ToString() });

            Console.WriteLine(result.CommandText);
            Assert.IsTrue(result.CommandText.Trim().ToUpper().StartsWith("INSERT INTO IMAGEURL"));
            Assert.IsTrue(result.CommandText.Trim().ToUpper().Contains("SELECT"));
            Assert.IsTrue(result.CommandText.Trim().ToUpper().Contains("SELECT LAST_INSERT_ROWID();"));
        }

        [TestMethod]
        public void AddImageUrl_By_DTO_QueryIsParameterized()
        {
            string text = Guid.NewGuid().ToString();
            SqliteQueryProvider queryProvider = new();
            var result = queryProvider.AddImageUrl(new ImageUrl() { Address = text });

            Console.WriteLine(result.CommandText);

            foreach (DbParameter parameter in result.Parameters)
            {
                Console.WriteLine(parameter.ParameterName + "\t" + parameter.Value);
            }

            // None of the input values should appear in the text
            Assert.IsFalse(result.CommandText.Trim().ToUpper().Contains(text.ToUpper()));

            // Instead, there should be parameters with these values
            Assert.IsTrue(result.Parameters.Contains("@ImageUrl"));
            Assert.AreEqual(text, result.Parameters["@ImageUrl"].Value);
        }

        #endregion

        #endregion

        #region UpdateImageUrl

        #region Validation

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void UpdateImageUrl_ByDTO_Null()
        {
            SqliteQueryProvider queryProvider = new();
            queryProvider.UpdateImageUrl(null);
        }

        #region (ID must be in range)

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void UpdateImageUrl_ByID_Zero()
        {
            SqliteQueryProvider queryProvider = new();
            queryProvider.UpdateImageUrl(new() { ID = 0, Address = $"https://test/{Guid.NewGuid()}" });
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void UpdateImageUrl_ByID_Negative()
        {
            SqliteQueryProvider queryProvider = new();
            queryProvider.UpdateImageUrl(new() { ID = -25, Address = $"https://test/{Guid.NewGuid()}"});
        }

        #endregion

        #region Name

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void UpdateImageUrl_ByID_Address_Null()
        {
            SqliteQueryProvider queryProvider = new();
            queryProvider.UpdateImageUrl(new() { ID = 125, Address = null });
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void UpdateImageUrl_ByID_Address_Empty()
        {
            SqliteQueryProvider queryProvider = new();
            queryProvider.UpdateImageUrl(new() { ID = 125, Address = string.Empty });
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void UpdateImageUrl_ByID_Address_Space()
        {
            SqliteQueryProvider queryProvider = new();
            queryProvider.UpdateImageUrl(new() { ID = 125, Address = " " });
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void UpdateImageUrl_ByID_Address_NewLines()
        {
            SqliteQueryProvider queryProvider = new();
            queryProvider.UpdateImageUrl(new() { ID = 125, Address = "\r\n\r\n" });
        }

        #endregion

        #endregion

        #region Correctness

        [TestMethod]
        public void UpdateImageUrl_ByID_ResultIsNotNull()
        {
            SqliteQueryProvider queryProvider = new();
            var result = queryProvider.UpdateImageUrl(new() { ID = 225, Address = $"https://test/{Guid.NewGuid()}" });

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void UpdateImageUrl_ByID_ResultIsCorrectType()
        {
            SqliteQueryProvider queryProvider = new();
            var result = queryProvider.UpdateImageUrl(new() { ID = 75, Address = $"https://test/{Guid.NewGuid()}" });

            Console.WriteLine(result.GetType().FullName);
            Assert.IsTrue(result.GetType().Name == "SQLiteCommand");
        }

        [TestMethod]
        public void UpdateImageUrl_ByID_CommandText()
        {
            SqliteQueryProvider queryProvider = new();
            var result = queryProvider.UpdateImageUrl(new() { ID = 5, Address = $"https://test/{Guid.NewGuid()}" });

            Console.WriteLine(result.CommandText);
            Assert.IsTrue(result.CommandText.Trim().ToUpper().StartsWith("UPDATE IMAGEURL"));
        }

        [TestMethod]
        public void UpdateImageUrl_ByID_QueryIsParameterized()
        {
            SqliteQueryProvider queryProvider = new();
            var result = queryProvider.UpdateImageUrl(new() { ID = 25, Address = $"https://test/{Guid.NewGuid()}" });

            Console.WriteLine(result.CommandText);

            foreach (DbParameter parameter in result.Parameters)
            {
                Console.WriteLine(parameter.ParameterName + "\t" + parameter.Value);
            }

            Assert.IsTrue(result.Parameters.Contains("@ID"));
        }

        #endregion

        #endregion

        #region DeleteImageUrl

        #region By ID

        #region ID must be in range

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void DeleteImageUrl_ByID_Zero()
        {
            SqliteQueryProvider queryProvider = new();
            queryProvider.DeleteImageUrl(0);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void DeleteImageUrl_ByID_Negative()
        {
            SqliteQueryProvider queryProvider = new();
            queryProvider.DeleteImageUrl(-25);
        }

        #endregion

        [TestMethod]
        public void DeleteImageUrl_ByID_ResultIsNotNull()
        {
            SqliteQueryProvider queryProvider = new();
            var result = queryProvider.DeleteImageUrl(100);

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void DeleteImageUrl_ByID_ResultIsCorrectType()
        {
            SqliteQueryProvider queryProvider = new();
            var result = queryProvider.DeleteImageUrl(100);

            Console.WriteLine(result.GetType().FullName);
            Assert.IsTrue(result.GetType().Name == "SQLiteCommand");
        }

        [TestMethod]
        public void DeleteImageUrl_ByID_CommandText()
        {
            SqliteQueryProvider queryProvider = new();
            var result = queryProvider.DeleteImageUrl(100);

            Console.WriteLine(result.CommandText);
            Assert.IsTrue(result.CommandText.Trim().ToUpper().StartsWith("DELETE FROM IMAGEURL"));
        }

        [TestMethod]
        public void DeleteImageUrl_ByID_QueryIsParameterized()
        {
            SqliteQueryProvider queryProvider = new();
            var result = queryProvider.DeleteImageUrl(int.MaxValue);

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
        public void DeleteImageUrl_ByDTO_Null()
        {
            SqliteQueryProvider queryProvider = new();
            queryProvider.DeleteImageUrl(null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void DeleteImageUrl_ByDTO_Zero()
        {
            SqliteQueryProvider queryProvider = new();
            queryProvider.DeleteImageUrl(new ImageUrl() { ID = 0 });
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void DeleteImageUrl_ByDTO_Negative()
        {
            SqliteQueryProvider queryProvider = new();
            queryProvider.DeleteImageUrl(new ImageUrl() { ID = -25 });
        }

        #endregion

        [TestMethod]
        public void DeleteImageUrl_ByDTO_ResultIsNotNull()
        {
            SqliteQueryProvider queryProvider = new();
            var result = queryProvider.DeleteImageUrl(new ImageUrl() { ID = 1 });

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void DeleteImageUrl_ByDTO_ResultIsCorrectType()
        {
            SqliteQueryProvider queryProvider = new();
            var result = queryProvider.DeleteImageUrl(new ImageUrl() { ID = 80 });

            Console.WriteLine(result.GetType().FullName);
            Assert.IsTrue(result.GetType().Name == "SQLiteCommand");
        }

        [TestMethod]
        public void DeleteImageUrl_ByDTO_CommandText()
        {
            SqliteQueryProvider queryProvider = new();
            var result = queryProvider.DeleteImageUrl(new ImageUrl() { ID = 100 });

            Console.WriteLine(result.CommandText);
            Assert.IsTrue(result.CommandText.Trim().ToUpper().StartsWith("DELETE FROM IMAGEURL"));
        }

        [TestMethod]
        public void DeleteImageUrl_ByDTO_QueryIsParameterized()
        {
            SqliteQueryProvider queryProvider = new();
            var result = queryProvider.DeleteImageUrl(new ImageUrl() { ID = 25000 });

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

        #region GetImageUrl

        #region ID must be in range

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void GetImageUrlZero()
        {
            SqliteQueryProvider queryProvider = new();
            queryProvider.GetImageUrl(0);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void GetImageUrlNegative()
        {
            SqliteQueryProvider queryProvider = new();
            queryProvider.GetImageUrl(-25);
        }

        #endregion

        [TestMethod]
        public void GetImageUrlResultIsNotNull()
        {
            SqliteQueryProvider queryProvider = new();
            var result = queryProvider.GetImageUrl(100);

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void GetImageUrlResultIsCorrectType()
        {
            SqliteQueryProvider queryProvider = new();
            var result = queryProvider.GetImageUrl(100);

            Console.WriteLine(result.GetType().FullName);
            Assert.IsTrue(result.GetType().Name == "SQLiteCommand");
        }

        [TestMethod]
        public void GetImageUrlCommandText()
        {
            SqliteQueryProvider queryProvider = new();
            var result = queryProvider.GetImageUrl(100);

            Console.WriteLine(result.CommandText);
            Assert.IsTrue(result.CommandText.Trim().ToUpper().StartsWith("SELECT"));
            Assert.IsTrue(result.CommandText.Trim().ToUpper().Contains("FROM IMAGEURL"));
        }

        [TestMethod]
        public void GetImageUrlQueryIsParameterized()
        {
            SqliteQueryProvider queryProvider = new();
            var result = queryProvider.GetImageUrl(int.MaxValue);

            Console.WriteLine(result.CommandText);

            foreach (DbParameter parameter in result.Parameters)
            {
                Console.WriteLine(parameter.ParameterName + "\t" + parameter.Value);
            }

            Assert.IsTrue(result.Parameters.Contains("@ID"));
            Assert.IsFalse(result.CommandText.Contains(int.MaxValue.ToString()));
        }

        #endregion

        #region GetImagesForTripReport

        #region ID must be in range

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void GetImagesForTripReport_Zero()
        {
            SqliteQueryProvider queryProvider = new();
            queryProvider.GetImagesForTripReport(0);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void GetImagesForTripReport_Negative()
        {
            SqliteQueryProvider queryProvider = new();
            queryProvider.GetImagesForTripReport(-25);
        }

        #endregion

        [TestMethod]
        public void GetImagesForTripReport_ResultIsNotNull()
        {
            SqliteQueryProvider queryProvider = new();
            var result = queryProvider.GetImagesForTripReport(100);

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void GetImagesForTripReport_ResultIsCorrectType()
        {
            SqliteQueryProvider queryProvider = new();
            var result = queryProvider.GetImagesForTripReport(100);

            Console.WriteLine(result.GetType().FullName);
            Assert.IsTrue(result.GetType().Name == "SQLiteCommand");
        }

        [TestMethod]
        public void GetImagesForTripReport_CommandText()
        {
            SqliteQueryProvider queryProvider = new();
            var result = queryProvider.GetImagesForTripReport(100);

            Console.WriteLine(result.CommandText);
            Assert.IsTrue(result.CommandText.Trim().ToUpper().StartsWith("SELECT"));
            Assert.IsTrue(result.CommandText.Trim().ToUpper().Contains("FROM"));
            Assert.IsTrue(result.CommandText.Trim().ToUpper().Contains("IMAGEURL"));
            Assert.IsTrue(result.CommandText.Trim().ToUpper().Contains("TRIPREPORTIMAGE"));
        }

        [TestMethod]
        public void GetImagesForTripReport_QueryIsParameterized()
        {
            SqliteQueryProvider queryProvider = new();
            var result = queryProvider.GetImagesForTripReport(int.MaxValue);

            Console.WriteLine(result.CommandText);

            foreach (DbParameter parameter in result.Parameters)
            {
                Console.WriteLine(parameter.ParameterName + "\t" + parameter.Value);
            }

            Assert.IsTrue(result.Parameters.Contains("@TripReportID"));
            Assert.IsFalse(result.CommandText.Contains(int.MaxValue.ToString()));
        }

        #endregion
    }
}
