using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace CascadePass.TrailBot.Tests
{
    public class WebDataProviderConcreteTestImplementation : WebDataProvider
    {
        public override SupportedTripReportSource TripReportSource => throw new NotImplementedException();

        public override int MinimumAllowedSleep => 50;

        public override int MaximumRandomSleep => 100;

        public void CallSleep() => base.Sleep();

        public void CallOnTripReportCompleted(string url)
        {
            this.OnTripReportCompleted(this, new TripReportCompletedEventArgs() { TripReport = new() { Url = url } });
        }
    }

    [TestClass]
    public class WebDataProviderTests
    {
        [TestMethod]
        public void ConstructorDoesNotThrow()
        {
            WebDataProviderConcreteTestImplementation testObject = new();
        }

        #region Default Values

        [TestMethod]
        public void MinimumSleepHasDefaultValue()
        {
            WebDataProviderConcreteTestImplementation testObject = new();

            Assert.IsTrue(testObject.MinimumSleep > 0);
        }

        [TestMethod]
        public void MaximumSleepHasDefaultValue()
        {
            WebDataProviderConcreteTestImplementation testObject = new();

            Assert.IsTrue(testObject.MinimumSleep > 0);
        }

        [TestMethod]
        public void PendingUrlsNotNull()
        {
            WebDataProviderConcreteTestImplementation testObject = new();

            Assert.IsNotNull(testObject.PendingUrls);
        }

        [TestMethod]
        public void CompletedUrlsNotNull()
        {
            WebDataProviderConcreteTestImplementation testObject = new();

            Assert.IsNotNull(testObject.CompletedUrls);
        }

        [TestMethod]
        public void StatisticsNotNull()
        {
            WebDataProviderConcreteTestImplementation testObject = new();

            Assert.IsNotNull(testObject.Statistics);
        }

        #endregion

        #region Properties hold correct values

        [TestMethod]
        public void Browser_ReturnsSetValue()
        {
            WebDataProviderConcreteTestImplementation testObject = new();

            // If testObject.Browser's default value changes, this test problably
            // won't get updated, so find a browser that isn't currently assigned
            // (or the whole test is inconclusive).
            SupportedBrowser browser = testObject.Browser == SupportedBrowser.Safari ? SupportedBrowser.Firefox : SupportedBrowser.Safari;

            testObject.Browser = browser;
            Assert.AreEqual(browser, testObject.Browser);

            // The purpose of this test is to catch property implementation bugs
            // where the wrong field (class level variable) is returned.
            //
            // Browser is currently an auto-property but this is subject to change.
        }

        [TestMethod]
        public void PendingUrls_ReturnsSetValue()
        {
            WebDataProviderConcreteTestImplementation testObject = new();

            List<string> newValue = new();

            testObject.PendingUrls = newValue;
            Assert.AreEqual(newValue, testObject.PendingUrls);

            // The purpose of this test is to catch property implementation bugs
            // where the wrong field (class level variable) is returned.
            //
            // PendingUrls is currently an auto-property but this is subject to change.
            //
            // PendingUrls needs a public setter to be serializable.
        }

        [TestMethod]
        public void CompletedUrls_ReturnsSetValue()
        {
            WebDataProviderConcreteTestImplementation testObject = new();

            List<string> newValue = new();

            testObject.CompletedUrls = newValue;
            Assert.AreEqual(newValue, testObject.CompletedUrls);

            // The purpose of this test is to catch property implementation bugs
            // where the wrong field (class level variable) is returned.
            //
            // CompletedUrls is currently an auto-property but this is subject to change.
            //
            // CompletedUrls needs a public setter to be serializable.
        }

        [TestMethod]
        public void Statistics_ReturnsSetValue()
        {
            WebDataProviderConcreteTestImplementation testObject = new();

            ProviderStatistics newValue = new();

            testObject.Statistics = newValue;
            Assert.AreEqual(newValue, testObject.Statistics);

            // The purpose of this test is to catch property implementation bugs
            // where the wrong field (class level variable) is returned.
            //
            // Statistics is currently an auto-property but this is subject to change.
            //
            // Statistics needs a public setter to be serializable.
        }

        [TestMethod]
        public void DestinationFolder_ReturnsSetValue()
        {
            WebDataProviderConcreteTestImplementation testObject = new();

            string newValue = Guid.NewGuid().ToString();

            testObject.DestinationFolder = newValue;
            Assert.AreEqual(newValue, testObject.DestinationFolder);

            // The purpose of this test is to catch property implementation bugs
            // where the wrong field (class level variable) is returned.
            //
            // DestinationFolder is currently an auto-property but this is subject to change.
            //
            // DestinationFolder needs a public setter to be serializable.
        }

        [TestMethod]
        public void PreservationRule_ReturnsSetValue()
        {
            WebDataProviderConcreteTestImplementation testObject = new();

            // If testObject.Browser's default value changes, this test problably
            // won't get updated, so find a rule that isn't currently assigned
            // (or the whole test is inconclusive).
            PreservationRule rule = testObject.PreservationRule == PreservationRule.All ? PreservationRule.Matching : PreservationRule.All;

            testObject.PreservationRule = rule;
            Assert.AreEqual(rule, testObject.PreservationRule);

            // The purpose of this test is to catch property implementation bugs
            // where the wrong field (class level variable) is returned.
            //
            // PreservationRule is currently an auto-property but this is subject to change.
        }

        #endregion

        #region Invalid sleep ranges

        [TestMethod]
        public void MinimumSleepTimeRejectsZero()
        {
            WebDataProviderConcreteTestImplementation testObject = new();

            testObject.MinimumSleep = 0;

            Assert.IsTrue(testObject.MinimumSleep > 0);
        }

        [TestMethod]
        public void MaximumSleepTimeRejectsZero()
        {
            WebDataProviderConcreteTestImplementation testObject = new();

            testObject.MaximumSleep = 0;

            Assert.IsTrue(testObject.MinimumSleep > 0);
        }

        [TestMethod]
        public void MinimumSleepTimeRejectsIntMax()
        {
            WebDataProviderConcreteTestImplementation testObject = new();

            testObject.MinimumSleep = int.MaxValue;

            Assert.IsTrue(testObject.MinimumSleep < int.MaxValue);
        }

        [TestMethod]
        public void MaximumSleepTimeRejectsIntMax()
        {
            WebDataProviderConcreteTestImplementation testObject = new();

            testObject.MaximumSleep = int.MaxValue;

            Assert.IsTrue(testObject.MinimumSleep < int.MaxValue);
        }

        #endregion

        #region Valid sleep ranges

        [TestMethod]
        public void MinimumSleepTimeAllowsMinValue()
        {
            WebDataProviderConcreteTestImplementation testObject = new();

            testObject.MinimumSleep = testObject.MinimumAllowedSleep;

            Assert.AreEqual(testObject.MinimumSleep, testObject.MinimumAllowedSleep);
        }

        [TestMethod]
        public void MinimumSleepTimeAllowsMaxValue()
        {
            WebDataProviderConcreteTestImplementation testObject = new();

            testObject.MinimumSleep = testObject.MaximumRandomSleep;

            Assert.AreEqual(testObject.MinimumSleep, testObject.MaximumRandomSleep);
        }

        [TestMethod]
        public void MaximumSleepSleepTimeAllowsMinValue()
        {
            WebDataProviderConcreteTestImplementation testObject = new();

            testObject.MaximumSleep = testObject.MinimumAllowedSleep;

            Assert.AreEqual(testObject.MaximumSleep, testObject.MinimumAllowedSleep);
        }

        [TestMethod]
        public void MaximumSleepSleepTimeAllowsMaxValue()
        {
            WebDataProviderConcreteTestImplementation testObject = new();

            testObject.MaximumSleep = testObject.MaximumRandomSleep;

            Assert.AreEqual(testObject.MaximumSleep, testObject.MaximumRandomSleep);
        }

        #endregion

        #region Sleep

        [TestMethod]
        public void SleepingRaisesSleepEvent()
        {
            bool sleepEventHappened = false;

            WebDataProviderConcreteTestImplementation testObject = new();

            testObject.Sleeping += delegate(object sender, SleepEventArgs e) { sleepEventHappened = true; };

            testObject.CallSleep();

            Assert.IsTrue(sleepEventHappened);
        }

        [TestMethod]
        public void SleepEventDurationIsAccurate()
        {
            TimeSpan claimedSleepTime = TimeSpan.Zero, actualSleepTime;

            WebDataProviderConcreteTestImplementation testObject = new();

            testObject.Sleeping += delegate (object sender, SleepEventArgs e) { claimedSleepTime = e.Duration; };

            Stopwatch stopwatch = Stopwatch.StartNew();
            testObject.CallSleep();
            stopwatch.Stop();
            actualSleepTime = stopwatch.Elapsed;

            double varianceInMS = claimedSleepTime.TotalMilliseconds - actualSleepTime.TotalMilliseconds;
            double threshold = claimedSleepTime.TotalSeconds > 1 ? 0.15 : 0.5;

            // In real-world use, sleep times will be in the multiple seconds to several minutes.
            // There is some overhead surrounding a Sleep call;  the faster the sleep, the more
            // overhead as a % of the total.
            //
            // I don't want tests to take several minutes to run, so this attempts to validate
            // that it's reasonably close with very short sleep timespans.

            Console.WriteLine($"Event reported sleep: {claimedSleepTime}");
            Console.WriteLine($"Measured sleep: {actualSleepTime}");
            Console.WriteLine($"{Math.Abs(varianceInMS) / claimedSleepTime.TotalMilliseconds}% difference");
            Console.WriteLine($"{threshold}% required for this test");

            Assert.IsTrue(Math.Abs(varianceInMS) / claimedSleepTime.TotalMilliseconds < threshold);
        }

        [TestMethod]
        public void Sleep_RaisingEventWithoutListenersDoesntThrow()
        {
            WebDataProviderConcreteTestImplementation testObject = new();

            testObject.CallSleep();
        }

        #endregion

        #region CanRun

        [TestMethod]
        public void CanRun_TrueWhenPreservationRuleIsNone()
        {
            WebDataProviderConcreteTestImplementation testObject = new();

            testObject.PreservationRule = PreservationRule.None;
            testObject.DestinationFolder = null;

            Assert.IsTrue(testObject.CanRun, "Destination folder is null");

            testObject.PreservationRule = PreservationRule.None;
            testObject.DestinationFolder = $@"Z:\{new string('z', 512)}";

            Assert.IsTrue(testObject.CanRun, "Destination folder does not exist and is an illegal path");
        }

        [TestMethod]
        public void CanRun_FalseWhenDestinationFolderIsNull()
        {
            WebDataProviderConcreteTestImplementation testObject = new();

            testObject.PreservationRule = PreservationRule.Matching;
            testObject.DestinationFolder = null;

            Assert.IsFalse(testObject.CanRun, "Destination folder is null");
        }

        [TestMethod]
        public void CanRun_FalseWhenDestinationFolderIsIllegal()
        {
            WebDataProviderConcreteTestImplementation testObject = new();

            testObject.PreservationRule = PreservationRule.Matching;
            testObject.DestinationFolder = $@"Z:\{new string('z', 512)}";

            Assert.IsFalse(testObject.CanRun, "Destination folder does not exist and is an illegal path");
        }

        [TestMethod]
        public void CanRun_FalseWhenDestinationFolderDoesNotExist()
        {
            WebDataProviderConcreteTestImplementation testObject = new();

            testObject.PreservationRule = PreservationRule.Matching;
            testObject.DestinationFolder = $@"{Environment.CurrentDirectory}\{Guid.NewGuid()}";

            Assert.IsFalse(testObject.CanRun, "Destination folder does not exist");
        }

        //TODO: Test that MatchOnly can't start without topics

        #endregion

        [TestMethod]
        public void OnTripReportCompleted_AddsNewUrlToCompletedList()
        {
            WebDataProviderConcreteTestImplementation testObject = new();

            string url = Guid.NewGuid().ToString();

            while (testObject.CompletedUrls.Contains(url))
            {
                url = Guid.NewGuid().ToString();
            }

            testObject.CallOnTripReportCompleted(url);
            Assert.IsTrue(testObject.CompletedUrls.Contains(url));
        }

        [TestMethod]
        public void OnTripReportCompleted_DoesNotAddExistingUrlToCompletedList()
        {
            WebDataProviderConcreteTestImplementation testObject = new();

            string url = Guid.NewGuid().ToString();

            testObject.CompletedUrls.Add(url);

            int currentCompletedCount = testObject.CompletedUrls.Count;
            testObject.CallOnTripReportCompleted(url);

            Assert.IsTrue(testObject.CompletedUrls.Contains(url));
            Assert.AreEqual(currentCompletedCount, testObject.CompletedUrls.Count);
        }

        [TestMethod]
        public void OnTripReportCompleted_FiresEvent()
        {
            bool eventHappened = false;
            WebDataProviderConcreteTestImplementation testObject = new();

            testObject.TripReportCompleted += (object sender, TripReportCompletedEventArgs e) =>
            {
                eventHappened = true;
            };

            testObject.CallOnTripReportCompleted(Guid.NewGuid().ToString());

            Assert.IsTrue(eventHappened);
        }
    }
}
