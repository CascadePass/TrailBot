using CascadePass.TrailBot.TextAnalysis;
using CascadePass.TrailBot.UI.Dialogs.AddTermToTopic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Xml.Serialization;

namespace CascadePass.TrailBot.UI.Feature.Found
{
    [Serializable]
    public class MatchedTripReportViewModel : ViewModel
    {
        private TripReport tripReport;
        private FlowDocument previewDocument;
        private string selectedPreviewText, topicExerpts, matchTermsAreFor;
        private bool isMatchDetailPanelVisible, isMatchTermListVisible;
        private DelegateCommand viewTripReportInWebBrowserCommand, addTextToTopicCommand, addExceptionTextCommand, createTopicCommand, copySelectedCommand, closeMatchDetailCommand;
        private ParameterizedDelegateCommand showSearchTermsCommand, editTopicCommand;

        public MatchedTripReportViewModel()
        {
            this.isMatchDetailPanelVisible = true;
        }

        public MatchedTripReport MatchedTripReport { get; set; }

        [XmlIgnore]
        public TripReport Report {
            get => this.tripReport;
            set
            {
                if (this.tripReport != value)
                {
                    this.tripReport = value;
                    this.OnPropertyChanged(nameof(this.Report));
                    this.OnPropertyChanged(nameof(this.PreviewDocument));
                }
            }
        }

        public bool HasBeenSeen { 
            get => this.MatchedTripReport.HasBeenSeen;
            set
            {
                if (this.MatchedTripReport.HasBeenSeen != value)
                {
                    this.MatchedTripReport.HasBeenSeen = value;
                    this.OnPropertyChanged(nameof(this.HasBeenSeen));
                    this.OnPropertyChanged(nameof(this.FontWeight));
                }
            }
        }

        public FontWeight FontWeight => this.HasBeenSeen ? FontWeights.Normal : FontWeights.SemiBold;

        public string SelectedPreviewText { 
            get => this.selectedPreviewText;
            set
            {
                if (!string.Equals(this.selectedPreviewText, value, StringComparison.Ordinal))
                {
                    this.selectedPreviewText = value;
                    this.OnPropertyChanged(nameof(this.SelectedPreviewText));
                    this.OnPropertyChanged(nameof(this.HasSelectedText));
                }
            }
        }

        public bool HasSelectedText => !string.IsNullOrWhiteSpace(this.SelectedPreviewText);

        public List<Topic> AllTopics { get; set; }

        public Settings Settings { get; set; }

        public bool IsMatchDetailPanelVisible
        {
            get => this.isMatchDetailPanelVisible;
            set
            {
                if (this.isMatchDetailPanelVisible != value)
                {
                    this.isMatchDetailPanelVisible = value;
                    this.OnPropertyChanged(nameof(this.IsMatchDetailPanelVisible));
                }
            }
        }

        public bool IsMatchTermListVisible
        {
            get => this.isMatchTermListVisible;
            set
            {
                if (this.isMatchTermListVisible != value)
                {
                    this.isMatchTermListVisible = value;
                    this.OnPropertyChanged(nameof(this.IsMatchTermListVisible));
                }
            }
        }

        public string TopicExerpts
        {
            get => this.topicExerpts;
            set
            {
                if (!string.Equals(this.topicExerpts, value, StringComparison.Ordinal))
                {
                    this.topicExerpts = value;
                    this.OnPropertyChanged(nameof(this.TopicExerpts));
                }
            }
        }


        [XmlIgnore]
        public Dictionary<string, string> FormattedExerpts { get; set; }

        public FlowDocument PreviewDocument
        {
            get
            {
                if (this.Report == null)
                {
                    return null;
                }

                return this.previewDocument ??= this.CreateFlowDocument(this.Report);
            }
        }

        #region Commands

        public ICommand ViewInBrowserCommand => this.viewTripReportInWebBrowserCommand ??= new(this.LaunchTripReportInBrowser);

        public ICommand AddTextToTopicCommand => this.addTextToTopicCommand ??= new(this.AddTextToTopicImplementation);

        public ICommand AddExceptionTextToTopicCommand => this.addExceptionTextCommand ??= new(this.AddExceptionTextToTopicImplementation);

        public ICommand CreateTopicCommand => this.createTopicCommand ??= new(this.CreateTopicImplementation);

        public ICommand CopySelectedTextCommand => this.copySelectedCommand ??= new(this.CopySelectedTextImplementation);

        public ICommand CloseMatchDetailCommand => this.closeMatchDetailCommand ??= new(this.CloseMatchDetailImplementation);

        public ICommand ShowSearchTermsCommand => this.showSearchTermsCommand ??= new(this.ShowMatchingSearchTermsImplementation);

        public ICommand EditTopicCommand => this.editTopicCommand ??= new(this.EditTopicImplementation);

        #endregion

        #region Command Implementations

        private void LaunchTripReportInBrowser()
        {
            this.ViewInBrowser(this.MatchedTripReport.SourceUri);
            this.HasBeenSeen = true;
        }

        private void AddTextToTopicImplementation()
        {
            this.AddTerm(AddTermMode.AddToExistingTopic, null);
        }

        private void AddExceptionTextToTopicImplementation()
        {
            this.AddTerm(AddTermMode.AddExceptionToExistingTopic, null);
        }

        private void CreateTopicImplementation()
        {
            this.AddTerm(AddTermMode.CreateNewTopic, new());
        }

        private void CopySelectedTextImplementation()
        {
            Clipboard.SetText(this.SelectedPreviewText);
        }

        private void CloseMatchDetailImplementation()
        {
            this.IsMatchDetailPanelVisible = false;
        }

        private void ShowMatchingSearchTermsImplementation(object topic)
        {
            string topicName = (string)topic;

            if (this.IsMatchTermListVisible && string.Equals(topicName, this.matchTermsAreFor, StringComparison.Ordinal))
            {
                this.IsMatchTermListVisible = false;
                return;
            }

            StringBuilder result = new();
            bool addNextLine = false;
            foreach (string line in this.MatchedTripReport.BroaderContext.Split(new char[] { '\r', '\n' }))
            {
                if (this.AllTopics.Any(m => string.Equals(m.Name, line.Trim(), StringComparison.OrdinalIgnoreCase)))
                {
                    addNextLine = false;
                } else if (addNextLine && !string.IsNullOrWhiteSpace(line))
                {
                    result.AppendLine(line);
                }

                if (string.Equals(topicName, line.Trim(), StringComparison.OrdinalIgnoreCase))
                {
                    addNextLine = true;
                }
            }

            this.TopicExerpts = result.ToString().Trim();
            this.IsMatchTermListVisible = true;
            this.matchTermsAreFor = topicName;
        }

        private void EditTopicImplementation(object topic)
        {
            string topicName = (string)topic;

            if (Application.Current is App tripReportReaderApplication && tripReportReaderApplication.MainWindow is MainWindow hostWindow)
            {
                hostWindow.CurrentContent = FeaturecreenProvider.GetTopicEditorScreen();
                TopicEditor.TopicEditorViewModel vm = (TopicEditor.TopicEditorViewModel)hostWindow.CurrentViewModel;
                vm.ExpandTopic(topicName);
            }
        }

        #endregion

        private void AddTerm(AddTermMode mode, Topic topic)
        {
            AddTermToTopicDialog dialog = new();
            AddTermToTopicViewModel viewModel = (AddTermToTopicViewModel)dialog.DataContext;
            Window hostWindow = new()
            {
                Width = 600,
                Height = 400,
                WindowStartupLocation = WindowStartupLocation.CenterOwner,
                WindowStyle = WindowStyle.None,
                Owner = App.Current.MainWindow,
                Content = dialog,
                Title = "Add to Topic"
            };

            if (topic == null && viewModel.Topics != null)
            {
                topic = viewModel.Topics.FirstOrDefault(m => m.Name == this.MatchedTripReport?.Topics.FirstOrDefault());
            }

            viewModel.Window = hostWindow;


            viewModel.Settings = this.Settings;
            viewModel.Topics = this.AllTopics;


            viewModel.Topic = topic;
            viewModel.EditMode = mode;
            viewModel.InitialTerm = this.SelectedPreviewText?.Trim();

            hostWindow.ShowDialog();

            if (viewModel.WasUpdated)
            {
                if (mode == AddTermMode.CreateNewTopic)
                {
                    this.AllTopics.Add(topic);
                }

                //TODO: redraw the preview
            }
        }

        #region Preview Flow Document

        public FlowDocument CreateFlowDocument(TripReport tripReportSource)
        {
            var document = new FlowDocument() { PagePadding = new Thickness(5) };
            document.SetResourceReference(FlowDocument.FontSizeProperty, "Font.Large");

            string[] paragraphs = tripReportSource.ReportText.Split(new char[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);

            document.Blocks.Add(this.CreatePreviewDocumentTitle(tripReportSource));            

            foreach (string paraText in paragraphs)
            {
                document.Blocks.Add(this.CreatePreviewDocumentParagraph(paraText));
            }

            return document;
        }

        public Paragraph CreatePreviewDocumentTitle(TripReport tripReportSource)
        {
            var titleRun = new Run($"{tripReportSource.Title} ({tripReportSource.TripDate.ToShortDateString()})");
            var titleLink = new Hyperlink(titleRun) { Command = this.ViewInBrowserCommand };
            var titleParagraph = new Paragraph(titleLink) { Margin = new Thickness(0, 0, 0, 10), FontWeight = FontWeights.ExtraBold };

            titleRun.SetResourceReference(FlowDocument.FontSizeProperty, "Font.Huger");

            return titleParagraph;
        }

        public Paragraph CreatePreviewDocumentParagraph(string paraText)
        {
            var flowParagraph = new Paragraph() { Margin = new Thickness(0, 0, 0, 15), Foreground = Brushes.DarkSlateGray };

            foreach (string sentence in Regex.Split(paraText, "(?<=[.!?])"))
            {
                this.CreatePreviewDocumentSentence(sentence, flowParagraph);
            }

            return flowParagraph;
        }

        public void CreatePreviewDocumentSentence(string sentence, Paragraph flowParagraph)
        {
            bool sentenceMatchesTopic = false;

            if (this.AllTopics != null)
            {
                foreach (Topic topic in this.AllTopics)
                {
                    if (!topic.GetMatchInfo(sentence).IsEmpty)
                    {
                        sentenceMatchesTopic = true;
                        break;
                    }
                }
            }

            if (!sentenceMatchesTopic)
            {
                var run = new Run(sentence.Trim() + " ") { Foreground = Brushes.DarkSlateGray };

                flowParagraph.Inlines.Add(run);

                return;
            }

            Tokenizer tokenizer = new();
            tokenizer.GetTokens(sentence);

            int wordsLeftInMatchingPhrase = 0;
            for (int i = 0; i < tokenizer.OrderedTokens.Count; i++)
            {
                bool wordIsTopicKeyword = false;

                if (wordsLeftInMatchingPhrase > 0)
                {
                    wordIsTopicKeyword = true;
                    wordsLeftInMatchingPhrase--;
                }
                else
                {
                    foreach (Topic topic in this.AllTopics)
                    {
                        foreach (Phrase topicPhrase in topic.MatchAnyPhrases)
                        {
                            if (tokenizer.IsMatchAt(topicPhrase, i))
                            {
                                wordIsTopicKeyword = true;
                                wordsLeftInMatchingPhrase = (int)topicPhrase.Length - 1;

                                break;
                            }
                        }

                        if (wordIsTopicKeyword)
                        {
                            break;
                        }
                    }
                }

                string nextChar = i < tokenizer.OrderedTokens.Count - 1 && !tokenizer.OrderedTokens[i + 1].IsPunctuation ? " " : string.Empty;

                var wordRun = new Run(tokenizer.OrderedTokens[i].Text + nextChar) { Background = Brushes.Yellow };

                if (wordIsTopicKeyword)
                {
                    wordRun.FontWeight = FontWeights.Bold;
                    wordRun.Foreground = Brushes.Black;
                }

                flowParagraph.Inlines.Add(wordRun);
            }

            flowParagraph.Inlines.Add(new Run(" "));
        }

        #endregion
    }
}
