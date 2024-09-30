//using CascadePass.TrailBot;
//using CascadePass.TrailBot.DataAccess;
//using CascadePass.TrailBot.DataAccess.DTO;
//using System;
//using System.Collections.Generic;
//using System.Data.Common;
//using static System.Net.Mime.MediaTypeNames;

//namespace TrailBot.DataAccess.Tests
//{
//    public class LoggingMockQueryProvider : ITrailBotQueryProvider
//    {
//        public LoggingMockQueryProvider()
//        {
//            this.MethodCalls = new();
//        }

//        public List<string> MethodCalls { get; private set; }

//        private DbCommand GetMethodReturnCall(string methodName)
//        {
//            return null;
//        }

//        public DbCommand AddImageUrl(ImageUrl imageUrl)
//        {
//            string methodName = nameof(this.AddImageUrl);
//            this.MethodCalls.Add($"{methodName}({imageUrl})");

//            return this.GetMethodReturnCall(methodName);
//        }

//        public DbCommand AddImageUrl(string imageUrl)
//        {
//            string methodName = nameof(this.AddImageUrl);
//            this.MethodCalls.Add($"{methodName}({imageUrl})");

//            return this.GetMethodReturnCall(methodName);
//        }

//        public DbCommand AddMatchedTripReportText(long tripReportID, long textID)
//        {
//            string methodName = nameof(this.AddMatchedTripReportText);
//            this.MethodCalls.Add($"{methodName}({tripReportID}, {textID})");

//            return this.GetMethodReturnCall(methodName);
//        }

//        public DbCommand AddMatchedTripReportText(MatchedTripReportText matchDetails)
//        {
//            string methodName = nameof(this.AddMatchedTripReportText);
//            this.MethodCalls.Add($"{methodName}({matchDetails})");

//            return this.GetMethodReturnCall(methodName);
//        }

//        public DbCommand AddMatchedTripReportText(WtaTripReport tripReport, MatchText text)
//        {
//            string methodName = nameof(this.AddMatchedTripReportText);
//            this.MethodCalls.Add($"{methodName}({tripReport}, {text})");

//            return this.GetMethodReturnCall(methodName);
//        }

//        public DbCommand AddMatchedTripReportTopic(long tripReportID, long topicID, string exerpt)
//        {
//            string methodName = nameof(this.AddMatchedTripReportTopic);
//            this.MethodCalls.Add($"{methodName}({tripReportID}, {topicID}, \"{exerpt}\")");

//            return this.GetMethodReturnCall(methodName);
//        }

//        public DbCommand AddMatchedTripReportTopic(MatchedTripReportTopic matchDetails)
//        {
//            string methodName = nameof(this.AddMatchedTripReportTopic);
//            this.MethodCalls.Add($"{methodName}({matchDetails})");

//            return this.GetMethodReturnCall(methodName);
//        }

//        public DbCommand AddMatchedTripReportTopic(WtaTripReport tripReport, Topic topic, string exerpt)
//        {
//            string methodName = nameof(this.AddMatchedTripReportTopic);
//            this.MethodCalls.Add($"{methodName}({matchDetails})");

//            return this.GetMethodReturnCall(methodName);
//        }

//        public DbCommand AddMatchText(MatchText matchText)
//        {
//            throw new NotImplementedException();
//        }

//        public DbCommand AddProvider(Provider provider)
//        {
//            throw new NotImplementedException();
//        }

//        public DbCommand AddProviderStatistics(ProviderStatistics statistics)
//        {
//            throw new NotImplementedException();
//        }

//        public DbCommand AddTopic(Topic topic)
//        {
//            throw new NotImplementedException();
//        }

//        public DbCommand AddTopicText(TopicText topicText)
//        {
//            throw new NotImplementedException();
//        }

//        public DbCommand AddUrl(Url url)
//        {
//            throw new NotImplementedException();
//        }

//        public DbCommand AddWtaTripReport(WtaTripReport tripReport)
//        {
//            throw new NotImplementedException();
//        }

//        public DbCommand AddWtaTripReportImage(long wtaTripReportID, long imageID)
//        {
//            throw new NotImplementedException();
//        }

//        public DbCommand AddWtaTripReportImage(WtaTripReportImage tripReportImage)
//        {
//            throw new NotImplementedException();
//        }

//        public DbCommand DeleteImageUrl(ImageUrl imageUrl)
//        {
//            throw new NotImplementedException();
//        }

//        public DbCommand DeleteImageUrl(long id)
//        {
//            throw new NotImplementedException();
//        }

//        public DbCommand DeleteMatchedTripReportText(long id)
//        {
//            throw new NotImplementedException();
//        }

//        public DbCommand DeleteMatchedTripReportText(MatchedTripReportText matchDetails)
//        {
//            throw new NotImplementedException();
//        }

//        public DbCommand DeleteMatchedTripReportTopic(long id)
//        {
//            throw new NotImplementedException();
//        }

//        public DbCommand DeleteMatchedTripReportTopic(MatchedTripReportTopic matchDetails)
//        {
//            throw new NotImplementedException();
//        }

//        public DbCommand DeleteMatchText(long id)
//        {
//            throw new NotImplementedException();
//        }

//        public DbCommand DeleteMatchText(MatchText matchText)
//        {
//            throw new NotImplementedException();
//        }

//        public DbCommand DeleteProvider(long id)
//        {
//            throw new NotImplementedException();
//        }

//        public DbCommand DeleteProvider(Provider provider)
//        {
//            throw new NotImplementedException();
//        }

//        public DbCommand DeleteProviderStatistics(long id)
//        {
//            throw new NotImplementedException();
//        }

//        public DbCommand DeleteProviderStatistics(ProviderStatistics statistics)
//        {
//            throw new NotImplementedException();
//        }

//        public DbCommand DeleteTopic(long id)
//        {
//            throw new NotImplementedException();
//        }

//        public DbCommand DeleteTopic(Topic topic)
//        {
//            throw new NotImplementedException();
//        }

//        public DbCommand DeleteTopicText(long id)
//        {
//            throw new NotImplementedException();
//        }

//        public DbCommand DeleteTopicText(TopicText topicText)
//        {
//            throw new NotImplementedException();
//        }

//        public DbCommand DeleteUrl(long id)
//        {
//            throw new NotImplementedException();
//        }

//        public DbCommand DeleteUrl(Url url)
//        {
//            throw new NotImplementedException();
//        }

//        public DbCommand DeleteWtaTripReport(long id)
//        {
//            throw new NotImplementedException();
//        }

//        public DbCommand DeleteWtaTripReport(WtaTripReport tripReport)
//        {
//            throw new NotImplementedException();
//        }

//        public DbCommand DeleteWtaTripReportImage(long id)
//        {
//            throw new NotImplementedException();
//        }

//        public DbCommand DeleteWtaTripReportImage(WtaTripReportImage tripReportImage)
//        {
//            throw new NotImplementedException();
//        }

//        public DbCommand GetImagesForTripReport(long tripReportID)
//        {
//            throw new NotImplementedException();
//        }

//        public DbCommand GetImageUrl(long id)
//        {
//            throw new NotImplementedException();
//        }

//        public DbCommand GetMatchedTripReportText(long id)
//        {
//            throw new NotImplementedException();
//        }

//        public DbCommand GetMatchedTripReportTopic(long id)
//        {
//            throw new NotImplementedException();
//        }

//        public DbCommand GetMatchText(long id)
//        {
//            throw new NotImplementedException();
//        }

//        public DbCommand GetMatchTextByTopic(long topicID)
//        {
//            throw new NotImplementedException();
//        }

//        public DbCommand GetProvider(long id)
//        {
//            throw new NotImplementedException();
//        }

//        public DbCommand GetProviders()
//        {
//            throw new NotImplementedException();
//        }

//        public DbCommand GetProviderStatistics(long id)
//        {
//            throw new NotImplementedException();
//        }

//        public DbCommand GetTopic(long id)
//        {
//            throw new NotImplementedException();
//        }

//        public DbCommand GetTopics()
//        {
//            throw new NotImplementedException();
//        }

//        public DbCommand GetTopicText(long id)
//        {
//            throw new NotImplementedException();
//        }

//        public DbCommand GetTopicTextByTopic(long topicID)
//        {
//            throw new NotImplementedException();
//        }

//        public DbCommand GetUrl(long id)
//        {
//            throw new NotImplementedException();
//        }

//        public DbCommand GetUrl(string url)
//        {
//            throw new NotImplementedException();
//        }

//        public DbCommand GetWtaTripReport(long id)
//        {
//            throw new NotImplementedException();
//        }

//        public DbCommand GetWtaTripReportImage(long id)
//        {
//            throw new NotImplementedException();
//        }

//        public DbCommand IncrementProviderStatistics(long id, long? additionalRequests, long? additionalFailedRequests, long? additionalMatches, long? additionalSleepMS)
//        {
//            throw new NotImplementedException();
//        }

//        public DbCommand LockUrlsForCollection()
//        {
//            throw new NotImplementedException();
//        }

//        public DbCommand UpdateImageUrl(ImageUrl imageUrl)
//        {
//            throw new NotImplementedException();
//        }

//        public DbCommand UpdateMatchText(long id, long? falseParentID, string text)
//        {
//            throw new NotImplementedException();
//        }

//        public DbCommand UpdateMatchText(long id, string text)
//        {
//            throw new NotImplementedException();
//        }

//        public DbCommand UpdateMatchText(MatchText matchText)
//        {
//            throw new NotImplementedException();
//        }

//        public DbCommand UpdateProvider(Provider provider)
//        {
//            throw new NotImplementedException();
//        }

//        public DbCommand UpdateProviderStatistics(ProviderStatistics statistics)
//        {
//            throw new NotImplementedException();
//        }

//        public DbCommand UpdateTopic(long id, string name)
//        {
//            throw new NotImplementedException();
//        }

//        public DbCommand UpdateTopic(Topic topic)
//        {
//            throw new NotImplementedException();
//        }

//        public DbCommand UpdateTopicText(long topicTextID, long topicID, long textID)
//        {
//            throw new NotImplementedException();
//        }

//        public DbCommand UpdateTopicText(TopicText topicText)
//        {
//            throw new NotImplementedException();
//        }

//        public DbCommand UpdateUrl(long id, DateTime? collected, DateTime? intentLocked)
//        {
//            throw new NotImplementedException();
//        }

//        public DbCommand UpdateUrl(Url url)
//        {
//            throw new NotImplementedException();
//        }

//        public DbCommand UpdateWtaTripReport(WtaTripReport tripReport)
//        {
//            throw new NotImplementedException();
//        }
//    }

//}
