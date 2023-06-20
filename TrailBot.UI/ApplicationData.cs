using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CascadePass.TrailBot.UI
{
    public static class ApplicationData
    {
        public static Settings Settings { get; set; }

        public static WebProviderManager WebProviderManager { get; set; }

        public static bool WasSettingsMissing { get; set; }

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

                MatchedTripReport.IncludeTopicNameInSummary = ApplicationData.WebProviderManager.Topics?.Count > 1;
            }

            return !ApplicationData.WasSettingsMissing;
        }

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
