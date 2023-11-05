using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;

namespace CascadePass.TrailBot.UI.Tests
{
    [TestClass]
    public class AppTests
    {
        private static App app;

        protected App TestAppInstance => AppTests.app ??= new();

        [TestMethod]
        public void CanCreateInstance()
        {
            _ = this.TestAppInstance;
        }

        [TestMethod]
        public void RequireSetupScreen_False()
        {
            ApplicationData.Settings = new();
            ApplicationData.WasSettingsMissing = false;
            ApplicationData.Settings.SqliteDatabaseFilename = Directory.GetFiles(Environment.CurrentDirectory)[0];

            Assert.IsFalse(App.RequireSetupScreen);
        }

        [TestMethod]
        public void RequireSetupScreen_True_NoDatabaseFilename()
        {
            ApplicationData.Settings = new();
            ApplicationData.WasSettingsMissing = false;
            ApplicationData.Settings.SqliteDatabaseFilename = null;

            Assert.IsTrue(App.RequireSetupScreen);
        }

        [TestMethod]
        public void RequireSetupScreen_True_SettingsWereMissing()
        {
            ApplicationData.Settings = new();
            ApplicationData.WasSettingsMissing = true;
            ApplicationData.Settings.SqliteDatabaseFilename = null;

            Assert.IsTrue(App.RequireSetupScreen);
        }

        [TestMethod]
        public void RequireSetupScreen_NullSettings()
        {
            ApplicationData.Settings = null;

            Assert.IsTrue(App.RequireSetupScreen);
        }

        [TestMethod]
        public void GetSettings()
        {
            App.GetSettings();
        }
    }
}
