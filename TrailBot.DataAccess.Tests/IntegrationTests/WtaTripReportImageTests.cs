using CascadePass.TrailBot.DataAccess;
using CascadePass.TrailBot.DataAccess.DTO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace TrailBot.DataAccess.Tests.IntegrationTests
{
    [TestClass]
    public class WtaTripReportImageTests : SqliteIntegrationTestClass
    {
        [TestMethod]
        public void AddWtaTripReportImage_ByDTO()
        {
            this.AssertRequirements();

            var imageAssociation = this.GetRandomWtaTripReportImage();

            Database.Add(imageAssociation);

            // Was it actually saved?
            var validate = Database.GetWtaTripReportImage(imageAssociation.ID);
            Assert.IsNotNull(validate);

            this.AssertSameWtaTripReportImage(imageAssociation, validate);

            // Cleanup
            Database.DeleteWtaTripReportImage(imageAssociation.ID);
        }

        [TestMethod]
        public void AddWtaTripReportImage_ByIDs()
        {
            this.AssertRequirements();

            var imageAssociation = this.GetRandomWtaTripReportImage();

            imageAssociation.ID = Database.AddWtaTripReportImage(imageAssociation.WtaTripReportID, imageAssociation.ImageID);

            // Was it actually saved?
            var validate = Database.GetWtaTripReportImage(imageAssociation.ID);
            Assert.IsNotNull(validate);

            this.AssertSameWtaTripReportImage(imageAssociation, validate);

            // Cleanup
            Database.DeleteWtaTripReportImage(imageAssociation.ID);
        }

        [TestMethod]
        public void DeleteWtaTripReportImage_ByDTO()
        {
            this.AssertRequirements();

            var imageAssociation = this.GetRandomWtaTripReportImage();

            Database.Add(imageAssociation);

            // Was it actually saved?
            var validate = Database.GetWtaTripReportImage(imageAssociation.ID);
            Assert.IsNotNull(validate);

            this.AssertSameWtaTripReportImage(imageAssociation, validate);

            // Cleanup
            Database.Delete(imageAssociation);
        }

        [TestMethod]
        public void DeleteWtaTripReportImage_ByID()
        {
            this.AssertRequirements();

            var imageAssociation = this.GetRandomWtaTripReportImage();

            Database.Add(imageAssociation);

            // Was it actually saved?
            var validate = Database.GetWtaTripReportImage(imageAssociation.ID);
            Assert.IsNotNull(validate);

            this.AssertSameWtaTripReportImage(imageAssociation, validate);

            // Cleanup
            Database.DeleteWtaTripReportImage(imageAssociation.ID);
        }

        [TestMethod]
        public void GetWtaTripReportImage_NotThere()
        {
            var nonExistentImage = Database.GetWtaTripReportImage(int.MaxValue);
            Assert.IsNull(nonExistentImage);
        }

        #region Private utility methods

        private WtaTripReportImage GetRandomWtaTripReportImage()
        {
            return this.GetRandomWtaTripReportImage(0);
        }

        private WtaTripReportImage GetRandomWtaTripReportImage(int id)
        {
            Random random = new();

            return new()
            {
                ID = id,
                ImageID = random.Next(0, 100000000),
                WtaTripReportID = random.Next(0, 100000000),
            };
        }

        private void AssertSameWtaTripReportImage(WtaTripReportImage image1, WtaTripReportImage image2)
        {
            Assert.AreEqual(image1.ID, image2.ID, "ID doesn't match");
            Assert.AreEqual(image1.ImageID, image2.ImageID, "ImageID doesn't match");
            Assert.AreEqual(image1.WtaTripReportID, image2.WtaTripReportID, "WtaTripReportID doesn't match");
        }

        #endregion

    }
}
