using System;
using System.Collections.Generic;
using System.IO;

namespace CascadePass.TrailBot.UI.Feature.WelcomeScreen
{
    public class WelcomeSetupFeatureViewModel : ViewModel
    {
        public WelcomeSetupFeatureViewModel() : base()
        {
            this.Settings = new();

            this.WebProviderManager = new();
            this.WebProviderManager.AddDefaultProviders();

            this.WebProviderManager.Topics = new() {
                new() {
                    MatchAny = "one\r\nkeyword\r\nor phrase\r\nper\r\nline",
                    MatchAnyUnless = "Anything listed here will prevent a match from counting as a match.  If you want to find any trip report that mentions snow, you would use \"no snow\" as an unless term."
                }
            };

            this.Tasks = new();

            this.DataStorageTaskViewModel = new()
            {
                Settings = this.Settings,
                XmlFolderPath = Path.Combine(Environment.CurrentDirectory, "Data"),
            };

            this.ProviderSetupViewModel = new(this.WebProviderManager);
            this.TopicSetupViewModel = new();

            this.DataStorageTaskViewModel.CompletenessChanged += this.SetupTask_Completed;
            this.ProviderSetupViewModel.CompletenessChanged += this.SetupTask_Completed;

            this.Tasks.Add(this.DataStorageTaskViewModel);
            this.Tasks.Add(this.ProviderSetupViewModel);
            this.Tasks.Add(this.TopicSetupViewModel);

            ApplicationData.WebProviderManager = this.WebProviderManager;
            ApplicationData.Settings = this.Settings;

            this.SetupTask_Completed(null, null);
        }

        public WebProviderManager WebProviderManager { get; set; }

        public Settings Settings { get; set; }

        public List<SetupTaskViewModel> Tasks { get; set; }

        public SetupDataStorageTaskViewModel DataStorageTaskViewModel { get; set; }

        public ProviderSetupViewModel ProviderSetupViewModel { get; set; }

        public TopicSetupViewModel TopicSetupViewModel { get; set; }

        private void SetupTask_Completed(object sender, EventArgs e)
        {
            this.TopicSetupViewModel.EnableTaskEditorButton = this.DataStorageTaskViewModel.IsComplete && this.ProviderSetupViewModel.IsComplete;
        }
    }
}
