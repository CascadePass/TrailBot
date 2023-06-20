using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace CascadePass.TrailBot.Tests
{
    [TestClass]
    public class PageRequestEventArgsTests
    {
        [TestMethod]
        public void CanCreateInstance()
        {
            _ = new PageRequestEventArgs();
        }

        [TestMethod]
        public void MatchedTripReport_GetSetAccessSameValue()
        {
            PageRequestEventArgs e = new();
            Uri uri = new("https://www.google.com/");

            e.Uri = uri;
            Assert.AreSame(uri, e.Uri);
        }
    }
}
