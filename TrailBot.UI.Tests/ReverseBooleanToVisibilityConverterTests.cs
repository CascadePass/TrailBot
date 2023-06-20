using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Windows;

namespace CascadePass.TrailBot.UI.Tests
{
    [TestClass]
    public class ReverseBooleanToVisibilityConverterTests
    {
        [TestMethod]
        public void CanCreateInstance()
        {
            ReverseBooleanToVisibilityConverter testObject = new();
        }

        #region null

        [TestMethod]
        public void Convert_Null_NoExceptionThrown()
        {
            ReverseBooleanToVisibilityConverter testObject = new();

            testObject.Convert(null, null, null, null);
        }

        [TestMethod]
        public void Convert_Null_ReturnsCollapsed()
        {
            ReverseBooleanToVisibilityConverter testObject = new();

            object result = testObject.Convert(null, null, null, null);

            Assert.IsTrue(result is Visibility);
            Assert.AreEqual(Visibility.Collapsed, result);
        }

        #endregion

        #region A string

        [TestMethod]
        public void Convert_WrongType_NoExceptionThrown()
        {
            ReverseBooleanToVisibilityConverter testObject = new();

            testObject.Convert("true", null, null, null);
        }

        [TestMethod]
        public void Convert_WrongType_ReturnsCollapsed()
        {
            ReverseBooleanToVisibilityConverter testObject = new();

            object result = testObject.Convert("true", null, null, null);

            Assert.IsTrue(result is Visibility);
            Assert.AreEqual(Visibility.Collapsed, result);
        }

        #endregion

        #region Bools

        [TestMethod]
        public void Convert_true_ReturnsCollapsed()
        {
            ReverseBooleanToVisibilityConverter testObject = new();

            object result = testObject.Convert(true, null, null, null);

            Assert.IsTrue(result is Visibility);
            Assert.AreEqual(Visibility.Collapsed, result);
        }


        [TestMethod]
        public void Convert_false_ReturnsVisible()
        {
            ReverseBooleanToVisibilityConverter testObject = new();

            object result = testObject.Convert(false, null, null, null);

            Assert.IsTrue(result is Visibility);
            Assert.AreEqual(Visibility.Visible, result);
        }

        #endregion

        [TestMethod]
        public void ConvertBack_DoesNotThrow()
        {
            ReverseBooleanToVisibilityConverter testObject = new();

            testObject.ConvertBack(null, null, null, null);
        }
    }
}
