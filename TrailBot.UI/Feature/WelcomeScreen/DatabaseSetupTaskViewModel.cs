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
        private bool? databaseFileExists;
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
                    this.TestFileExistence();
                    this.Validate();
                    this.OnPropertyChanged(nameof(this.DatabaseFilename));
                }
            }
        }

        public bool? DatabaseFileExists
        {
            get => this.databaseFileExists;
            set
            {
                if (this.databaseFileExists != value)
                {
                    this.databaseFileExists = value;
                    this.OnPropertyChanged(nameof(this.DatabaseFileExists));
                }
            }
        }

        private void TestFileExistence()
        {
            if (string.IsNullOrWhiteSpace(this.databaseFilename))
            {
                this.DatabaseFileExists = null;
            }

            this.DatabaseFileExists = File.Exists(this.databaseFilename);
        }

        public override void Validate()
        {
            throw new NotImplementedException();
        }
    }
}
