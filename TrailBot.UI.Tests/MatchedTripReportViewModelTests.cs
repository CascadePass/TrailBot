using CascadePass.TrailBot.UI.Feature.Found;
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

        [TestMethod]
        public void IsMatchDetailPanelVisible_TrueByDefault()
        {
            MatchedTripReportViewModel vm = new();
            Assert.IsTrue(vm.IsMatchDetailPanelVisible);
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

        #region get/set access same value

        [TestMethod]
        public void HasBeenSeen_GetSetAccessSameValue()
        {
            MatchedTripReportViewModel vm = new() { MatchedTripReport = new() };
            bool originalValue = vm.HasBeenSeen;

            vm.HasBeenSeen = !originalValue;
            
            Assert.AreEqual(!originalValue, vm.HasBeenSeen);
        }

        [TestMethod]
        public void IsMatchDetailPanelVisible_GetSetAccessSameValue()
        {
            MatchedTripReportViewModel vm = new();
            bool originalValue = vm.IsMatchDetailPanelVisible;

            vm.IsMatchDetailPanelVisible = !originalValue;

            Assert.AreEqual(!originalValue, vm.IsMatchDetailPanelVisible);
        }

        [TestMethod]
        public void IsMatchTermListVisible_GetSetAccessSameValue()
        {
            MatchedTripReportViewModel vm = new();
            bool originalValue = vm.IsMatchTermListVisible;

            vm.IsMatchTermListVisible = !originalValue;

            Assert.AreEqual(!originalValue, vm.IsMatchTermListVisible);
        }

        [TestMethod]
        public void TopicExerpts_GetSetAccessSameValue()
        {
            MatchedTripReportViewModel vm = new();
            string value = Guid.NewGuid().ToString();

            vm.TopicExerpts = value;

            Assert.AreEqual(value, vm.TopicExerpts);
        }

        [TestMethod]
        public void SelectedPreviewText_GetSetAccessSameValue()
        {
            MatchedTripReportViewModel vm = new();
            string value = Guid.NewGuid().ToString();

            vm.SelectedPreviewText = value;

            Assert.AreEqual(value, vm.SelectedPreviewText);
        }

        [TestMethod]
        public void Report_GetSetAccessSameValue()
        {
            MatchedTripReportViewModel vm = new();
            TripReport value = new();

            vm.Report = value;

            Assert.AreEqual(value, vm.Report);
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
        public void CloseMatchDetailCommand_NotNull()
        {
            MatchedTripReportViewModel vm = new();
            Assert.IsNotNull(vm.CloseMatchDetailCommand);
        }

        [TestMethod]
        public void ShowSearchTermsCommand_NotNull()
        {
            MatchedTripReportViewModel vm = new();
            Assert.IsNotNull(vm.ShowSearchTermsCommand);
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
            commands.Add(vm.CloseMatchDetailCommand);
            commands.Add(vm.ShowSearchTermsCommand);

            // The purpose of this test is to catch copy/paste errors in property definitions
        }

        #endregion

        #region Commands: Behavior

        #region ShowSearchTermsCommand

        [TestMethod]
        public void ShowSearchTermsCommand_CanShow()
        {
            MatchedTripReportViewModel vm = new();
            vm.MatchedTripReport = new() { BroaderContext = "crime\r\nkeyword" };
            vm.AllTopics = new() { new() { Name = "crime" } };

            Assert.IsFalse(vm.IsMatchTermListVisible);

            vm.ShowSearchTermsCommand.Execute("crime");
            Assert.IsTrue(vm.IsMatchTermListVisible);
            Assert.AreEqual("keyword", vm.TopicExerpts);
        }

        [TestMethod]
        public void ShowSearchTermsCommand_CanHide()
        {
            MatchedTripReportViewModel vm = new();
            vm.MatchedTripReport = new() { BroaderContext = "crime\r\nkeyword" };
            vm.AllTopics = new() { new() { Name = "crime" } };

            // This has to be called twice, because the view model uses
            // private variables to track the state.
            vm.ShowSearchTermsCommand.Execute("crime");
            vm.ShowSearchTermsCommand.Execute("crime");

            Assert.IsFalse(vm.IsMatchTermListVisible);
            Assert.AreEqual("keyword", vm.TopicExerpts);
        }

        [TestMethod]
        public void ShowSearchTermsCommand_Multiline()
        {
            MatchedTripReportViewModel vm = new();
            vm.MatchedTripReport = new() { BroaderContext = "crime\r\ncredit card\r\nbroken window\r\ncalled 911" };
            vm.AllTopics = new() { new() { Name = "crime" } };

            Assert.IsFalse(vm.IsMatchTermListVisible);

            vm.ShowSearchTermsCommand.Execute("crime");
            Assert.IsTrue(vm.IsMatchTermListVisible);
            Assert.AreEqual("credit card\r\nbroken window\r\ncalled 911", vm.TopicExerpts);
        }

        #endregion

        [TestMethod]
        public void CloseMatchDetailCommand()
        {
            MatchedTripReportViewModel vm = new();

            vm.IsMatchDetailPanelVisible = true;
            vm.CloseMatchDetailCommand.Execute(null);

            Assert.IsFalse(vm.IsMatchDetailPanelVisible);
        }

        [TestMethod]
        public void CopySelectedTextCommand()
        {
            MatchedTripReportViewModel vm = new();
            vm.SelectedPreviewText = Guid.NewGuid().ToString();

            vm.CopySelectedTextCommand.Execute(null);

            Assert.AreEqual(vm.SelectedPreviewText, Clipboard.GetText());
        }

        #endregion
    }
}
