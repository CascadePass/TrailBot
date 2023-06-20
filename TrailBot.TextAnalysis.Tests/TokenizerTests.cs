using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;

namespace CascadePass.TrailBot.TextAnalysis.Tests
{
    [TestClass]
    public class TokenizerTests
    {
        [TestMethod]
        public void TypeCanBeCreated()
        {
            Tokenizer tokenizer = new();

            // Expected result is no exception
            // Static type constructor (runs on first use of type) uses a compiled regex
        }

        [TestMethod]
        public void GetTokensBreaksStringsAsExpected()
        {
            string testString = "the quick brown fox jumps over the lazy dog";
            string[] expectedResult = { "the", "quick", "brown", "fox", "jumps", "over", "the", "lazy", "dog" };

            Tokenizer tokenizer = new ();

            tokenizer.GetTokens(testString);
            var result = tokenizer.OrderedTokens;

            Assert.IsNotNull(result);

            for (int i = 0; i < result.Count; i++)
            {
                string token = result[i].Text;

                Console.WriteLine($"{expectedResult[i]}\t{token}");
                Assert.AreEqual(expectedResult[i], token);
            }

            Assert.AreEqual(expectedResult.Length, result.Count);
        }

        [TestMethod]
        public void EmptySourceTextReturnsNothing()
        {
            Tokenizer tokenizer = new();
            tokenizer.GetTokens(string.Empty);

            Assert.AreEqual(0, tokenizer.OrderedTokens.Count);
        }

        [TestMethod]
        public void SourceTextPreservedCorrectly()
        {
            Tokenizer tokenizer = new();
            string s = Guid.NewGuid().ToString();

            tokenizer.GetTokens(s);

            Assert.AreEqual(tokenizer.SourceText, s);
        }

        // symbol tests .cs

        [TestMethod]
        public void MultiCharacterSymbolsCapturedProperly()
        {
            string testString = "There was a large brown bear !!!!! right in the meadow.";

            Tokenizer tokenizer = new();

            tokenizer.GetTokens(testString);
            var result = tokenizer.OrderedTokens;

            Assert.IsNotNull(result);

            for (int i = 0; i < result.Count; i++)
            {
                string token = result[i].Text;

                Console.WriteLine(token);
            }

            Assert.IsTrue(result.Any(m => string.Equals(m.Text, "!!!!!", StringComparison.Ordinal)));
        }

        // contractions tests .cs

        [TestMethod]
        public void Contraction_Ive()
        {
            string text = "I've";
            Tokenizer tokenizer = new();

            tokenizer.GetTokens(text);

            var result = tokenizer.OrderedTokens;

            Assert.AreEqual(1, result.Count);
            Assert.AreEqual(text, result[0].Text);
        }
    }
}
