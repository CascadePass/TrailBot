using CascadePass.TrailBot.DataAccess;
using CascadePass.TrailBot.DataAccess.DTO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace TrailBot.DataAccess.Tests.IntegrationTests
{
    [TestClass]
    public class TopicTests : SqliteIntegrationTestClass
    {
        [TestMethod]
        public void AddTopic()
        {
            this.AssertRequirements();
            Database.QueryProvider = new SqliteQueryProvider();

            var topic = this.GetRandomTopic();

            Database.Add(topic);

            // Was it actually saved?
            var validate = Database.GetTopic(topic.ID);
            Assert.IsNotNull(validate);

            this.AssertSameTopic(topic, validate);

            // Cleanup
            Database.DeleteTopic(topic.ID);
        }

        [TestMethod]
        public void UpdateTopic()
        {
            this.AssertRequirements();
            Database.QueryProvider = new SqliteQueryProvider();

            Topic
                original = this.GetRandomTopic(),
                updatedValues = this.GetRandomTopic();

            // Add some values to the database
            Database.Add(original);

            // Update those values to other random ones
            updatedValues.ID = original.ID;
            Database.Update(updatedValues);

            // See what's actually stored
            Topic storedInTable = Database.GetTopic(updatedValues.ID);

            Assert.IsNotNull(storedInTable, $"Topic {updatedValues.ID} not found in database");
            this.AssertSameTopic(updatedValues, storedInTable);

            // Cleanup
            Database.DeleteTopic(updatedValues.ID);
        }

        [TestMethod]
        public void DeleteTopic_ByID()
        {
            this.AssertRequirements();
            Database.QueryProvider = new SqliteQueryProvider();

            // Create a trip report to delete
            Topic topic = this.GetRandomTopic();

            Database.Add(topic);

            // Now delete it
            Database.DeleteTopic(topic.ID);

            // Make sure it's really been deleted
            // (no longer exists, can't be loaded by ID)
            var validate = Database.GetTopic(topic.ID);
            Assert.IsNull(validate);
        }

        [TestMethod]
        public void DeleteTopic_ByDTO()
        {
            this.AssertRequirements();
            Database.QueryProvider = new SqliteQueryProvider();

            // Create a trip report to delete
            Topic topic = this.GetRandomTopic();

            Database.Add(topic);

            // Now delete it
            Database.Delete(topic);

            // Make sure it's really been deleted
            // (no longer exists, can't be loaded by ID)
            var validate = Database.GetTopic(topic.ID);
            Assert.IsNull(validate);
        }

        [TestMethod]
        public void GetTopic()
        {
            this.AssertRequirements();
            Database.QueryProvider = new SqliteQueryProvider();

            var topic = this.GetRandomTopic();

            // Save, to be able to load it
            Database.Add(topic);

            // Make sure it loads
            var validate = Database.GetTopic(topic.ID);
            Assert.IsNotNull(validate);

            // Make sure the loaded values are correct
            this.AssertSameTopic(topic, validate);

            // Cleanup
            Database.DeleteTopic(topic.ID);
        }

        [TestMethod]
        public void GetTopics()
        {
            this.AssertRequirements();
            Database.QueryProvider = new SqliteQueryProvider();

            List<Topic> topics = new();

            for (int i = 0; i < 10; i++)
            {
                var topic = this.GetRandomTopic();

                Database.Add(topic);
                topics.Add(topic);
            }

            var all = Database.GetTopics();
            Assert.IsNotNull(all);
            Console.WriteLine($"{all.Count} topics found");

            foreach (Topic lookup in topics)
            {
                Assert.IsTrue(all.Any(m => m.ID == lookup.ID && m.Name == lookup.Name), $"{lookup.ID} {lookup.Name} missing");
            }

            foreach (Topic lookup in topics)
            {
                Database.Delete(lookup);
            }
        }

        #region Private utility methods

        private Topic GetRandomTopic()
        {
            return this.GetRandomTopic(0);
        }

        private Topic GetRandomTopic(int id)
        {
            Random random = new();
            List<TopicText> matchText = new();

            for (int i = 0; i < random.Next(1, 10); i++)
            {
                matchText.Add(new() { ID = id });
            }

            return new()
            {
                ID = id,
                Name = Guid.NewGuid().ToString(),
            };
        }

        private void AssertSameTopic(Topic topic1, Topic topic2)
        {
            Assert.AreEqual(topic1.ID, topic2.ID);
            Assert.AreEqual(topic1.Name, topic2.Name);

            //if (topic1.MatchText != null && topic2.MatchText == null)
            //{
            //    Assert.Fail($"Topic1.MatchText has {topic1.MatchText.Count} items; Topic2.MatchText is null");
            //}

            //if (topic2.MatchText != null && topic1.MatchText == null)
            //{
            //    Assert.Fail($"Topic2.MatchText has {topic2.MatchText.Count} items; Topic1.MatchText is null");
            //}

            //Assert.AreEqual(topic1.MatchText.Count, topic2.MatchText.Count, "Topic.MatchText counts don't agree");

            //TODO: Implement list comparison
        }

        #endregion
    }
}
