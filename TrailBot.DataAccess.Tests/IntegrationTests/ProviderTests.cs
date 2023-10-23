using CascadePass.TrailBot.DataAccess;
using CascadePass.TrailBot.DataAccess.DTO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace TrailBot.DataAccess.Tests.IntegrationTests
{
    [TestClass]
    public class ProviderTests : SqliteIntegrationTestClass
    {
        #region Constructor

        public ProviderTests()
        {
            this.DatabaseFilename = "C:\\Users\\User\\Documents\\TrailBot\\TrailBot-test.db";
            Database.ConnectionString = $"Data Source={this.DatabaseFilename}";
        }

        #endregion

        [TestMethod]
        public void AddProvider()
        {
            this.AssertRequirements();
            Database.QueryProvider = new SqliteQueryProvider();

            var provider = this.GetRandomProvider();

            Database.Add(provider);

            // Was it actually saved?
            var validate = Database.GetProvider(provider.ID);
            Assert.IsNotNull(validate);

            this.AssertSameProvider(provider, validate);

            // Cleanup
            Database.DeleteProvider(provider.ID);
        }

        [TestMethod]
        public void AddProvider_NullDates()
        {
            this.AssertRequirements();
            Database.QueryProvider = new SqliteQueryProvider();

            var provider = this.GetRandomProvider();

            provider.LastTripReportRequest = null;
            provider.LastGetRecentRequest = null;

            Database.Add(provider);

            // Was it actually saved?
            var validate = Database.GetProvider(provider.ID);
            Assert.IsNotNull(validate);

            this.AssertSameProvider(provider, validate);

            // Cleanup
            Database.DeleteProvider(provider.ID);
        }

        [TestMethod]
        public void UpdateProvider()
        {
            this.AssertRequirements();
            Database.QueryProvider = new SqliteQueryProvider();

            Provider
                original = this.GetRandomProvider(),
                updatedValues = this.GetRandomProvider();

            // Add some values to the database
            Database.Add(original);

            // Update those values to other random ones
            updatedValues.ID = original.ID;
            Database.Update(updatedValues);

            // See what's actually stored
            Provider storedInTable = Database.GetProvider(updatedValues.ID);

            Assert.IsNotNull(storedInTable, $"Provider {updatedValues.ID} not found in database");
            this.AssertSameProvider(updatedValues, storedInTable);

            // Cleanup
            Database.DeleteProvider(updatedValues.ID);
        }

        [TestMethod]
        public void DeleteProvider_ByID()
        {
            this.AssertRequirements();
            Database.QueryProvider = new SqliteQueryProvider();

            // Create a trip report to delete
            Provider provider = this.GetRandomProvider();

            var addRowCount = Database.Add(provider);
            Console.WriteLine($"Added {addRowCount} provider rows.");

            if (addRowCount == 0)
            {
                Assert.Inconclusive("Unable to add a provider.");
            }

            // Now delete it
            Database.DeleteProvider(provider.ID);

            // Make sure it's really been deleted
            // (no longer exists, can't be loaded by ID)
            var validate = Database.GetProvider(provider.ID);
            Assert.IsNull(validate);
        }

        [TestMethod]
        public void DeleteProvider_ByDTO()
        {
            this.AssertRequirements();
            Database.QueryProvider = new SqliteQueryProvider();

            // Create a trip report to delete
            Provider provider = this.GetRandomProvider();

            var addRowCount = Database.Add(provider);
            Console.WriteLine($"Added {addRowCount} provider rows.");

            if (addRowCount == 0)
            {
                Assert.Inconclusive("Unable to add a provider.");
            }

            // Now delete it
            Database.Delete(provider);

            // Make sure it's really been deleted
            // (no longer exists, can't be loaded by ID)
            var validate = Database.GetProvider(provider.ID);
            Assert.IsNull(validate);
        }

        [TestMethod]
        public void GetProvider()
        {
            this.AssertRequirements();
            Database.QueryProvider = new SqliteQueryProvider();

            var provider = this.GetRandomProvider();

            // Save, to be able to load it
            Database.Add(provider);

            // Make sure it loads
            var validate = Database.GetProvider(provider.ID);
            Assert.IsNotNull(validate);

            // Make sure the loaded values are correct
            this.AssertSameProvider(provider, validate);

            // Cleanup
            Database.DeleteProvider(provider.ID);
        }

        [TestMethod]
        public void GetProviders()
        {
            this.AssertRequirements();
            Database.QueryProvider = new SqliteQueryProvider();

            List<Provider> generatedProviders = new();

            for (int i = 0; i < 25; i++)
            {
                var provider = this.GetRandomProvider();
                generatedProviders.Add(provider);

                // Save, to be able to load it
                Database.Add(provider);
            }

            var allDatabaseProviders = Database.GetProviders();

            string countMessage = $"Generated {generatedProviders.Count} providers, found {allDatabaseProviders.Count} in the database";
            Console.WriteLine(countMessage);
            Assert.IsTrue(allDatabaseProviders.Count >= generatedProviders.Count, countMessage);

            foreach (var generated in generatedProviders)
            {
                Console.WriteLine($"Validating Name={generated.Name}");
                var associated = generatedProviders.FirstOrDefault(m => m.Name == generated.Name);
                Assert.IsNotNull(associated, $"No match found for ID={generated.ID}, Name={generated.Name}");

                // Make sure the loaded values are correct
                this.AssertSameProvider(associated, generated);

                // Cleanup
                Database.DeleteProvider(generated.ID);
            }
        }

        #region Private utility methods

        private Provider GetRandomProvider()
        {
            return this.GetRandomProvider(0);
        }

        private Provider GetRandomProvider(int id)
        {
            Random random = new();

            return new()
            {
                ID = id,
                Name = Guid.NewGuid().ToString(),
                TypeName = Guid.NewGuid().ToString(),
                PreservationRule = random.Next(0, 10),
                State = random.Next(0, 10),
                Browser = random.Next(0, 10),
                LastTripReportRequest = this.GetRandomDateTime(),
                LastGetRecentRequest = this.GetRandomDateTime(),
                ProviderXml = Guid.NewGuid().ToString(),
            };
        }

        private void AssertSameProvider(Provider provider1, Provider provider2)
        {
            Assert.AreEqual(provider1.ID, provider2.ID, "ID doesn't match");
            Assert.AreEqual(provider1.Name, provider2.Name, "Name doesn't match");
            Assert.AreEqual(provider1.TypeName, provider2.TypeName, "TypeName doesn't match");
            Assert.AreEqual(provider1.PreservationRule, provider2.PreservationRule, "PreservationRule doesn't match");
            Assert.AreEqual(provider1.State, provider2.State, "State doesn't match");
            Assert.AreEqual(provider1.Browser, provider2.Browser, "Browser doesn't match");
            Assert.AreEqual(provider1.ProviderXml, provider2.ProviderXml, "ProviderXml doesn't match");
            Assert.IsTrue(this.AreDatesSame(provider1.LastTripReportRequest, provider2.LastTripReportRequest));
            Assert.IsTrue(this.AreDatesSame(provider1.LastGetRecentRequest, provider2.LastGetRecentRequest));
        }

        #endregion
    }
}
