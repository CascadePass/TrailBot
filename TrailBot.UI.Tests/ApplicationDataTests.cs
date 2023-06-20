using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CascadePass.TrailBot.UI.Tests
{
    [TestClass]
    public class ApplicationDataTests
    {
        #region Load

        #region null

        [TestMethod]
        public void Load_null_NoException()
        {
            ApplicationData.Load(null);
        }

        [TestMethod]
        public void Load_null_ReturnsFalse()
        {
            bool result = ApplicationData.Load(null);

            Console.WriteLine($"ApplicationData.Load(null) == {result}");
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void Load_null_WasSettingsMissing()
        {
            ApplicationData.Load(null);
            bool result = ApplicationData.WasSettingsMissing;

            Console.WriteLine($"ApplicationData.Load(null) => WasSettingsMissing == {result}");
            Assert.IsTrue(result);
        }

        #endregion

        #region Illegal Filename

        [TestMethod]
        public void Load_IllegalFilename_NoException()
        {
            ApplicationData.Load(@"xyz:\Program Files\Group\Product\File'name.txt");
        }

        [TestMethod]
        public void Load_IllegalFilename_ReturnsFalse()
        {
            string filename = @"xyz:\Program Files\Group\Product\File'name.txt";
            bool result = ApplicationData.Load(filename);

            Console.WriteLine($"ApplicationData.Load({filename}) == {result}");
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void Load_IllegalFilename_WasSettingsMissing()
        {
            string filename = @"xyz:\Program Files\Group\Product\File'name.txt";
            ApplicationData.Load(filename);
            bool result = ApplicationData.WasSettingsMissing;

            Console.WriteLine($"ApplicationData.Load(null) => WasSettingsMissing == {result}");
            Assert.IsTrue(result);
        }

        #endregion

        #region Non-Existent Filename

        [TestMethod]
        public void Load_NonExistentFilename_NoException()
        {
            string filename = Path.Combine(Environment.CurrentDirectory, Path.GetRandomFileName());
            ApplicationData.Load(filename);
        }

        [TestMethod]
        public void Load_NonExistentFilename_ReturnsFalse()
        {
            string filename = Path.Combine(Environment.CurrentDirectory, Path.GetRandomFileName());
            bool result = ApplicationData.Load(filename);

            Console.WriteLine($"ApplicationData.Load({filename}) == {result}");
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void Load_NonExistentFilename_WasSettingsMissing()
        {
            string filename = Path.Combine(Environment.CurrentDirectory, Path.GetRandomFileName());
            ApplicationData.Load(filename);
            bool result = ApplicationData.WasSettingsMissing;

            Console.WriteLine($"ApplicationData.Load(null) => WasSettingsMissing == {result}");
            Assert.IsTrue(result);
        }

        #endregion

        #endregion

        #region Property Correctness (get/set access same backing object)

        [TestMethod]
        public void Settings_GetSetAccessSameValue()
        {
            Settings settingsInUse = ApplicationData.Settings;
            Settings testSettings = new();

            ApplicationData.Settings = testSettings;

            bool pass = ApplicationData.Settings == testSettings;

            ApplicationData.Settings = settingsInUse;

            Assert.IsTrue(pass);
        }

        [TestMethod]
        public void WebProviderManager_GetSetAccessSameValue()
        {
            WebProviderManager managerInUse = ApplicationData.WebProviderManager;
            WebProviderManager testManager = new();

            ApplicationData.WebProviderManager = testManager;

            bool pass = ApplicationData.WebProviderManager == testManager;

            ApplicationData.WebProviderManager = managerInUse;

            Assert.IsTrue(pass);
        }

        #endregion
    }
}
