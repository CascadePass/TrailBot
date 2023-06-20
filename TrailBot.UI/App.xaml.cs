using System.Windows;

namespace CascadePass.TrailBot.UI
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private const string SETTINGS_FILENAME = "TripReporterSettings.xml";

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            ApplicationData.Load(App.SETTINGS_FILENAME);

            MainWindow mainWindow = new()
            {
                Settings = ApplicationData.Settings,
                CurrentContent = FeaturecreenProvider.GetFirstScreen(ApplicationData.WasSettingsMissing),
            };

            mainWindow.Show();
        }

        protected override void OnExit(ExitEventArgs e)
        {            
            base.OnExit(e);

            ApplicationData.Save(App.SETTINGS_FILENAME);
        }
    }
}
