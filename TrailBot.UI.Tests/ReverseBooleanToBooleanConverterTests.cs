using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Windows;

namespace CascadePass.TrailBot.UI.Tests
{
    [TestClass]
    public class ReverseBooleanToBooleanConverterTests
    {
        [TestMethod]
        public void CanCreateInstance()
        {
            ReverseBooleanToBooleanConverter testObject = new();
        }

        #region Convert

        #region null

        [TestMethod]
        public void Convert_Null_NoExceptionThrown()
        {
            ReverseBooleanToBooleanConverter testObject = new();

            testObject.Convert(null, null, null, null);
        }

        [TestMethod]
        public void Convert_Null_Returns_UnsetValue()
        {
            ReverseBooleanToBooleanConverter testObject = new();

            object result = testObject.Convert(null, null, null, null);

            Assert.AreEqual(DependencyProperty.UnsetValue, result);
        }

        #endregion

        #region A string

        [TestMethod]
        public void Convert_WrongType_NoExceptionThrown()
        {
            ReverseBooleanToBooleanConverter testObject = new();

            testObject.Convert("true", null, null, null);
        }

        [TestMethod]
        public void Convert_WrongType_Returns_UnsetValue()
        {
            ReverseBooleanToBooleanConverter testObject = new();

            object result = testObject.Convert("true", null, null, null);

            Assert.AreEqual(DependencyProperty.UnsetValue, result);
        }

        #endregion

        #region Bools

        [TestMethod]
        public void Convert_True_Returns_False()
        {
            ReverseBooleanToBooleanConverter testObject = new();

            object result = testObject.Convert(true, null, null, null);

            Assert.IsTrue(result is bool);
            Assert.AreEqual(false, result);
        }


        [TestMethod]
        public void Convert_False_Returns_True()
        {
            ReverseBooleanToBooleanConverter testObject = new();

            object result = testObject.Convert(false, null, null, null);

            Assert.IsTrue(result is bool);
            Assert.AreEqual(true, result);
        }

        #endregion

        #endregion

        #region ConvertBack

        #region null

        [TestMethod]
        public void ConvertBack_Null_NoExceptionThrown()
        {
            ReverseBooleanToBooleanConverter testObject = new();

            testObject.ConvertBack(null, null, null, null);
        }

        [TestMethod]
        public void ConvertBack_Null_Returns_UnsetValue()
        {
            ReverseBooleanToBooleanConverter testObject = new();

            object result = testObject.ConvertBack(null, null, null, null);

            Assert.AreEqual(DependencyProperty.UnsetValue, result);
        }

        #endregion

        #region A string

        [TestMethod]
        public void ConvertBack_WrongType_NoExceptionThrown()
        {
            ReverseBooleanToBooleanConverter testObject = new();

            testObject.ConvertBack("true", null, null, null);
        }

        [TestMethod]
        public void ConvertBack_WrongType_Returns_UnsetValue()
        {
            ReverseBooleanToBooleanConverter testObject = new();

            object result = testObject.ConvertBack("true", null, null, null);

            Assert.AreEqual(DependencyProperty.UnsetValue, result);
        }

        #endregion

        #region Bools

        [TestMethod]
        public void ConvertBack_True_Returns_False()
        {
            ReverseBooleanToBooleanConverter testObject = new();

            object result = testObject.ConvertBack(true, null, null, null);

            Assert.IsTrue(result is bool);
            Assert.AreEqual(false, result);
        }


        [TestMethod]
        public void ConvertBack_False_Returns_True()
        {
            ReverseBooleanToBooleanConverter testObject = new();

            object result = testObject.ConvertBack(false, null, null, null);

            Assert.IsTrue(result is bool);
            Assert.AreEqual(true, result);
        }

        #endregion

        #endregion
    }
}
