using CascadePass.TrailBot.DataAccess.DTO;
using System;
using System.Data.Common;

namespace CascadePass.TrailBot.DataAccess
{
    public interface ITrailBotQueryProvider
    {
        DbCommand AddMatchedTripReportText(long tripReportID, long textID);
        DbCommand AddMatchedTripReportText(MatchedTripReportText matchDetails);
        DbCommand AddMatchedTripReportText(WtaTripReport tripReport, MatchText text);
        DbCommand AddMatchedTripReportTopic(long tripReportID, long topicID, string exerpt);
        DbCommand AddMatchedTripReportTopic(MatchedTripReportTopic matchDetails);
        DbCommand AddMatchedTripReportTopic(WtaTripReport tripReport, Topic topic, string exerpt);
        DbCommand AddMatchText(MatchText matchText);
        DbCommand AddProvider(Provider provider);
        DbCommand AddProviderStatistics(ProviderStatistics statistics);
        DbCommand AddTopic(Topic topic);
        DbCommand AddTopicText(TopicText topicText);
        DbCommand AddUrl(Url url);
        DbCommand AddWtaTripReport(WtaTripReport tripReport);
        DbCommand DeleteMatchedTripReportText(long id);
        DbCommand DeleteMatchedTripReportText(MatchedTripReportText matchDetails);
        DbCommand DeleteMatchedTripReportTopic(long id);
        DbCommand DeleteMatchedTripReportTopic(MatchedTripReportTopic matchDetails);
        DbCommand DeleteMatchText(long id);
        DbCommand DeleteMatchText(MatchText matchText);
        DbCommand DeleteProvider(long id);
        DbCommand DeleteProvider(Provider provider);
        DbCommand DeleteProviderStatistics(long id);
        DbCommand DeleteProviderStatistics(ProviderStatistics statistics);
        DbCommand DeleteTopic(long id);
        DbCommand DeleteTopic(Topic topic);
        DbCommand DeleteTopicText(long id);
        DbCommand DeleteTopicText(TopicText topicText);
        DbCommand DeleteUrl(long id);
        DbCommand DeleteUrl(Url url);
        DbCommand DeleteWtaTripReport(long id);
        DbCommand DeleteWtaTripReport(WtaTripReport tripReport);
        DbCommand GetMatchedTripReportText(long id);
        DbCommand GetMatchedTripReportTopic(long id);
        DbCommand GetMatchText(long id);
        DbCommand GetMatchTextByTopic(long topicID);
        DbCommand GetProvider(long id);
        DbCommand GetProviders();
        DbCommand GetProviderStatistics(long id);
        DbCommand GetTopic(long id);
        DbCommand GetTopics();
        DbCommand GetTopicText(long id);
        DbCommand GetTopicTextByTopic(long topicID);
        DbCommand GetUrl(long id);
        DbCommand GetUrl(string url);
        DbCommand GetWtaTripReport(long id);
        DbCommand IncrementProviderStatistics(long id, long? additionalRequests, long? additionalFailedRequests, long? additionalMatches, long? additionalSleepMS);
        DbCommand UpdateMatchText(long id, long? falseParentID, string text);
        DbCommand UpdateMatchText(long id, string text);
        DbCommand UpdateMatchText(MatchText matchText);
        DbCommand UpdateProvider(Provider provider);
        DbCommand UpdateProviderStatistics(ProviderStatistics statistics);
        DbCommand UpdateTopic(long id, string name);
        DbCommand UpdateTopic(Topic topic);
        DbCommand UpdateTopicText(long topicTextID, long topicID, long textID);
        DbCommand UpdateTopicText(TopicText topicText);
        DbCommand UpdateUrl(long id, DateTime? collected, DateTime? intentLocked);
        DbCommand UpdateUrl(Url url);
        DbCommand UpdateWtaTripReport(WtaTripReport tripReport);
    }
}