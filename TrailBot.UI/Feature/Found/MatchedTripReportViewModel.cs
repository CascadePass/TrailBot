using CascadePass.TrailBot.UI.Dialogs.AddTermToTopic;
using System;
using System.Linq;
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
        private string selectedPreviewText;
        private DelegateCommand viewTripReportInWebBrowserCommand, addTextToTopicCommand, addExceptionTextCommand, createTopicCommand, copySelectedCommand;

        public MatchedTripReport MatchedTripReport { get; set; }

        //public string SourceUri { get; set; }

        //public DateTime TripDate { get; set; }

        //public string Title { get; set; }

        //public string HikeType { get; set; }

        //public string Region { get; set; }

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

        //public string Matches { get; set; }

        //public string Filename { get; set; }

        //public int WordCount { get; set; }

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

        //public string BroaderContext { get; set; }

        //public List<string> Topics { get; set; }

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

        public FlowDocument PreviewDocument
        {
            get
            {
                if (this.Report == null)
                {
                    return null;
                }
                
                return this.previewDocument ??= this.FormatFlowDocument(this.CreateFlowDocument(this.Report));
            }
        }

        public ICommand ViewInBrowserCommand => this.viewTripReportInWebBrowserCommand ??= new(this.LaunchTripReportInBrowser);

        public ICommand AddTextToTopicCommand => this.addTextToTopicCommand ??= new(this.AddTextToTopicImplementation);

        public ICommand AddExceptionTextToTopicCommand => this.addExceptionTextCommand ??= new(this.AddExceptionTextToTopicImplementation);

        public ICommand CreateTopicCommand => this.createTopicCommand ??= new(this.CreateTopicImplementation);

        public ICommand CopySelectedTextCommand => this.copySelectedCommand ??= new(this.CopySelectedTextImplementation);

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


            //TODO: Make a local property don't use this globally
            viewModel.Settings = ApplicationData.Settings;
            viewModel.Topics = ApplicationData.WebProviderManager.Topics;


            viewModel.Topic = topic;
            viewModel.EditMode = mode;
            viewModel.InitialTerm = this.SelectedPreviewText?.Trim();
            hostWindow.ShowDialog();

            if (viewModel.WasUpdated)
            {
                if (mode == AddTermMode.CreateNewTopic)
                {
                    ApplicationData.WebProviderManager.Topics.Add(topic);
                }
                // redraw the preview
            }
        }

        #region Preview Flow Document

        private FlowDocument CreateFlowDocument(TripReport tripReportSource)
        {
            var document = new FlowDocument() { PagePadding = new Thickness(5) };
            document.SetResourceReference(FlowDocument.FontSizeProperty, "Font.Large");

            string[] paragraphs = tripReportSource.ReportText.Split(new char[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);

            var titleRun = new Run($"{tripReportSource.Title} ({tripReportSource.TripDate.ToShortDateString()})");
            var titleLink = new Hyperlink(titleRun) { Command = this.ViewInBrowserCommand };
            var titleParagraph = new Paragraph(titleLink) { Margin = new Thickness(0, 0, 0, 10), FontWeight = FontWeights.ExtraBold };
            titleRun.SetResourceReference(FlowDocument.FontSizeProperty, "Font.Huger");
            document.Blocks.Add(titleParagraph);


            foreach (string paraText in paragraphs)
            {
                var run = new Run(paraText) { Foreground = Brushes.DarkSlateGray };
                var flowParagraph = new Paragraph(run) { Margin = new Thickness(0, 0, 0, 15) };

                document.Blocks.Add(flowParagraph);
            }

            return document;
        }

        private FlowDocument FormatFlowDocument(FlowDocument document)
        {
            TextPointer currentPosition = document.ContentStart;
            char[] punctuation = Topic.ClauseBoundaries;


            while (currentPosition != null)
            {
                if (currentPosition.CompareTo(document.ContentEnd) == 0)
                {
                    break;
                }

                if (currentPosition.GetPointerContext(LogicalDirection.Forward) == TextPointerContext.Text)
                {
                    foreach (var wordCount in this.MatchedTripReport.Matches.Split(','))
                    {
                        string textInCurrentRun = currentPosition.GetTextInRun(LogicalDirection.Forward);

                        string compareText = textInCurrentRun.ToLower();
                        if (compareText.Contains(wordCount))
                        {
                            int wordStartIndex = compareText.IndexOf(wordCount);
                            int previousSentenceEnd = 0;

                            for (int i = wordStartIndex; i >= 0; i--)
                            {
                                if (punctuation.Contains(compareText[i]))
                                {
                                    previousSentenceEnd = i + 1;
                                    break;
                                }
                            }

                            int curentSentenceEnd = compareText.IndexOfAny(punctuation, wordStartIndex);

                            int offset = 0;
                            if (previousSentenceEnd > 0 && curentSentenceEnd > 0)
                            {
                                TextPointer
                                    sentenceStart = currentPosition.GetPositionAtOffset(previousSentenceEnd),
                                    sentenceEnd = currentPosition.GetPositionAtOffset(curentSentenceEnd);

                                offset = 3;
                                var sentenceRange = new TextRange(sentenceStart, sentenceEnd);

                                sentenceRange.ApplyPropertyValue(TextElement.ForegroundProperty, Brushes.Black);
                                sentenceRange.ApplyPropertyValue(TextElement.FontWeightProperty, FontWeights.Medium);
                                sentenceRange.ApplyPropertyValue(TextElement.BackgroundProperty, Brushes.Yellow);
                            }

                            TextPointer
                                start = currentPosition.GetPositionAtOffset(offset + wordStartIndex),
                                end = currentPosition.GetPositionAtOffset(offset + wordStartIndex + wordCount.Length)
                                ;

                            var colorRange = new TextRange(start, end);

                            colorRange.ApplyPropertyValue(TextElement.ForegroundProperty, Brushes.Red);
                            colorRange.ApplyPropertyValue(TextElement.FontWeightProperty, FontWeights.Bold);
                        }
                    }

                    currentPosition = currentPosition.GetNextContextPosition(LogicalDirection.Forward);
                }
                else
                {
                    currentPosition = currentPosition.GetNextContextPosition(LogicalDirection.Forward);
                }
            }

            return document;
        }

        #endregion
    }
}
