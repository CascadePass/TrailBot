using CascadePass.TrailBot.UI.Feature.Found;
using System.Windows;
using System.Windows.Input;

namespace CascadePass.TrailBot.UI
{
    public class RibbonViewModel : ObservableObject
    {
        private DelegateCommand viewInBrowserCommand, startCommand, stopCommand, removeCommand, markReadCommand, markUnreadCommand, editTopicCommand;
        private bool isSettingsTabSelected;

        public RibbonViewModel()
        {
            if (Application.Current is App tripReportReaderApplication)
            {
                this.Settings = ApplicationData.Settings;
                this.ReaderViewModel = tripReportReaderApplication.MainWindow.DataContext as ReaderViewModel;
                this.WebProviderManager = ApplicationData.WebProviderManager;
            }

            this.QuickSettingsViewModel = new() {
                Settings = this.Settings,
                WebProviderManager = this.WebProviderManager,
            };
        }

        public ReaderViewModel ReaderViewModel { get; set; }

        public WebProviderManager WebProviderManager { get; set; }

        public Settings Settings { get; set; }

        public QuickSettingsViewModel QuickSettingsViewModel { get; set; }


        public bool IsSettingsTabSelected
        {
            get => this.isSettingsTabSelected;
            set
            {
                if (this.isSettingsTabSelected != value)
                {
                    this.isSettingsTabSelected = value;
                    this.OnPropertyChanged(nameof(this.IsSettingsTabSelected));

                    if (this.isSettingsTabSelected)
                    {
                        this.QuickSettingsViewModel.NotifyComboBoxes();
                    }
                }
            }
        }

        public ICommand ViewInBrowserCommand => this.viewInBrowserCommand ??= new DelegateCommand(this.ViewInBrowserImplementation);

        public ICommand StartCommand => this.startCommand ??= new DelegateCommand(this.StartImplementation);

        public ICommand StopCommand => this.stopCommand ??= new DelegateCommand(this.StopImplementation);

        public ICommand RemoveSelectedMatchCommand => this.removeCommand ??= new DelegateCommand(this.RemoveSelectedMatchImplementation);

        public ICommand MarkReadCommand => this.markReadCommand ??= new DelegateCommand(this.MarkReadImplementation);

        public ICommand MarkUnreadCommand => this.markUnreadCommand ??= new DelegateCommand(this.MarkUnreadImplementation);

        public ICommand EditTopicCommand => this.editTopicCommand ??= new DelegateCommand(this.EditTopicImplementation);

        private void ViewInBrowserImplementation()
        {
            if (this.ReaderViewModel.SelectedMatch != null)
            {
                this.ReaderViewModel.SelectedMatch.ViewInBrowserCommand.Execute(null);
            }
        }

        private void StartImplementation()
        {
            this.ReaderViewModel.WebProviderManager.Start();
        }

        private void StopImplementation()
        {
            this.ReaderViewModel.WebProviderManager.Stop();
        }

        private void RemoveSelectedMatchImplementation()
        {
            this.ReaderViewModel.RemoveSelectedMatch();
        }

        private void MarkReadImplementation()
        {
            this.ReaderViewModel.MarkRead();
        }

        private void MarkUnreadImplementation()
        {
            this.ReaderViewModel.MarkUnread();
        }

        private void EditTopicImplementation()
        {
            if (Application.Current is App tripReportReaderApplication && tripReportReaderApplication.MainWindow is MainWindow hostWindow)
            {
                hostWindow.CurrentContent = FeaturecreenProvider.GetTopicEditorScreen();
            }
        }
    }
}
