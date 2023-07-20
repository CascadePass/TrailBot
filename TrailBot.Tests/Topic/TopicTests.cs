using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace CascadePass.TrailBot.Tests
{
    [TestClass]
    public class TopicTests
    {
        [TestMethod]
        public void CanCreateEmptyTopic()
        {
            Topic t = new();
        }

        #region Don't crash for Null/Empty input

        [TestMethod]
        public void MatchAny_EmptyStringDoesntThrow()
        {
            Topic t = new();
            t.MatchAny = string.Empty;

            // Expectation is that no exception was thrown.
        }

        [TestMethod]
        public void MatchAny_NullDoesntThrow()
        {
            Topic t = new();
            t.MatchAny = null;

            // Expectation is that no exception was thrown.
        }

        [TestMethod]
        public void MatchAnyUnless_EmptyStringDoesntThrow()
        {
            Topic t = new();
            t.MatchAnyUnless = string.Empty;

            // Expectation is that no exception was thrown.
        }

        [TestMethod]
        public void MatchAnyUnless_NullDoesntThrow()
        {
            Topic t = new();
            t.MatchAnyUnless = null;

            // Expectation is that no exception was thrown.
        }

        [TestMethod]
        public void GetMatchInfo_NullTripReportDoesntThrow()
        {
            Topic t = new();
            _ = t.GetMatchInfo((TripReport)null);
        }

        [TestMethod]
        public void GetMatchInfo_NullTripReportNotAMatch()
        {
            Topic t = new();
            var result = t.GetMatchInfo((TripReport)null);

            Assert.IsTrue(result == null || result.Count == 0);
        }

        [TestMethod]
        public void GetMatchInfo_NullStringNotAMatch()
        {
            Topic t = new();
            var result = t.GetMatchInfo((string)null);

            Assert.IsTrue(result == null || result.Count == 0);
        }

        [TestMethod]
        public void ParseSearchTerms_NullDoesntThrow()
        {
            Topic.ParseSearchTerms(null);

            // Expectation is that no exception was thrown.
        }

        [TestMethod]
        public void ParseSearchTerms_EmptyStringDoesntThrow()
        {
            Topic.ParseSearchTerms(string.Empty);

            // Expectation is that no exception was thrown.
        }

        #endregion

        #region ParseSearchTerms (correctness)

        [TestMethod]
        public void ParseSearchTerms_EmptyStringReturnsNothing()
        {
            var result = Topic.ParseSearchTerms(string.Empty);

            Assert.AreEqual(0, result.Count);
        }


        [TestMethod]
        public void ParseSearchTerms_EmptyLinesAreIgnored()
        {
            var result = Topic.ParseSearchTerms("test\r\n\r\ntext");

            Assert.AreEqual(2, result.Count);
        }

        [TestMethod]
        public void ParseSearchTerms_OneWord()
        {
            var result = Topic.ParseSearchTerms("word");

            Assert.AreEqual(1, result.Count);
        }

        [TestMethod]
        public void ParseSearchTerms_OnePhrase()
        {
            var result = Topic.ParseSearchTerms("one phrase");

            Assert.AreEqual(1, result.Count);
        }

        [TestMethod]
        public void ParseSearchTerms_OnePerLine()
        {
            var result = Topic.ParseSearchTerms("one phrase\rper\nline");

            Assert.AreEqual(3, result.Count);
        }

        [TestMethod]
        public void ParseSearchTerms_EmptyLinesIgnored()
        {
            var result = Topic.ParseSearchTerms("one phrase\rper\n\r\n\r\nline");

            Assert.AreEqual(3, result.Count);
        }

        [TestMethod]
        public void ParseSearchTerms_NonWordsAllowed()
        {
            var result = Topic.ParseSearchTerms("911");

            Assert.AreEqual(1, result.Count);
        }

        [TestMethod]
        public void ParseSearchTerms_ApostropheIsPartOfWord()
        {
            var result = Topic.ParseSearchTerms("occam's razor");

            // 1 term (one line, terms are separated by line, this method was intended for parsing Match terms)
            Assert.AreEqual(1, result.Count);

            // 1 term has 2 keywords
            // If the ' is not handled correctly, there will be 3 words:  occam s razor
            Assert.AreEqual(2, result[0].Parts.Length);

            Assert.AreEqual(result[0].Parts[0].Text, "occam's");
            Assert.AreEqual(result[0].Parts[1].Text, "razor");
        }

        #endregion

        #region Count Properties

        [TestMethod]
        public void AnyCount_NullDoesntThrow()
        {
            Topic topic = new();

            Assert.AreEqual(0, topic.AnyCount);
        }

        [TestMethod]
        public void UnlessCount_NullDoesntThrow()
        {
            Topic topic = new();

            Assert.AreEqual(0, topic.UnlessCount);
        }

        [TestMethod]
        public void AnyCount_CountsLines()
        {
            Topic topic = new()
            {
                MatchAny = "one phrase\rper\n\r\n\r\nline",
            };

            Assert.AreEqual(3, topic.AnyCount);
        }

        [TestMethod]
        public void AnyCount_AgreesWith_MatchAnyPhrases()
        {
            Topic topic = new()
            {
                MatchAny = "one phrase\rper\n\r\n\r\nline",
            };

            Assert.AreEqual(topic.MatchAnyPhrases.Count, topic.AnyCount);
        }

        [TestMethod]
        public void UnlessCount_CountsLines()
        {
            Topic topic = new()
            {
                MatchAnyUnless = "one phrase\rper\n\r\n\r\nline",
            };

            Assert.AreEqual(3, topic.UnlessCount);
        }

        [TestMethod]
        public void UnlessCount_AgreesWith_MatchAnyUnlessPhrases()
        {
            Topic topic = new()
            {
                MatchAnyUnless = "one phrase\rper\n\r\n\r\nline",
            };

            Assert.AreEqual(topic.MatchAnyUnlessPhrases.Count, topic.UnlessCount);
        }

        #endregion

        #region get/set access same value

        [TestMethod]
        public void MatchAny()
        {
            string testText = Guid.NewGuid().ToString();
            Topic topic = new();
            topic.MatchAny = testText;

            Assert.AreEqual(testText, topic.MatchAny);
        }

        [TestMethod]
        public void MatchAnyUnless()
        {
            string testText = Guid.NewGuid().ToString();
            Topic topic = new();
            topic.MatchAnyUnless = testText;

            Assert.AreEqual(testText, topic.MatchAnyUnless);
        }

        #endregion

        #region GetMatchInfo

        [TestMethod]
        public void GetMatchInfo_NullString()
        {
            Topic topic = new();
            MatchInfo result = topic.GetMatchInfo((string)null);
            Assert.IsTrue(result.IsEmpty);
        }

        [TestMethod]
        public void GetMatchInfo_NullTripReport()
        {
            Topic topic = new();
            MatchInfo result = topic.GetMatchInfo((TripReport)null);
            Assert.IsTrue(result.IsEmpty);
        }

        [TestMethod]
        public void GetMatchInfo_EmptyString()
        {
            Topic topic = new();
            MatchInfo result = topic.GetMatchInfo(string.Empty);
            Assert.IsTrue(result.IsEmpty);
        }

        [TestMethod]
        public void GetMatchInfo_WhiteSpace()
        {
            Topic topic = new();
            MatchInfo result = topic.GetMatchInfo("   ");
            Assert.IsTrue(result.IsEmpty);
        }

        #endregion

        #region ToString

        [TestMethod]
        public void ToString_Returns_Name()
        {
            Topic topic = new()
            {
                Name = "crime",
                MatchAny = "broken\r\nwindow",
            };

            Assert.AreEqual(topic.Name, topic.ToString());
        }

        [TestMethod]
        public void ToString_NoName_Returns_ClassName()
        {
            Topic topic = new()
            {
                MatchAny = "broken\r\nwindow",
            };

            Console.WriteLine(topic.ToString());
            Assert.IsTrue(topic.ToString().Contains(typeof(Topic).FullName));
        }

        #endregion
    }
}
