using CascadePass.TrailBot.UI.Feature.WelcomeScreen;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
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
    }
}
