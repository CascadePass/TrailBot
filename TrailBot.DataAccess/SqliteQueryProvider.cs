using CascadePass.TrailBot.DataAccess.DTO;
using System;
using System.Data.Common;
using System.Data.SQLite;

namespace CascadePass.TrailBot.DataAccess
{
    public class SqliteQueryProvider : ITrailBotQueryProvider
    {
        #region Url

        /// <summary>
        /// Creates a <see cref="DbCommand"/> to add a url object to the database.
        /// </summary>
        /// <param name="url">A <see cref="Url"/> <see cref="DataTransferObject"/>.</param>
        /// <returns>A <see cref="DbCommand"/>.</returns>
        /// <exception cref="ArgumentNullException">Thrown when url is null.</exception>
        /// <exception cref="ArgumentException">Thrown when url.Address is null, empty, or white space.</exception>
        public DbCommand AddUrl(Url url)
        {
            #region Sanity checks

            if (url == null)
            {
                throw new ArgumentNullException(nameof(url));
            }

            if (url.Address == null)
            {
                throw new ArgumentException("Address cannot be null.", nameof(url));
            }

            if (string.IsNullOrWhiteSpace(url.Address))
            {
                throw new ArgumentException($"Address cannot be null.", nameof(url));
            }

            if (url.ID != 0)
            {
                throw new ArgumentException($"{nameof(url)}.ID has a value of {url.ID} and can't be added to the database.  If it already exists, call UpdateUrl instead.  If not, set its ID to zero.", nameof(url));
            }

            #endregion

            string sql = @"
Insert Into Url (Url, FoundDate, CollectedDate, IntentLockedDate)
Values (@Url, @FoundDate, @CollectedDate, @IntentLockedDate);

Select last_insert_rowid();";

            SQLiteCommand insertCommand = new(sql);

            insertCommand.Parameters.AddWithValue("@Url", url.Address);
            insertCommand.Parameters.AddWithValue("@FoundDate", DataTransferObject.ToUnixDate(url.Found, DateTime.UtcNow));
            insertCommand.Parameters.AddWithValue("@CollectedDate", url.Collected.HasValue ? DataTransferObject.ToUnixDate(url.Collected.Value) : Convert.DBNull);
            insertCommand.Parameters.AddWithValue("@IntentLockedDate", url.IntentLocked.HasValue ? DataTransferObject.ToUnixDate(url.IntentLocked.Value) : Convert.DBNull);

            return insertCommand;
        }

        /// <summary>
        /// Creates a <see cref="DbCommand"/> to update a url object in the database.
        /// </summary>
        /// <param name="id">The ID of the url in the database.</param>
        /// <param name="collected">A <see cref="DateTime?"/> when the url was read.</param>
        /// <param name="intentLocked">A <see cref="DateTime?"/> when the url was locked for reading.</param>
        /// <returns>A <see cref="DbCommand"/>.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when id is negative or zero.</exception>
        public DbCommand UpdateUrl(long id, DateTime? collected, DateTime? intentLocked)
        {
            #region Sanity checks

            if (id < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(id), "Value must be positive and greater than zero.");
            }

            #endregion

            string sql = @"Update Url Set CollectedDate = @CollectedDate, IntentLockedDate = @IntentLockedDate Where UrlID = @ID";

            SQLiteCommand updateCommand = new(sql);

            updateCommand.Parameters.AddWithValue("@ID", id);
            updateCommand.Parameters.AddWithValue("@CollectedDate", collected.HasValue ? DataTransferObject.ToUnixDate(collected.Value) : Convert.DBNull);
            updateCommand.Parameters.AddWithValue("@IntentLockedDate", intentLocked.HasValue ? DataTransferObject.ToUnixDate(intentLocked.Value) : Convert.DBNull);

            return updateCommand;
        }

        public DbCommand UpdateUrl(Url url)
        {
            #region Sanity checks

            if (url is null)
            {
                throw new ArgumentNullException(nameof(url));
            }

            #endregion

            string sql = @"Update Url Set Url = @Address, CollectedDate = @CollectedDate, IntentLockedDate = @IntentLockedDate Where UrlID = @ID";

            SQLiteCommand updateCommand = new(sql);

            updateCommand.Parameters.AddWithValue("@ID", url.ID);
            updateCommand.Parameters.AddWithValue("@Address", url.Address);
            updateCommand.Parameters.AddWithValue("@CollectedDate", url.Collected.HasValue ? DataTransferObject.ToUnixDate(url.Collected.Value) : Convert.DBNull);
            updateCommand.Parameters.AddWithValue("@IntentLockedDate", url.IntentLocked.HasValue ? DataTransferObject.ToUnixDate(url.IntentLocked.Value) : Convert.DBNull);

            return updateCommand;
        }

        /// <summary>
        /// Creates a <see cref="DbCommand"/> to remove a url from the database.
        /// </summary>
        /// <param name="url">A <see cref="Url"/> <see cref="DataTransferObject"/> containing the ID of the Url to delete.</param>
        /// <returns>A <see cref="DbCommand"/>.</returns>
        /// <exception cref="ArgumentNullException">Thrown when url is null.</exception>
        public DbCommand DeleteUrl(Url url)
        {
            #region Sanity checks

            if (url is null)
            {
                throw new ArgumentNullException(nameof(url));
            }

            #endregion

            return this.DeleteUrl(url.ID);
        }

        /// <summary>
        /// Creates a <see cref="DbCommand"/> to remove a url from the database.
        /// </summary>
        /// <param name="id">The ID of the url in the database.</param>
        /// <returns>A <see cref="DbCommand"/>.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when id is negative or zero.</exception>
        public DbCommand DeleteUrl(long id)
        {
            #region Sanity checks

            if (id < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(id), "Value must be greater than zero.");
            }

            #endregion

            string sql = @"Delete From Url Where UrlID = @ID";

            SQLiteCommand deleteCommand = new(sql);

            deleteCommand.Parameters.AddWithValue("@ID", id);

            return deleteCommand;
        }

        /// <summary>
        /// Creates a <see cref="DbCommand"/> to fetch a url from the database.
        /// </summary>
        /// <param name="id">The ID of the url in the database.</param>
        /// <returns>A <see cref="DbCommand"/>.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when id is negative or zero.</exception>
        public DbCommand GetUrl(long id)
        {
            #region Sanity checks

            if (id < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(id), "Value must be positive and greater than zero.");
            }

            #endregion

            string sql = @"Select * From Url Where UrlID = @ID";

            SQLiteCommand selectCommand = new(sql);

            selectCommand.Parameters.AddWithValue("@ID", id);

            return selectCommand;
        }

        /// <summary>
        /// Creates a <see cref="DbCommand"/> to fetch a url from the database.
        /// </summary>
        /// <param name="url">The address of the url to fetch.</param>
        /// <returns>A <see cref="DbCommand"/>.</returns>
        /// <exception cref="ArgumentNullException">Thrown when url is null.</exception>
        /// <exception cref="ArgumentException">Thrown when url is empty or whitespace.</exception>
        public DbCommand GetUrl(string url)
        {
            #region Sanity checks

            if (url == null)
            {
                throw new ArgumentNullException(nameof(url));
            }

            if (string.IsNullOrWhiteSpace(url))
            {
                throw new ArgumentException($"{nameof(url)} cannot be empty or whitespace", nameof(url));
            }

            #endregion

            string sql = @"Select * From Url Where Url = @Url";

            SQLiteCommand selectCommand = new(sql);

            selectCommand.Parameters.AddWithValue("@Url", url);

            return selectCommand;
        }

        public DbCommand LockUrlsForCollection()
        {
            string sql = @"
Create Temp Table LockedID ( UrlID integer );

Insert Into LockedID ( UrlID )
Select UrlID
From Url
Where Collected Is Null
Limit @UrlCount;

Update Url Set IntentLockedDate = strftime('%s', current_timestamp) Where UrlID In (Select UrlID From LockedID);

Select u.* From Url u Join LockedID l On u.UrlID = l.UrlID;

Drop Table LockedID
";

            SQLiteCommand complexCommand = new(sql);

            return complexCommand;
        }

        #endregion

        #region WtaTripReport

        public DbCommand AddWtaTripReport(WtaTripReport tripReport)
        {
            #region Sanity checks

            if (tripReport is null)
            {
                throw new ArgumentNullException(nameof(tripReport));
            }

            if (tripReport.ID != 0)
            {
                throw new ArgumentException($"{nameof(tripReport)}.ID has a value of {tripReport.ID} and can't be added to the database.  If it already exists, call UpdateWtaTripReport instead.  If not, set its ID to zero.", nameof(tripReport));
            }

            if (tripReport.Url is null)
            {
                throw new ArgumentException($"{nameof(tripReport)}.Url can't be null.", nameof(tripReport));
            }

            if (tripReport.Url.ID < 1)
            {
                throw new ArgumentException($"{nameof(tripReport)}.Url.ID must be a positive number.", nameof(tripReport));
            }

            #endregion

            string sql = @"
Insert Into WtaTripReport (UrlID, Title, Region, HikeType, Author, TripDate, PublishedDate, ProcessedDate, ReportText)
Values (@UrlID, @Title, @Region, @HikeType, @Author, @TripDate, @PublishedDate, @ProcessedDate, @ReportText);

Select last_insert_rowid();";

            SQLiteCommand insertCommand = new(sql);

            insertCommand.Parameters.AddWithValue("@UrlID", tripReport.Url.ID);
            insertCommand.Parameters.AddWithValue("@Title", tripReport.Title);
            insertCommand.Parameters.AddWithValue("@Region", tripReport.Region);
            insertCommand.Parameters.AddWithValue("@HikeType", tripReport.HikeType);
            insertCommand.Parameters.AddWithValue("@Author", tripReport.Author);
            insertCommand.Parameters.AddWithValue("@TripDate", DataTransferObject.ToUnixDate(tripReport.TripDate));
            insertCommand.Parameters.AddWithValue("@PublishedDate", DataTransferObject.ToUnixDate(tripReport.PublishedDate));
            insertCommand.Parameters.AddWithValue("@ProcessedDate", DataTransferObject.ToUnixDate(tripReport.ProcessedDate));
            insertCommand.Parameters.AddWithValue("@ReportText", tripReport.ReportText);


            return insertCommand;
        }

        public DbCommand UpdateWtaTripReport(WtaTripReport tripReport)
        {
            #region Sanity checks

            if (tripReport is null)
            {
                throw new ArgumentNullException(nameof(tripReport));
            }

            if (tripReport.ID == 0)
            {
                throw new ArgumentException($"{nameof(tripReport)}.ID must have a non zero value.", nameof(tripReport));
            }

            if (tripReport.Url is null)
            {
                throw new ArgumentException($"{nameof(tripReport)}.Url can't be null.", nameof(tripReport));
            }

            if (tripReport.Url.ID < 1)
            {
                throw new ArgumentException($"{nameof(tripReport)}.Url.ID must be a positive number.", nameof(tripReport));
            }

            #endregion

            string sql = @"Update WtaTripReport Set UrlID = @UrlID, Title = @Title, Region = @Region, HikeType = @HikeType, Author = @Author, TripDate = @TripDate, PublishedDate = @PublishedDate, ProcessedDate = @ProcessedDate, ReportText = @ReportText Where WtaTripReportID = @WtaTripReportID";

            SQLiteCommand updateCommand = new(sql);

            updateCommand.Parameters.AddWithValue("@WtaTripReportID", tripReport.ID);
            updateCommand.Parameters.AddWithValue("@UrlID", tripReport.Url.ID);
            updateCommand.Parameters.AddWithValue("@Title", tripReport.Title);
            updateCommand.Parameters.AddWithValue("@Region", tripReport.Region);
            updateCommand.Parameters.AddWithValue("@HikeType", tripReport.HikeType);
            updateCommand.Parameters.AddWithValue("@Author", tripReport.Author);
            updateCommand.Parameters.AddWithValue("@TripDate", DataTransferObject.ToUnixDate(tripReport.TripDate));
            updateCommand.Parameters.AddWithValue("@PublishedDate", DataTransferObject.ToUnixDate(tripReport.PublishedDate));
            updateCommand.Parameters.AddWithValue("@ProcessedDate", DataTransferObject.ToUnixDate(tripReport.ProcessedDate));
            updateCommand.Parameters.AddWithValue("@ReportText", tripReport.ReportText);

            return updateCommand;
        }

        public DbCommand DeleteWtaTripReport(WtaTripReport tripReport)
        {
            #region Sanity checks

            if (tripReport is null)
            {
                throw new ArgumentNullException(nameof(tripReport));
            }

            if (tripReport.ID <= 0)
            {
                throw new ArgumentException($"{nameof(tripReport)}.ID must have a non zero, positive value.", nameof(tripReport));
            }

            #endregion

            return this.DeleteWtaTripReport(tripReport.ID);
        }

        public DbCommand DeleteWtaTripReport(long id)
        {
            #region Sanity checks

            if (id <= 0)
            {
                throw new ArgumentException($"{nameof(id)} must have a non zero, positive value.", nameof(id));
            }

            #endregion

            string sql = @"Delete From WtaTripReport Where WtaTripReportID = @WtaTripReportID";

            SQLiteCommand deleteCommand = new(sql);

            deleteCommand.Parameters.AddWithValue("@WtaTripReportID", id);

            return deleteCommand;
        }

        public DbCommand GetWtaTripReport(long id)
        {
            #region Sanity checks

            if (id <= 0)
            {
                throw new ArgumentOutOfRangeException($"{nameof(id)} must have a non zero, positive value.", nameof(id));
            }

            #endregion

            string sql = @"Select * From WtaTripReport t Join Url u On t.UrlID = u.UrlID Where WtaTripReportID = @WtaTripReportID";

            SQLiteCommand selectCommand = new(sql);

            selectCommand.Parameters.AddWithValue("@WtaTripReportID", id);

            return selectCommand;
        }

        #endregion

        #region Topic

        public DbCommand AddTopic(Topic topic)
        {
            #region Sanity checks

            if (topic is null)
            {
                throw new ArgumentNullException(nameof(topic));
            }

            if (string.IsNullOrWhiteSpace(topic.Name))
            {
                throw new ArgumentException($"{nameof(topic)}.Name can't be null, empty, or whitespace.", nameof(topic));
            }

            if (topic.ID != 0)
            {
                throw new ArgumentException($"{nameof(topic)}.ID must be zero.  If the topic has already been added to the database, call UpdateTopic instead.", nameof(topic));
            }

            #endregion

            string sql = @"
Insert Into Topic (Name)
Values (@Name);

Select last_insert_rowid();";

            SQLiteCommand insertCommand = new(sql);

            insertCommand.Parameters.AddWithValue("@Name", topic.Name);

            return insertCommand;
        }

        public DbCommand UpdateTopic(Topic topic)
        {
            #region Sanity checks

            if (topic is null)
            {
                throw new ArgumentNullException(nameof(topic));
            }

            if (string.IsNullOrWhiteSpace(topic.Name))
            {
                throw new ArgumentException($"{nameof(topic)}.Name can't be null, empty, or whitespace.", nameof(topic));
            }

            if (topic.ID <= 0)
            {
                throw new ArgumentOutOfRangeException($"{nameof(topic)}.ID must be a non zero, positive value.", nameof(topic));
            }

            #endregion

            return this.UpdateTopic(topic.ID, topic.Name);
        }

        public DbCommand UpdateTopic(long id, string name)
        {
            #region Sanity checks

            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException($"{nameof(name)} can't be null, empty, or whitespace.", nameof(name));
            }

            if (id <= 0)
            {
                throw new ArgumentOutOfRangeException($"{nameof(id)} must be a non zero, positive value.", nameof(id));
            }

            #endregion

            string sql = @"Update Topic Set Name = @Name Where TopicID = @ID";

            SQLiteCommand updateCommand = new(sql);

            updateCommand.Parameters.AddWithValue("@ID", id);
            updateCommand.Parameters.AddWithValue("@Name", name);

            return updateCommand;
        }

        public DbCommand DeleteTopic(Topic topic)
        {
            #region Sanity checks

            if (topic is null)
            {
                throw new ArgumentNullException(nameof(topic));
            }

            #endregion

            return this.DeleteTopic(topic.ID);
        }

        public DbCommand DeleteTopic(long id)
        {
            #region Sanity checks

            if (id < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(id), "Value must be greater than zero.");
            }

            #endregion

            string sql = @"Delete From Topic Where TopicID = @ID";

            SQLiteCommand deleteCommand = new(sql);

            deleteCommand.Parameters.AddWithValue("@ID", id);

            return deleteCommand;
        }

        public DbCommand GetTopic(long id)
        {
            #region Sanity checks

            if (id < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(id), "Value must be greater than zero.");
            }

            #endregion

            string sql = @"Select * From Topic Where TopicID = @ID";

            SQLiteCommand selectCommand = new(sql);

            selectCommand.Parameters.AddWithValue("@ID", id);

            return selectCommand;
        }

        public DbCommand GetTopics()
        {
            string sql = @"Select * From Topic";

            SQLiteCommand selectCommand = new(sql);

            return selectCommand;
        }

        #endregion

        #region MatchText

        public DbCommand AddMatchText(MatchText matchText)
        {
            #region Sanity checks

            if (matchText is null)
            {
                throw new ArgumentNullException(nameof(matchText));
            }

            if (string.IsNullOrWhiteSpace(matchText.Text))
            {
                throw new ArgumentException($"{nameof(matchText)}.Text can't be null, empty, or white space.", nameof(matchText));
            }

            if (matchText.ID != 0)
            {
                throw new ArgumentOutOfRangeException($"{nameof(matchText)}.ID must be zero.  If this text has already been added to the database, call UpdateMatchText instead.", nameof(matchText));
            }

            #endregion

            string sql = @"
Insert Into MatchText (FalseMatchParentID, Text)
Values (@FalseMatchParentID, @Text);

Select last_insert_rowid();";

            SQLiteCommand insertCommand = new(sql);

            insertCommand.Parameters.AddWithValue("@FalseMatchParentID", matchText.ParentID);
            insertCommand.Parameters.AddWithValue("@Text", matchText.Text);

            return insertCommand;
        }

        public DbCommand UpdateMatchText(MatchText matchText)
        {
            #region Sanity checks

            if (matchText is null)
            {
                throw new ArgumentNullException(nameof(matchText));
            }

            if (string.IsNullOrWhiteSpace(matchText.Text))
            {
                throw new ArgumentException($"{nameof(matchText)}.Text can't be null, empty, or white space.", nameof(matchText));
            }

            if (matchText.ID <= 0)
            {
                throw new ArgumentOutOfRangeException($"{nameof(matchText)}.ID must be a positive number.  If this text has not been added to the database, call AddMatchText instead.", nameof(matchText));
            }

            #endregion

            return this.UpdateMatchText(matchText.ID, matchText.ParentID, matchText.Text);
        }

        public DbCommand UpdateMatchText(long id, string text)
        {
            #region Sanity checks

            if (text is null)
            {
                throw new ArgumentNullException(nameof(text));
            }

            if (string.IsNullOrWhiteSpace(text))
            {
                throw new ArgumentException($"{nameof(text)} can't be null, empty, or white space.", nameof(text));
            }

            if (id <= 0)
            {
                throw new ArgumentOutOfRangeException($"{nameof(id)} must be a positive number.  If this text has not been added to the database, call AddMatchText instead.", nameof(id));
            }

            #endregion

            string sql = @"Update MatchText Set Text = @Text Where MatchTextID = @MatchTextID;";

            SQLiteCommand updateCommand = new(sql);

            updateCommand.Parameters.AddWithValue("@MatchTextID", id);
            updateCommand.Parameters.AddWithValue("@Text", text);

            return updateCommand;
        }

        public DbCommand UpdateMatchText(long id, long? falseParentID, string text)
        {
            #region Sanity checks

            if (text is null)
            {
                throw new ArgumentNullException(nameof(text));
            }

            if (string.IsNullOrWhiteSpace(text))
            {
                throw new ArgumentException($"{nameof(text)} can't be null, empty, or white space.", nameof(text));
            }

            if (id <= 0)
            {
                throw new ArgumentOutOfRangeException($"{nameof(id)} must be a positive number.  If this text has not been added to the database, call AddMatchText instead.", nameof(id));
            }

            if (falseParentID < 1)
            {
                throw new ArgumentOutOfRangeException($"{nameof(id)} must be a positive number, a value in the database.", nameof(id));
            }

            #endregion

            string sql = @"Update MatchText Set FalseMatchParentID = @FalseMatchParentID, Text = @Text Where MatchTextID = @MatchTextID;";

            SQLiteCommand updateCommand = new(sql);

            updateCommand.Parameters.AddWithValue("@MatchTextID", id);
            updateCommand.Parameters.AddWithValue("@FalseMatchParentID", falseParentID);
            updateCommand.Parameters.AddWithValue("@Text", text);

            return updateCommand;
        }

        public DbCommand DeleteMatchText(MatchText matchText)
        {
            #region Sanity checks

            if (matchText is null)
            {
                throw new ArgumentNullException(nameof(matchText));
            }

            #endregion

            return this.DeleteMatchText(matchText.ID);
        }

        public DbCommand DeleteMatchText(long id)
        {
            #region Sanity checks

            if (id <= 0)
            {
                throw new ArgumentOutOfRangeException($"{nameof(id)} must be a positive number.  If this text has not been added to the database, call AddMatchText instead.", nameof(id));
            }

            #endregion

            string sql = @"Delete From MatchText Where MatchTextID = @MatchTextID";

            SQLiteCommand deleteCommand = new(sql);

            deleteCommand.Parameters.AddWithValue("@MatchTextID", id);

            return deleteCommand;
        }

        public DbCommand GetMatchText(long id)
        {
            #region Sanity checks

            if (id < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(id), "Value must be positive and greater than zero.");
            }

            #endregion

            string sql = @"Select * From MatchText Where MatchTextID = @ID";

            SQLiteCommand selectCommand = new(sql);

            selectCommand.Parameters.AddWithValue("@ID", id);

            return selectCommand;
        }

        public DbCommand GetMatchTextByTopic(long topicID)
        {
            #region Sanity checks

            if (topicID < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(topicID), "Value must be positive and greater than zero.");
            }

            #endregion

            string sql = @"Select mt.* From MatchText mt Join TopicText tt On mt.MatchTextID = tt.MatchTextID Where tt.TopicID = @TopicID";

            SQLiteCommand selectCommand = new(sql);

            selectCommand.Parameters.AddWithValue("@TopicID", topicID);

            return selectCommand;
        }

        #endregion

        #region TopicText

        public DbCommand AddTopicText(TopicText topicText)
        {
            #region Sanity checks

            if (topicText is null)
            {
                throw new ArgumentNullException(nameof(topicText));
            }

            if (topicText.ID != 0)
            {
                throw new ArgumentOutOfRangeException($"{nameof(topicText)}.TopicID must be zero.  If this association has already been added to the database, call UpdateTopicText instead.", nameof(topicText));
            }

            if (topicText.TopicID <= 0)
            {
                throw new ArgumentOutOfRangeException($"{nameof(topicText)}.TopicID must be a valid database ID.", nameof(topicText));
            }

            if (topicText.TextID <= 0)
            {
                throw new ArgumentOutOfRangeException($"{nameof(topicText)}.TextID must be a valid database ID.", nameof(topicText));
            }

            #endregion

            string sql = @"
Insert Into TopicText (TopicID, MatchTextID)
Values (@TopicID, @MatchTextID);

Select last_insert_rowid();";

            SQLiteCommand insertCommand = new(sql);

            insertCommand.Parameters.AddWithValue("@TopicID", topicText.TopicID);
            insertCommand.Parameters.AddWithValue("@MatchTextID", topicText.TextID);

            return insertCommand;
        }

        public DbCommand UpdateTopicText(TopicText topicText)
        {
            #region Sanity checks

            if (topicText is null)
            {
                throw new ArgumentNullException(nameof(topicText));
            }

            if (topicText.TopicID <= 0)
            {
                throw new ArgumentOutOfRangeException($"{nameof(topicText)}.TopicID must be a valid database ID.", nameof(topicText));
            }

            if (topicText.TextID <= 0)
            {
                throw new ArgumentOutOfRangeException($"{nameof(topicText)}.TextID must be a valid database ID.", nameof(topicText));
            }

            #endregion

            return this.UpdateTopicText(topicText.ID, topicText.TopicID, topicText.TextID);
        }

        public DbCommand UpdateTopicText(long topicTextID, long topicID, long textID)
        {
            #region Sanity checks

            if (topicTextID <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(topicTextID));
            }

            if (topicID <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(topicTextID));
            }

            if (textID <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(topicTextID));
            }

            #endregion

            string sql = @"Update TopicText Set MatchTextID = @MatchTextID, TopicID = TopicID Where TopicTextID = @TopicTextID;";

            SQLiteCommand updateCommand = new(sql);

            updateCommand.Parameters.AddWithValue("@TopicTextID", topicTextID);
            updateCommand.Parameters.AddWithValue("@TopicID", topicID);
            updateCommand.Parameters.AddWithValue("@MatchTextID", textID);

            return updateCommand;
        }

        public DbCommand DeleteTopicText(TopicText topicText)
        {
            #region Sanity checks

            if (topicText is null)
            {
                throw new ArgumentNullException(nameof(topicText));
            }

            #endregion

            return this.DeleteTopicText(topicText.ID);
        }

        public DbCommand DeleteTopicText(long id)
        {
            #region Sanity checks

            if (id <= 0)
            {
                throw new ArgumentOutOfRangeException($"{nameof(id)} must be a positive number.  If this text has not been added to the database, call AddTopicText instead.", nameof(id));
            }

            #endregion

            string sql = @"Delete From TopicText Where TopicTextID = @TopicTextID";

            SQLiteCommand deleteCommand = new(sql);

            deleteCommand.Parameters.AddWithValue("@TopicTextID", id);

            return deleteCommand;
        }

        public DbCommand GetTopicText(long id)
        {
            #region Sanity checks

            if (id < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(id), "Value must be positive and greater than zero.");
            }

            #endregion

            string sql = @"Select * From TopicText Where TopicTextID = @ID";

            SQLiteCommand selectCommand = new(sql);

            selectCommand.Parameters.AddWithValue("@ID", id);

            return selectCommand;
        }

        public DbCommand GetTopicTextByTopic(long topicID)
        {
            #region Sanity checks

            if (topicID < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(topicID), "Value must be positive and greater than zero.");
            }

            #endregion

            string sql = @"Select * From TopicText Where TopicID = @ID";

            SQLiteCommand selectCommand = new(sql);

            selectCommand.Parameters.AddWithValue("@ID", topicID);

            return selectCommand;
        }

        #endregion

        #region Provider

        /// <summary>
        /// Creates a <see cref="DbCommand"/> to add a provider to the database.
        /// </summary>
        /// <param name="url">A <see cref="Provider"/> <see cref="DataTransferObject"/>.</param>
        /// <returns>A <see cref="DbCommand"/>.</returns>
        /// <exception cref="ArgumentNullException">Thrown when provider is null.</exception>
        /// <exception cref="ArgumentException">Thrown when Name or TypeName is null, empty, or white space.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown ID is not zero.</exception>
        public DbCommand AddProvider(Provider provider)
        {
            #region Sanity checks

            if (provider == null)
            {
                throw new ArgumentNullException(nameof(provider));
            }

            if (provider.Name == null)
            {
                throw new ArgumentException("Name cannot be null.", nameof(provider));
            }

            if (string.IsNullOrWhiteSpace(provider.TypeName))
            {
                throw new ArgumentException($"TypeName cannot be null.", nameof(provider));
            }

            if (provider.ID != 0)
            {
                throw new ArgumentOutOfRangeException($"{nameof(provider)}.ID has a value of {provider.ID} and can't be added to the database.  If it already exists, call UpdateProvider instead.  If not, set its ID to zero.", nameof(provider));
            }

            #endregion

            string sql = @"
Insert Into Provider (Name, TypeName, PreservationRule, State, Browser, LastTripReportRequest, LastGetRecentRequest, ProviderXml)
Values (@Name, @TypeName, @PreservationRule, @State, @Browser, @LastTripReportRequest, @LastGetRecentRequest, @ProviderXml);

Select last_insert_rowid();";

            SQLiteCommand insertCommand = new(sql);

            insertCommand.Parameters.AddWithValue("@Name", provider.Name);
            insertCommand.Parameters.AddWithValue("@TypeName", provider.TypeName);
            insertCommand.Parameters.AddWithValue("@PreservationRule", provider.PreservationRule);
            insertCommand.Parameters.AddWithValue("@State", provider.State);
            insertCommand.Parameters.AddWithValue("@Browser", provider.Browser);
            insertCommand.Parameters.AddWithValue("@LastTripReportRequest", provider.LastTripReportRequest);
            insertCommand.Parameters.AddWithValue("@LastGetRecentRequest", provider.LastGetRecentRequest);
            insertCommand.Parameters.AddWithValue("@ProviderXml", provider.ProviderXml);

            insertCommand.Parameters.AddWithValue("@LastTripReportRequest", provider.LastTripReportRequest.HasValue ? DataTransferObject.ToUnixDate(provider.LastTripReportRequest.Value) : Convert.DBNull);
            insertCommand.Parameters.AddWithValue("@LastGetRecentRequest", provider.LastGetRecentRequest.HasValue ? DataTransferObject.ToUnixDate(provider.LastGetRecentRequest.Value) : Convert.DBNull);

            return insertCommand;
        }

        /// <summary>
        /// Creates a <see cref="DbCommand"/> to update a provider in the database.
        /// </summary>
        /// <param name="provider">A <see cref="Provider"/> <see cref="DataTransferObject"/> with values to update the database to.</param>
        /// <returns>A <see cref="DbCommand"/>.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when providerid is negative or zero.</exception>
        public DbCommand UpdateProvider(Provider provider)
        {
            #region Sanity checks

            if (provider is null)
            {
                throw new ArgumentNullException(nameof(provider));
            }

            if (provider.ID <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(provider));
            }

            #endregion

            string sql = @"Update Provider Set Name = @Name, TypeName = @TypeName, PreservationRule = @PreservationRule, State = @State, Browser = @Browser, LastTripReportRequest = @LastTripReportRequest, LastGetRecentRequest = @LastGetRecentRequest, ProviderXml = @ProviderXml Where ProviderID = @ID";

            SQLiteCommand updateCommand = new(sql);

            updateCommand.Parameters.AddWithValue("@ID", provider.ID);
            updateCommand.Parameters.AddWithValue("@Name", provider.Name);
            updateCommand.Parameters.AddWithValue("@TypeName", provider.TypeName);
            updateCommand.Parameters.AddWithValue("@PreservationRule", provider.PreservationRule);
            updateCommand.Parameters.AddWithValue("@State", provider.State);
            updateCommand.Parameters.AddWithValue("@Browser", provider.Browser);
            updateCommand.Parameters.AddWithValue("@LastTripReportRequest", provider.LastTripReportRequest);
            updateCommand.Parameters.AddWithValue("@LastGetRecentRequest", provider.LastGetRecentRequest);
            updateCommand.Parameters.AddWithValue("@ProviderXml", provider.ProviderXml);

            updateCommand.Parameters.AddWithValue("@LastTripReportRequest", provider.LastTripReportRequest.HasValue ? DataTransferObject.ToUnixDate(provider.LastTripReportRequest.Value) : Convert.DBNull);
            updateCommand.Parameters.AddWithValue("@LastGetRecentRequest", provider.LastGetRecentRequest.HasValue ? DataTransferObject.ToUnixDate(provider.LastGetRecentRequest.Value) : Convert.DBNull);

            return updateCommand;
        }

        /// <summary>
        /// Creates a <see cref="DbCommand"/> to remove a provider from the database.
        /// </summary>
        /// <param name="url">A <see cref="Provider"/> <see cref="DataTransferObject"/> containing the ID of the Provider to delete.</param>
        /// <returns>A <see cref="DbCommand"/>.</returns>
        /// <exception cref="ArgumentNullException">Thrown when provider is null.</exception>
        public DbCommand DeleteProvider(Provider provider)
        {
            #region Sanity checks

            if (provider is null)
            {
                throw new ArgumentNullException(nameof(provider));
            }

            #endregion

            return this.DeleteProvider(provider.ID);
        }

        /// <summary>
        /// Creates a <see cref="DbCommand"/> to remove a provider from the database.
        /// </summary>
        /// <param name="id">The ID of the url in the database.</param>
        /// <returns>A <see cref="DbCommand"/>.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when id is negative or zero.</exception>
        public DbCommand DeleteProvider(long id)
        {
            #region Sanity checks

            if (id < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(id), "Value must be greater than zero.");
            }

            #endregion

            string sql = @"Delete From Provider Where ProviderID = @ID";

            SQLiteCommand deleteCommand = new(sql);

            deleteCommand.Parameters.AddWithValue("@ID", id);

            return deleteCommand;
        }

        /// <summary>
        /// Creates a <see cref="DbCommand"/> to fetch a provider from the database.
        /// </summary>
        /// <param name="id">The ID of the provider in the database.</param>
        /// <returns>A <see cref="DbCommand"/>.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when id is negative or zero.</exception>
        public DbCommand GetProvider(long id)
        {
            #region Sanity checks

            if (id < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(id), "Value must be positive and greater than zero.");
            }

            #endregion

            string sql = @"Select * From Provider Where ProviderID = @ID";

            SQLiteCommand selectCommand = new(sql);

            selectCommand.Parameters.AddWithValue("@ID", id);

            return selectCommand;
        }

        public DbCommand GetProviders()
        {
            string sql = @"Select * From Provider";

            SQLiteCommand selectCommand = new(sql);

            return selectCommand;
        }

        #endregion

        #region ProviderStatistics

        /// <summary>
        /// Creates a <see cref="DbCommand"/> to add a statistics to the database.
        /// </summary>
        /// <param name="url">A <see cref="ProviderStatistics"/> <see cref="DataTransferObject"/>.</param>
        /// <returns>A <see cref="DbCommand"/>.</returns>
        /// <exception cref="ArgumentNullException">Thrown when statistics is null.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown ID is not zero.</exception>
        public DbCommand AddProviderStatistics(ProviderStatistics statistics)
        {
            #region Sanity checks

            if (statistics == null)
            {
                throw new ArgumentNullException(nameof(statistics));
            }

            if (statistics.ID < 1)
            {
                throw new ArgumentOutOfRangeException($"{nameof(statistics)}.ID has a value of {statistics.ID} and can't be added to the database.  If it already exists, call UpdateProviderStatistics instead.  If not, set its ID to zero.", nameof(statistics));
            }

            #endregion

            string sql = @"
Insert Into ProviderStatistics (ProviderID, RequestsMade, FailedRequests, MatchesFound, SleepTimeInMS)
Values (@ProviderID, @RequestsMade, @FailedRequests, @MatchesFound, @SleepTimeInMS);

Select last_insert_rowid();";

            SQLiteCommand insertCommand = new(sql);

            insertCommand.Parameters.AddWithValue("@ProviderID", statistics.ID);
            insertCommand.Parameters.AddWithValue("@RequestsMade", statistics.TotalRequestsMade);
            insertCommand.Parameters.AddWithValue("@FailedRequests", statistics.FailedRequests);
            insertCommand.Parameters.AddWithValue("@MatchesFound", statistics.MatchesFound);
            insertCommand.Parameters.AddWithValue("@SleepTimeInMS", statistics.SleepTimeInMS);

            return insertCommand;
        }

        /// <summary>
        /// Creates a <see cref="DbCommand"/> to update a statistics in the database.
        /// </summary>
        /// <param name="statistics">A <see cref="ProviderStatistics"/> <see cref="DataTransferObject"/> with values to update the database to.</param>
        /// <returns>A <see cref="DbCommand"/>.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when statisticsid is negative or zero.</exception>
        public DbCommand UpdateProviderStatistics(ProviderStatistics statistics)
        {
            #region Sanity checks

            if (statistics is null)
            {
                throw new ArgumentNullException(nameof(statistics));
            }

            if (statistics.ID <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(statistics));
            }

            #endregion

            string sql = @"Update ProviderStatistics Set RequestsMade = @RequestsMade, FailedRequests = @FailedRequests, MatchesFound = @MatchesFound, SleepTimeInMS = @SleepTimeInMS Where ProviderID = @ProviderID";

            SQLiteCommand updateCommand = new(sql);

            updateCommand.Parameters.AddWithValue("@ProviderID", statistics.ID);
            updateCommand.Parameters.AddWithValue("@RequestsMade", statistics.TotalRequestsMade);
            updateCommand.Parameters.AddWithValue("@FailedRequests", statistics.FailedRequests);
            updateCommand.Parameters.AddWithValue("@MatchesFound", statistics.MatchesFound);
            updateCommand.Parameters.AddWithValue("@SleepTimeInMS", statistics.SleepTimeInMS);

            return updateCommand;
        }

        /// <summary>
        /// Creates a <see cref="DbCommand"/> to remove a statistics from the database.
        /// </summary>
        /// <param name="url">A <see cref="ProviderStatistics"/> <see cref="DataTransferObject"/> containing the ID of the ProviderStatistics to delete.</param>
        /// <returns>A <see cref="DbCommand"/>.</returns>
        /// <exception cref="ArgumentNullException">Thrown when statistics is null.</exception>
        public DbCommand DeleteProviderStatistics(ProviderStatistics statistics)
        {
            #region Sanity checks

            if (statistics is null)
            {
                throw new ArgumentNullException(nameof(statistics));
            }

            #endregion

            return this.DeleteProviderStatistics(statistics.ID);
        }

        /// <summary>
        /// Creates a <see cref="DbCommand"/> to remove a statistics from the database.
        /// </summary>
        /// <param name="id">The ID of the url in the database.</param>
        /// <returns>A <see cref="DbCommand"/>.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when id is negative or zero.</exception>
        public DbCommand DeleteProviderStatistics(long id)
        {
            #region Sanity checks

            if (id < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(id), "Value must be greater than zero.");
            }

            #endregion

            string sql = @"Delete From ProviderStatistics Where ProviderID = @ID";

            SQLiteCommand deleteCommand = new(sql);

            deleteCommand.Parameters.AddWithValue("@ID", id);

            return deleteCommand;
        }

        /// <summary>
        /// Creates a <see cref="DbCommand"/> to fetch a statistics from the database.
        /// </summary>
        /// <param name="id">The ID of the statistics in the database.</param>
        /// <returns>A <see cref="DbCommand"/>.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when id is negative or zero.</exception>
        public DbCommand GetProviderStatistics(long id)
        {
            #region Sanity checks

            if (id < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(id), "Value must be positive and greater than zero.");
            }

            #endregion

            string sql = @"Select * From ProviderStatistics Where ProviderID = @ID";

            SQLiteCommand selectCommand = new(sql);

            selectCommand.Parameters.AddWithValue("@ID", id);

            return selectCommand;
        }

        public DbCommand IncrementProviderStatistics(long id, long? additionalRequests, long? additionalFailedRequests, long? additionalMatches, long? additionalSleepMS)
        {
            #region Sanity checks

            if (id <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(id));
            }

            #endregion

            string sql = @"
Update ProviderStatistics Set
    RequestsMade = RequestsMade + Coalesce(@RequestsMade, 0),
    FailedRequests = FailedRequests + Coalesce(@FailedRequests, 0),
    MatchesFound = MatchesFound + Coalesce(@MatchesFound, 0),
    SleepTimeInMS = SleepTimeInMS + Coalesce(@SleepTimeInMS, 0)
Where
    ProviderID = @ProviderID
";

            SQLiteCommand updateCommand = new(sql);

            updateCommand.Parameters.AddWithValue("@ProviderID", id);
            updateCommand.Parameters.AddWithValue("@RequestsMade", additionalRequests);
            updateCommand.Parameters.AddWithValue("@FailedRequests", additionalFailedRequests);
            updateCommand.Parameters.AddWithValue("@MatchesFound", additionalMatches);
            updateCommand.Parameters.AddWithValue("@SleepTimeInMS", additionalSleepMS);

            return updateCommand;
        }

        #endregion

        #region MatchedTripReportText

        /// <summary>
        /// Creates a <see cref="DbCommand"/> to add a MatchedTripReportText to the database.
        /// </summary>
        /// <param name="url">A <see cref="MatchedTripReportText"/> <see cref="DataTransferObject"/>.</param>
        /// <returns>A <see cref="DbCommand"/>.</returns>
        /// <exception cref="ArgumentNullException">Thrown when matchDetails is null.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown ID is not zero.</exception>
        public DbCommand AddMatchedTripReportText(MatchedTripReportText matchDetails)
        {
            #region Sanity checks

            if (matchDetails is null)
            {
                throw new ArgumentNullException(nameof(matchDetails));
            }

            if (matchDetails.ID != 0)
            {
                throw new ArgumentOutOfRangeException($"{nameof(matchDetails)}.ID has a value of {matchDetails.ID} and can't be added to the database.  If it already exists, call UpdateMatchedTripReportText instead.  If not, set its ID to zero.", nameof(matchDetails));
            }

            #endregion

            return this.AddMatchedTripReportText(matchDetails.TripReportID, matchDetails.TextID);
        }

        public DbCommand AddMatchedTripReportText(WtaTripReport tripReport, MatchText text)
        {
            #region Sanity checks

            if (tripReport is null)
            {
                throw new ArgumentNullException(nameof(tripReport));
            }

            if (text is null)
            {
                throw new ArgumentNullException(nameof(text));
            }

            #endregion

            return this.AddMatchedTripReportText(tripReport.ID, text.ID);
        }

        public DbCommand AddMatchedTripReportText(long tripReportID, long textID)
        {
            #region Sanity checks

            if (tripReportID <= 0)
            {
                throw new ArgumentOutOfRangeException($"{nameof(tripReportID)} must be a positive number, it is a foreign key to WtaTripReport.", nameof(tripReportID));
            }


            if (textID <= 0)
            {
                throw new ArgumentOutOfRangeException($"{nameof(textID)} must be a positive number, it is a foreign key to MatchText.", nameof(textID));
            }

            #endregion

            string sql = @"
Insert Into MatchedTripReportText (TripReportID, MatchTextID)
Values (@TripReportID, @MatchTextID);

Select last_insert_rowid();";

            SQLiteCommand insertCommand = new(sql);

            insertCommand.Parameters.AddWithValue("@TripReportID", tripReportID);
            insertCommand.Parameters.AddWithValue("@MatchTextID", textID);

            return insertCommand;
        }

        /// <summary>
        /// Creates a <see cref="DbCommand"/> to remove a MatchedTripReportText from the database.
        /// </summary>
        /// <param name="url">A <see cref="MatchedTripReportText"/> <see cref="DataTransferObject"/> containing the ID of the MatchedTripReportText to delete.</param>
        /// <returns>A <see cref="DbCommand"/>.</returns>
        /// <exception cref="ArgumentNullException">Thrown when matchDetails is null.</exception>
        public DbCommand DeleteMatchedTripReportText(MatchedTripReportText matchDetails)
        {
            #region Sanity checks

            if (matchDetails is null)
            {
                throw new ArgumentNullException(nameof(matchDetails));
            }

            #endregion

            return this.DeleteMatchedTripReportText(matchDetails.ID);
        }

        /// <summary>
        /// Creates a <see cref="DbCommand"/> to remove a MatchedTripReportText from the database.
        /// </summary>
        /// <param name="id">The ID of the MatchedTripReportText in the database.</param>
        /// <returns>A <see cref="DbCommand"/>.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when id is negative or zero.</exception>
        public DbCommand DeleteMatchedTripReportText(long id)
        {
            #region Sanity checks

            if (id < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(id), "Value must be greater than zero.");
            }

            #endregion

            string sql = @"Delete From MatchedTripReportText Where MatchID = @ID";

            SQLiteCommand deleteCommand = new(sql);

            deleteCommand.Parameters.AddWithValue("@ID", id);

            return deleteCommand;
        }

        /// <summary>
        /// Creates a <see cref="DbCommand"/> to fetch a matchDetails from the database.
        /// </summary>
        /// <param name="id">The ID of the matchDetails in the database.</param>
        /// <returns>A <see cref="DbCommand"/>.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when id is negative or zero.</exception>
        public DbCommand GetMatchedTripReportText(long id)
        {
            #region Sanity checks

            if (id < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(id), "Value must be positive and greater than zero.");
            }

            #endregion

            string sql = @"Select * From MatchedTripReportText Where MatchID = @ID";

            SQLiteCommand selectCommand = new(sql);

            selectCommand.Parameters.AddWithValue("@ID", id);

            return selectCommand;
        }

        #endregion

        #region MatchedTripReportTopic

        /// <summary>
        /// Creates a <see cref="DbCommand"/> to add a MatchedTripReportTopic to the database.
        /// </summary>
        /// <param name="url">A <see cref="MatchedTripReportTopic"/> <see cref="DataTransferObject"/>.</param>
        /// <returns>A <see cref="DbCommand"/>.</returns>
        /// <exception cref="ArgumentNullException">Thrown when matchDetails is null.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown ID is not zero.</exception>
        public DbCommand AddMatchedTripReportTopic(MatchedTripReportTopic matchDetails)
        {
            #region Sanity checks

            if (matchDetails is null)
            {
                throw new ArgumentNullException(nameof(matchDetails));
            }

            if (matchDetails.ID != 0)
            {
                throw new ArgumentOutOfRangeException($"{nameof(matchDetails)}.ID has a value of {matchDetails.ID} and can't be added to the database.  If it already exists, call UpdateMatchedTripReportTopic instead.  If not, set its ID to zero.", nameof(matchDetails));
            }

            #endregion

            return this.AddMatchedTripReportTopic(matchDetails.TripReportID, matchDetails.TopicID, matchDetails.Exerpt);
        }

        public DbCommand AddMatchedTripReportTopic(WtaTripReport tripReport, Topic topic, string exerpt)
        {
            #region Sanity checks

            if (tripReport is null)
            {
                throw new ArgumentNullException(nameof(tripReport));
            }

            if (topic is null)
            {
                throw new ArgumentNullException(nameof(topic));
            }

            #endregion

            return this.AddMatchedTripReportTopic(tripReport.ID, topic.ID, exerpt);
        }

        public DbCommand AddMatchedTripReportTopic(long tripReportID, long topicID, string exerpt)
        {
            #region Sanity checks

            if (tripReportID <= 0)
            {
                throw new ArgumentOutOfRangeException($"{nameof(tripReportID)} must be a positive number, it is a foreign key to WtaTripReport.", nameof(tripReportID));
            }


            if (topicID <= 0)
            {
                throw new ArgumentOutOfRangeException($"{nameof(topicID)} must be a positive number, it is a foreign key to Topic.", nameof(topicID));
            }

            #endregion

            string sql = @"
Insert Into MatchedTripReportTopic (TripReportID, TopicID, Exerpt)
Values (@TripReportID, @TopicID, @Exerpt);

Select last_insert_rowid();";

            SQLiteCommand insertCommand = new(sql);

            insertCommand.Parameters.AddWithValue("@TripReportID", tripReportID);
            insertCommand.Parameters.AddWithValue("@TopicID", topicID);
            insertCommand.Parameters.AddWithValue("@Exerpt", exerpt);

            return insertCommand;
        }

        /// <summary>
        /// Creates a <see cref="DbCommand"/> to remove a MatchedTripReportTopic from the database.
        /// </summary>
        /// <param name="url">A <see cref="MatchedTripReportTopic"/> <see cref="DataTransferObject"/> containing the ID of the MatchedTripReportTopic to delete.</param>
        /// <returns>A <see cref="DbCommand"/>.</returns>
        /// <exception cref="ArgumentNullException">Thrown when matchDetails is null.</exception>
        public DbCommand DeleteMatchedTripReportTopic(MatchedTripReportTopic matchDetails)
        {
            #region Sanity checks

            if (matchDetails is null)
            {
                throw new ArgumentNullException(nameof(matchDetails));
            }

            #endregion

            return this.DeleteMatchedTripReportTopic(matchDetails.ID);
        }

        /// <summary>
        /// Creates a <see cref="DbCommand"/> to remove a MatchedTripReportTopic from the database.
        /// </summary>
        /// <param name="id">The ID of the MatchedTripReportTopic in the database.</param>
        /// <returns>A <see cref="DbCommand"/>.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when id is negative or zero.</exception>
        public DbCommand DeleteMatchedTripReportTopic(long id)
        {
            #region Sanity checks

            if (id < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(id), "Value must be greater than zero.");
            }

            #endregion

            string sql = @"Delete From MatchedTripReportTopic Where MatchID = @ID";

            SQLiteCommand deleteCommand = new(sql);

            deleteCommand.Parameters.AddWithValue("@ID", id);

            return deleteCommand;
        }

        /// <summary>
        /// Creates a <see cref="DbCommand"/> to fetch a MatchedTripReportTopic from the database.
        /// </summary>
        /// <param name="id">The ID of the matchDetails in the database.</param>
        /// <returns>A <see cref="DbCommand"/>.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when id is negative or zero.</exception>
        public DbCommand GetMatchedTripReportTopic(long id)
        {
            #region Sanity checks

            if (id < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(id), "Value must be positive and greater than zero.");
            }

            #endregion

            string sql = @"Select * From MatchedTripReportTopic Where MatchID = @ID";

            SQLiteCommand selectCommand = new(sql);

            selectCommand.Parameters.AddWithValue("@ID", id);

            return selectCommand;
        }

        #endregion

        #region ImageUrl

        /// <summary>
        /// Creates a <see cref="DbCommand"/> to add a imageUrl object to the database.
        /// </summary>
        /// <param name="imageUrl">A <see cref="ImageUrl"/> <see cref="DataTransferObject"/>.</param>
        /// <returns>A <see cref="DbCommand"/>.</returns>
        /// <exception cref="ArgumentNullException">Thrown when imageUrl is null.</exception>
        /// <exception cref="ArgumentException">Thrown when imageUrl.Address is null, empty, or white space.</exception>
        public DbCommand AddImageUrl(ImageUrl imageUrl)
        {
            #region Sanity checks

            if (imageUrl == null)
            {
                throw new ArgumentNullException(nameof(imageUrl));
            }

            if (imageUrl.Address == null)
            {
                throw new ArgumentException("Address cannot be null.", nameof(imageUrl));
            }

            if (string.IsNullOrWhiteSpace(imageUrl.Address))
            {
                throw new ArgumentException($"Address cannot be null.", nameof(imageUrl));
            }

            if (imageUrl.ID != 0)
            {
                throw new ArgumentException($"{nameof(imageUrl)}.ID has a value of {imageUrl.ID} and can't be added to the database.", nameof(imageUrl));
            }

            #endregion

            string sql = @"
Insert Into ImageUrl (Url, ImageWidth, ImageHeight, FileSize, Comments)
Values (@ImageUrl, @ImageWidth, @ImageHeight, @FileSize, @Comments)
Where Not Exists (Select * From ImageUrl ix Where ix.Url = @ImageUrl);

Select last_insert_rowid();";

            SQLiteCommand insertCommand = new(sql);

            insertCommand.Parameters.AddWithValue("@ImageUrl", imageUrl.Address);
            insertCommand.Parameters.AddWithValue("@ImageWidth", imageUrl.ImageWidth);
            insertCommand.Parameters.AddWithValue("@ImageHeight", imageUrl.ImageHeight);
            insertCommand.Parameters.AddWithValue("@FileSize", imageUrl.FileSize);
            insertCommand.Parameters.AddWithValue("@Comments", imageUrl.Comments);

            return insertCommand;
        }

        public DbCommand AddImageUrl(string imageUrl)
        {
            #region Sanity checks

            if (imageUrl == null)
            {
                throw new ArgumentNullException(nameof(imageUrl));
            }

            if (string.IsNullOrWhiteSpace(imageUrl))
            {
                throw new ArgumentException($"Address cannot be null.", nameof(imageUrl));
            }

            #endregion

            string sql = @"
Insert Into ImageUrl (Url)
Values (@ImageUrl);

Select last_insert_rowid();";

            SQLiteCommand insertCommand = new(sql);

            insertCommand.Parameters.AddWithValue("@ImageUrl", imageUrl);

            return insertCommand;
        }

        /// <summary>
        /// Creates a <see cref="DbCommand"/> to remove a imageUrl from the database.
        /// </summary>
        /// <param name="imageUrl">A <see cref="ImageUrl"/> <see cref="DataTransferObject"/> containing the ID of the ImageUrl to delete.</param>
        /// <returns>A <see cref="DbCommand"/>.</returns>
        /// <exception cref="ArgumentNullException">Thrown when imageUrl is null.</exception>
        public DbCommand DeleteImageUrl(ImageUrl imageUrl)
        {
            #region Sanity checks

            if (imageUrl is null)
            {
                throw new ArgumentNullException(nameof(imageUrl));
            }

            #endregion

            return this.DeleteImageUrl(imageUrl.ID);
        }

        /// <summary>
        /// Creates a <see cref="DbCommand"/> to remove a imageUrl from the database.
        /// </summary>
        /// <param name="id">The ID of the imageUrl in the database.</param>
        /// <returns>A <see cref="DbCommand"/>.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when id is negative or zero.</exception>
        public DbCommand DeleteImageUrl(long id)
        {
            #region Sanity checks

            if (id < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(id), "Value must be greater than zero.");
            }

            #endregion

            string sql = @"Delete From ImageUrl Where ImageID = @ID";

            SQLiteCommand deleteCommand = new(sql);

            deleteCommand.Parameters.AddWithValue("@ID", id);

            return deleteCommand;
        }

        /// <summary>
        /// Creates a <see cref="DbCommand"/> to fetch a imageUrl from the database.
        /// </summary>
        /// <param name="id">The ID of the imageUrl in the database.</param>
        /// <returns>A <see cref="DbCommand"/>.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when id is negative or zero.</exception>
        public DbCommand GetImageUrl(long id)
        {
            #region Sanity checks

            if (id < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(id), "Value must be positive and greater than zero.");
            }

            #endregion

            string sql = @"Select * From ImageUrl Where ImageID = @ID";

            SQLiteCommand selectCommand = new(sql);

            selectCommand.Parameters.AddWithValue("@ID", id);

            return selectCommand;
        }

        public DbCommand GetImagesForTripReport(long tripReportID)
        {
            string sql = @"
Select img.*
From
    ImageUrl img Join
    TripReportImage tri On img.ImageID = tri.ImageID
Where
    tri.TripReportID = @TripReportID
";

            SQLiteCommand selectCommand = new(sql);
            selectCommand.Parameters.AddWithValue("@TripReportID", tripReportID);

            return selectCommand;
        }

        #endregion
    }
}
