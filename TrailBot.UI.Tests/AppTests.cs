using Microsoft.VisualStudio.TestTools.UnitTesting;

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
    }
}
