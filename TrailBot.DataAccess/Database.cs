using CascadePass.TrailBot.DataAccess;
using CascadePass.TrailBot.DataAccess.DTO;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SQLite;
using System.Threading;

namespace CascadePass.TrailBot.DataAccess
{
    public static class Database
    {
        #region Properties

        /// <summary>
        /// Gets or sets the connection string used to communicate
        /// with the database.
        /// </summary>
        public static string ConnectionString { get; set; }

        /// <summary>
        /// Gets or sets a <see cref="ITrailBotQueryProvider"/>
        /// to generate <see cref="DbCommand"/>s for the database
        /// system in use.
        /// </summary>
        /// <remarks>
        /// It is intended that TrailBot should support SQLite and
        /// SQL Server.  Part of how this will happen is by allowing
        /// multiple <see cref="ITrailBotQueryProvider"/>s to
        /// implement DBMS specific query provision.
        /// </remarks>
        public static ITrailBotQueryProvider QueryProvider { get; set; }

        #endregion

        #region Url

        public static long AddUrl(Url url)
        {
            using DbConnection connection = Database.GetConnection();

            var result = Database.AddUrl(url, connection);

            connection.Close();
            return result;
        }

        public static long AddUrl(Url url, DbConnection connection)
        {
            using DbCommand query = Database.QueryProvider.AddUrl(url);
            return url.ID = (long)Database.ExecuteScalar(query, connection);
        }

        public static long UpdateUrl(long id, DateTime? collected, DateTime? intentLocked)
        {
            using DbCommand query = Database.QueryProvider.UpdateUrl(id, collected, intentLocked);
            return Database.ExecuteNonQuery(query);
        }

        public static long UpdateUrl(Url url)
        {
            using DbCommand query = Database.QueryProvider.UpdateUrl(url);
            return Database.ExecuteNonQuery(query);
        }

        public static long DeleteUrl(long id)
        {
            using DbCommand query = Database.QueryProvider.DeleteUrl(id);
            return Database.ExecuteNonQuery(query);
        }

        public static long DeleteUrl(Url url)
        {
            #region Sanity checks

            if (url is null)
            {
                throw new ArgumentNullException(nameof(url));
            }

            #endregion

            return Database.DeleteUrl(url.ID);
        }

        public static Url GetUrl(long id)
        {
            using DbCommand query = Database.QueryProvider.GetUrl(id);
            using DbDataReader reader = Database.ExecuteReader(query);

            Url result = null;

            if (reader.Read())
            {
                result = ObjectRelationalMapper.GetUrl(reader);
            }

            query.Connection.Close();

            return result;
        }

        public static Url GetUrl(string address)
        {
            using DbConnection connection = Database.GetConnection();
            return Database.GetUrl(address, connection);
        }

        public static Url GetUrl(string address, DbConnection connection)
        {
            using DbCommand query = Database.QueryProvider.GetUrl(address);
            using DbDataReader reader = Database.ExecuteReader(query, connection);

            Url result = null;

            if (reader.Read())
            {
                result = ObjectRelationalMapper.GetUrl(reader);
            }

            return result;
        }

        public static bool CheckUrlExistance(string address)
        {
            Url url = Database.GetUrl(address);
            return url != null;
        }

        #endregion

        #region WtaTripReport

        public static long AddWtaTripReport(WtaTripReport tripReport)
        {
            #region Sanity checks

            if (tripReport is null)
            {
                throw new ArgumentNullException(nameof(tripReport));
            }

            if (tripReport.Url is null)
            {
                throw new ArgumentException("Url property is null.", nameof(tripReport));
            }

            if (tripReport.ID != 0)
            {
                throw new ArgumentOutOfRangeException("ID property must be zero.  Call UpdateWtaTripReport for trip reports that have already been added to the database.", nameof(tripReport));
            }

            #endregion

            using DbConnection connection = Database.GetConnection();

            if (tripReport.Url.ID == 0)
            {
                var lookup = Database.GetUrl(tripReport.Url.Address, connection);

                if (lookup != null)
                {
                    tripReport.Url.ID = lookup.ID;
                    Database.UpdateUrl(tripReport.Url);
                }
                else
                {
                    tripReport.Url.ID = Database.AddUrl(tripReport.Url, connection);
                }
            }

            using DbCommand query = Database.QueryProvider.AddWtaTripReport(tripReport);
            return tripReport.ID = (long)Database.ExecuteScalar(query, connection);
        }

        public static long UpdateWtaTripReport(WtaTripReport tripReport)
        {
            #region Sanity checks

            if (tripReport is null)
            {
                throw new ArgumentNullException(nameof(tripReport));
            }

            if (tripReport.Url is null)
            {
                throw new ArgumentException("Url property can't be null", nameof(tripReport));
            }

            #endregion

            if (tripReport.Url.ID == 0)
            {
                Database.AddUrl(tripReport.Url);
            }
            else
            {
                Database.UpdateUrl(tripReport.Url);
            }

            using DbCommand query = Database.QueryProvider.UpdateWtaTripReport(tripReport);
            return Database.ExecuteNonQuery(query);
        }

        public static long DeleteWtaTripReport(WtaTripReport tripReport)
        {
            using DbCommand query = Database.QueryProvider.DeleteWtaTripReport(tripReport);
            return Database.ExecuteNonQuery(query);
        }

        public static long DeleteWtaTripReport(long id)
        {
            using DbCommand query = Database.QueryProvider.DeleteWtaTripReport(id);
            return Database.ExecuteNonQuery(query);
        }

        public static WtaTripReport GetWtaTripReport(long id)
        {
            using DbCommand query = Database.QueryProvider.GetWtaTripReport(id);
            using DbDataReader reader = Database.ExecuteReader(query);

            WtaTripReport result = null;

            if (reader.Read())
            {
                result = ObjectRelationalMapper.GetWtaTripReport(reader);
            }

            query.Connection.Close();

            return result;
        }

        #endregion

        #region Topic

        public static long AddTopic(Topic topic)
        {
            using DbCommand query = Database.QueryProvider.AddTopic(topic);
            return topic.ID = (long)Database.ExecuteScalar(query);
        }

        public static long UpdateTopic(Topic topic)
        {
            using DbCommand query = Database.QueryProvider.UpdateTopic(topic);
            return (long)Database.ExecuteNonQuery(query);
        }

        public static long DeleteTopic(Topic topic)
        {
            using DbCommand query = Database.QueryProvider.DeleteTopic(topic);
            return (long)Database.ExecuteNonQuery(query);
        }

        public static long DeleteTopic(long id)
        {
            using DbCommand query = Database.QueryProvider.DeleteTopic(id);
            return (long)Database.ExecuteNonQuery(query);
        }

        public static Topic GetTopic(long id)
        {
            using DbCommand query = Database.QueryProvider.GetTopic(id);
            using DbDataReader reader = Database.ExecuteReader(query);

            Topic result = null;

            if (reader.Read())
            {
                result = ObjectRelationalMapper.GetTopic(reader);
            }

            query.Connection.Close();

            return result;
        }

        public static List<Topic> GetTopics()
        {
            using DbCommand query = Database.QueryProvider.GetTopics();
            using DbDataReader reader = Database.ExecuteReader(query);

            List<Topic> result = new();

            while (reader.Read())
            {
                var topic = ObjectRelationalMapper.GetTopic(reader);
                result.Add(topic);
            }

            query.Connection.Close();

            return result;
        }

        #endregion

        #region MatchText

        public static long AddMatchText(MatchText matchText)
        {
            using DbCommand query = Database.QueryProvider.AddMatchText(matchText);
            return matchText.ID = (long)Database.ExecuteScalar(query);
        }

        public static long UpdateMatchText(MatchText matchText)
        {
            using DbCommand query = Database.QueryProvider.UpdateMatchText(matchText);
            return (long)Database.ExecuteNonQuery(query);
        }

        public static long DeleteMatchText(MatchText matchText)
        {
            using DbCommand query = Database.QueryProvider.DeleteMatchText(matchText);
            return (long)Database.ExecuteNonQuery(query);
        }

        public static long DeleteMatchText(long id)
        {
            using DbCommand query = Database.QueryProvider.DeleteMatchText(id);
            return (long)Database.ExecuteNonQuery(query);
        }

        public static MatchText GetMatchText(long id)
        {
            using DbCommand query = Database.QueryProvider.GetMatchText(id);
            using DbDataReader reader = Database.ExecuteReader(query);

            MatchText result = null;

            if (reader.Read())
            {
                result = ObjectRelationalMapper.GetMatchText(reader);
            }

            query.Connection.Close();

            return result;
        }

        public static List<MatchText> GetMatchTextByTopic(long topicID)
        {
            using DbCommand query = Database.QueryProvider.GetMatchTextByTopic(topicID);
            using DbDataReader reader = Database.ExecuteReader(query);

            List<MatchText> result = new();

            while (reader.Read())
            {
                MatchText text = ObjectRelationalMapper.GetMatchText(reader);
                result.Add(text);
            }

            query.Connection.Close();

            return result;
        }

        #endregion

        #region TopicText

        public static long AddTopicText(TopicText topicText)
        {
            using DbCommand query = Database.QueryProvider.AddTopicText(topicText);
            return topicText.ID = (long)Database.ExecuteScalar(query);
        }

        public static long UpdateTopicText(TopicText topicText)
        {
            using DbCommand query = Database.QueryProvider.UpdateTopicText(topicText);
            return (long)Database.ExecuteNonQuery(query);
        }

        public static long DeleteTopicText(TopicText topicText)
        {
            using DbCommand query = Database.QueryProvider.DeleteTopicText(topicText);
            return (long)Database.ExecuteNonQuery(query);
        }

        public static long DeleteTopicText(long id)
        {
            using DbCommand query = Database.QueryProvider.DeleteTopicText(id);
            return (long)Database.ExecuteNonQuery(query);
        }

        public static TopicText GetTopicText(long id)
        {
            using DbCommand query = Database.QueryProvider.GetTopicText(id);
            using DbDataReader reader = Database.ExecuteReader(query);

            TopicText result = null;

            if (reader.Read())
            {
                result = ObjectRelationalMapper.GetTopicText(reader);
            }

            query.Connection.Close();

            return result;
        }

        public static List<TopicText> GetTopicTextByTopic(long topicID)
        {
            using DbCommand query = Database.QueryProvider.GetTopicTextByTopic(topicID);
            using DbDataReader reader = Database.ExecuteReader(query);

            List<TopicText> result = new();

            while (reader.Read())
            {
                TopicText text = ObjectRelationalMapper.GetTopicText(reader);
                result.Add(text);
            }

            query.Connection.Close();

            return result;
        }

        #endregion

        #region Provider

        public static long AddProvider(Provider provider)
        {
            using DbCommand query = Database.QueryProvider.AddProvider(provider);
            return provider.ID = (long)Database.ExecuteScalar(query);
        }

        public static long UpdateProvider(Provider provider)
        {
            using DbCommand query = Database.QueryProvider.UpdateProvider(provider);
            return (long)Database.ExecuteNonQuery(query);
        }

        public static long DeleteProvider(Provider provider)
        {
            using DbCommand query = Database.QueryProvider.DeleteProvider(provider);
            return (long)Database.ExecuteNonQuery(query);
        }

        public static long DeleteProvider(long id)
        {
            using DbCommand query = Database.QueryProvider.DeleteProvider(id);
            return (long)Database.ExecuteNonQuery(query);
        }

        public static Provider GetProvider(long id)
        {
            using DbCommand query = Database.QueryProvider.GetProvider(id);
            using DbDataReader reader = Database.ExecuteReader(query);

            Provider result = null;

            if (reader.Read())
            {
                result = ObjectRelationalMapper.GetProvider(reader);
            }

            query.Connection.Close();

            return result;
        }

        public static List<Provider> GetProviders()
        {
            using DbCommand query = Database.QueryProvider.GetProviders();
            using DbDataReader reader = Database.ExecuteReader(query);

            List<Provider> result = new();

            while (reader.Read())
            {
                var provider = ObjectRelationalMapper.GetProvider(reader);
                result.Add(provider);
            }

            query.Connection.Close();

            return result;
        }

        #endregion

        #region ProviderStatistics

        public static long AddProviderStatistics(ProviderStatistics statistics)
        {
            using DbCommand query = Database.QueryProvider.AddProviderStatistics(statistics);
            return statistics.ID = (long)Database.ExecuteScalar(query);
        }

        public static long UpdateProviderStatistics(ProviderStatistics statistics)
        {
            using DbCommand query = Database.QueryProvider.UpdateProviderStatistics(statistics);
            return (long)Database.ExecuteNonQuery(query);
        }

        public static long IncrementProviderStatistics(long id, long? additionalRequests, long? additionalFailedRequests, long? additionalMatches, long? additionalSleepMS)
        {
            using DbCommand query = Database.QueryProvider.IncrementProviderStatistics(id, additionalRequests, additionalFailedRequests, additionalMatches, additionalSleepMS);
            return (long)Database.ExecuteNonQuery(query);
        }

        public static long DeleteProviderStatistics(ProviderStatistics statistics)
        {
            using DbCommand query = Database.QueryProvider.DeleteProviderStatistics(statistics);
            return (long)Database.ExecuteNonQuery(query);
        }

        public static long DeleteProviderStatistics(long id)
        {
            using DbCommand query = Database.QueryProvider.DeleteProviderStatistics(id);
            return (long)Database.ExecuteNonQuery(query);
        }

        public static ProviderStatistics GetProviderStatistics(long id)
        {
            using DbCommand query = Database.QueryProvider.GetProviderStatistics(id);
            using DbDataReader reader = Database.ExecuteReader(query);

            ProviderStatistics result = null;

            if (reader.Read())
            {
                result = ObjectRelationalMapper.GetProviderStatistics(reader);
            }

            query.Connection.Close();

            return result;
        }

        #endregion

        #region MatchedTripReportText

        public static long AddMatchedTripReportText(MatchedTripReportText matchedText)
        {
            using DbCommand query = Database.QueryProvider.AddMatchedTripReportText(matchedText);
            return matchedText.ID = (long)Database.ExecuteScalar(query);
        }

        public static long DeleteMatchedTripReportText(MatchedTripReportText matchedText)
        {
            using DbCommand query = Database.QueryProvider.DeleteMatchedTripReportText(matchedText);
            return (long)Database.ExecuteNonQuery(query);
        }

        public static long DeleteMatchedTripReportText(long id)
        {
            using DbCommand query = Database.QueryProvider.DeleteMatchedTripReportText(id);
            return (long)Database.ExecuteNonQuery(query);
        }

        public static MatchedTripReportText GetMatchedTripReportText(long id)
        {
            using DbCommand query = Database.QueryProvider.GetMatchedTripReportText(id);
            using DbDataReader reader = Database.ExecuteReader(query);

            MatchedTripReportText result = null;

            if (reader.Read())
            {
                result = ObjectRelationalMapper.GetMatchedTripReportText(reader);
            }

            query.Connection.Close();

            return result;
        }

        #endregion

        #region MatchedTripReportTopic

        public static long AddMatchedTripReportTopic(MatchedTripReportTopic matchedTopic)
        {
            using DbCommand query = Database.QueryProvider.AddMatchedTripReportTopic(matchedTopic);
            return matchedTopic.ID = (long)Database.ExecuteScalar(query);
        }

        public static long AddMatchedTripReportTopic(WtaTripReport tripReport, Topic topic, string exerpt)
        {
            using DbCommand query = Database.QueryProvider.AddMatchedTripReportTopic(tripReport, topic, exerpt);
            return (long)Database.ExecuteScalar(query);
        }

        public static long DeleteMatchedTripReportTopic(MatchedTripReportTopic matchedTopic)
        {
            using DbCommand query = Database.QueryProvider.DeleteMatchedTripReportTopic(matchedTopic);
            return (long)Database.ExecuteNonQuery(query);
        }

        public static long DeleteMatchedTripReportTopic(long id)
        {
            using DbCommand query = Database.QueryProvider.DeleteMatchedTripReportTopic(id);
            return (long)Database.ExecuteNonQuery(query);
        }

        public static MatchedTripReportTopic GetMatchedTripReportTopic(long id)
        {
            using DbCommand query = Database.QueryProvider.GetMatchedTripReportTopic(id);
            using DbDataReader reader = Database.ExecuteReader(query);

            MatchedTripReportTopic result = null;

            if (reader.Read())
            {
                result = ObjectRelationalMapper.GetMatchedTripReportTopic(reader);
            }

            query.Connection.Close();

            return result;
        }

        #endregion

        #region ADO.NET abstraction methods

        #region Get Connection

        /// <summary>
        /// Gets a connection to the database.
        /// </summary>
        /// <returns>An open <see cref="DbConnection"/>.</returns>
        /// <exception cref="InvalidOperationException">Thrown when there is no <see cref="ConnectionString"/> (eg when the value is null, empty, or whitespace).</exception>
        public static DbConnection GetConnection()
        {
            if (string.IsNullOrWhiteSpace(Database.ConnectionString))
            {
                throw new InvalidOperationException();
            }

            SQLiteConnection connection = new(Database.ConnectionString);
            connection.Open();

            return connection;
        }

        #endregion

        private static int ExecuteNonQuery(DbCommand query)
        {
            using DbConnection connection = Database.GetConnection();
            return Database.ExecuteNonQuery(query, connection);
        }

        private static int ExecuteNonQuery(DbCommand query, DbConnection connection)
        {
            query.Connection = connection;

            int rowsAffected = query.ExecuteNonQuery();

            return rowsAffected;
        }

        private static object ExecuteScalar(DbCommand query)
        {
            using DbConnection connection = Database.GetConnection();
            return Database.ExecuteScalar(query, connection);
        }

        private static object ExecuteScalar(DbCommand query, DbConnection connection)
        {
            query.Connection = connection;

            object result = query.ExecuteScalar();

            return result;
        }

        private static DbDataReader ExecuteReader(DbCommand query)
        {
            DbConnection connection = Database.GetConnection();
            query.Connection = connection;

            DbDataReader reader = query.ExecuteReader();

            return reader;
        }

        private static DbDataReader ExecuteReader(DbCommand query, DbConnection connection)
        {
            query.Connection = connection;

            DbDataReader reader = query.ExecuteReader();

            return reader;
        }

        #endregion
    }
}
