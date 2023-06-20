using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace CascadePass.TrailBot.Tests
{
    [TestClass]
    public class FileStoreTests
    {
        [TestMethod]
        public void TestWriteAccess_Windows()
        {
            bool result = FileStore.TestWriteAccess(@"C:\Program Files");
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void TestWriteAccess_CurrentFolder()
        {
            Console.WriteLine(Environment.CurrentDirectory);
            bool result = FileStore.TestWriteAccess(Environment.CurrentDirectory);
            Assert.IsTrue(result);
        }
    }
}
