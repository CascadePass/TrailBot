using CascadePass.TrailBot.UI.Feature.TopicEditor;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace CascadePass.TrailBot.UI.Feature.WelcomeScreen
{
    public class WelcomeSetupFeatureViewModel : ViewModel
    {
        private int currentTaskIndex;
        private DelegateCommand previousPageCommand, nextPageCommand;

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

            this.Tasks_OriginalImplementation = new();

            this.DatabaseSetupTaskViewModel = new() { Settings = this.Settings, };

            this.TopicSetupViewModel = new();

            this.Tasks_OriginalImplementation.Add(this.DatabaseSetupTaskViewModel);
            this.Tasks_OriginalImplementation.Add(this.TopicSetupViewModel);

            ApplicationData.WebProviderManager = this.WebProviderManager;
            ApplicationData.Settings = this.Settings;

            this.Tasks = new()
            {
                new DatabaseSetupView() { DataContext = this.DatabaseSetupTaskViewModel },
                //new ProviderSetupView() { DataContext = new ProviderSetupViewModel(this.WebProviderManager) },
                new TopicSetupView() { DataContext = this.TopicSetupViewModel },
                //new Feature.TopicEditor.TopicEditorFeature(),
            };

            this.SetupTask_Completed(null, null);
        }

        public WebProviderManager WebProviderManager { get; set; }

        public Settings Settings { get; set; }

        public List<UserControl> Tasks { get; set; }

        public ICommand NextPageCommand => this.nextPageCommand ??= new(this.MoveNextImplementation);

        public ICommand PreviousPageCommand => this.previousPageCommand ??= new(this.MovePreviousImplementation);

        public List<SetupTaskViewModel> Tasks_OriginalImplementation { get; set; }

        public FrameworkElement CurrentTask => this.Tasks[this.CurrentTaskIndex];

        public int CurrentTaskIndex {
            get => this.currentTaskIndex;
            private set
            {
                if (this.currentTaskIndex != value)
                {
                    this.currentTaskIndex = value;
                    this.OnPropertyChanged(nameof(this.CurrentTaskIndex));
                    this.OnPropertyChanged(nameof(this.CurrentTask));
                }
            }
        }

        public DatabaseSetupTaskViewModel DatabaseSetupTaskViewModel { get; set; }

        public TopicSetupViewModel TopicSetupViewModel { get; set; }

        private void MovePreviousImplementation()
        {
            if (this.CurrentTaskIndex > 0)
            {
                this.CurrentTaskIndex--;
            }
        }

        private void MoveNextImplementation()
        {
            if (this.CurrentTaskIndex < this.Tasks.Count - 1)
            {
                this.CurrentTaskIndex++;
            }
        }

        private void SetupTask_Completed(object sender, EventArgs e)
        {
            this.TopicSetupViewModel.EnableTaskEditorButton = this.DatabaseSetupTaskViewModel.IsComplete;
        }
    }
}
