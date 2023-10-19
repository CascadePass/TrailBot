using CascadePass.TrailBot.DataAccess;
using CascadePass.TrailBot.DataAccess.DTO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace TrailBot.DataAccess.Tests.IntegrationTests
{
    [TestClass]
    public class ImageUrlTests : SqliteIntegrationTestClass
    {
        #region Constructor

        public ImageUrlTests()
        {
            this.DatabaseFilename = "C:\\Users\\User\\Documents\\TrailBot\\TrailBot-test.db";
            Database.ConnectionString = $"Data Source={this.DatabaseFilename}";
            Database.QueryProvider = new SqliteQueryProvider();
        }

        #endregion

        [TestMethod]
        public void AddImageUrl_ByDTO()
        {
            this.AssertRequirements();

            var imageUrl = this.GetRandomImageUrl();

            Database.AddImageUrl(imageUrl);

            // Was it actually saved?
            var validate = Database.GetImageUrl(imageUrl.ID);
            Assert.IsNotNull(validate);

            this.AssertSameImageUrl(imageUrl, validate);

            // Cleanup
            Database.DeleteProvider(imageUrl.ID);
        }

        [TestMethod]
        public void AddImageUrl_ByAddress()
        {
            this.AssertRequirements();

            var imageUrl = $"https://test/image/{Guid.NewGuid()}";

            long generatedID = Database.AddImageUrl(imageUrl);

            // Was it actually saved?
            var validate = Database.GetImageUrl(generatedID);
            Assert.IsNotNull(validate);

            Assert.AreEqual(imageUrl, validate.Address);
            //this.AssertSameImageUrl(imageUrl, validate);

            // Cleanup
            Database.DeleteProvider(generatedID);
        }

        [TestMethod]
        public void UpdateImageUrl()
        {
            this.AssertRequirements();

            var imageUrl = new ImageUrl() { Address = $"https://integrationTest/imageUrl/{Guid.NewGuid()}", };

            Database.AddImageUrl(imageUrl);

            imageUrl.ImageWidth = new Random().Next(800, 8000);
            imageUrl.ImageHeight = new Random().Next(800, 8000);
            imageUrl.FileSize = new Random().Next(100000, int.MaxValue);
            imageUrl.Comments = Guid.NewGuid().ToString();

            Database.UpdateImageUrl(imageUrl);

            // Was it actually saved?
            var validate2 = Database.GetImageUrl(imageUrl.ID);
            Assert.IsNotNull(validate2);

            this.AssertSameImageUrl(imageUrl, validate2);

            // Cleanup
            Database.DeleteProvider(imageUrl.ID);
        }

        [TestMethod]
        public void UpdateImageUrl_NullsAllowed()
        {
            this.AssertRequirements();

            var imageUrl = this.GetRandomImageUrl();

            Database.AddImageUrl(imageUrl);

            imageUrl.ImageWidth = null;
            imageUrl.ImageHeight = null;
            imageUrl.FileSize = null;
            imageUrl.Comments = null;

            Database.UpdateImageUrl(imageUrl);

            // Was it actually saved?
            var validate2 = Database.GetImageUrl(imageUrl.ID);
            Assert.IsNotNull(validate2);

            this.AssertSameImageUrl(imageUrl, validate2);

            // Cleanup
            Database.DeleteProvider(imageUrl.ID);
        }

        [TestMethod]
        public void DeleteProvider_ByDTO()
        {
            this.AssertRequirements();

            var imageUrl = this.GetRandomImageUrl();

            Database.AddImageUrl(imageUrl);
            Database.DeleteImageUrl(imageUrl);
        }

        [TestMethod]
        public void DeleteProvider_ByID()
        {
            this.AssertRequirements();

            var imageUrl = this.GetRandomImageUrl();

            Database.AddImageUrl(imageUrl);
            Database.DeleteImageUrl(imageUrl.ID);
        }

        [TestMethod]
        public void GetImageUrl_NotThere()
        {
            var nonExistentImage = Database.GetImageUrl(int.MaxValue);
            Assert.IsNull(nonExistentImage);
        }

        [TestMethod]
        public void GetImagesForTripReport()
        {
            this.AssertRequirements();

            var url = new Url() { Address = $"https://GetImagesForTripReport/{Guid.NewGuid()}/" };
            Database.AddUrl(url);

            var tripReport = new WtaTripReport() { Url = url };
            Database.AddWtaTripReport(tripReport);

            List<ImageUrl> images = new();
            for (int i = 0; i < 10; i++)
            {
                var imageUrl = this.GetRandomImageUrl();
                Database.AddImageUrl(imageUrl);
                Database.AddWtaTripReportImage(tripReport.ID, imageUrl.ID);

                images.Add(imageUrl);
            }

            var byTripReport = Database.GetImagesForTripReport(tripReport.ID);

            Assert.AreEqual(images.Count, byTripReport.Count);

            foreach (ImageUrl image in images)
            {
                Assert.IsTrue(byTripReport.Any(m => m.ID == image.ID && string.Equals(m.Address, image.Address, StringComparison.OrdinalIgnoreCase)));

                Database.DeleteImageUrl(image.ID);
            }

            Database.DeleteWtaTripReport(tripReport);
            Database.DeleteUrl(url);
        }

        #region Private utility methods

        private ImageUrl GetRandomImageUrl()
        {
            return this.GetRandomImageUrl(0);
        }

        private ImageUrl GetRandomImageUrl(int id)
        {
            Random random = new();

            return new()
            {
                ID = id,
                Address = $"https://test/{Guid.NewGuid()}",
                ImageWidth = random.Next(0, 1) == 0 ? null : random.Next(0, 6000),
                ImageHeight = random.Next(0, 1) == 0 ? null : random.Next(0, 6000),
                FileSize = random.Next(0, 100000000),
                Comments = Guid.NewGuid().ToString(),
            };
        }

        private void AssertSameImageUrl(ImageUrl image1, ImageUrl image2)
        {
            Assert.AreEqual(image1.ID, image2.ID, "ID doesn't match");
            Assert.AreEqual(image1.Address, image2.Address, "Address doesn't match");
            Assert.AreEqual(image1.ImageWidth, image2.ImageWidth, "ImageWidth doesn't match");
            Assert.AreEqual(image1.ImageHeight, image2.ImageHeight, "ImageHeight doesn't match");
            Assert.AreEqual(image1.FileSize, image2.FileSize, "FileSize doesn't match");
            Assert.AreEqual(image1.Comments, image2.Comments, "Comments doesn't match");
        }

        #endregion
    }
}
