using CascadePass.TrailBot;
using CascadePass.TrailBot.DataAccess;
using CascadePass.TrailBot.DataAccess.DTO;
using CascadePass.TrailBot.TextAnalysis;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.IO;
using System.Linq;
using System.Text;

namespace TrailBot.DataAccess.Tests.SqliteQueryProviderTests
{
    [TestClass]
    public class UrlTests
    {
        #region AddUrl

        #region Validation

        #region Url can't be blank

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void AddUrl_Null()
        {
            SqliteQueryProvider queryProvider = new();
            queryProvider.AddUrl(null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void AddUrl_Url_Null()
        {
            SqliteQueryProvider queryProvider = new();
            queryProvider.AddUrl(Url.Create(null, DateTime.Now, DateTime.Now, DateTime.Now));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void AddUrl_Url_EmptyString()
        {
            SqliteQueryProvider queryProvider = new();
            queryProvider.AddUrl(Url.Create(string.Empty, DateTime.Now, DateTime.Now, DateTime.Now));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void AddUrl_Url_WhiteSpace()
        {
            SqliteQueryProvider queryProvider = new();
            queryProvider.AddUrl(Url.Create(" ", DateTime.Now, DateTime.Now, DateTime.Now));
        }

        #endregion

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void AddUrl_HasID()
        {
            SqliteQueryProvider queryProvider = new();
            queryProvider.AddUrl(new Url() { ID = 25, Address = "https://test/" });
        }

        #endregion

        [TestMethod]
        public void AddUrl_ResultIsNotNull()
        {
            SqliteQueryProvider queryProvider = new();
            var result = queryProvider.AddUrl(Url.Create("https://test/", DateTime.Now, DateTime.Now, DateTime.Now));

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void AddUrl_ResultIsCorrectType()
        {
            SqliteQueryProvider queryProvider = new();
            var result = queryProvider.AddUrl(Url.Create("https://test/", DateTime.Now, DateTime.Now, DateTime.Now));

            Console.WriteLine(result.GetType().FullName);
            Assert.IsTrue(result.GetType().Name == "SQLiteCommand");
        }

        [TestMethod]
        public void AddUrl_CommandText()
        {
            SqliteQueryProvider queryProvider = new();
            var result = queryProvider.AddUrl(Url.Create("https://test/", DateTime.Now, DateTime.Now, DateTime.Now));

            Console.WriteLine(result.CommandText);
            Assert.IsTrue(result.CommandText.Trim().ToUpper().StartsWith("INSERT INTO URL"));
            Assert.IsTrue(result.CommandText.Trim().ToUpper().Contains("VALUES"));
            Assert.IsTrue(result.CommandText.Trim().ToUpper().Contains("SELECT LAST_INSERT_ROWID();"));
        }

        [TestMethod]
        public void AddUrl_QueryIsParameterized()
        {
            SqliteQueryProvider queryProvider = new();
            var result = queryProvider.AddUrl(Url.Create("https://test/", DateTime.Now, DateTime.Now, DateTime.Now));

            Console.WriteLine(result.CommandText);

            foreach (DbParameter parameter in result.Parameters)
            {
                Console.WriteLine(parameter.ParameterName + "\t" + parameter.Value);
            }

            // None of the input values should appear in the text
            Assert.IsFalse(result.CommandText.Trim().ToUpper().Contains("HTTPS://TEST/"));

            // Instead, there should be parameters with these values
            Assert.IsTrue(result.Parameters.Contains("@Url"));
            Assert.AreEqual("https://test/", result.Parameters["@Url"].Value);
        }

        [TestMethod]
        public void AddUrl_NullFoundDateDefaultsToNow()
        {
            SqliteQueryProvider queryProvider = new();
            var result = queryProvider.AddUrl(Url.Create("https://test/", null, DateTime.Now, DateTime.Now));

            Console.WriteLine(result.CommandText);

            foreach (DbParameter parameter in result.Parameters)
            {
                Console.WriteLine(parameter.ParameterName + "\t" + parameter.Value);
            }

            long linuxTimestamp = (long)result.Parameters["@FoundDate"].Value;
            DateTime timestamp = DataTransferObject.ToDateTime(linuxTimestamp);
            TimeSpan elapsed = DateTime.Now - timestamp;

            Assert.IsTrue(elapsed.Seconds < 2);
        }

        [TestMethod]
        public void AddUrl_AllDatesAreUnixTimestamps_Explicit()
        {
            SqliteQueryProvider queryProvider = new();
            var result = queryProvider.AddUrl(Url.Create("https://test/", DateTime.Now, DateTime.Now, DateTime.Now));

            Console.WriteLine(result.CommandText);

            foreach (DbParameter parameter in result.Parameters)
            {
                Console.WriteLine(parameter.ParameterName + "\t" + parameter.Value);
            }

            Assert.IsTrue(result.Parameters["@FoundDate"].Value is long, $"@FoundDate is {result.Parameters["@FoundDate"].Value.GetType()}");
            Assert.IsTrue(result.Parameters["@CollectedDate"].Value is long, $"@CollectedDate is {result.Parameters["@CollectedDate"].Value.GetType()}");
            Assert.IsTrue(result.Parameters["@IntentLockedDate"].Value is long, $"@IntentLockedDate is {result.Parameters["@IntentLockedDate"].Value.GetType()}");
        }

        [TestMethod]
        public void AddUrl_AllDatesAreUnixTimestamps_Default()
        {
            SqliteQueryProvider queryProvider = new();
            var result = queryProvider.AddUrl(Url.Create("https://test/", null, null, null));

            Console.WriteLine(result.CommandText);

            foreach (DbParameter parameter in result.Parameters)
            {
                Console.WriteLine(parameter.ParameterName + "\t" + parameter.Value);
            }

            Assert.IsTrue(result.Parameters["@FoundDate"].Value is long, $"@FoundDate is {result.Parameters["@FoundDate"].Value.GetType()}");
            Assert.IsTrue(result.Parameters["@CollectedDate"].Value == Convert.DBNull, $"@CollectedDate is {result.Parameters["@CollectedDate"].Value.GetType()}");
            Assert.IsTrue(result.Parameters["@IntentLockedDate"].Value == Convert.DBNull, $"@IntentLockedDate is {result.Parameters["@IntentLockedDate"].Value.GetType()}");

        }

        #endregion

        #region UpdateUrl

        #region ID must be in range

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void UpdateUrl_Zero()
        {
            SqliteQueryProvider queryProvider = new();
            queryProvider.UpdateUrl(0, DateTime.Now, DateTime.Now);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void UpdateUrl_Negative()
        {
            SqliteQueryProvider queryProvider = new();
            queryProvider.UpdateUrl(-25, DateTime.Now, DateTime.Now);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void UpdateUrl_Null()
        {
            SqliteQueryProvider queryProvider = new();
            queryProvider.UpdateUrl(null);
        }

        #endregion

        [TestMethod]
        public void UpdateUrl_ResultIsNotNull()
        {
            SqliteQueryProvider queryProvider = new();
            var result = queryProvider.UpdateUrl(100, DateTime.Now, DateTime.Now);

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void UpdateUrl_ResultIsCorrectType()
        {
            SqliteQueryProvider queryProvider = new();
            var result = queryProvider.UpdateUrl(100, DateTime.Now, DateTime.Now);

            Console.WriteLine(result.GetType().FullName);
            Assert.IsTrue(result.GetType().Name == "SQLiteCommand");
        }

        [TestMethod]
        public void UpdateUrl_CommandText()
        {
            SqliteQueryProvider queryProvider = new();
            var result = queryProvider.UpdateUrl(100, DateTime.Now, DateTime.Now);

            Console.WriteLine(result.CommandText);
            Assert.IsTrue(result.CommandText.Trim().ToUpper().StartsWith("UPDATE URL"));
        }

        [TestMethod]
        public void UpdateUrl_QueryIsParameterized()
        {
            SqliteQueryProvider queryProvider = new();
            var result = queryProvider.UpdateUrl(100, DateTime.Now, DateTime.Now);

            Console.WriteLine(result.CommandText);

            foreach (DbParameter parameter in result.Parameters)
            {
                Console.WriteLine(parameter.ParameterName + "\t" + parameter.Value);
            }

            Assert.IsTrue(result.Parameters.Contains("@CollectedDate"));
            Assert.IsTrue(result.Parameters.Contains("@IntentLockedDate"));
        }

        [TestMethod]
        public void UpdateUrl_CannotEditUrl()
        {
            SqliteQueryProvider queryProvider = new();
            var result = queryProvider.UpdateUrl(100, DateTime.Now, DateTime.Now);

            Console.WriteLine(result.CommandText);

            foreach (DbParameter parameter in result.Parameters)
            {
                Console.WriteLine(parameter.ParameterName + "\t" + parameter.Value);
            }

            Assert.IsFalse(result.Parameters.Contains("@Url"));
        }

        [TestMethod]
        public void UpdateUrl_CannotEditFoundDate()
        {
            SqliteQueryProvider queryProvider = new();
            var result = queryProvider.UpdateUrl(100, DateTime.Now, DateTime.Now);

            Console.WriteLine(result.CommandText);

            foreach (DbParameter parameter in result.Parameters)
            {
                Console.WriteLine(parameter.ParameterName + "\t" + parameter.Value);
            }

            Assert.IsFalse(result.Parameters.Contains("@FoundDate"));
        }

        [TestMethod]
        public void UpdateUrl_AllDatesAreUnixTimestamps_Explicit()
        {
            SqliteQueryProvider queryProvider = new();
            var result = queryProvider.UpdateUrl(500, DateTime.Now, DateTime.Now);

            Console.WriteLine(result.CommandText);

            foreach (DbParameter parameter in result.Parameters)
            {
                Console.WriteLine(parameter.ParameterName + "\t" + parameter.Value);
            }

            Assert.IsTrue(result.Parameters["@CollectedDate"].Value is long, $"@CollectedDate is {result.Parameters["@CollectedDate"].Value.GetType()}");
            Assert.IsTrue(result.Parameters["@IntentLockedDate"].Value is long, $"@IntentLockedDate is {result.Parameters["@IntentLockedDate"].Value.GetType()}");
        }

        [TestMethod]
        public void UpdateUrl_AllDatesAreUnixTimestamps_Default()
        {
            SqliteQueryProvider queryProvider = new();
            var result = queryProvider.UpdateUrl(500, null, null);

            Console.WriteLine(result.CommandText);

            foreach (DbParameter parameter in result.Parameters)
            {
                Console.WriteLine(parameter.ParameterName + "\t" + parameter.Value);
            }

            Assert.IsTrue(result.Parameters["@CollectedDate"].Value == Convert.DBNull, $"@CollectedDate is {result.Parameters["@CollectedDate"].Value.GetType()}");
            Assert.IsTrue(result.Parameters["@IntentLockedDate"].Value == Convert.DBNull, $"@IntentLockedDate is {result.Parameters["@IntentLockedDate"].Value.GetType()}");

        }

        #endregion

        #region DeleteUrl

        #region By ID

        #region Validation (ID must be in range)

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void DeleteUrl_ID_Zero()
        {
            SqliteQueryProvider queryProvider = new();
            queryProvider.DeleteUrl(0);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void DeleteUrl_ID_Negative()
        {
            SqliteQueryProvider queryProvider = new();
            queryProvider.DeleteUrl(-25);
        }

        #endregion

        #region Correctness

        [TestMethod]
        public void DeleteUrl_ResultIsNotNull()
        {
            SqliteQueryProvider queryProvider = new();
            var result = queryProvider.DeleteUrl(100);

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void DeleteUrl_ResultIsCorrectType()
        {
            SqliteQueryProvider queryProvider = new();
            var result = queryProvider.DeleteUrl(100);

            Console.WriteLine(result.GetType().FullName);
            Assert.IsTrue(result.GetType().Name == "SQLiteCommand");
        }

        [TestMethod]
        public void DeleteUrl_CommandText()
        {
            SqliteQueryProvider queryProvider = new();
            var result = queryProvider.DeleteUrl(100);

            Console.WriteLine(result.CommandText);
            Assert.IsTrue(result.CommandText.Trim().ToUpper().StartsWith("DELETE FROM URL"));
        }

        [TestMethod]
        public void DeleteUrl_QueryIsParameterized()
        {
            SqliteQueryProvider queryProvider = new();
            var result = queryProvider.DeleteUrl(int.MaxValue);

            Console.WriteLine(result.CommandText);

            foreach (DbParameter parameter in result.Parameters)
            {
                Console.WriteLine(parameter.ParameterName + "\t" + parameter.Value);
            }

            Assert.IsTrue(result.Parameters.Contains("@ID"));
            Assert.IsFalse(result.CommandText.Contains(int.MaxValue.ToString()));
        }

        #endregion

        #endregion

        #region By DTO

        #region Validation

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void DeleteUrl_ByDTO_null()
        {
            SqliteQueryProvider queryProvider = new();
            queryProvider.DeleteUrl(null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void DeleteUrl_ByDTO_ID_Zero()
        {
            SqliteQueryProvider queryProvider = new();
            queryProvider.DeleteUrl(new Url() { ID = 0 });
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void DeleteUrl_ByDTO_ID_Negative()
        {
            SqliteQueryProvider queryProvider = new();
            queryProvider.DeleteUrl(new Url() { ID = -389247 });
        }
        #endregion

        #region Correctness

        [TestMethod]
        public void DeleteUrl_ByDTO_ResultIsNotNull()
        {
            SqliteQueryProvider queryProvider = new();
            var result = queryProvider.DeleteUrl(new Url() { ID = 100 });

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void DeleteUrl_ByDTO_ResultIsCorrectType()
        {
            SqliteQueryProvider queryProvider = new();
            var result = queryProvider.DeleteUrl(new Url() { ID = 100 });

            Console.WriteLine(result.GetType().FullName);
            Assert.IsTrue(result.GetType().Name == "SQLiteCommand");
        }

        [TestMethod]
        public void DeleteUrl_ByDTO_CommandText()
        {
            SqliteQueryProvider queryProvider = new();
            var result = queryProvider.DeleteUrl(new Url() { ID = 100 });

            Console.WriteLine(result.CommandText);
            Assert.IsTrue(result.CommandText.Trim().ToUpper().StartsWith("DELETE FROM URL"));
        }

        [TestMethod]
        public void DeleteUrl_ByDTO_QueryIsParameterized()
        {
            SqliteQueryProvider queryProvider = new();
            var result = queryProvider.DeleteUrl(new Url() { ID = int.MaxValue });

            Console.WriteLine(result.CommandText);

            foreach (DbParameter parameter in result.Parameters)
            {
                Console.WriteLine(parameter.ParameterName + "\t" + parameter.Value);
            }

            Assert.IsTrue(result.Parameters.Contains("@ID"));
            Assert.IsFalse(result.CommandText.Contains(int.MaxValue.ToString()));
        }

        #endregion

        #endregion

        #endregion

        #region GetUrl (by ID)

        #region ID must be in range

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void GetUrlByID_Zero()
        {
            SqliteQueryProvider queryProvider = new();
            queryProvider.GetUrl(0);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void GetUrlByID_Negative()
        {
            SqliteQueryProvider queryProvider = new();
            queryProvider.GetUrl(-25);
        }

        #endregion

        [TestMethod]
        public void GetUrlByID_ResultIsNotNull()
        {
            SqliteQueryProvider queryProvider = new();
            var result = queryProvider.GetUrl(100);

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void GetUrlByID_ResultIsCorrectType()
        {
            SqliteQueryProvider queryProvider = new();
            var result = queryProvider.GetUrl(100);

            Console.WriteLine(result.GetType().FullName);
            Assert.IsTrue(result.GetType().Name == "SQLiteCommand");
        }

        [TestMethod]
        public void GetUrlByID_CommandText()
        {
            SqliteQueryProvider queryProvider = new();
            var result = queryProvider.GetUrl(100);

            Console.WriteLine(result.CommandText);
            Assert.IsTrue(result.CommandText.Trim().ToUpper().StartsWith("SELECT"));
            Assert.IsTrue(result.CommandText.Trim().ToUpper().Contains("FROM URL"));
        }

        [TestMethod]
        public void GetUrlByID_QueryIsParameterized()
        {
            SqliteQueryProvider queryProvider = new();
            var result = queryProvider.GetUrl(int.MaxValue);

            Console.WriteLine(result.CommandText);

            foreach (DbParameter parameter in result.Parameters)
            {
                Console.WriteLine(parameter.ParameterName + "\t" + parameter.Value);
            }

            Assert.IsTrue(result.Parameters.Contains("@ID"));
            Assert.IsFalse(result.CommandText.Contains(int.MaxValue.ToString()));
        }

        #endregion

        #region GetUrl (by text/url)

        #region Url can't be blank

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void GetUrlByText_id_Null()
        {
            SqliteQueryProvider queryProvider = new();
            queryProvider.GetUrl(null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void GetUrlByText_id_EmptyString()
        {
            SqliteQueryProvider queryProvider = new();
            queryProvider.GetUrl(string.Empty);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void GetUrlByText_id_WhiteSpace()
        {
            SqliteQueryProvider queryProvider = new();
            queryProvider.GetUrl("\t");
        }

        #endregion

        [TestMethod]
        public void GetUrlByText_ResultIsNotNull()
        {
            SqliteQueryProvider queryProvider = new();
            var result = queryProvider.GetUrl("https://test/");

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void GetUrlByText_ResultIsCorrectType()
        {
            SqliteQueryProvider queryProvider = new();
            var result = queryProvider.GetUrl("https://test/");

            Console.WriteLine(result.GetType().FullName);
            Assert.IsTrue(result.GetType().Name == "SQLiteCommand");
        }

        [TestMethod]
        public void GetUrlByText_CommandText()
        {
            SqliteQueryProvider queryProvider = new();
            var result = queryProvider.GetUrl("https://test/");

            Console.WriteLine(result.CommandText);
            Assert.IsTrue(result.CommandText.Trim().ToUpper().StartsWith("SELECT"));
            Assert.IsTrue(result.CommandText.Trim().ToUpper().Contains("FROM URL"));
        }

        [TestMethod]
        public void GetUrlByText_QueryIsParameterized()
        {
            SqliteQueryProvider queryProvider = new();
            var result = queryProvider.GetUrl("https://test/");

            Console.WriteLine(result.CommandText);

            foreach (DbParameter parameter in result.Parameters)
            {
                Console.WriteLine(parameter.ParameterName + "\t" + parameter.Value);
            }

            Assert.IsTrue(result.Parameters.Contains("@Url"));
            Assert.IsFalse(result.CommandText.Contains("https://test/"));
        }

        #endregion

        #region LockUrlsForCollection

        [TestMethod]
        public void LockUrlsForCollection_ResultIsNotNull()
        {
            SqliteQueryProvider queryProvider = new();
            var result = queryProvider.LockUrlsForCollection();

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void LockUrlsForCollection_ResultIsCorrectType()
        {
            SqliteQueryProvider queryProvider = new();
            var result = queryProvider.LockUrlsForCollection();

            Console.WriteLine(result.GetType().FullName);
            Assert.IsTrue(result.GetType().Name == "SQLiteCommand");
        }

        [TestMethod]
        public void LockUrlsForCollection_CommandText()
        {
            SqliteQueryProvider queryProvider = new();
            var result = queryProvider.LockUrlsForCollection();

            Console.WriteLine(result.CommandText);
            Assert.IsTrue(result.CommandText.Trim().ToUpper().Contains("UPDATE URL"));
            Assert.IsTrue(result.CommandText.Trim().ToUpper().Contains("CREATE TEMP TABLE"));
        }

        #endregion
    }

    //[TestClass]
    public class TempExport
    {
        [TestMethod]
        public void ExportLarchTripReports()
        {
            Database.ConnectionString = "Data Source=C:\\Users\\User\\Documents\\TrailBot\\TrailBot-test.db";
            Database.QueryProvider = new SqliteQueryProvider();

            int larchTRcount = 0, nonLarchTRcount = 0;
            List<TripReport> larchTRs = new(), nonLarchTRs = new();
            StringBuilder allRelevantReportText = new(), allIrrelevantText = new();

            foreach (string filename in Directory.GetFiles("C:\\Users\\User\\Documents\\TrailBot\\TRs"))
            {
                var tr = CascadePass.TrailBot.FileStore.DeserializeFromXmlFile<CascadePass.TrailBot.WtaTripReport>(filename);

                if (tr.ReportText.ToLower().Contains("larch"))
                {
                    string destinationFilename = Path.Combine(@"C:\Users\User\Documents\TrailBot\LarchTRs", Path.GetFileName(filename));

                    if (File.Exists(destinationFilename))
                    {
                        File.Delete(destinationFilename);
                    }

                    File.Copy(
                        filename,
                        destinationFilename
                        );
                }

                if (tr.TripDate.Year == 2023 && (tr.TripDate.Month == 9 || tr.TripDate.Month == 10))
                {
                    if (tr.ReportText.ToLower().Contains("larch"))
                    {
                        allRelevantReportText.AppendLine(tr.ReportText);
                        larchTRcount += 1;
                        larchTRs.Add(tr);
                    }
                    else
                    {
                        allIrrelevantText.AppendLine(tr.ReportText);
                        nonLarchTRcount += 1;
                        nonLarchTRs.Add(tr);
                    }
                }
            }

            Tokenizer larchTokenizer = new(), nonLarchTokenizer = new();
            larchTokenizer.GetTokens(allRelevantReportText.ToString().ToLower());
            nonLarchTokenizer.GetTokens(allIrrelevantText.ToString().ToLower());

            double
                larchTotalWordCount = larchTokenizer.OrderedTokens.Count(m => m.IsWord),
                nonLarchTotalWordCount = nonLarchTokenizer.OrderedTokens.Count(m => m.IsWord);

            string[] stopwords = { "was", "but", "for", "there", "with", "were", "this", "that", "you", "are", "from", "so", "not", "out", "had",
                "the", "and", "to", "of", "in", "on", "at", "is", "we", "it", "as", "some", "be", "about", "just", "all", "my", "day", "one", "no",
                "more", "very", "or", "few", "get", "our", "an", "would", "by", "which", "if", "got", "then", "than", "did", "where", "they", "your",
                "also", "well", "lots", "bit", "will", "can", "off", "us", "it's", "its", "his", "hers", "him", "her", "do", "make"
            };
            List<WordComparisonItem> terms = new();
            foreach (var item in larchTokenizer.TokenCounts.Where(m => m.Token.IsWord))
            {
                if (stopwords.Contains(item.Token.Text))
                {
                    continue;
                }

                int larchCount = larchTRs.Count(m => m.ReportText.Contains(item.Token.Text));
                WordComparisonItem compare = new()
                {
                    Word = item.Token.Text,
                    LarchWordCount = item.Count,
                    LarchWordPercent = 100 * ((double)item.Count / larchTotalWordCount),
                    LarchDocumentCount = larchCount,
                    LarchDocumentPercent = 100 * ((double)larchCount / (double)larchTRcount),
                };

                if (nonLarchTokenizer.OrderedTokens.Any(m => string.Equals(m.Text, item.Token.Text, StringComparison.OrdinalIgnoreCase)))
                {
                    var nonLarchItem = nonLarchTokenizer.TokenCounts.FirstOrDefault(m => string.Equals(m.Token.Text, item.Token.Text, StringComparison.OrdinalIgnoreCase));

                    compare.NonLarchWordCount = nonLarchItem.Count;
                    compare.NonLarchWordPercent = 100 * ((double)nonLarchItem.Count / nonLarchTotalWordCount);
                    compare.NonLarchDocumentCount = nonLarchTRs.Count(m => m.ReportText.Contains(item.Token.Text));
                    compare.NonLarchDocumentPercent = 100 * ((double)compare.NonLarchDocumentCount / (double)nonLarchTRcount);
                }

                terms.Add(compare);
            }




            var list = terms.OrderByDescending(m => m.WordDifference);

            int i = 0;
            Console.WriteLine($"{larchTRcount.ToString("#,##0")} reports with {larchTokenizer.OrderedTokens.Count(m => m.IsWord).ToString("#,##0")} words - larch.");
            Console.WriteLine($"{nonLarchTRcount.ToString("#,##0")} reports with {nonLarchTokenizer.OrderedTokens.Count(m => m.IsWord).ToString("#,##0")} words - no larch.");

            foreach (var item in list)
            {
                if (item.LarchDocumentPercent + item.NonLarchDocumentPercent >= 200)
                {
                    continue;
                }

                Console.WriteLine(string.Format(
                    "{0}:\t{1} vs {2} words in\t\t{3} vs {4} TRs\t\t({5} vs {6} % words)\t\t({7} vs {8} % TR)\t\t{9}",
                    $"   {i}".Substring($"   {i}".Length - 2),

                    item.LarchWordCount.ToString("#,##0"),
                    item.NonLarchWordCount.ToString("#,##0"),

                    item.LarchDocumentCount.ToString("#,##0"),
                    item.NonLarchDocumentCount.ToString("#,##0"),

                    item.LarchWordPercent.ToString("0.0"),
                    item.NonLarchWordPercent.ToString("0.0"),

                    item.LarchDocumentPercent.ToString("0.0"),
                    item.NonLarchDocumentPercent.ToString("0.0"),

                    item.Word
                    )
                    //$"{i}: {item.LarchWordCount.ToString("#,##0")}/{item.LarchDocumentCount.ToString("#,##0")}\t\t({item.LarchWordPercent.ToString("0.00")}/{item.LarchDocumentPercent.ToString("0.00")})\t\t\t\t{item.Word}\t\t{item.NonLarchWordCount.ToString("#,##0")}\\{item.NonLarchDocumentCount.ToString("#,##0")}\t\t({item.NonLarchWordPercent.ToString("0.00")}\\{item.NonLarchDocumentPercent.ToString("0.00")})"
                    );

                if (i++ > 25)
                {
                    //break;
                }
            }
        }
    }

    public class WordComparisonItem
    {
        public string Word { get; set; }

        public int LarchWordCount { get; set; }

        public int NonLarchWordCount { get; set; }

        public double LarchWordPercent { get; set; }

        public double NonLarchWordPercent { get; set; }

        public int LarchDocumentCount { get; set; }

        public int NonLarchDocumentCount { get; set; }

        public double LarchDocumentPercent { get; set; }

        public double NonLarchDocumentPercent { get; set; }

        public double DocumentDifference => Math.Abs(this.LarchDocumentPercent - this.NonLarchDocumentPercent);

        public double WordDifference => Math.Abs(this.LarchWordPercent - this.NonLarchWordPercent);
    }

}
