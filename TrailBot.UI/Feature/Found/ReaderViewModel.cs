using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Data;

namespace CascadePass.TrailBot.UI.Feature.Found
{
    public class ReaderViewModel : ObservableObject
    {
        private WebProviderManager webProviderManager;
        private MatchedTripReportViewModel selectedMatch;
        private Settings settings;

        public ReaderViewModel()
        {
            this.MatchedTripReports = new();
            BindingOperations.EnableCollectionSynchronization(this.MatchedTripReports, new object());
        }

        public Settings Settings {
            get => this.settings;
            set
            {
                if (this.settings != value)
                {
                    this.Unsubscribe(this.settings);
                    this.settings = value;
                    this.OnPropertyChanged(nameof(this.Settings));
                    this.Subscribe(this.settings);
                }
            }
        }

        public ObservableCollection<MatchedTripReportViewModel> MatchedTripReports { get; set; }

        public WebProviderManager WebProviderManager {
            get => this.webProviderManager;
            set
            {
                if (this.webProviderManager != null)
                {
                    this.webProviderManager.FoundMatch -= this.WebProviderManager_FoundMatch;
                }

                this.webProviderManager = value;
                this.MatchedTripReports.Clear();
                this.GetFoundTripReports();

                this.OnPropertyChanged(nameof(this.MatchedTripReports));
                this.OnPropertyChanged(nameof(this.SearchTerms));
            }
        }

        public MatchedTripReportViewModel SelectedMatch {
            get => this.selectedMatch;
            set
            {
                if (this.selectedMatch != value)
                {
                    this.selectedMatch = value;
                    this.OnPropertyChanged(nameof(this.SelectedMatch));
                    this.OnPropertyChanged(nameof(this.HasSelectedMatch));
                    this.OnPropertyChanged(nameof(this.ShowPreviewPane));
                }
            }
        }

        public bool HasSelectedMatch => this.SelectedMatch != null;

        public bool ShowPreviewPane => this.selectedMatch != null && this.Settings.ShowPreviewPane;

        public string[] SearchTerms => this.WebProviderManager?.Topics[0]?.MatchAny?.Split('\n');

        public void RemoveSelectedMatch()
        {
            if (this.SelectedMatch == null)
            {
                return;
            }

            this.WebProviderManager.RemoveMatch(this.SelectedMatch.MatchedTripReport);
        }

        public void MarkRead()
        {
            if (this.SelectedMatch == null)
            {
                return;
            }

            this.SelectedMatch.HasBeenSeen = true;
        }

        public void MarkUnread()
        {
            if (this.SelectedMatch == null)
            {
                return;
            }

            this.SelectedMatch.HasBeenSeen = false;
        }

        private void GetFoundTripReports()
        {
            if (this.webProviderManager != null)
            {
                this.webProviderManager.FoundMatch += this.WebProviderManager_FoundMatch;

                if (this.webProviderManager.Found != null)
                {
                    foreach (var item in this.webProviderManager.Found)
                    {
                        this.MatchedTripReports.Add(ReaderViewModel.CreateMatchedViewModel(item));
                    }
                }
            }
        }

        public static MatchedTripReportViewModel CreateMatchedViewModel(MatchedTripReport item)
        {
            return new() {
                MatchedTripReport = item,
                AllTopics = ApplicationData.WebProviderManager.Topics,
                Settings = ApplicationData.Settings,
                FormattedExerpts = ReaderViewModel.GetExerpts(item),
            };
        }

        private static Dictionary<string, string> GetExerpts(MatchedTripReport matchedTripReport)
        {
            Dictionary<string, string> result = new();
            StringBuilder exerpts = new();

            StringBuilder topicAppender = new();
            string key = null;
            foreach (string rawLine in (matchedTripReport.BroaderContext).Split(new char[] { '\r', '\n' }))
            {
                string line = rawLine.Trim();

                if (ApplicationData.WebProviderManager.Topics.Any(m => string.Equals(m.Name, line, System.StringComparison.OrdinalIgnoreCase)))
                {
                    if (!string.IsNullOrEmpty(key) && !result.ContainsKey(key))
                    {
                        result.Add(key, topicAppender.ToString().Trim());
                    }

                    key = line;
                    topicAppender.Clear();
                }
                else if (!string.IsNullOrWhiteSpace(line))
                {
                    topicAppender.Append("• ");
                    topicAppender.Append(line);
                    topicAppender.Append(" ");
                }
            }

            if (!string.IsNullOrEmpty(key) && !result.ContainsKey(key))
            {
                result.Add(key, topicAppender.ToString().Trim());
            }

            return result;
        }

        private void Subscribe(object o)
        {
            if (o == null)
            {
                return;
            }

            if (o is Settings settingsObject)
            {
                settingsObject.PropertyChanged += this.Settings_PropertyChanged;
            }
        }

        private void Unsubscribe(object o)
        {
            if (o == null)
            {
                return;
            }

            if (o is Settings settingsObject)
            {
                settingsObject.PropertyChanged -= this.Settings_PropertyChanged;
            }
        }

        private void WebProviderManager_FoundMatch(object sender, MatchingTripReportEventArgs e)
        {
            this.MatchedTripReports.Add(ReaderViewModel.CreateMatchedViewModel(e.MatchedTripReport));
        }

        private void Settings_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(ShowPreviewPane))
            {
                this.OnPropertyChanged(nameof(ShowPreviewPane));
            }
        }
    }
}
