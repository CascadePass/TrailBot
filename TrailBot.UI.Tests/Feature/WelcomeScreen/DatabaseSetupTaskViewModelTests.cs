using CascadePass.TrailBot.UI.Feature.WelcomeScreen;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CascadePass.TrailBot.UI.Tests.Feature.WelcomeScreen
{
    [TestClass]
    public class DatabaseSetupTaskViewModelTests
    {
        [TestMethod]
        public void CanCreateInstance()
        {
            _ = new DatabaseSetupTaskViewModel();
        }

        #region Get/set access same value

        [TestMethod]
        public void Settings_GetSetAccessSameValue()
        {
            var expectedValue = new Settings();
            DatabaseSetupTaskViewModel testObject = new()
            {
                Settings = expectedValue
            };

            Assert.AreEqual(expectedValue, testObject.Settings);
        }

        [TestMethod]
        public void DatabaseFilename_GetSetAccessSameValue()
        {
            var expectedValue = Guid.NewGuid().ToString();
            DatabaseSetupTaskViewModel testObject = new()
            {
                Settings = new(),
                DatabaseFilename = expectedValue
            };

            Assert.AreEqual(expectedValue, testObject.DatabaseFilename);
        }

        #endregion

        [TestMethod]
        public void SettingNullFilenameDoesNotThrow()
        {
            DatabaseSetupTaskViewModel viewModel = new() { Settings = new() };

            viewModel.DatabaseFilename = null;
        }

        [TestMethod]
        public void DatabaseFilename_IgnoresSameValue()
        {
            bool eventFired = false;
            string value = Guid.NewGuid().ToString();
            DatabaseSetupTaskViewModel viewModel = new() { Settings = new() };
            viewModel.PropertyChanged += (sender, args) => { eventFired = true; };

            viewModel.DatabaseFilename = value;
            Assert.IsTrue(eventFired);

            eventFired = false;

            viewModel.DatabaseFilename = value;
            Assert.IsFalse(eventFired);
        }

        [TestMethod]
        public void DatabaseFileExists_False()
        {
            var viewModel = new DatabaseSetupTaskViewModel
            {
                Settings = new(),
                DatabaseFilename = Guid.NewGuid().ToString()
            };

            Assert.IsFalse(viewModel.DatabaseFileExists);
        }

        [TestMethod]
        public void DatabaseFileExists_True()
        {
            var viewModel = new DatabaseSetupTaskViewModel
            {
                Settings = new(),
                DatabaseFilename = @"C:\Windows\cmd.exe"
            };

            Assert.IsFalse(viewModel.DatabaseFileExists);
        }

        [TestMethod]
        public void CanConnectToDatabase_Null()
        {
            string filename = Guid.NewGuid().ToString();

            File.WriteAllText(filename, filename);

            var viewModel = new DatabaseSetupTaskViewModel
            {
                Settings = new(),
                DatabaseFilename = filename
            };

            File.Delete(filename);

            Assert.IsFalse(viewModel.CanConnectToDatabase);
        }
    }
}
