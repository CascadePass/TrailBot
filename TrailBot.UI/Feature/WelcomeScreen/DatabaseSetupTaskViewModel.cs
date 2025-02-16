using CascadePass.TrailBot.DataAccess;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CascadePass.TrailBot.UI.Feature.WelcomeScreen
{
    public class DatabaseSetupTaskViewModel : SetupTaskViewModel
    {
        private bool? databaseFileIsMissing, canConnectToDatabase;
        private string databaseFilename;

        public Settings Settings { get; set; }

        public string DatabaseFilename
        {
            get => this.databaseFilename;
            set
            {
                if (!string.Equals(this.databaseFilename, value, StringComparison.Ordinal))
                {
                    this.databaseFilename = value;
                    this.Settings.SqliteDatabaseFilename = value;
                    this.Validate();
                    this.OnPropertyChanged(nameof(this.DatabaseFilename));
                }
            }
        }

        public bool? DatabaseFileIsMissing
        {
            get => this.databaseFileIsMissing;
            private set
            {
                if (this.databaseFileIsMissing != value)
                {
                    this.databaseFileIsMissing = value;
                    this.OnPropertyChanged(nameof(this.DatabaseFileIsMissing));
                }
            }
        }

        public bool? CanConnectToDatabase
        {
            get => this.canConnectToDatabase;
            private set
            {
                if (this.canConnectToDatabase != value)
                {
                    this.canConnectToDatabase = value;
                    this.OnPropertyChanged(nameof(this.CanConnectToDatabase));
                }
            }
        }

        private void TestFileExistence()
        {
            if (string.IsNullOrWhiteSpace(this.databaseFilename))
            {
                this.DatabaseFileIsMissing = null;
            }

            this.DatabaseFileIsMissing = !File.Exists(this.databaseFilename);
        }

        private void TestDatabaseConnection()
        {
            var currentConnectionString = Database.ConnectionString;
            Database.ConnectionString = Database.GetConnectionString(this.DatabaseFilename);

            try
            {
                var conn = Database.GetConnection();

                if (conn.State != System.Data.ConnectionState.Open)
                {
                    conn.Open();
                }

                conn.Close();
                this.canConnectToDatabase = true;
            }
            catch (Exception ex)
            {
                this.canConnectToDatabase = false;
            }
            finally
            {
                Database.ConnectionString = currentConnectionString;
            }
        }

        public override void Validate()
        {
            this.databaseFileIsMissing = null;
            this.canConnectToDatabase = null;

            this.TestFileExistence();

            if (!this.databaseFileIsMissing.Value)
            {
                this.TestDatabaseConnection();
            }

            this.IsComplete = this.canConnectToDatabase.HasValue && this.canConnectToDatabase.Value;
        }
    }
}
