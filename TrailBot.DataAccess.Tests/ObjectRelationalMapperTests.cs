using CascadePass.TrailBot.DataAccess;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrailBot.DataAccess.Tests
{
    [TestClass]
    public class ObjectRelationalMapperTests
    {
        [ExpectedException(typeof(ArgumentNullException))]
        [TestMethod]
        public void GetUrl_null()
        {
            ObjectRelationalMapper.GetUrl(null);
        }

        [ExpectedException(typeof(ArgumentNullException))]
        [TestMethod]
        public void GetWtaTripReport_null()
        {
            ObjectRelationalMapper.GetWtaTripReport(null);
        }

        [ExpectedException(typeof(ArgumentNullException))]
        [TestMethod]
        public void GetTopic_null()
        {
            ObjectRelationalMapper.GetTopic(null);
        }

        [ExpectedException(typeof(ArgumentNullException))]
        [TestMethod]
        public void GetMatchText_null()
        {
            ObjectRelationalMapper.GetMatchText(null);
        }

        [ExpectedException(typeof(ArgumentNullException))]
        [TestMethod]
        public void GetTopicText_null()
        {
            ObjectRelationalMapper.GetTopicText(null);
        }

        [ExpectedException(typeof(ArgumentNullException))]
        [TestMethod]
        public void GetProvider_null()
        {
            ObjectRelationalMapper.GetProvider(null);
        }

        [ExpectedException(typeof(ArgumentNullException))]
        [TestMethod]
        public void GetProviderStatistics_null()
        {
            ObjectRelationalMapper.GetProviderStatistics(null);
        }

        [ExpectedException(typeof(ArgumentNullException))]
        [TestMethod]
        public void GetMatchedTripReportText_null()
        {
            ObjectRelationalMapper.GetMatchedTripReportText(null);
        }

        [ExpectedException(typeof(ArgumentNullException))]
        [TestMethod]
        public void GetMatchedTripReportTopic_null()
        {
            ObjectRelationalMapper.GetMatchedTripReportTopic(null);
        }
    }
}
