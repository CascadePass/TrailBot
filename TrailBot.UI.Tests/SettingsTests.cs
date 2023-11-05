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
        public void SqliteDatabaseFilename_IgnoresSameValue()
        {
            Assert.Inconclusive();
            //string value = Guid.NewGuid().ToString();
            //Settings settings = new();
            //bool eventFired = false;

            //PropertyChangedEventHandler handler = Delegate() => { eventFired = true; };

            //settings.SqliteDatabaseFilename = value;

            //settings.PropertyChanged += handler;
            //settings.SqliteDatabaseFilename = value;

            //Assert.IsTrue(eventFired);
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
    }
}
