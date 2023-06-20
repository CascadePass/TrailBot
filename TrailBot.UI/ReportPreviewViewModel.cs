//using CascadePass.TextAnalysis;
//using System;
//using System.Collections;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using System.Windows;
//using System.Windows.Controls;
//using System.Windows.Documents;
//using System.Windows.Input;
//using System.Windows.Media;
//using CascadePass.TripReporter;

//namespace CascadePass.TrailBot.UI
//{
//    public class ReportPreviewViewModel : ViewModel
//    {
//        private string text;
//        private IEnumerable<TokenCount> words;
//        private IEnumerable<Phrase> phrases;
//        private DelegateCommand viewTripReportInWebBrowserCommand;

//        public int Depth { get; set; }

//        public string Url { get; set; }

//        public string Title { get; set; }

//        public string Matches { get; set; }
//        public int WordCount { get; set; }
//        public List<string> BroaderContext { get; set; }

//        public TripReport Report { get; set; }

//        public string Text
//        {
//            get => this.text;
//            set
//            {
//                if (!string.Equals(this.text, value, StringComparison.Ordinal))
//                {
//                    this.text = value;
//                    this.OnPropertyChanged(nameof(this.Text));

//                    this.Tokenizer = new();
//                    this.Tokenizer.GetTokens(value.ToLower(), this.Depth + 8);

//                    this.words = null;
//                    this.phrases = null;

//                    this.OnPropertyChanged(nameof(this.Tokenizer));
//                    this.OnPropertyChanged(nameof(this.Words));
//                    this.OnPropertyChanged(nameof(this.Phrases));

//                    this.OnPropertyChanged(nameof(this.Title));
//                    this.OnPropertyChanged(nameof(this.Matches));
//                    this.OnPropertyChanged(nameof(this.WordCount));
//                    this.OnPropertyChanged(nameof(this.Url));
//                    this.OnPropertyChanged(nameof(this.BroaderContext));

//                    if (this.Report != null)
//                    {
//                        this.FormattedTextDocument = this.FormatFlowDocument(this.CreateFlowDocument(this.Report));

//                        this.OnPropertyChanged(nameof(this.FormattedTextDocument));
//                    }
//                }
//            }
//        }


//        public Tokenizer Tokenizer { get; set; }

//        public FlowDocument FormattedTextDocument { get; set; }


//        public IEnumerable<TokenCount> Words => this.words ??= this.Tokenizer?.TokenCounts.Where(m => m.Token.IsWord).OrderByDescending(m => m.Count);

//        public IEnumerable<Phrase> Phrases => this.phrases ??= this.Tokenizer?.Phrases.Where(p => p.Parts.Any(m => m.IsWord));


//        public ICommand ViewInBrowserCommand => this.viewTripReportInWebBrowserCommand ??= new DelegateCommand(this.LaunchTripReportInBrowser);

//        private void LaunchTripReportInBrowser()
//        {
//            this.ViewInBrowser(this.Url);
//        }

//        private FlowDocument CreateFlowDocument(TripReport tripReport)
//        {
//            var document = new FlowDocument() { PagePadding = new Thickness(5) };
//            document.SetResourceReference(FlowDocument.FontSizeProperty, "Font.Large");

//            string[] paragraphs = tripReport.ReportText.Split(new char[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);

//            var titleRun = new Run($"{tripReport.Title} ({tripReport.TripDate.ToShortDateString()})");
//            var titleLink = new Hyperlink(titleRun) { Command = this.ViewInBrowserCommand };
//            var titleParagraph = new Paragraph(titleLink) { Margin = new Thickness(0, 0, 0, 10), FontWeight = FontWeights.ExtraBold };
//            titleRun.SetResourceReference(FlowDocument.FontSizeProperty, "Font.Huger");
//            document.Blocks.Add(titleParagraph);



//            foreach (string paraText in paragraphs)
//            {
//                var run = new Run(paraText) { Foreground = Brushes.DarkSlateGray };
//                var flowParagraph = new Paragraph(run) { Margin = new Thickness(0, 0, 0, 15) };

//                document.Blocks.Add(flowParagraph);
//            }

//            return document;
//        }

//        private FlowDocument FormatFlowDocument(FlowDocument document)
//        {
//            TextPointer currentPosition = document.ContentStart;
//            char[] punctuation = MatchingPolicy.ClauseBoundaries;


//            while (currentPosition != null)
//            {
//                if (currentPosition.CompareTo(document.ContentEnd) == 0)
//                {
//                    break;
//                }

//                if (currentPosition.GetPointerContext(LogicalDirection.Forward) == TextPointerContext.Text)
//                {
//                    foreach (var wordCount in this.Matches.Split(','))
//                    {
//                        string textInCurrentRun = currentPosition.GetTextInRun(LogicalDirection.Forward);

//                        string compareText = textInCurrentRun.ToLower();
//                        if (compareText.Contains(wordCount))
//                        {
//                            int wordStartIndex = compareText.IndexOf(wordCount);
//                            int previousSentenceEnd = 0;

//                            for (int i = wordStartIndex; i >= 0; i--)
//                            {
//                                if (punctuation.Contains(compareText[i]))
//                                {
//                                    previousSentenceEnd = i + 1;
//                                    break;
//                                }
//                            }

//                            int curentSentenceEnd = compareText.IndexOfAny(punctuation, wordStartIndex);

//                            int offset = 0;
//                            if (previousSentenceEnd > 0 && curentSentenceEnd > 0)
//                            {
//                                TextPointer
//                                    sentenceStart = currentPosition.GetPositionAtOffset(previousSentenceEnd),
//                                    sentenceEnd = currentPosition.GetPositionAtOffset(curentSentenceEnd);

//                                offset = 3;
//                                var sentenceRange = new TextRange(sentenceStart, sentenceEnd);

//                                sentenceRange.ApplyPropertyValue(TextElement.ForegroundProperty, Brushes.Black);
//                                sentenceRange.ApplyPropertyValue(TextElement.FontWeightProperty, FontWeights.Medium);
//                                sentenceRange.ApplyPropertyValue(TextElement.BackgroundProperty, Brushes.Yellow);
//                            }

//                            TextPointer
//                                start = currentPosition.GetPositionAtOffset(offset + wordStartIndex),
//                                end = currentPosition.GetPositionAtOffset(offset + wordStartIndex + wordCount.Length)
//                                ;

//                            var colorRange = new TextRange(start, end);

//                            colorRange.ApplyPropertyValue(TextElement.ForegroundProperty, Brushes.Red);
//                            colorRange.ApplyPropertyValue(TextElement.FontWeightProperty, FontWeights.Bold);
//                        }
//                    }

//                    currentPosition = currentPosition.GetNextContextPosition(LogicalDirection.Forward);
//                }
//                else
//                {
//                    currentPosition = currentPosition.GetNextContextPosition(LogicalDirection.Forward);
//                }
//            }

//            return document;
//        }
//    }
//}
