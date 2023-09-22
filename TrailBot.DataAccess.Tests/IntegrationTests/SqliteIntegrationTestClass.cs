using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;

namespace TrailBot.DataAccess.Tests.IntegrationTests
{
    public class SqliteIntegrationTestClass
    {
        public string DatabaseFilename { get; set; }

        internal void AssertDatabaseExistence()
        {
            if (string.IsNullOrWhiteSpace(this.DatabaseFilename))
            {
                Assert.Inconclusive("DatabaseFilename is missing.");
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
