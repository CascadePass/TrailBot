using CascadePass.TrailBot.DataAccess;
using CascadePass.TrailBot.DataAccess.DTO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace TrailBot.DataAccess.Tests.IntegrationTests
{
    [TestClass]
    public class TopicTextTests : SqliteIntegrationTestClass
    {
        [TestMethod]
        public void AddTopicText()
        {
            this.AssertRequirements();
            Database.QueryProvider = new SqliteQueryProvider();

            var topicText = this.GetRandomTopicText();

            Database.Add(topicText);

            // Was it actually saved?
            var validate = Database.GetTopicText(topicText.ID);
            Assert.IsNotNull(validate);

            this.AssertSameTopicText(topicText, validate);

            // Cleanup
            Database.DeleteTopicText(topicText.ID);
        }

        [TestMethod]
        public void UpdateTopicText()
        {
            this.AssertRequirements();
            Database.QueryProvider = new SqliteQueryProvider();

            TopicText
                original = this.GetRandomTopicText(),
                updatedValues = this.GetRandomTopicText();

            // Add some values to the database
            Database.Add(original);

            // Update those values to other random ones
            updatedValues.ID = original.ID;
            Database.Update(updatedValues);

            // See what's actually stored
            TopicText storedInTable = Database.GetTopicText(updatedValues.ID);

            Assert.IsNotNull(storedInTable, $"TopicText {updatedValues.ID} not found in database");
            this.AssertSameTopicText(updatedValues, storedInTable);

            // Cleanup
            Database.DeleteTopicText(updatedValues.ID);
        }

        [TestMethod]
        public void DeleteTopicText_ByID()
        {
            this.AssertRequirements();
            Database.QueryProvider = new SqliteQueryProvider();

            // Create a trip report to delete
            TopicText topicText = this.GetRandomTopicText();

            Database.Add(topicText);

            // Now delete it
            Database.DeleteTopicText(topicText.ID);

            // Make sure it's really been deleted
            // (no longer exists, can't be loaded by ID)
            var validate = Database.GetTopicText(topicText.ID);
            Assert.IsNull(validate);
        }

        [TestMethod]
        public void DeleteTopicText_ByDTO()
        {
            this.AssertRequirements();
            Database.QueryProvider = new SqliteQueryProvider();

            // Create a trip report to delete
            TopicText topicText = this.GetRandomTopicText();

            Database.Add(topicText);

            // Now delete it
            Database.Delete(topicText);

            // Make sure it's really been deleted
            // (no longer exists, can't be loaded by ID)
            var validate = Database.GetTopicText(topicText.ID);
            Assert.IsNull(validate);
        }

        [TestMethod]
        public void GetTopicText()
        {
            this.AssertRequirements();
            Database.QueryProvider = new SqliteQueryProvider();

            var topicText = this.GetRandomTopicText();

            // Save, to be able to load it
            Database.Add(topicText);

            // Make sure it loads
            var validate = Database.GetTopicText(topicText.ID);
            Assert.IsNotNull(validate);

            // Make sure the loaded values are correct
            this.AssertSameTopicText(topicText, validate);

            // Cleanup
            Database.DeleteTopicText(topicText.ID);
        }

        [TestMethod]
        public void GetTopicTextByTopic()
        {
            this.AssertRequirements();
            Database.QueryProvider = new SqliteQueryProvider();

            Topic topic = new() { Name = Guid.NewGuid().ToString() };
            Database.Add(topic);

            Random random = new();
            List<TopicText> topicMatchText = new();
            for (int i = 0; i < random.Next(5, 50); i++)
            {
                var text = this.GetRandomTopicText();
                text.TopicID = topic.ID;

                topicMatchText.Add(text);
                Database.Add(text);
            }

            Console.WriteLine($"Created {topicMatchText.Count} test items.");

            // Make sure it loads
            var validate = Database.GetTopicTextByTopic(topic.ID);
            Assert.IsNotNull(validate);

            Console.WriteLine($"Found {validate.Count} items in database for topic {topic.ID}.");
            Console.WriteLine();

            foreach (var text in topicMatchText)
            {
                Assert.IsTrue(validate.Any(m => m.ID == text.ID));

                var correspondingText = validate.FirstOrDefault(m => m.ID == text.ID);

                Console.WriteLine($"Validating ID={text.ID} TextID={text.TextID}.");

                // Make sure the loaded values are correct
                this.AssertSameTopicText(text, correspondingText);
            }

            // Cleanup
            Database.DeleteTopicText(topic.ID);
        }

        #region Private utility methods

        private TopicText GetRandomTopicText()
        {
            return this.GetRandomTopicText(0);
        }

        private TopicText GetRandomTopicText(long id)
        {
            Random random = new();

            return new()
            {
                ID = id,
                TopicID = random.Next(1, 100),
                TextID = random.Next(1, 100),
            };
        }

        private void AssertSameTopicText(TopicText topicText1, TopicText topicText2)
        {
            Assert.AreEqual(topicText1.ID, topicText2.ID);
            Assert.AreEqual(topicText1.TopicID, topicText2.TopicID);
            Assert.AreEqual(topicText1.TextID, topicText2.TextID);
        }

        #endregion
    }
}
