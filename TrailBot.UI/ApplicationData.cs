using System.Collections.Generic;
using System.IO;

namespace CascadePass.TrailBot.UI
{
    /// <summary>
    /// References to data shared across the entire application, as well
    /// as code to load and save required data.
    /// </summary>
    public static class ApplicationData
    {
        /// <summary>
        /// Gets or sets the <see cref="Settings"/> being used.
        /// </summary>
        public static Settings Settings { get; set; }

        /// <summary>
        /// Gets or sets a reference to the <see cref="WebProviderManager"/>.
        /// </summary>
        public static WebProviderManager WebProviderManager { get; set; }

        /// <summary>
        /// Gets or sets a value indicating that the <see cref="Settings"/>
        /// object was just created, rather than having been loaded from
        /// the file system.
        /// </summary>
        public static bool WasSettingsMissing { get; set; }

        /// <summary>
        /// Loads the <see cref="ApplicationData"/> from xml files.
        /// </summary>
        /// <param name="settingsFilename">The name of the settings file to use.</param>
        /// <returns>True if successful, false otherwise.</returns>
        public static bool Load(string settingsFilename)
        {
            ApplicationData.Settings = FileStore.DeserializeFromXmlFile<Settings>(settingsFilename);

            if (ApplicationData.Settings == null)
            {
                ApplicationData.Settings = new();
                ApplicationData.WasSettingsMissing = true;
            }
            else
            {
                ApplicationData.WebProviderManager = WebProviderManager.ReadProvidersFromFile(ApplicationData.Settings.IndexFilename);

                ApplicationData.WebProviderManager.IndexFile = ApplicationData.Settings.IndexFilename;

                ApplicationData.WebProviderManager.Topics = FileStore.DeserializeFromXmlFile<List<Topic>>(Path.Combine(ApplicationData.Settings.XmlFolder, "Topics.xml"));



                ApplicationData.WebProviderManager.Topics = FileStore.DeserializeFromXmlFile<List<Topic>>(@"C:\Users\User\Documents\TrailBot\Topics.xml");



                MatchedTripReport.IncludeTopicNameInSummary = ApplicationData.WebProviderManager.Topics?.Count > 1;
            }

            return !ApplicationData.WasSettingsMissing;
        }

        /// <summary>
        /// Saves the <see cref="ApplicationData"/> to disc.
        /// </summary>
        /// <param name="settingsFilename">The name of the settings file to use.</param>
        public static void Save(string settingsFilename)
        {
            FileStore.SerializeToXmlFile(ApplicationData.Settings, settingsFilename);

            //FileStore.SerializeToXmlFile(this.WebProviderManager, "Index.xml");
            ApplicationData.WebProviderManager?.SaveToFile();

            FileStore.SerializeToXmlFile(
                ApplicationData.WebProviderManager.Topics,
                Path.Combine(ApplicationData.Settings.XmlFolder, "Topics.xml")
            );

            foreach (var provider in ApplicationData.WebProviderManager.DataProviders)
            {
                FileStore.SerializeToXmlFile(
                    provider,
                    Path.Combine(ApplicationData.Settings.XmlFolder, $"{provider.TripReportSource}.xml")
                );
            }
        }
    }
}
