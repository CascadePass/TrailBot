using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace CascadePass.TrailBot.UI.Tests
{
    [TestClass]
    public class SettingsTests
    {
        [TestMethod]
        public void CanCreateSettings()
        {
            Settings settings = new();

            // Expectation is that no exception is thrown
        }

        [TestMethod]
        public void NewSettingsIsEmpty()
        {
            Settings settings = new();

            Assert.IsTrue(string.IsNullOrEmpty(settings.XmlFolder));
        }

        [TestMethod]
        public void NewSettingsIsNotDirty()
        {
            Settings settings = new();

            Assert.IsFalse(settings.IsDirty);
        }

        #region get/set access same value

        [TestMethod]
        public void XmlFolder()
        {
            string value = Guid.NewGuid().ToString();
            Settings settings = new();

            settings.XmlFolder = value;
            Assert.IsTrue(string.Equals(settings.XmlFolder, value, StringComparison.Ordinal));
        }

        [TestMethod]
        public void ShowPreviewPane()
        {
            Settings settings = new();

            settings.ShowPreviewPane = true;
            Assert.AreEqual(true, settings.ShowPreviewPane);

            settings.ShowPreviewPane = false;
            Assert.AreEqual(false, settings.ShowPreviewPane);
        }

        [TestMethod]
        public void IndexFilename()
        {
            string value = Guid.NewGuid().ToString();
            Settings settings = new();

            settings.IndexFilename = value;
            Assert.IsTrue(string.Equals(settings.IndexFilename, value, StringComparison.Ordinal));
        }

        #endregion

        #region Properties turn IsDirty true

        [TestMethod]
        public void XmlFolder_Makes_IsDirty_True()
        {
            string value = Guid.NewGuid().ToString();
            Settings settings = new();

            settings.XmlFolder = value;
            Assert.IsTrue(settings.IsDirty);
        }

        [TestMethod]
        public void ShowPreviewPane_Makes_IsDirty_True()
        {
            Settings settings = new();

            settings.ShowPreviewPane = !settings.ShowPreviewPane;
            Assert.IsTrue(settings.IsDirty);
        }

        [TestMethod]
        public void IndexFilename_Makes_IsDirty_True()
        {
            string value = Guid.NewGuid().ToString();
            Settings settings = new();

            settings.IndexFilename = value;
            Assert.IsTrue(settings.IsDirty);
        }

        #endregion
    }
}
