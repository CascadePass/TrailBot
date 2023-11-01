using CascadePass.TrailBot.DataAccess.DTO;
using CascadePass.TrailBot.DataAccess;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrailBot.DataAccess.Tests.SqliteQueryProviderTests
{
    [TestClass]
    public class WtaTripReportImageTests
    {
        #region AddWtaTripReportImage

        #region By IDs

        #region Validation

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void AddWtaTripReportImage_ByID_TripReportID_0()
        {
            SqliteQueryProvider queryProvider = new();
            queryProvider.AddWtaTripReportImage(0, 1);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void AddWtaTripReportImage_ByID_ImageID_0()
        {
            SqliteQueryProvider queryProvider = new();
            queryProvider.AddWtaTripReportImage(2, 0);
        }

        #endregion

        [TestMethod]
        public void AddWtaTripReportImage_ByID_ResultIsNotNull()
        {
            SqliteQueryProvider queryProvider = new();
            var result = queryProvider.AddWtaTripReportImage(1, 2);

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void AddWtaTripReportImage_ByID_ResultIsCorrectType()
        {
            SqliteQueryProvider queryProvider = new();
            var result = queryProvider.AddWtaTripReportImage(2, 3);

            Console.WriteLine(result.GetType().FullName);
            Assert.IsTrue(result.GetType().Name == "SQLiteCommand");
        }

        [TestMethod]
        public void AddWtaTripReportImage_ByID_CommandText()
        {
            SqliteQueryProvider queryProvider = new();
            var result = queryProvider.AddWtaTripReportImage(3, 4);

            Console.WriteLine(result.CommandText);
            Assert.IsTrue(result.CommandText.Trim().ToUpper().StartsWith("INSERT INTO TRIPREPORTIMAGE"));
            Assert.IsTrue(result.CommandText.Trim().ToUpper().Contains("SELECT"));
            Assert.IsTrue(result.CommandText.Trim().ToUpper().Contains("SELECT LAST_INSERT_ROWID();"));
        }

        [TestMethod]
        public void AddWtaTripReportImage_ByID_QueryIsParameterized()
        {
            SqliteQueryProvider queryProvider = new();
            var result = queryProvider.AddWtaTripReportImage(5, 6);

            Console.WriteLine(result.CommandText);

            foreach (DbParameter parameter in result.Parameters)
            {
                Console.WriteLine(parameter.ParameterName + "\t" + parameter.Value);
            }

            // None of the input values should appear in the text
            Assert.IsFalse(result.CommandText.Trim().ToUpper().Contains("6"));

            // Instead, there should be parameters with these values
            Assert.IsTrue(result.Parameters.Contains("@ImageID"));
            Assert.AreEqual(6L, result.Parameters["@ImageID"].Value);
        }

        #endregion

        #region By DTO

        #region Validation

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void AddWtaTripReportImage_By_DTO_Null()
        {
            SqliteQueryProvider queryProvider = new();
            queryProvider.AddWtaTripReportImage((WtaTripReportImage)null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void AddWtaTripReportImage_By_DTO_TripReportID_Zero()
        {
            SqliteQueryProvider queryProvider = new();
            queryProvider.AddWtaTripReportImage(new WtaTripReportImage() { WtaTripReportID = 0, ImageID = 12 });
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void AddWtaTripReportImage_By_DTO_ImageID_Zero()
        {
            SqliteQueryProvider queryProvider = new();
            queryProvider.AddWtaTripReportImage(new WtaTripReportImage() { WtaTripReportID = 100, ImageID = 0 });
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void AddWtaTripReportImage_By_DTO_ID_Positive()
        {
            SqliteQueryProvider queryProvider = new();
            queryProvider.AddWtaTripReportImage(new WtaTripReportImage() { ID = 1, WtaTripReportID = 100, ImageID = 10 });
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void AddWtaTripReportImage_By_DTO_ID_Negative()
        {
            SqliteQueryProvider queryProvider = new();
            queryProvider.AddWtaTripReportImage(new WtaTripReportImage() { ID = -1, WtaTripReportID = 100, ImageID = 10 });
        }

        #endregion

        [TestMethod]
        public void AddWtaTripReportImage_By_DTO_ResultIsNotNull()
        {
            SqliteQueryProvider queryProvider = new();
            var result = queryProvider.AddWtaTripReportImage(new WtaTripReportImage() { WtaTripReportID = 1, ImageID = 2 });

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void AddWtaTripReportImage_By_DTO_ResultIsCorrectType()
        {
            SqliteQueryProvider queryProvider = new();
            var result = queryProvider.AddWtaTripReportImage(new WtaTripReportImage() { WtaTripReportID = 1, ImageID = 2 });

            Console.WriteLine(result.GetType().FullName);
            Assert.IsTrue(result.GetType().Name == "SQLiteCommand");
        }

        [TestMethod]
        public void AddWtaTripReportImage_By_DTO_CommandText()
        {
            SqliteQueryProvider queryProvider = new();
            var result = queryProvider.AddWtaTripReportImage(new WtaTripReportImage() { WtaTripReportID = 1, ImageID = 2 });

            Console.WriteLine(result.CommandText);
            Assert.IsTrue(result.CommandText.Trim().ToUpper().StartsWith("INSERT INTO TRIPREPORTIMAGE"));
            Assert.IsTrue(result.CommandText.Trim().ToUpper().Contains("SELECT"));
            Assert.IsTrue(result.CommandText.Trim().ToUpper().Contains("SELECT LAST_INSERT_ROWID();"));
        }

        [TestMethod]
        public void AddWtaTripReportImage_By_DTO_QueryIsParameterized()
        {
            SqliteQueryProvider queryProvider = new();
            var result = queryProvider.AddWtaTripReportImage(new WtaTripReportImage() { WtaTripReportID = 1, ImageID = 2 });

            Console.WriteLine(result.CommandText);

            foreach (DbParameter parameter in result.Parameters)
            {
                Console.WriteLine(parameter.ParameterName + "\t" + parameter.Value);
            }

            // None of the input values should appear in the text
            Assert.IsFalse(result.CommandText.Trim().ToUpper().Contains("2"));

            // Instead, there should be parameters with these values
            Assert.IsTrue(result.Parameters.Contains("@ImageID"));
            Assert.AreEqual(2L, result.Parameters["@ImageID"].Value);
        }

        #endregion

        #endregion

        #region DeleteWtaTripReportImage

        #region By ID

        #region ID must be in range

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void DeleteWtaTripReportImage_ByID_Zero()
        {
            SqliteQueryProvider queryProvider = new();
            queryProvider.DeleteWtaTripReportImage(0);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void DeleteWtaTripReportImage_ByID_Negative()
        {
            SqliteQueryProvider queryProvider = new();
            queryProvider.DeleteWtaTripReportImage(-25);
        }

        #endregion

        [TestMethod]
        public void DeleteWtaTripReportImage_ByID_ResultIsNotNull()
        {
            SqliteQueryProvider queryProvider = new();
            var result = queryProvider.DeleteWtaTripReportImage(100);

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void DeleteWtaTripReportImage_ByID_ResultIsCorrectType()
        {
            SqliteQueryProvider queryProvider = new();
            var result = queryProvider.DeleteWtaTripReportImage(100);

            Console.WriteLine(result.GetType().FullName);
            Assert.IsTrue(result.GetType().Name == "SQLiteCommand");
        }

        [TestMethod]
        public void DeleteWtaTripReportImage_ByID_CommandText()
        {
            SqliteQueryProvider queryProvider = new();
            var result = queryProvider.DeleteWtaTripReportImage(100);

            Console.WriteLine(result.CommandText);
            Assert.IsTrue(result.CommandText.Trim().ToUpper().StartsWith("DELETE FROM TRIPREPORTIMAGE"));
        }

        [TestMethod]
        public void DeleteWtaTripReportImage_ByID_QueryIsParameterized()
        {
            SqliteQueryProvider queryProvider = new();
            var result = queryProvider.DeleteWtaTripReportImage(int.MaxValue);

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
        public void DeleteWtaTripReportImage_ByDTO_Null()
        {
            SqliteQueryProvider queryProvider = new();
            queryProvider.DeleteWtaTripReportImage(null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void DeleteWtaTripReportImage_ByDTO_Zero()
        {
            SqliteQueryProvider queryProvider = new();
            queryProvider.DeleteWtaTripReportImage(new WtaTripReportImage() { ID = 0 });
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void DeleteWtaTripReportImage_ByDTO_Negative()
        {
            SqliteQueryProvider queryProvider = new();
            queryProvider.DeleteWtaTripReportImage(new WtaTripReportImage() { ID = -25 });
        }

        #endregion

        [TestMethod]
        public void DeleteWtaTripReportImage_ByDTO_ResultIsNotNull()
        {
            SqliteQueryProvider queryProvider = new();
            var result = queryProvider.DeleteWtaTripReportImage(new WtaTripReportImage() { ID = 1 });

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void DeleteWtaTripReportImage_ByDTO_ResultIsCorrectType()
        {
            SqliteQueryProvider queryProvider = new();
            var result = queryProvider.DeleteWtaTripReportImage(new WtaTripReportImage() { ID = 80 });

            Console.WriteLine(result.GetType().FullName);
            Assert.IsTrue(result.GetType().Name == "SQLiteCommand");
        }

        [TestMethod]
        public void DeleteWtaTripReportImage_ByDTO_CommandText()
        {
            SqliteQueryProvider queryProvider = new();
            var result = queryProvider.DeleteWtaTripReportImage(new WtaTripReportImage() { ID = 100 });

            Console.WriteLine(result.CommandText);
            Assert.IsTrue(result.CommandText.Trim().ToUpper().StartsWith("DELETE FROM TRIPREPORTIMAGE"));
        }

        [TestMethod]
        public void DeleteWtaTripReportImage_ByDTO_QueryIsParameterized()
        {
            SqliteQueryProvider queryProvider = new();
            var result = queryProvider.DeleteWtaTripReportImage(new WtaTripReportImage() { ID = 25000 });

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

        #region GetWtaTripReportImage

        #region ID must be in range

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void GetWtaTripReportImageZero()
        {
            SqliteQueryProvider queryProvider = new();
            queryProvider.GetWtaTripReportImage(0);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void GetWtaTripReportImageNegative()
        {
            SqliteQueryProvider queryProvider = new();
            queryProvider.GetWtaTripReportImage(-25);
        }

        #endregion

        [TestMethod]
        public void GetWtaTripReportImageResultIsNotNull()
        {
            SqliteQueryProvider queryProvider = new();
            var result = queryProvider.GetWtaTripReportImage(100);

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void GetWtaTripReportImageResultIsCorrectType()
        {
            SqliteQueryProvider queryProvider = new();
            var result = queryProvider.GetWtaTripReportImage(100);

            Console.WriteLine(result.GetType().FullName);
            Assert.IsTrue(result.GetType().Name == "SQLiteCommand");
        }

        [TestMethod]
        public void GetWtaTripReportImageCommandText()
        {
            SqliteQueryProvider queryProvider = new();
            var result = queryProvider.GetWtaTripReportImage(100);

            Console.WriteLine(result.CommandText);
            Assert.IsTrue(result.CommandText.Trim().ToUpper().StartsWith("SELECT"));
            Assert.IsTrue(result.CommandText.Trim().ToUpper().Contains("FROM TRIPREPORTIMAGE"));
        }

        [TestMethod]
        public void GetWtaTripReportImageQueryIsParameterized()
        {
            SqliteQueryProvider queryProvider = new();
            var result = queryProvider.GetWtaTripReportImage(int.MaxValue);

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
