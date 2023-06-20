using System;
using System.Collections.Generic;
using System.Text;

namespace CascadePass.TrailBot.UI.Feature.TopicEditor
{
    public class TopicViewModel : SetupTaskViewModel
    {
        private Topic topic;

        public Topic Topic
        {
            get => this.topic;
            set
            {
                if (this.topic != value)
                {
                    this.topic = value;
                    this.OnPropertyChanged(nameof(this.Topic));

                    this.Validate();
                }
            }
        }

        public string Name
        {
            get => this.Topic?.Name;
            set
            {
                if (this.Topic == null)
                {
                    return;
                }

                if (!string.Equals(this.Topic.Name, value, StringComparison.OrdinalIgnoreCase))
                {
                    this.Topic.Name = value;
                    this.OnPropertyChanged(nameof(this.Name));

                    this.Validate();
                }
            }
        }

        public string MatchAny
        {
            get => this.Topic?.MatchAny;
            set
            {
                if (!string.Equals(this.Topic.MatchAny, value, StringComparison.Ordinal))
                {
                    this.Topic.MatchAny = this.SortLines(value);
                    this.OnPropertyChanged(nameof(this.MatchAny));
                    this.OnPropertyChanged(nameof(this.AnyCount));

                    this.Validate();
                }
            }
        }

        public string MatchUnless
        {
            get => this.Topic?.MatchAnyUnless;
            set
            {
                if (!string.Equals(this.topic.MatchAnyUnless, value, StringComparison.Ordinal))
                {
                    this.topic.MatchAnyUnless = this.SortLines(value);
                    this.OnPropertyChanged(nameof(this.MatchUnless));
                    this.OnPropertyChanged(nameof(this.UnlessCount));

                    this.Validate();
                }
            }
        }

        public int AnyCount => this.Topic?.AnyCount ?? 0;

        public int UnlessCount => this.Topic?.UnlessCount ?? 0;

        public string SortLines(string multilineString)
        {
            if (string.IsNullOrEmpty(multilineString))
            {
                return null;
            }

            char[] lineEndings = new char[] { '\r', '\n' };
            List<string> lines = new();

            lines.AddRange(multilineString.Split(lineEndings, StringSplitOptions.RemoveEmptyEntries));
            lines.Sort();

            StringBuilder stringBuilder = new();
            foreach (string line in lines)
            {
                stringBuilder.AppendLine(line);
            }

            return stringBuilder.ToString();
        }

        public override void Validate()
        {
            this.IsComplete =
                this.Topic != null &&
                !string.IsNullOrWhiteSpace(this.Topic.Name) &&
                !string.IsNullOrWhiteSpace(this.MatchAny);
        }
    }
}
