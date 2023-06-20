using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CascadePass.TrailBot.Tests
{
    [TestClass]
    public class MatchingTripReportEventArgsTests
    {
        [TestMethod]
        public void CanCreateInstance()
        {
            _ = new MatchingTripReportEventArgs();
        }

        [TestMethod]
        public void MatchedTripReport_GetSetAccessSameValue()
        {
            MatchingTripReportEventArgs e = new();
            MatchedTripReport matchedTripReport = new();

            e.MatchedTripReport = matchedTripReport;
            Assert.AreSame(matchedTripReport, e.MatchedTripReport);
        }
    }
}
