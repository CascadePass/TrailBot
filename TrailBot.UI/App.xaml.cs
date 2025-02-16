using CascadePass.TrailBot.DataAccess;
using CascadePass.TrailBot.UI.Feature.WelcomeScreen;
using System;
using System.IO;
using System.Windows;

namespace CascadePass.TrailBot.UI
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private const string SETTINGS_FILENAME = "TripReporterSettings.xml";

        public static bool RequireSetupScreen
        {
            get
            {
                if (ApplicationData.Settings is null)
                {
                    return true;
                }

                return
                    ApplicationData.WasSettingsMissing ||
                    string.IsNullOrWhiteSpace(ApplicationData.Settings.SqliteDatabaseFilename) ||
                    !File.Exists(ApplicationData.Settings.SqliteDatabaseFilename)
                ;
            }
        }

        public static void GetSettings()
        {
            ApplicationData.Settings = new();
            ApplicationData.Settings.SqliteDatabaseFilename = @"C:\Users\forre\OneDrive\Documents\TrailBot-DEV.db";
            //ApplicationData.Settings.IndexFilename = @"C:\Users\User\Documents\TrailBot\Index.xml";
            ApplicationData.Load(App.SETTINGS_FILENAME);

            Database.ConnectionString = Database.GetConnectionString(ApplicationData.Settings?.SqliteDatabaseFilename);
            Database.QueryProvider = new SqliteQueryProvider();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            App.GetSettings();

            if (App.RequireSetupScreen)
            {
                new SetupHostWindow().ShowDialog();
            }
            else
            {
                MainWindow mainWindow = new()
                {
                    Settings = ApplicationData.Settings,
                    CurrentContent = FeaturecreenProvider.GetFirstScreen(App.RequireSetupScreen),
                };

                mainWindow.Show();
            }
        }

        protected override void OnExit(ExitEventArgs e)
        {            
            base.OnExit(e);

            //ApplicationData.Save(App.SETTINGS_FILENAME);
        }
    }
}
