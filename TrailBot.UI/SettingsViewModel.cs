using System;
using System.Collections.Generic;
using System.Windows;

namespace CascadePass.TrailBot.UI
{
    public class SettingsViewModel : ObservableObject
    {
        private List<SupportedBrowser> browserTypes;

        public SettingsViewModel()
        {
            if (Application.Current is App tripReportReaderApplication)
            {
                //TODO: Pass this from the outside when showing the view associated w/ this
                this.ApplicationSettings = ApplicationData.Settings;
                this.WebProviderManager = ApplicationData.WebProviderManager;
            }
        }

        public Settings ApplicationSettings { get; set; }

        public WebProviderManager WebProviderManager { get; set; }

        public List<SupportedBrowser> SupportedBrowsers
        {
            get
            {
                if (this.browserTypes == null)
                {
                    this.browserTypes = new();

                    foreach (var item in Enum.GetValues(typeof(SupportedBrowser)))
                    {
                        if ((int)item != 0)
                        {
                            this.browserTypes.Add((SupportedBrowser)item);
                        }
                    }
                }

                return this.browserTypes;
            }
        }
    }
}
