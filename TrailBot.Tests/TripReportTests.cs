using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace CascadePass.TrailBot.Tests
{
    [TestClass]
    public class TripReportTests
    {
        [TestMethod]
        public void CanCreateTripReport()
        {
            TripReport tripReport = new();
        }

        [TestMethod]
        public void GetSearchableReportTextDoesntThrowWithNoText()
        {
            TripReport tripReport = new();

            tripReport.GetSearchableReportText();
        }

        [TestMethod]
        public void EmptyTripReportHasNoSearchableText()
        {
            TripReport tripReport = new();

            string searchText = tripReport.GetSearchableReportText();

            Console.WriteLine("tripReport.GetSearchableReportText()");
            Console.WriteLine(searchText);
            Console.WriteLine($"({searchText.Length} chars)");

            Assert.IsTrue(string.IsNullOrWhiteSpace(searchText));
        }

        [TestMethod]
        public void TitleIsContainedInSearchableText()
        {
            string title = Guid.NewGuid().ToString();
            TripReport tripReport = new() { Title = title };

            string searchText = tripReport.GetSearchableReportText();
            Assert.IsTrue(searchText.Contains(title));
        }

        [TestMethod]
        public void BodyIsContainedInSearchableText()
        {
            string body = Guid.NewGuid().ToString();
            TripReport tripReport = new() { ReportText = body };

            string searchText = tripReport.GetSearchableReportText();
            Assert.IsTrue(searchText.Contains(body));
        }

        [TestMethod]
        public void BodyAndTitleBothContainedInSearchableText()
        {
            string body = Guid.NewGuid().ToString(), title = Guid.NewGuid().ToString();
            TripReport tripReport = new() { Title = title, ReportText = body };

            string searchText = tripReport.GetSearchableReportText();

            Assert.IsTrue(searchText.Contains(title));
            Assert.IsTrue(searchText.Contains(body));
        }
    }
}
