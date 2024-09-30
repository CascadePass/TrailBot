using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.ComponentModel;

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
        public void XmlFolder_GetSetAccessSameValue()
        {
            string value = Guid.NewGuid().ToString();
            Settings settings = new();

            settings.XmlFolder = value;
            Assert.IsTrue(string.Equals(settings.XmlFolder, value, StringComparison.Ordinal));
        }

        [TestMethod]
        public void ShowPreviewPane_GetSetAccessSameValue()
        {
            Settings settings = new();

            settings.ShowPreviewPane = true;
            Assert.AreEqual(true, settings.ShowPreviewPane);

            settings.ShowPreviewPane = false;
            Assert.AreEqual(false, settings.ShowPreviewPane);
        }

        [TestMethod]
        public void SqliteDatabaseFilename_GetSetAccessSameValue()
        {
            string value = Guid.NewGuid().ToString();
            Settings settings = new();

            settings.SqliteDatabaseFilename = value;
            Assert.IsTrue(string.Equals(settings.SqliteDatabaseFilename, value, StringComparison.Ordinal));
        }

        [TestMethod]
        public void IndexFilename_GetSetAccessSameValue()
        {
            string value = Guid.NewGuid().ToString();
            Settings settings = new();

            settings.IndexFilename = value;
            Assert.IsTrue(string.Equals(settings.IndexFilename, value, StringComparison.Ordinal));
        }

        [TestMethod]
        public void DebugMode_GetSetAccessSameValue()
        {
            Settings settings = new();

            bool value = !settings.DebugMode;
            settings.DebugMode = value;
            Assert.AreEqual(settings.DebugMode, value);
        }

        [TestMethod]
        public void SuggestAdditionalTerms_GetSetAccessSameValue()
        {
            Settings settings = new();

            bool value = !settings.SuggestAdditionalTerms;
            settings.SuggestAdditionalTerms = value;
            Assert.AreEqual(settings.SuggestAdditionalTerms, value);
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
        public void SqliteDatabaseFilename_Makes_IsDirty_True()
        {
            string value = Guid.NewGuid().ToString();
            Settings settings = new();

            settings.SqliteDatabaseFilename = value;
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

        [TestMethod]
        public void DebugMode_Makes_IsDirty_True()
        {
            Settings settings = new();

            settings.DebugMode = !settings.DebugMode;
            Assert.IsTrue(settings.IsDirty);
        }

        [TestMethod]
        public void SuggestAdditionalTerms_Makes_IsDirty_True()
        {
            Settings settings = new();

            settings.SuggestAdditionalTerms = !settings.SuggestAdditionalTerms;
            Assert.IsTrue(settings.IsDirty);
        }

        #endregion


        [TestMethod]
        public void SqliteDatabaseFilename_IgnoresSameValue()
        {
            bool eventFired = false;
            string value = Guid.NewGuid().ToString();
            Settings settings = new();
            settings.PropertyChanged += (sender, args) => { eventFired = true; };

            settings.SqliteDatabaseFilename = value;
            Assert.IsTrue(eventFired);

            eventFired = false;

            settings.SqliteDatabaseFilename = value;
            Assert.IsFalse(eventFired);
        }

        #region Correct property name

        [TestMethod]
        public void SqliteDatabaseFilename_CorrectPropertyName()
        {
            string propertyName = null;
            Settings settings = new();
            settings.PropertyChanged += (sender, args) => { propertyName = args.PropertyName; };

            settings.SqliteDatabaseFilename = Guid.NewGuid().ToString();
            Assert.IsTrue(string.Equals(nameof(Settings.SqliteDatabaseFilename), propertyName));
        }

        [TestMethod]
        public void ShowPreviewPane_CorrectPropertyName()
        {
            string propertyName = null;
            Settings settings = new();
            settings.PropertyChanged += (sender, args) => { propertyName = args.PropertyName; };

            settings.ShowPreviewPane = !settings.ShowPreviewPane;
            Assert.IsTrue(string.Equals(nameof(Settings.ShowPreviewPane), propertyName));
        }

        [TestMethod]
        public void SuggestAdditionalTerms_CorrectPropertyName()
        {
            string propertyName = null;
            Settings settings = new();
            settings.PropertyChanged += (sender, args) => { propertyName = args.PropertyName; };

            settings.SuggestAdditionalTerms = !settings.SuggestAdditionalTerms;
            Assert.IsTrue(string.Equals(nameof(Settings.SuggestAdditionalTerms), propertyName));
        }

        [TestMethod]
        public void DebugMode_CorrectPropertyName()
        {
            string propertyName = null;
            Settings settings = new();
            settings.PropertyChanged += (sender, args) => { propertyName = args.PropertyName; };

            settings.DebugMode = !settings.DebugMode;
            Assert.IsTrue(string.Equals(nameof(Settings.DebugMode), propertyName));
        }

        #endregion
    }
}
