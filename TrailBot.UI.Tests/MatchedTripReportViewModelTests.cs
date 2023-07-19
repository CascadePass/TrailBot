using CascadePass.TrailBot.UI.Feature.Found;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;

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
            MatchedTripReportViewModel vm = new() {
                AllTopics = new(),
                Report = new() {
                    ReportText = Guid.NewGuid().ToString()
                }
            };

            Assert.IsNotNull(vm.PreviewDocument);

            // Gibberish input should not crash the UI
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
        }

        [TestMethod]
        public void PreviewDocument_TripReport_TwoSentences()
        {
            MatchedTripReportViewModel vm = new()
            {
                AllTopics = new(),
                Report = new()
                {
                    ReportText = "Somebody broke my passenger side window while I was hiking.  I should have looked out for prowlers."
                }
            };

            Assert.IsNotNull(vm.PreviewDocument);
        }

        [TestMethod]
        public void PreviewDocument_TripReport_TwoParagraphs()
        {
            MatchedTripReportViewModel vm = new()
            {
                AllTopics = new(),
                Report = new()
                {
                    ReportText = "Somebody broke my passenger side window while I was hiking.  I should have looked out for prowlers.\r\n\r\nIf you hike here, don't leave anything in your car."
                }
            };

            Assert.IsNotNull(vm.PreviewDocument);
        }

        [TestMethod]
        public void PreviewDocument_TripReport_TwoParagraphs_MatchingTopics()
        {
            MatchedTripReportViewModel vm = new()
            {
                AllTopics = new(),
                Report = new()
                {
                    ReportText = "Somebody broke my passenger side window while I was hiking.  I should have looked out for prowlers.\r\n\r\nIf you hike here, don't leave anything in your car."
                }
            };

            vm.AllTopics.Add(new() { Name = "Crime", MatchAny = "broke\r\nwindow" });

            Assert.IsNotNull(vm.PreviewDocument);
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
