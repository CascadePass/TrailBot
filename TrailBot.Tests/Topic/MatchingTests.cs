using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace CascadePass.TrailBot.Tests
{
    [TestClass]
    public class MatchingTests
    {
        [TestMethod]
        public void SimpleMatchIsFound()
        {
            Topic t = new()
            {
                MatchAny = "broken"
            };

            TripReport tr = new() { ReportText = "There was a car with a broken window when I got back to the trailhead." };

            var matchInfo = t.GetMatchInfo(tr);

            Assert.IsNotNull(matchInfo);
            Assert.IsFalse(matchInfo.IsEmpty);

            Console.WriteLine($"{matchInfo.Count} matches:");
            foreach (string key in matchInfo.MatchCounts.Keys)
            {
                Console.WriteLine($"\t{key} = {matchInfo.MatchCounts[key]}");
            }

            Assert.IsTrue(matchInfo.Count == 1);
        }

        [TestMethod]
        public void SimpleMatchMultipleOccurances()
        {
            Topic t = new()
            {
                MatchAny = "broken"
            };

            TripReport tr = new() { ReportText = "There was a car with a broken window when I got back to the trailhead.  I can't believe my window was broken!" };

            var matchInfo = t.GetMatchInfo(tr);

            Assert.IsNotNull(matchInfo);
            Assert.IsFalse(matchInfo.IsEmpty);

            Console.WriteLine($"{matchInfo.Count} matches:");
            foreach (string key in matchInfo.MatchCounts.Keys)
            {
                Console.WriteLine($"\t{key} = {matchInfo.MatchCounts[key]}");
            }

            Assert.IsTrue(matchInfo.Count == 1);
            Assert.IsTrue(matchInfo.MatchCounts["broken"] == 2);
        }

        [TestMethod]
        public void SimplePhraseMatchIsFound()
        {
            Topic t = new()
            {
                MatchAny = "broken window"
            };

            TripReport tr = new() { ReportText = "There was a car with a broken window when I got back to the trailhead." };

            var matchInfo = t.GetMatchInfo(tr);

            Assert.IsNotNull(matchInfo);
            Assert.IsFalse(matchInfo.IsEmpty);

            Console.WriteLine($"{matchInfo.Count} matches:");
            foreach (string key in matchInfo.MatchCounts.Keys)
            {
                Console.WriteLine($"\t{key} = {matchInfo.MatchCounts[key]}");
            }

            Assert.AreEqual(1, matchInfo.Count);
        }

        [TestMethod]
        public void MatchingIsCaseInsensitive()
        {
            Topic t = new()
            {
                MatchAny = "BROKEN"
            };

            TripReport tr = new() { ReportText = "There was a car with a broken window when I got back to the trailhead." };

            var matchInfo = t.GetMatchInfo(tr);

            Assert.IsNotNull(matchInfo);
            Assert.IsFalse(matchInfo.IsEmpty);

            Console.WriteLine($"{matchInfo.Count} matches:");
            foreach (string key in matchInfo.MatchCounts.Keys)
            {
                Console.WriteLine($"\t{key} = {matchInfo.MatchCounts[key]}");
            }

            Assert.IsTrue(matchInfo.Count == 1);
        }

        #region Edges of the array

        #region Single word

        [TestMethod]
        public void FirstWordMatchIsFound()
        {
            Topic t = new() { MatchAny = "There" };

            TripReport tr = new() { ReportText = "There was a car with a broken window when I got back to the trailhead." };

            var matchInfo = t.GetMatchInfo(tr);

            Assert.IsNotNull(matchInfo);
            Assert.IsFalse(matchInfo.IsEmpty);

            Console.WriteLine($"{matchInfo.Count} matches:");
            foreach (string key in matchInfo.MatchCounts.Keys)
            {
                Console.WriteLine($"\t{key} = {matchInfo.MatchCounts[key]}");
            }

            Assert.IsTrue(matchInfo.Count == 1);
        }

        [TestMethod]
        public void LastWordMatchIsFound()
        {
            Topic t = new() { MatchAny = "trailhead" };

            TripReport tr = new() { ReportText = "There was a car with a broken window when I got back to the trailhead" };

            var matchInfo = t.GetMatchInfo(tr);

            Assert.IsNotNull(matchInfo);
            Assert.IsFalse(matchInfo.IsEmpty);

            Console.WriteLine($"{matchInfo.Count} matches:");
            foreach (string key in matchInfo.MatchCounts.Keys)
            {
                Console.WriteLine($"\t{key} = {matchInfo.MatchCounts[key]}");
            }

            Assert.IsTrue(matchInfo.Count == 1);
        }

        [TestMethod]
        public void LastWordMatchIsFoundWithPunctuation()
        {
            Topic t = new() { MatchAny = "trailhead" };

            TripReport tr = new() { ReportText = "There was a car with a broken window when I got back to the trailhead." };

            var matchInfo = t.GetMatchInfo(tr);

            Assert.IsNotNull(matchInfo);
            Assert.IsFalse(matchInfo.IsEmpty);

            Console.WriteLine($"{matchInfo.Count} matches:");
            foreach (string key in matchInfo.MatchCounts.Keys)
            {
                Console.WriteLine($"\t{key} = {matchInfo.MatchCounts[key]}");
            }

            Assert.IsTrue(matchInfo.Count == 1);
        }

        #endregion

        #region Multiple words (Phrase)

        [TestMethod]
        public void FirstPhraseMatchIsFound()
        {
            Topic t = new() { MatchAny = "There was" };

            TripReport tr = new() { ReportText = "There was a car with a broken window when I got back to the trailhead." };

            var matchInfo = t.GetMatchInfo(tr);

            Assert.IsNotNull(matchInfo);
            Assert.IsFalse(matchInfo.IsEmpty);

            Console.WriteLine($"{matchInfo.Count} matches:");
            foreach (string key in matchInfo.MatchCounts.Keys)
            {
                Console.WriteLine($"\t{key} = {matchInfo.MatchCounts[key]}");
            }

            Assert.IsTrue(matchInfo.Count == 1);
        }

        [TestMethod]
        public void LastPhraseMatchIsFound()
        {
            Topic t = new() { MatchAny = "the trailhead" };

            TripReport tr = new() { ReportText = "There was a car with a broken window when I got back to the trailhead" };

            var matchInfo = t.GetMatchInfo(tr);

            Assert.IsNotNull(matchInfo);
            Assert.IsFalse(matchInfo.IsEmpty);

            Console.WriteLine($"{matchInfo.Count} matches:");
            foreach (string key in matchInfo.MatchCounts.Keys)
            {
                Console.WriteLine($"\t{key} = {matchInfo.MatchCounts[key]}");
            }

            Assert.IsTrue(matchInfo.Count == 1);
        }

        [TestMethod]
        public void LastPhraseMatchIsFoundWithPunctuation()
        {
            Topic t = new() { MatchAny = "the trailhead" };

            TripReport tr = new() { ReportText = "There was a car with a broken window when I got back to the trailhead." };

            var matchInfo = t.GetMatchInfo(tr);

            Assert.IsNotNull(matchInfo);
            Assert.IsFalse(matchInfo.IsEmpty);

            Console.WriteLine($"{matchInfo.Count} matches:");
            foreach (string key in matchInfo.MatchCounts.Keys)
            {
                Console.WriteLine($"\t{key} = {matchInfo.MatchCounts[key]}");
            }

            Assert.IsTrue(matchInfo.Count == 1);
        }

        #endregion

        #endregion

        [TestMethod]
        public void MatchAnyUnlessPreventsMatch()
        {
            Topic t = new()
            {
                MatchAny = "warning",
                MatchAnyUnless = "cougar warning"
            };

            TripReport tr = new() { ReportText = "I was a little put off by the cougar warning at the trailhead." };

            var matchInfo = t.GetMatchInfo(tr);

            Assert.IsNotNull(matchInfo);
            Assert.IsTrue(matchInfo.IsEmpty);

            Console.WriteLine($"{matchInfo.Count} matches:");
            foreach (string key in matchInfo.MatchCounts.Keys)
            {
                Console.WriteLine($"\t{key} = {matchInfo.MatchCounts[key]}");
            }

            Assert.AreEqual(0, matchInfo.Count);
        }

        [TestMethod]
        public void MatchAnyUnlessDoesntPreventEveryOccuranceFromMatching()
        {
            Topic t = new()
            {
                MatchAny = "warning",
                MatchAnyUnless = "cougar warning"
            };

            TripReport tr = new() { ReportText = "I was a little put off by the cougar warning at the trailhead.  There should have been a warning about thieves too." };

            var matchInfo = t.GetMatchInfo(tr);

            Assert.IsNotNull(matchInfo);
            Assert.IsFalse(matchInfo.IsEmpty);

            Console.WriteLine($"{matchInfo.Count} matches:");
            foreach (string key in matchInfo.MatchCounts.Keys)
            {
                Console.WriteLine($"\t{key} = {matchInfo.MatchCounts[key]}");
            }

            Assert.AreEqual(matchInfo.Count, 1);
        }

        #region WordCount

        [TestMethod]
        public void WordCount_ZeroForEmptyString()
        {
            Topic t = new() { MatchAny = "broken" };

            TripReport tr = new() { ReportText = string.Empty };

            var matchInfo = t.GetMatchInfo(tr);

            Assert.IsNotNull(matchInfo);
            Console.WriteLine($"{matchInfo.WordCount} words reported.");
            Assert.AreEqual(0, matchInfo.WordCount);
        }

        [TestMethod]
        public void WordCount_SimpleMatch()
        {
            Topic t = new() { MatchAny = "broken" };

            TripReport tr = new() { ReportText = "There was a car with a broken window when I got back to the trailhead." };

            var matchInfo = t.GetMatchInfo(tr);

            Assert.IsNotNull(matchInfo);
            Console.WriteLine($"{matchInfo.WordCount} words reported.");
            Assert.AreEqual(15, matchInfo.WordCount);
        }

        [TestMethod]
        public void WordCount_IgnoresPunctuation()
        {
            Topic t = new() { MatchAny = "broken" };

            TripReport tr = new() { ReportText = "The trailhead !!! ??? $$$$$" };

            var matchInfo = t.GetMatchInfo(tr);

            Assert.IsNotNull(matchInfo);
            Console.WriteLine($"{matchInfo.WordCount} words reported.");
            Assert.AreEqual(2, matchInfo.WordCount);
        }

        #endregion

        #region Topic

        [TestMethod]
        public void Topic_SameInstanceIsReturned()
        {
            Topic topic = new() { MatchAny = "broken window" };

            TripReport tr = new() { ReportText = "There was a car with a broken window when I got back to the trailhead." };

            var matchInfo = topic.GetMatchInfo(tr);

            Assert.IsNotNull(matchInfo);
            Assert.IsNotNull(matchInfo.Topic);
            Assert.AreSame(topic, matchInfo.Topic);
        }

        [TestMethod]
        public void Topic_CorrectInstanceIsReturned()
        {
            Topic foundTopic = new() { MatchAny = "broken window" };
            Topic notInvolvedTopic = new() { MatchAny = "cats" };

            TripReport tr = new() { ReportText = "There was a car with a broken window when I got back to the trailhead." };

            var matchInfo = foundTopic.GetMatchInfo(tr);

            Assert.IsNotNull(matchInfo);
            Assert.IsNotNull(matchInfo.Topic);
            Assert.AreNotSame(notInvolvedTopic, matchInfo.Topic);
            Assert.AreSame(foundTopic, matchInfo.Topic);
        }

        #endregion
    }
}
