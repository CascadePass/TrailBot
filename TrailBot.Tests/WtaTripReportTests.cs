using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace CascadePass.TrailBot.Tests
{
    [TestClass]
    public class WtaTripReportTests
    {
        [TestMethod]
        public void CanCreateTripReport()
        {
            WtaTripReport tripReport = new();
        }

        [TestMethod]
        public void SourceIsWTA()
        {
            WtaTripReport tripReport = new();

            Assert.AreEqual(SupportedTripReportSource.WashingtonTrailsAssociation, tripReport.Source);
        }

        #region Properties not null by default

        [TestMethod]
        public void TrailConditionsDefaultsToEmptyList()
        {
            WtaTripReport tripReport = new();

            Assert.IsNotNull(tripReport.TrailConditions);
            Assert.AreEqual(0, tripReport.TrailConditions.Count);
        }

        [TestMethod]
        public void TrailsDefaultsToEmptyList()
        {
            WtaTripReport tripReport = new();

            Assert.IsNotNull(tripReport.Trails);
            Assert.AreEqual(0, tripReport.Trails.Count);
        }

        [TestMethod]
        public void FeatureDefaultsToEmptyList()
        {
            WtaTripReport tripReport = new();

            Assert.IsNotNull(tripReport.Feature);
            Assert.AreEqual(0, tripReport.Feature.Count);
        }

        #endregion

        #region GetSearchableReportText (content)

        [TestMethod]
        public void GetSearchableReportTextDoesntThrowWithNoText()
        {
            WtaTripReport tripReport = new();

            tripReport.GetSearchableReportText();
        }

        [TestMethod]
        public void EmptyTripReportHasNoSearchableText()
        {
            WtaTripReport tripReport = new();

            string searchText = tripReport.GetSearchableReportText();

            Console.WriteLine("tripReport.GetSearchableReportText()");
            Console.WriteLine(searchText);
            Console.WriteLine($"({searchText.Length} chars)");

            Assert.IsTrue(string.IsNullOrWhiteSpace(searchText));
        }

        [TestMethod]
        public void TitleNotContainedInSearchableText()
        {
            string title = Guid.NewGuid().ToString();
            WtaTripReport tripReport = new() { Title = title };

            string searchText = tripReport.GetSearchableReportText();
            Assert.IsFalse(searchText.Contains(title));
        }

        [TestMethod]
        public void BodyIsContainedInSearchableText()
        {
            string body = Guid.NewGuid().ToString();
            WtaTripReport tripReport = new() { ReportText = body };

            string searchText = tripReport.GetSearchableReportText();
            Assert.IsTrue(searchText.Contains(body));
        }

        [TestMethod]
        public void BodyNotTitleContainedInSearchableText()
        {
            string body = Guid.NewGuid().ToString(), title = Guid.NewGuid().ToString();
            WtaTripReport tripReport = new() { Title = title, ReportText = body };

            string searchText = tripReport.GetSearchableReportText();

            Assert.IsFalse(searchText.Contains(title));
            Assert.IsTrue(searchText.Contains(body));
        }

        [TestMethod]
        public void FeatureContainedInSearchableText()
        {
            string feature1 = Guid.NewGuid().ToString(), feature2 = Guid.NewGuid().ToString();
            WtaTripReport tripReport = new();

            tripReport.Feature.Add(feature1);
            tripReport.Feature.Add(feature2);

            string searchText = tripReport.GetSearchableReportText();

            Assert.IsTrue(searchText.Contains(feature1));
            Assert.IsTrue(searchText.Contains(feature2));
        }

        [TestMethod]
        public void EmptyFeatureNotContainedInSearchableText()
        {
            WtaTripReport tripReport = new();

            for (int i = 0; i < 100; i++)
            {
                tripReport.Feature.Add(string.Empty);
            }

            string searchText = tripReport.GetSearchableReportText();

            Console.WriteLine(searchText);
            Assert.IsFalse(searchText.Split('\n').Length >= 100);
        }

        [TestMethod]
        public void TrailConditionsContainedInSearchableText()
        {
            string feature1 = Guid.NewGuid().ToString(), feature2 = Guid.NewGuid().ToString();

            WtaTripReport tripReport = new();

            tripReport.TrailConditions.Add(new WtaTrailCondition() { Title = "1", Description = feature1 });
            tripReport.TrailConditions.Add(new WtaTrailCondition() { Title = "2", Description = feature2 });

            string searchText = tripReport.GetSearchableReportText();

            Assert.IsTrue(searchText.Contains(feature1));
            Assert.IsTrue(searchText.Contains(feature2));
        }

        [TestMethod]
        public void EmptyTrailConditionsNotContainedInSearchableText()
        {
            WtaTripReport tripReport = new();

            for (int i = 0; i < 100; i++)
            {
                tripReport.TrailConditions.Add(new WtaTrailCondition());
            }


            string searchText = tripReport.GetSearchableReportText();

            Console.WriteLine(searchText);
            Assert.IsFalse(searchText.Split('\n').Length >= 100);
        }

        [TestMethod]
        public void TrailConditionNamesNotContainedInSearchableText()
        {
            string label1 = Guid.NewGuid().ToString(), label2 = Guid.NewGuid().ToString();
            string feature1 = Guid.NewGuid().ToString(), feature2 = Guid.NewGuid().ToString();

            WtaTripReport tripReport = new();

            tripReport.TrailConditions.Add(new WtaTrailCondition() { Title = label1, Description = feature1 });
            tripReport.TrailConditions.Add(new WtaTrailCondition() { Title = label2, Description = feature2 });

            string searchText = tripReport.GetSearchableReportText();

            Assert.IsFalse(searchText.Contains(label1));
            Assert.IsFalse(searchText.Contains(label2));

            Assert.IsTrue(searchText.Contains(feature1));
            Assert.IsTrue(searchText.Contains(feature2));
        }

        #endregion
    }
}
