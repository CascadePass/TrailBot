using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Xml.Serialization;

namespace CascadePass.TrailBot.Tests
{
    [TestClass]
    public class WtaTrailConditionTests
    {
        [TestMethod]
        public void CanCreateInstance()
        {
            _ = new WtaTrailCondition();
        }

        [TestMethod]
        public void Text_DoesNotSerialize()
        {
            string title = Guid.NewGuid().ToString(), description = Guid.NewGuid().ToString(), text = Guid.NewGuid().ToString();

            WtaTrailCondition testObject = new()
            {
                Title = title,
                Description = description,
                Text = text,
            };

            string xml = this.GetXmlFromTrailCondition(testObject);

            Console.WriteLine(xml);
            Assert.IsFalse(xml.Contains(text));
        }

        [TestMethod]
        public void Title_DoesSerialize()
        {
            string title = Guid.NewGuid().ToString(), description = Guid.NewGuid().ToString(), text = Guid.NewGuid().ToString();

            WtaTrailCondition testObject = new()
            {
                Title = title,
                Description = description,
                Text = text,
            };

            string xml = this.GetXmlFromTrailCondition(testObject);

            Console.WriteLine(xml);
            Assert.IsTrue(xml.Contains(title));
        }

        [TestMethod]
        public void Description_DoesSerialize()
        {
            string title = Guid.NewGuid().ToString(), description = Guid.NewGuid().ToString(), text = Guid.NewGuid().ToString();

            WtaTrailCondition testObject = new()
            {
                Title = title,
                Description = description,
                Text = text,
            };

            string xml = this.GetXmlFromTrailCondition(testObject);

            Console.WriteLine(xml);
            Assert.IsTrue(xml.Contains(description));
        }

        #region ToString

        [TestMethod]
        public void ToString_ReturnsTypeNameIfEmpty()
        {
            WtaTrailCondition testObject = new();

            string toString = testObject.ToString();

            Console.WriteLine(toString);
            Console.WriteLine(testObject.GetType().FullName);

            Assert.IsTrue(string.Equals(testObject.GetType().FullName, toString, StringComparison.Ordinal));
        }

        [TestMethod]
        public void ToString_ReturnsRawTextIfAny()
        {
            string title = Guid.NewGuid().ToString(), description = Guid.NewGuid().ToString(), text = Guid.NewGuid().ToString();

            WtaTrailCondition testObject = new()
            {
                Title = title,
                Description = description,
                Text = text,
            };

            string toString = testObject.ToString();

            Console.WriteLine(toString);

            Assert.IsTrue(string.Equals(text, toString, StringComparison.Ordinal));
        }

        [TestMethod]
        public void ToString_ReturnsTitleAndDescriptionIfNoRawText()
        {
            string title = Guid.NewGuid().ToString(), description = Guid.NewGuid().ToString();

            WtaTrailCondition testObject = new()
            {
                Title = title,
                Description = description,
            };

            string toString = testObject.ToString();

            Console.WriteLine(toString);

            Assert.IsTrue(string.Equals($"{title}: {description}", toString, StringComparison.Ordinal));
        }

        [TestMethod]
        public void ToString_ReturnsTitleIfNoRawTextOrDescription()
        {
            string title = Guid.NewGuid().ToString(), description = Guid.NewGuid().ToString();

            WtaTrailCondition testObject = new()
            {
                Title = title,
            };

            string toString = testObject.ToString();

            Console.WriteLine(toString);

            Assert.IsTrue(string.Equals(title, toString, StringComparison.Ordinal));
        }

        [TestMethod]
        public void ToString_ReturnsDescriptionIfOnlyDataPresent()
        {
            string description = Guid.NewGuid().ToString();

            WtaTrailCondition testObject = new()
            {
                Description = description,
            };

            string toString = testObject.ToString();

            Console.WriteLine(toString);

            Assert.IsTrue(string.Equals(description, toString, StringComparison.Ordinal));
        }

        #endregion

        private string GetXmlFromTrailCondition(WtaTrailCondition wtaTrailCondition)
        {
            XmlSerializer serializer = new(wtaTrailCondition.GetType());
            using MemoryStream memoryStream = new();
            using StreamWriter writer = new(memoryStream);

            serializer.Serialize(writer, wtaTrailCondition);
            writer.Flush();

            memoryStream.Position = 0;
            using StreamReader streamReader = new(memoryStream);

            string xml = streamReader.ReadToEnd();

            return xml;
        }
    }
}
