using CascadePass.TrailBot.DataAccess.DTO;
using System;
using System.Data.Common;

namespace CascadePass.TrailBot.DataAccess
{
    public static class ObjectRelationalMapper
    {
        public static Url GetUrl(DbDataReader dataReader)
        {
            #region Sanity checks

            if (dataReader == null)
            {
                throw new ArgumentNullException(nameof(dataReader));
            }

            #endregion

            Url url = new();

            url.ID = dataReader.GetInt64(dataReader.GetOrdinal("UrlID"));
            url.Address = dataReader.GetString(dataReader.GetOrdinal("Url"));
            url.Found = DataTransferObject.ToDateTime(dataReader.GetInt64(dataReader.GetOrdinal("FoundDate")));
            url.Collected = dataReader.IsDBNull(dataReader.GetOrdinal("CollectedDate")) ? null : DataTransferObject.ToDateTime(dataReader.GetInt64(dataReader.GetOrdinal("CollectedDate")));
            url.IntentLocked = dataReader.IsDBNull(dataReader.GetOrdinal("IntentLockedDate")) ? null : DataTransferObject.ToDateTime(dataReader.GetInt64(dataReader.GetOrdinal("IntentLockedDate")));

            return url;
        }

        public static WtaTripReport GetWtaTripReport(DbDataReader dataReader)
        {
            #region Sanity checks

            if (dataReader == null)
            {
                throw new ArgumentNullException(nameof(dataReader));
            }

            #endregion

            WtaTripReport tripReport = new();

            tripReport.Title = dataReader.GetString(dataReader.GetOrdinal("Title"));
            tripReport.Region = dataReader.GetString(dataReader.GetOrdinal("Region"));
            tripReport.HikeType = dataReader.GetString(dataReader.GetOrdinal("HikeType"));
            tripReport.Author = dataReader.GetString(dataReader.GetOrdinal("Author"));
            tripReport.ReportText = dataReader.GetString(dataReader.GetOrdinal("ReportText"));
            tripReport.TripDate = DataTransferObject.ToDateTime(dataReader.GetInt64(dataReader.GetOrdinal("TripDate")));
            tripReport.PublishedDate = DataTransferObject.ToDateTime(dataReader.GetInt64(dataReader.GetOrdinal("PublishedDate")));
            tripReport.ProcessedDate = DataTransferObject.ToDateTime(dataReader.GetInt64(dataReader.GetOrdinal("ProcessedDate")));

            tripReport.Url = ObjectRelationalMapper.GetUrl(dataReader);

            return tripReport;
        }

        public static Topic GetTopic(DbDataReader dataReader)
        {
            #region Sanity checks

            if (dataReader == null)
            {
                throw new ArgumentNullException(nameof(dataReader));
            }

            #endregion

            Topic topic = new();

            topic.ID = dataReader.GetInt64(dataReader.GetOrdinal("TopicID"));
            topic.Name = dataReader.GetString(dataReader.GetOrdinal("Name"));

            return topic;
        }

        public static MatchText GetMatchText(DbDataReader dataReader)
        {
            #region Sanity checks

            if (dataReader == null)
            {
                throw new ArgumentNullException(nameof(dataReader));
            }

            #endregion

            MatchText matchText = new();

            matchText.ID = dataReader.GetInt64(dataReader.GetOrdinal("MatchTextID"));
            matchText.Text = dataReader.GetString(dataReader.GetOrdinal("Text"));

            if (!dataReader.IsDBNull(dataReader.GetOrdinal("FalseMatchParentID")))
            {
                matchText.ParentID = dataReader.GetInt64(dataReader.GetOrdinal("FalseMatchParentID"));
            }

            return matchText;
        }

        public static TopicText GetTopicText(DbDataReader dataReader)
        {
            #region Sanity checks

            if (dataReader == null)
            {
                throw new ArgumentNullException(nameof(dataReader));
            }

            #endregion

            TopicText topicText = new();

            topicText.ID = dataReader.GetInt64(dataReader.GetOrdinal("TopicTextID"));
            topicText.TopicID = dataReader.GetInt64(dataReader.GetOrdinal("TopicID"));
            topicText.TextID = dataReader.GetInt64(dataReader.GetOrdinal("MatchTextID"));

            return topicText;
        }

        public static Provider GetProvider(DbDataReader dataReader)
        {
            #region Sanity checks

            if (dataReader == null)
            {
                throw new ArgumentNullException(nameof(dataReader));
            }

            #endregion

            Provider provider = new();

            provider.ID = dataReader.GetInt64(dataReader.GetOrdinal("ProviderID"));
            provider.Name = dataReader.GetString(dataReader.GetOrdinal("Name"));
            provider.TypeName = dataReader.GetString(dataReader.GetOrdinal("TypeName"));
            provider.PreservationRule = (int)dataReader.GetInt64(dataReader.GetOrdinal("PreservationRule"));
            provider.State = (int)dataReader.GetInt64(dataReader.GetOrdinal("State"));
            provider.Browser = (int)dataReader.GetInt64(dataReader.GetOrdinal("Browser"));
            provider.LastTripReportRequest = dataReader.IsDBNull(dataReader.GetOrdinal("LastTripReportRequest")) ? null : DataTransferObject.ToDateTime(dataReader.GetInt64(dataReader.GetOrdinal("LastTripReportRequest")));
            provider.LastGetRecentRequest = dataReader.IsDBNull(dataReader.GetOrdinal("LastGetRecentRequest")) ? null : DataTransferObject.ToDateTime(dataReader.GetInt64(dataReader.GetOrdinal("LastGetRecentRequest")));
            provider.ProviderXml = dataReader.GetString(dataReader.GetOrdinal("ProviderXml"));

            return provider;
        }

        public static ProviderStatistics GetProviderStatistics(DbDataReader dataReader)
        {
            #region Sanity checks

            if (dataReader == null)
            {
                throw new ArgumentNullException(nameof(dataReader));
            }

            #endregion

            ProviderStatistics statistics = new();

            statistics.ID = dataReader.GetInt64(dataReader.GetOrdinal("ProviderID"));
            statistics.TotalRequestsMade = dataReader.GetInt64(dataReader.GetOrdinal("RequestsMade"));
            statistics.FailedRequests = dataReader.GetInt64(dataReader.GetOrdinal("FailedRequests"));
            statistics.MatchesFound = dataReader.GetInt64(dataReader.GetOrdinal("MatchesFound"));
            statistics.SleepTimeInMS = dataReader.GetInt64(dataReader.GetOrdinal("SleepTimeInMS"));

            return statistics;
        }

        public static MatchedTripReportText GetMatchedTripReportText(DbDataReader dataReader)
        {
            #region Sanity checks

            if (dataReader == null)
            {
                throw new ArgumentNullException(nameof(dataReader));
            }

            #endregion

            MatchedTripReportText matchDetails = new();

            matchDetails.ID = dataReader.GetInt64(dataReader.GetOrdinal("MatchID"));
            matchDetails.TripReportID = dataReader.GetInt64(dataReader.GetOrdinal("TripReportID"));
            matchDetails.TextID = dataReader.GetInt64(dataReader.GetOrdinal("MatchTextID"));

            return matchDetails;
        }

        public static MatchedTripReportTopic GetMatchedTripReportTopic(DbDataReader dataReader)
        {
            #region Sanity checks

            if (dataReader == null)
            {
                throw new ArgumentNullException(nameof(dataReader));
            }

            #endregion

            MatchedTripReportTopic matchDetails = new();

            matchDetails.ID = dataReader.GetInt64(dataReader.GetOrdinal("MatchID"));
            matchDetails.TripReportID = dataReader.GetInt64(dataReader.GetOrdinal("TripReportID"));
            matchDetails.TopicID = dataReader.GetInt64(dataReader.GetOrdinal("TopicID"));
            matchDetails.Exerpt = dataReader.GetString(dataReader.GetOrdinal("Exerpt"));

            return matchDetails;
        }
    }
}
