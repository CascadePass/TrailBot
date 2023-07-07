using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace CascadePass.TrailBot.UI.Dialogs.AddTermToTopic
{
    public class AddTermToTopicViewModel : ViewModel
    {
        private List<Topic> availableTopics;
        private Topic topic;
        private string anyTerms, unlessTerms, initialTerm;
        private AddTermMode addTermMode;
        private Settings settings;
        private DelegateCommand addCommand, cancelCommand;

        public AddTermToTopicViewModel()
        {
            this.anyTerms = string.Empty;
            this.unlessTerms = string.Empty;
        }

        public Settings Settings {
            get => this.settings;
            set
            {
                if (this.settings != value)
                {
                    this.settings = value;
                    this.OnPropertyChanged(nameof(this.Settings));
                    this.OnPropertyChanged(nameof(this.SuggestAdditionalTerms));
                }
            }
        }

        public List<Topic> Topics
        {
            get => this.availableTopics; set
            {
                if (this.availableTopics != value)
                {
                    this.availableTopics = value;
                    this.OnPropertyChanged(nameof(this.Topics));
                    this.OnPropertyChanged(nameof(this.Title));
                }
            }
        }

        public Topic Topic
        {
            get => this.topic; set
            {
                if (this.topic != value)
                {
                    this.topic = value;
                    this.OnPropertyChanged(nameof(this.Topic));
                    this.OnPropertyChanged(nameof(this.Title));
                    this.OnPropertyChanged(nameof(this.CanAdd));
                }
            }
        }

        public bool SuggestAdditionalTerms
        {
            get => this.Settings?.SuggestAdditionalTerms ?? false;
            set
            {
                if (this.Settings == null)
                {
                    return;
                }

                if (this.Settings.SuggestAdditionalTerms != value)
                {
                    this.Settings.SuggestAdditionalTerms = value;
                    this.OnPropertyChanged(nameof(this.SuggestAdditionalTerms));

                    this.SetSuggestionState();
                }
            }
        }

        public string InitialTerm
        {
            get => this.initialTerm;
            set
            {
                if (!string.Equals(this.initialTerm, value, StringComparison.Ordinal))
                {
                    this.initialTerm = value;
                    this.OnPropertyChanged(nameof(this.InitialTerm));

                    this.SetSuggestionState();
                }
            }
        }

        public string AnyTerms
        {
            get => this.anyTerms;
            set
            {
                string effectiveValue = value?.Trim();
                if (!string.Equals(this.anyTerms, effectiveValue, StringComparison.Ordinal))
                {
                    this.anyTerms = effectiveValue;
                    this.OnPropertyChanged(nameof(this.AnyTerms));
                    this.OnPropertyChanged(nameof(this.CanAdd));
                }
            }
        }

        public string UnlessTerms
        {
            get => this.unlessTerms;
            set
            {
                string effectiveValue = value?.Trim();
                if (!string.Equals(this.unlessTerms, effectiveValue, StringComparison.Ordinal))
                {
                    this.unlessTerms = effectiveValue;
                    this.OnPropertyChanged(nameof(this.UnlessTerms));
                }
            }
        }

        public AddTermMode EditMode
        {
            get => this.addTermMode;
            set
            {
                if (this.addTermMode != value)
                {
                    this.addTermMode = value;
                    this.OnPropertyChanged(nameof(this.EditMode));
                    this.OnPropertyChanged(nameof(this.Title));
                    this.OnPropertyChanged(nameof(this.ShowTopicDropdown));
                    this.OnPropertyChanged(nameof(this.ShowTopicNameEditBox));
                }
            }
        }

        public bool ShowTopicDropdown => this.EditMode != AddTermMode.CreateNewTopic;

        public bool ShowTopicNameEditBox => this.EditMode == AddTermMode.CreateNewTopic;

        public string Title => this.EditMode switch
        {
            AddTermMode.CreateNewTopic => "Create a Topic",
            AddTermMode.AddToExistingTopic => "Add to Topic",
            AddTermMode.AddExceptionToExistingTopic => "Add to Topic",
            AddTermMode.ValueNotSet => "You found a bug",
            _ => "You found a bug"
        };

        public Window Window { get; set; }

        public bool WasUpdated { get; internal set; }

        public bool CanAdd => this.Topic != null && !string.IsNullOrWhiteSpace(this.Topic.Name);

        public ICommand AddCommand => this.addCommand ??= new(this.AddTermImplementation);

        public ICommand CancelCommand => this.cancelCommand ??= new(this.CancelImplementation);

        public static string GetSuggestions(string baseTerm)
        {
            if (string.IsNullOrWhiteSpace(baseTerm))
            {
                return string.Empty;
            }

            string[] postfixes = { "s", "ing", "er", "ers" };
            StringBuilder stringBuilder = new();

            string lowerBaseTerm = baseTerm.ToLower();
            foreach (string postfix in postfixes)
            {
                if (!lowerBaseTerm.EndsWith(postfix))
                {
                    stringBuilder.AppendLine($"{baseTerm}{postfix}");
                }
            }

            return stringBuilder.ToString().Trim();
        }

        public void AddSuggestions(string baseTerm)
        {
            StringBuilder stringBuilder = new();
            stringBuilder.AppendLine(baseTerm);

            string suggestions = AddTermToTopicViewModel.GetSuggestions(baseTerm);
            if (!string.IsNullOrWhiteSpace(suggestions))
            {
                stringBuilder.Append(suggestions);
            }

            if (this.EditMode == AddTermMode.AddExceptionToExistingTopic)
            {
                this.UnlessTerms = stringBuilder.ToString();
            }
            else
            {
                this.AnyTerms = stringBuilder.ToString();
            }
        }

        public void RemoveSuggestions(string baseTerm)
        {
            string sourceText = this.EditMode == AddTermMode.AddExceptionToExistingTopic ? this.UnlessTerms : this.AnyTerms;

            if (string.IsNullOrWhiteSpace(sourceText))
            {
                // Nothing to do
                return;
            }

            string[] suggestions = AddTermToTopicViewModel.GetSuggestions(baseTerm).Split('\n', '\r');

            StringBuilder result = new();
            foreach (string item in sourceText.Split('\n', '\r'))
            {
                if (!suggestions.Contains(item) && !string.IsNullOrWhiteSpace(item))
                {
                    result.AppendLine(item.Trim());
                }
            }

            if (this.EditMode == AddTermMode.AddExceptionToExistingTopic)
            {
                this.UnlessTerms = result.ToString();
            }
            else
            {
                this.AnyTerms = result.ToString();
            }
        }

        public static string GetMergedText(string source, string target)
        {
            if (string.IsNullOrEmpty(target))
            {
                return source;
            }

            char[] delimiter = new char[] { '\r', '\n' };

            string[] sourceLines = AddTermToTopicViewModel.SplitIntoLines(source);
            string[] targetLines = AddTermToTopicViewModel.SplitIntoLines(target);

            StringBuilder result = new();

            result.AppendLine(target);

            foreach (string term in sourceLines)
            {
                if (!targetLines.Contains(term))
                {
                    result.AppendLine(term);
                }
            }

            return result.ToString().Trim();
        }

        public static string[] SplitIntoLines(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                return null;
            }

            List<string> result = new();
            char[] delimiter = new char[] { '\r', '\n' };
            string[] sourceLines = text.Split(delimiter, StringSplitOptions.RemoveEmptyEntries);

            foreach (string line in sourceLines)
            {
                if (!string.IsNullOrWhiteSpace(line))
                {
                    result.Add(line.Trim());
                }
            }

            return result.ToArray();
        }

        public void SetSuggestionState()
        {
            if (this.SuggestAdditionalTerms)
            {
                this.AddSuggestions(this.InitialTerm);
            }
            else
            {
                this.RemoveSuggestions(this.InitialTerm);
            }

            if (this.EditMode == AddTermMode.AddToExistingTopic && !this.AnyTerms.Contains(this.InitialTerm))
            {
                this.AnyTerms = this.InitialTerm;
            }
            else if (this.EditMode == AddTermMode.AddExceptionToExistingTopic && !this.UnlessTerms.Contains(this.InitialTerm))
            {
                this.UnlessTerms = this.InitialTerm;
            }

        }

        public void AddTermImplementation()
        {
            #region Sanity checks

            if (this.Topic == null)
            {
                throw new InvalidOperationException($"{nameof(this.Topic)} can't be null.");
            }

            if (this.EditMode == AddTermMode.ValueNotSet)
            {
                throw new InvalidOperationException($"{nameof(this.EditMode)} must have a valid value.");
            }

            if (!this.CanAdd)
            {
                throw new InvalidOperationException($"{nameof(this.CanAdd)} is false.");
            }

            #endregion

            this.Topic.MatchAny = AddTermToTopicViewModel.GetMergedText(this.AnyTerms, this.Topic.MatchAny);
            this.Topic.MatchAnyUnless = AddTermToTopicViewModel.GetMergedText(this.UnlessTerms, this.Topic.MatchAnyUnless);

            this.WasUpdated = true;

            this.Close();
        }

        public void CancelImplementation()
        {
            this.Close();
        }

        public void Close()
        {
            if (this.Window != null && this.Window != App.Current.MainWindow)
            {
                this.Window.Close();
            }
        }
    }
}
