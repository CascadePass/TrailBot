﻿using CascadePass.TrailBot.UI.Feature.Found;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;
using static System.Net.Mime.MediaTypeNames;

namespace CascadePass.TrailBot.UI.Tests
{
    [TestClass]
    public class MatchedTripReportViewModelTests
    {
        [TestMethod]
        public void CanCreateInstance()
        {
            MatchedTripReportViewModel vm = new();
        }

        #region HasSelectedText

        [TestMethod]
        public void HasSelectedText_False()
        {
            MatchedTripReportViewModel vm = new() { SelectedPreviewText = null };
            Assert.IsFalse(vm.HasSelectedText);
        }

        [TestMethod]
        public void HasSelectedText_True()
        {
            MatchedTripReportViewModel vm = new() { SelectedPreviewText = Guid.NewGuid().ToString() };
            Assert.IsTrue(vm.HasSelectedText);
        }

        #endregion

        #region FontWeight

        [TestMethod]
        public void FontWeight_HasBeenSeen()
        {
            MatchedTripReportViewModel vm = new() { MatchedTripReport = new(), HasBeenSeen = true };
            Assert.AreEqual(vm.FontWeight, FontWeights.Normal);
        }

        [TestMethod]
        public void FontWeight_HasNotBeenSeen()
        {
            MatchedTripReportViewModel vm = new() { MatchedTripReport = new(), HasBeenSeen = false };
            Assert.AreEqual(vm.FontWeight, FontWeights.DemiBold);
        }

        #endregion

        #region PreviewDocument

        [TestMethod]
        public void PreviewDocument_NoTripReport()
        {
            MatchedTripReportViewModel vm = new() { Report = null };
            Assert.IsNull(vm.PreviewDocument);
        }

        [TestMethod]
        public void PreviewDocument_TripReport_EmptyString()
        {
            MatchedTripReportViewModel vm = new() { Report = new() { ReportText = string.Empty } };
            Assert.IsNotNull(vm.PreviewDocument);
        }

        [TestMethod]
        public void PreviewDocument_TripReport_Guid()
        {
            string text = Guid.NewGuid().ToString();

            MatchedTripReportViewModel vm = new() {
                AllTopics = new(),
                Report = new() {
                    ReportText = text,
                }
            };

            Assert.IsNotNull(vm.PreviewDocument);
            Assert.IsTrue(vm.PreviewDocument.Blocks.FirstBlock is Paragraph);

            // Gibberish input should not crash the UI

            var paragraph = (Paragraph)vm.PreviewDocument.Blocks.LastBlock;

            Assert.AreEqual(1, paragraph.Inlines.Count);
            Assert.IsTrue(paragraph.Inlines.FirstInline is Run);
            Assert.AreEqual(text, ((Run)paragraph.Inlines.FirstInline).Text.Trim());
        }

        [TestMethod]
        public void PreviewDocument_IsCached()
        {
            MatchedTripReportViewModel vm = new()
            {
                AllTopics = new(),
                Report = new()
                {
                    ReportText = "broke my passenger side window"
                }
            };

            object originalValue = vm.PreviewDocument;
            Assert.AreSame(originalValue, vm.PreviewDocument);
        }

        [TestMethod]
        public void PreviewDocument_TripReport_PhraseNoPunctuation()
        {
            MatchedTripReportViewModel vm = new() {
                AllTopics = new(),
                Report = new() {
                    ReportText = "broke my passenger side window"
                }
            };

            Assert.IsNotNull(vm.PreviewDocument);
            Assert.IsTrue(vm.PreviewDocument.Blocks.FirstBlock is Paragraph);
            Assert.IsTrue(vm.PreviewDocument.Blocks.LastBlock is Paragraph);

            var paragraph = (Paragraph)vm.PreviewDocument.Blocks.LastBlock;
            Assert.AreEqual(1, paragraph.Inlines.Count);
            Assert.IsTrue(paragraph.Inlines.FirstInline is Run);
            Assert.AreEqual(vm.Report.ReportText, ((Run)paragraph.Inlines.FirstInline).Text.Trim());
        }

        [TestMethod]
        public void PreviewDocument_TripReport_OneSentence()
        {
            MatchedTripReportViewModel vm = new()
            {
                AllTopics = new(),
                Report = new()
                {
                    ReportText = "Somebody broke my passenger side window while I was hiking."
                }
            };

            Assert.IsNotNull(vm.PreviewDocument);
            Assert.IsTrue(vm.PreviewDocument.Blocks.FirstBlock is Paragraph);
            Assert.IsTrue(vm.PreviewDocument.Blocks.LastBlock is Paragraph);

            var paragraph = (Paragraph)vm.PreviewDocument.Blocks.LastBlock;
            Assert.IsTrue(paragraph.Inlines.FirstInline is Run);
            Assert.AreEqual(vm.Report.ReportText, ((Run)paragraph.Inlines.FirstInline).Text.Trim());
        }

        [TestMethod]
        public void PreviewDocument_TripReport_TwoSentences()
        {
            MatchedTripReportViewModel vm = new()
            {
                AllTopics = new(),
                Report = new()
                {
                    ReportText = "Somebody broke my passenger side window while I was hiking. I should have looked out for prowlers."
                }
            };

            Assert.IsNotNull(vm.PreviewDocument);
            Assert.IsTrue(vm.PreviewDocument.Blocks.FirstBlock is Paragraph);
            Assert.IsTrue(vm.PreviewDocument.Blocks.LastBlock is Paragraph);

            var paragraph = (Paragraph)vm.PreviewDocument.Blocks.LastBlock;
            Assert.IsTrue(paragraph.Inlines.FirstInline is Run);

            string fullText =
                ((Run)paragraph.Inlines.FirstInline).Text.Trim() + " " +
                ((Run)paragraph.Inlines.FirstInline.NextInline).Text.Trim();
            
            Assert.AreEqual(vm.Report.ReportText, fullText);
        }

        [TestMethod]
        public void PreviewDocument_TripReport_TwoParagraphs()
        {
            MatchedTripReportViewModel vm = new()
            {
                AllTopics = new(),
                Report = new()
                {
                    ReportText = "Somebody broke my passenger side window while I was hiking. I should have looked out for prowlers.\r\n\r\nIf you hike here, don't leave anything in your car."
                }
            };

            Assert.IsNotNull(vm.PreviewDocument);
            Assert.IsTrue(vm.PreviewDocument.Blocks.FirstBlock is Paragraph);
            Assert.IsTrue(vm.PreviewDocument.Blocks.LastBlock is Paragraph);

            var paragraph = (Paragraph)vm.PreviewDocument.Blocks.FirstBlock.NextBlock;
            Assert.IsTrue(paragraph.Inlines.FirstInline is Run);

            string fullText =
                ((Run)paragraph.Inlines.FirstInline).Text.Trim() + " " +
                ((Run)paragraph.Inlines.FirstInline.NextInline).Text.Trim();

            Assert.AreEqual("Somebody broke my passenger side window while I was hiking. I should have looked out for prowlers.", fullText);

            paragraph = (Paragraph)vm.PreviewDocument.Blocks.LastBlock;
            fullText = ((Run)paragraph.Inlines.FirstInline).Text.Trim();

            Assert.AreEqual("If you hike here, don't leave anything in your car.", fullText);
        }

        [TestMethod]
        public void PreviewDocument_TripReport_TwoParagraphs_MatchingTopics()
        {
            MatchedTripReportViewModel vm = new()
            {
                AllTopics = new(),
                Report = new()
                {
                    ReportText = "Somebody broke my passenger side window while I was hiking. I should have looked out for prowlers.\r\n\r\nIf you hike here, don't leave anything in your car."
                }
            };

            vm.AllTopics.Add(new() { Name = "Crime", MatchAny = "broke\r\nwindow" });

            Assert.IsNotNull(vm.PreviewDocument);
        }

        [TestMethod]
        public void PreviewDocument_TripReport_TwoParagraphs_NullTopics()
        {
            MatchedTripReportViewModel vm = new()
            {
                AllTopics = null,
                Report = new()
                {
                    ReportText = "Somebody broke my passenger side window while I was hiking.  I should have looked out for prowlers.\r\n\r\nIf you hike here, don't leave anything in your car."
                }
            };

            Assert.IsNotNull(vm.PreviewDocument);
        }

        [TestMethod]
        public void PreviewDocument_TripReport_TwoParagraphs_MatchingTopicWithApostrophe()
        {
            MatchedTripReportViewModel vm = new()
            {
                AllTopics = new(),
                Report = new()
                {
                    ReportText = "Somebody broke my passenger side window while I was hiking.  I should have looked out for prowlers.\r\n\r\nIf you hike here, don't leave anything in your car."
                }
            };

            vm.AllTopics.Add(new());

            vm.AllTopics[0].Name = "Test Topic";
            vm.AllTopics[0].MatchAny = "don't leave";

            Assert.IsNotNull(vm.PreviewDocument);
        }

        [TestMethod]
        public void PreviewDocument_TripReport_Title()
        {
            MatchedTripReportViewModel vm = new()
            {
                AllTopics = new(),
                Report = new()
                {
                    Title = "Test",
                    TripDate = new DateTime(1977, 12, 21),
                    ReportText = "Somebody broke my passenger side window while I was hiking. I should have looked out for prowlers.",
                }
            };

            Assert.IsNotNull(vm.PreviewDocument);
            Assert.IsTrue(vm.PreviewDocument.Blocks.FirstBlock is Paragraph);

            var paragraph = (Paragraph)vm.PreviewDocument.Blocks.FirstBlock;
            Assert.IsTrue(paragraph.Inlines.FirstInline is Hyperlink);

            var titleHyperlink = (Hyperlink)paragraph.Inlines.FirstInline;
            var titleRun = (Run)titleHyperlink.Inlines.FirstInline;
            string expectedTitleText = $"{vm.Report.Title} ({vm.Report.TripDate.ToShortDateString()})";

            Assert.AreEqual(expectedTitleText, titleRun.Text.Trim());
        }

        #endregion

        #region Commands

        [TestMethod]
        public void ViewInBrowserCommand_NotNull()
        {
            MatchedTripReportViewModel vm = new();
            Assert.IsNotNull(vm.ViewInBrowserCommand);
        }

        [TestMethod]
        public void AddTextToTopicCommand_NotNull()
        {
            MatchedTripReportViewModel vm = new();
            Assert.IsNotNull(vm.AddTextToTopicCommand);
        }

        [TestMethod]
        public void AddExceptionTextToTopicCommand_NotNull()
        {
            MatchedTripReportViewModel vm = new();
            Assert.IsNotNull(vm.AddExceptionTextToTopicCommand);
        }

        [TestMethod]
        public void CreateTopicCommand_NotNull()
        {
            MatchedTripReportViewModel vm = new();
            Assert.IsNotNull(vm.CreateTopicCommand);
        }

        [TestMethod]
        public void CopySelectedTextCommand_NotNull()
        {
            MatchedTripReportViewModel vm = new();
            Assert.IsNotNull(vm.CopySelectedTextCommand);
        }

        [TestMethod]
        public void AllCommandsAreUnique()
        {
            MatchedTripReportViewModel vm = new();
            HashSet<ICommand> commands = new();

            commands.Add(vm.ViewInBrowserCommand);
            commands.Add(vm.AddTextToTopicCommand);
            commands.Add(vm.AddExceptionTextToTopicCommand);
            commands.Add(vm.CreateTopicCommand);
            commands.Add(vm.CopySelectedTextCommand);

            // The purpose of this test is to catch copy/paste errors in property definitions
        }
        
        #endregion
    }
}