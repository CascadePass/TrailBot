using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace CascadePass.TrailBot.Tests
{
    [TestClass]
    public class TopicExerptTests
    {
        [TestMethod]
        public void NoExerptWithNoMatch()
        {
            Topic t = new()
            {
                MatchAny = Guid.NewGuid().ToString(),
            };

            TripReport tr = new() { ReportText = "There should have been a warning about thieves breaking windows." };

            var matchInfo = t.GetMatchInfo(tr);

            Assert.IsNotNull(matchInfo);
            Assert.IsTrue(matchInfo.IsEmpty);
            Assert.IsTrue(matchInfo.MatchQuotes.Count == 0);
        }

        [TestMethod]
        public void TwoMatchesInOneSentenceYieldOneExerpt()
        {
            Topic t = new()
            {
                MatchAny = "warning\r\nbreaking windows",
            };

            TripReport tr = new() { ReportText = "There should have been a warning about thieves breaking windows." };

            var matchInfo = t.GetMatchInfo(tr);

            Assert.IsNotNull(matchInfo);
            Assert.IsFalse(matchInfo.IsEmpty);

            Console.WriteLine($"{matchInfo.MatchQuotes.Count} exerpts:");
            foreach (string exerpt in matchInfo.MatchQuotes)
            {
                Console.WriteLine($"\t{exerpt}");
            }

            // There was a bug, the two different MatchAny strings in the sentence used to cause
            // the sentence to be output twice in the result.  Each match causes the entire sentence
            // or clause it's in to be experted.  This test validates the duplicate check.
            Assert.AreEqual(1, matchInfo.MatchQuotes.Count);
        }

        [TestMethod]
        public void QuoteBeginsAtComma()
        {
            Topic t = new()
            {
                MatchAny = "broken into",
            };

            TripReport tr = new() { ReportText = "We did this, but other cars were broken into." };

            var matchInfo = t.GetMatchInfo(tr);

            Assert.IsNotNull(matchInfo);
            Assert.IsFalse(matchInfo.IsEmpty);

            Console.WriteLine("Text:");
            Console.WriteLine();
            Console.WriteLine(tr.ReportText);
            Console.WriteLine();
            Console.WriteLine();

            Console.WriteLine($"Match: {t.MatchAny}");
            Console.WriteLine();

            Console.WriteLine($"{matchInfo.MatchQuotes.Count} exerpts:");
            foreach (string exerpt in matchInfo.MatchQuotes)
            {
                Console.WriteLine($"\t{exerpt}");
            }

            Assert.IsTrue(matchInfo.MatchQuotes.Contains("but other cars were broken into."));
            Assert.AreEqual(1, matchInfo.MatchQuotes.Count);
        }
    }
}
