using CascadePass.TrailBot.DataAccess;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TrailBot.DataAccess.Tests.SqliteQueryProviderTests
{
    [TestClass]
    public class QueryProviderTests
    {
        [TestMethod]
        public void CanCreateInstance()
        {
            SqliteQueryProvider queryProvider = new();
        }
    }
}
