using CascadePass.TrailBot.UI.Feature.TopicEditor;
using System.Windows;
using System.Windows.Input;

namespace CascadePass.TrailBot.UI.Feature.WelcomeScreen
{
    public class TopicSetupViewModel : SetupTaskViewModel
    {
        private bool enableTaskEditorButton;
        private DelegateCommand topicEditorCommand;

        public TopicSetupViewModel()
        {
            this.TopicEditorViewModel = new()
            {
                Topics = new Topic[] { new() { Name = "Snow", MatchAny = "snow\r\ngroomed" } }
            };
        }

        public bool EnableTaskEditorButton
        {
            get => this.enableTaskEditorButton;
            set
            {
                if (this.enableTaskEditorButton != value)
                {
                    this.enableTaskEditorButton = value;
                    this.OnPropertyChanged(nameof(this.EnableTaskEditorButton));
                }
            }
        }

        public TopicEditorViewModel TopicEditorViewModel { get; set; }


        public ICommand LaunchTopicEditorCommand => this.topicEditorCommand ??= new DelegateCommand(this.LaunchTopicEditorImplementation);

        public void LaunchTopicEditorImplementation()
        {
            if (Application.Current is App tripReportReaderApplication && tripReportReaderApplication.MainWindow is MainWindow hostWindow)
            {
                hostWindow.CurrentContent = FeaturecreenProvider.GetTopicEditorScreen();
            }
        }

        public override void Validate()
        {
        }
    }
}
