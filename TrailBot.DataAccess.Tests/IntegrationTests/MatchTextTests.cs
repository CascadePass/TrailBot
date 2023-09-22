using CascadePass.TrailBot.DataAccess;
using CascadePass.TrailBot.DataAccess.DTO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace TrailBot.DataAccess.Tests.IntegrationTests
{
    [TestClass]
    public class MatchTextTests : SqliteIntegrationTestClass
    {
        #region Constructor

        public MatchTextTests()
        {
            this.DatabaseFilename = "C:\\Users\\User\\Documents\\TrailBot\\TrailBot-test.db";
            Database.ConnectionString = $"Data Source={this.DatabaseFilename}";
        }

        #endregion

        [TestMethod]
        public void AddMatchText()
        {
            this.AssertRequirements();
            Database.QueryProvider = new SqliteQueryProvider();

            var matchText = this.GetRandomMatchText();

            Database.AddMatchText(matchText);

            // Was it actually saved?
            var validate = Database.GetMatchText(matchText.ID);
            Assert.IsNotNull(validate);

            this.AssertSameMatchText(matchText, validate);

            // Cleanup
            Database.DeleteMatchText(matchText.ID);
        }

        [TestMethod]
        public void UpdateMatchText()
        {
            this.AssertRequirements();
            Database.QueryProvider = new SqliteQueryProvider();

            MatchText
                original = this.GetRandomMatchText(),
                updatedValues = this.GetRandomMatchText();

            // Add some values to the database
            Database.AddMatchText(original);

            // Update those values to other random ones
            updatedValues.ID = original.ID;
            Database.UpdateMatchText(updatedValues);

            // See what's actually stored
            MatchText storedInTable = Database.GetMatchText(updatedValues.ID);

            Assert.IsNotNull(storedInTable, $"MatchText {updatedValues.ID} not found in database");
            this.AssertSameMatchText(updatedValues, storedInTable);

            // Cleanup
            Database.DeleteMatchText(updatedValues.ID);
        }

        [TestMethod]
        public void DeleteMatchText_ByID()
        {
            this.AssertRequirements();
            Database.QueryProvider = new SqliteQueryProvider();

            // Create a trip report to delete
            MatchText matchText = this.GetRandomMatchText();

            Database.AddMatchText(matchText);

            // Now delete it
            Database.DeleteMatchText(matchText.ID);

            // Make sure it's really been deleted
            // (no longer exists, can't be loaded by ID)
            var validate = Database.GetMatchText(matchText.ID);
            Assert.IsNull(validate);
        }

        [TestMethod]
        public void DeleteMatchText_ByDTO()
        {
            this.AssertRequirements();
            Database.QueryProvider = new SqliteQueryProvider();

            // Create a trip report to delete
            MatchText matchText = this.GetRandomMatchText();

            Database.AddMatchText(matchText);

            // Now delete it
            Database.DeleteMatchText(matchText);

            // Make sure it's really been deleted
            // (no longer exists, can't be loaded by ID)
            var validate = Database.GetMatchText(matchText.ID);
            Assert.IsNull(validate);
        }

        [TestMethod]
        public void GetMatchText()
        {
            this.AssertRequirements();
            Database.QueryProvider = new SqliteQueryProvider();

            var matchText = this.GetRandomMatchText();

            // Save, to be able to load it
            Database.AddMatchText(matchText);

            // Make sure it loads
            var validate = Database.GetMatchText(matchText.ID);
            Assert.IsNotNull(validate);

            // Make sure the loaded values are correct
            this.AssertSameMatchText(matchText, validate);

            // Cleanup
            Database.DeleteMatchText(matchText.ID);
        }

        [TestMethod]
        public void GetMatchText_ChildText()
        {
            this.AssertRequirements();
            Database.QueryProvider = new SqliteQueryProvider();

            MatchText
                parentText = this.GetRandomMatchText(),
                childText = this.GetRandomMatchText();

            // Save, to be able to load it
            Database.AddMatchText(parentText);

            childText.ParentID = parentText.ID;
            Database.AddMatchText(childText);

            // Make sure it loads
            var validate = Database.GetMatchText(childText.ID);
            Assert.IsNotNull(validate);

            // Make sure the loaded values are correct
            this.AssertSameMatchText(childText, validate);

            // Cleanup
            Database.DeleteMatchText(parentText.ID);
            Database.DeleteMatchText(childText.ID);
        }

        [TestMethod]
        public void GetMatchTextByTopic()
        {
            this.AssertRequirements();
            Database.QueryProvider = new SqliteQueryProvider();

            Topic topic = new() { Name = Guid.NewGuid().ToString() };
            Database.AddTopic(topic);

            Console.WriteLine($"Created topic {topic.ID}: {topic.Name}");

            List<MatchText> topicMatchText = new();
            for (int i = 0; i < 15; i++)
            {
                MatchText randomValue = new() { Text = Guid.NewGuid().ToString() };
                topicMatchText.Add(randomValue);
                Database.AddMatchText(randomValue);

                TopicText topicTextMapping = new()
                {
                    TopicID = topic.ID,
                    TextID = randomValue.ID,
                };

                Database.AddTopicText(topicTextMapping);
            }

            var perTopicListFromDatabase = Database.GetMatchTextByTopic(topic.ID);

            Console.WriteLine($"Generated {topicMatchText.Count} texts; loaded {perTopicListFromDatabase.Count} from database.");
            Assert.AreEqual(topicMatchText.Count, perTopicListFromDatabase.Count);

            string couldNotFind = null;
            foreach (var databaseItem in perTopicListFromDatabase)
            {
                var correspondingItem = topicMatchText.FirstOrDefault(m => m.Text == databaseItem.Text);

                if (correspondingItem == null)
                {
                    couldNotFind += databaseItem.Text + "\r\n";
                    continue;
                }

                // Make sure the loaded values are correct
                this.AssertSameMatchText(correspondingItem, databaseItem);

                // Cleanup
                Database.DeleteMatchText(databaseItem.ID);
            }

            Database.DeleteTopic(topic.ID);

            if (!string.IsNullOrWhiteSpace(couldNotFind))
            {
                Assert.Inconclusive($"Could not find the following items: {couldNotFind}");
            }
        }

        #region Private utility methods

        private MatchText GetRandomMatchText()
        {
            return this.GetRandomMatchText(0);
        }

        private MatchText GetRandomMatchText(int id)
        {
            Random random = new();

            return new()
            {
                ID = id,
                Text = Guid.NewGuid().ToString(),
                ParentID = random.Next(0, 1) == 0 ? null : random.Next(1, int.MaxValue),
            };
        }

        private void AssertSameMatchText(MatchText matchText1, MatchText matchText2)
        {
            Assert.AreEqual(matchText1.ParentID, matchText2.ParentID);
            Assert.AreEqual(matchText1.Text, matchText2.Text);
        }

        #endregion
    }
}
