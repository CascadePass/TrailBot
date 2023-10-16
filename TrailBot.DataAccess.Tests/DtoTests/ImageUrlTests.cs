using CascadePass.TrailBot.DataAccess.DTO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace TrailBot.DataAccess.Tests.DtoTests
{
    [TestClass]
    public class ImageUrlTests
    {
        [TestMethod]
        public void CanCreateInstance()
        {
            _ = new ImageUrl();
        }

        #region Nullable properties start null

        [TestMethod]
        public void ImageWidth_DefaultsToNull()
        {
            ImageUrl testImageUrl = new();
            Assert.IsNull(testImageUrl.ImageWidth);
        }

        [TestMethod]
        public void ImageHeight_DefaultsToNull()
        {
            ImageUrl testImageUrl = new();
            Assert.IsNull(testImageUrl.ImageHeight);
        }

        [TestMethod]
        public void FileSize_DefaultsToNull()
        {
            ImageUrl testImageUrl = new();
            Assert.IsNull(testImageUrl.FileSize);
        }

        #endregion

        #region Get/set access same value

        [TestMethod]
        public void ID_GetSetAccessSameValue()
        {
            long expectedValue = new Random().Next();
            ImageUrl testImageUrl = new()
            {
                ID = expectedValue
            };

            Assert.AreEqual(expectedValue, testImageUrl.ID);
        }

        [TestMethod]
        public void Address_GetSetAccessSameValue()
        {
            string expectedValue = Guid.NewGuid().ToString();
            ImageUrl testImageUrl = new()
            {
                Address = expectedValue
            };

            Assert.AreEqual(expectedValue, testImageUrl.Address);
        }

        [TestMethod]
        public void ImageWidth_GetSetAccessSameValue()
        {
            long expectedValue = new Random().Next();
            ImageUrl testImageUrl = new()
            {
                ImageWidth = expectedValue
            };

            Assert.AreEqual(expectedValue, testImageUrl.ImageWidth);
        }

        [TestMethod]
        public void ImageHeight_GetSetAccessSameValue()
        {
            long expectedValue = new Random().Next();
            ImageUrl testImageUrl = new()
            {
                ImageHeight = expectedValue
            };

            Assert.AreEqual(expectedValue, testImageUrl.ImageHeight);
        }

        [TestMethod]
        public void FileSize_GetSetAccessSameValue()
        {
            long expectedValue = new Random().Next();
            ImageUrl testImageUrl = new()
            {
                FileSize = expectedValue
            };

            Assert.AreEqual(expectedValue, testImageUrl.FileSize);
        }

        [TestMethod]
        public void Comments_GetSetAccessSameValue()
        {
            string expectedValue = Guid.NewGuid().ToString();
            ImageUrl testImageUrl = new()
            {
                Comments = expectedValue
            };

            Assert.AreEqual(expectedValue, testImageUrl.Comments);
        }

        #endregion
    }
}
