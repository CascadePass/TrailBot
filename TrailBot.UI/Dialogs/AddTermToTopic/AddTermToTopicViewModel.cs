using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

                    if (value)
                    {
                        this.SuggestTerms(this.InitialTerm);
                    }
                    else
                    {
                        this.RemoveSuggestions(this.InitialTerm);
                    }
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

                    if (this.SuggestAdditionalTerms)
                    {
                        this.SuggestTerms(this.InitialTerm);
                    }
                    else
                    {
                        this.RemoveSuggestions(this.InitialTerm);
                    }
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
            _ => "You found a bug"
        };

        public Window Window { get; set; }

        public bool WasUpdated { get; set; }

        public bool IsNewTopic => this.Topics?.Contains(this.Topic) ?? true;

        public bool CanAdd => this.Topic != null;

        // Is something not dirty?
        public bool CanCancel => true;

        public ICommand AddCommand => this.addCommand ??= new(this.AddTermImplementation);

        public ICommand CancelCommand => this.cancelCommand ??= new(this.CancelImplementation);

        public static string GetSuggestions(string baseTerm)
        {
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

            return stringBuilder.ToString();
        }

        public void SuggestTerms(string baseTerm)
        {
            StringBuilder stringBuilder = new();
            stringBuilder.AppendLine(baseTerm);
            stringBuilder.AppendLine(AddTermToTopicViewModel.GetSuggestions(baseTerm));

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

        public string GetMergedText(string source, string target)
        {
            if (string.IsNullOrEmpty(target))
            {
                return source;
            }

            char[] delimiter = new char[] { '\r', '\n' };

            string[] sourceLines = source.Split(delimiter, StringSplitOptions.RemoveEmptyEntries);
            string[] targetLines = target.Split(delimiter, StringSplitOptions.RemoveEmptyEntries);

            StringBuilder result = new();

            result.AppendLine(target);

            foreach (string term in sourceLines)
            {
                if (!string.IsNullOrWhiteSpace(term) && !targetLines.Contains(term))
                {
                    result.AppendLine(term);
                }
            }

            return result.ToString().Trim();
        }

        public void AddTermImplementation()
        {
            if (this.Topic == null)
            {
                throw new InvalidOperationException($"{nameof(this.Topic)} is null");
            }

            //TODO: Test both of these lines well!
            this.Topic.MatchAny = this.GetMergedText(this.AnyTerms, this.Topic.MatchAny);
            this.Topic.MatchAnyUnless = this.GetMergedText(this.UnlessTerms, this.Topic.MatchAnyUnless);

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

    public enum AddTermMode
    {
        ValueNotSet,

        AddToExistingTopic,

        AddExceptionToExistingTopic,

        CreateNewTopic
    }
}
