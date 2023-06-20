using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace CascadePass.TrailBot.TextAnalysis.Tests
{
    [TestClass]
    public class TokenTests
    {
        [TestMethod]
        public void CanCreateToken()
        {
            _ = new Token("token");
        }

        [TestMethod]
        public void ToString_ReturnsTokenText()
        {
            string text = Guid.NewGuid().ToString();
            Token token = new(text);

            Assert.AreEqual(text, token.ToString());
        }

        #region IsWord

        [TestMethod]
        public void IsWord_word()
        {
            Token token = new ("word");

            Assert.IsTrue(token.IsWord);
        }

        [TestMethod]
        public void IsWord_123()
        {
            Token token = new ("123");

            Assert.IsFalse(token.IsWord);
        }

        [TestMethod]
        public void IsWord_Ive()
        {
            Token token = new("I've");

            Assert.IsTrue(token.IsWord);
        }

        #endregion

        #region IsNumber

        [TestMethod]
        public void IsNumber_123()
        {
            Token token = new ("123");

            Assert.IsTrue(token.IsNumber);
        }

        [TestMethod]
        public void IsNumber_123dot3()
        {
            Token token = new ("123.3");

            Assert.IsTrue(token.IsNumber);
        }

        [TestMethod]
        public void IsNumber_123comma456()
        {
            Token token = new("123,456");

            Assert.IsTrue(token.IsNumber);
        }

        [TestMethod]
        public void IsNumber_dot()
        {
            Token token = new(".");

            Assert.IsFalse(token.IsNumber);
        }

        [TestMethod]
        public void IsNumber_word()
        {
            Token token = new ("word");

            Assert.IsFalse(token.IsNumber);
        }

        #endregion

        #region IsPunctuation

        [TestMethod]
        public void IsPunctuation_Period()
        {
            Token token = new(".");

            Assert.IsTrue(token.IsPunctuation);
        }

        [TestMethod]
        public void IsPunctuation_QuestionMark()
        {
            Token token = new("?");

            Assert.IsTrue(token.IsPunctuation);
        }

        [TestMethod]
        public void IsPunctuation_ExclamationMark()
        {
            Token token = new("!");

            Assert.IsTrue(token.IsPunctuation);
        }

        [TestMethod]
        public void IsPunctuation_Comma()
        {
            Token token = new(",");

            Assert.IsTrue(token.IsPunctuation);
        }

        [TestMethod]
        public void IsPunctuation_SemiColon()
        {
            Token token = new(";");

            Assert.IsTrue(token.IsPunctuation);
        }

        [TestMethod]
        public void IsPunctuation_Colon()
        {
            Token token = new(":");

            Assert.IsTrue(token.IsPunctuation);
        }

        [TestMethod]
        public void IsPunctuation_MultipleExclamationMarks()
        {
            Token token = new("!!!");

            Assert.IsTrue(token.IsPunctuation);
        }

        [TestMethod]
        public void IsPunctuation_ValueCachedProperly()
        {
            Token token = new("!!!");

            Assert.IsTrue(token.IsPunctuation);
            Assert.IsTrue(token.IsPunctuation);
        }

        [TestMethod]
        public void IsPunctuation_123()
        {
            Token token = new("123");

            Assert.IsFalse(token.IsPunctuation);
        }

        [TestMethod]
        public void IsPunctuation_word()
        {
            Token token = new("word");

            Assert.IsFalse(token.IsPunctuation);
        }

        #endregion
    }
}
