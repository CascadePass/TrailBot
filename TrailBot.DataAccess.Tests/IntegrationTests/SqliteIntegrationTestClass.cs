using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;

namespace TrailBot.DataAccess.Tests.IntegrationTests
{
    public class SqliteIntegrationTestClass
    {
        private const string CONFIG_FILENAME = "test.config";

        public string DatabaseFilename { get; set; }

        public DatabaseTestConfiguration ReadConfig()
        {
            if (File.Exists("test.config"))
            {
                XmlSerializer xmlSerializer = new();

                using FileStream fileStream = new("test.config", FileMode.Open);
                var config = (DatabaseTestConfiguration)xmlSerializer.Deserialize(fileStream);

                this.DatabaseFilename = config.DatabaseFilename;

                return config;
            }

            return null;
        }

        internal void AssertDatabaseExistence()
        {
            // It hasn't been set yet
            if (string.IsNullOrWhiteSpace(this.DatabaseFilename))
            {
                this.ReadConfig();
            }

            // If it's still null, the config is broken.
            if (string.IsNullOrWhiteSpace(this.DatabaseFilename))
            {
                Assert.Inconclusive($"DatabaseFilename is missing in {SqliteIntegrationTestClass.CONFIG_FILENAME}");
            }

            if (!File.Exists(this.DatabaseFilename))
            {
                Assert.Inconclusive($"Database file '{this.DatabaseFilename}' does not exist.");
            }
        }

        internal void AssertRequirements()
        {
            this.AssertDatabaseExistence();
        }

        internal bool AreDatesSame(DateTime dateTime1, DateTime dateTime2)
        {
            if (dateTime1.Date != dateTime2.Date)
            {
                return false;
            }

            if ((int)dateTime1.Date.TimeOfDay.TotalSeconds != (int)dateTime2.Date.TimeOfDay.TotalSeconds)
            {
                return false;
            }

            return true;
        }

        internal bool AreDatesSame(DateTime? dateTime1, DateTime? dateTime2)
        {
            if (dateTime1.HasValue && !dateTime2.HasValue)
            {
                return false;
            }

            if (dateTime2.HasValue && !dateTime1.HasValue)
            {
                return false;
            }

            if (!dateTime1.HasValue && !dateTime2.HasValue)
            {
                // Otherwise there will be null reference exceptions below
                return true;
            }

            if (dateTime1.Value.Date != dateTime2.Value.Date)
            {
                return false;
            }

            if ((int)dateTime1.Value.Date.TimeOfDay.TotalSeconds != (int)dateTime2.Value.Date.TimeOfDay.TotalSeconds)
            {
                return false;
            }

            return true;
        }

        internal DateTime GetRandomDateTime()
        {
            Random random = new();

            return new(
                random.Next(1970, DateTime.Now.Year),
                random.Next(1, 12),
                random.Next(1, 28),  // I don't know what month was generated
                random.Next(0, 23),
                random.Next(0, 59),
                random.Next(0, 59)
            );
        }
    }
}
