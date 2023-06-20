using CascadePass.TrailBot.TextAnalysis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace CascadePass.TrailBot
{
    public class Topic
    {
        private string matchAny, matchAnyUnless;
        private List<Phrase> anyPhrases, anyUnlessPhrases;

        private static char[] punctuation;

        public static char[] ClauseBoundaries => Topic.punctuation ??= new char[] { '\r', '\n', '\t', '.', '!', '?', '(', ')', ',', '-', ';', '"' };


        [XmlAttribute("name")]
        public string Name { get; set; }

        #region Search Terms

        public string MatchAny
        {
            get => this.matchAny;
            set
            {
                if (!string.Equals(this.matchAny, value, StringComparison.Ordinal))
                {
                    this.matchAny = value;
                    this.anyPhrases = Topic.ParseSearchTerms(value);
                }
            }
        }

        public string MatchAnyUnless
        {
            get => this.matchAnyUnless;
            set
            {
                if (!string.Equals(this.matchAnyUnless, value, StringComparison.Ordinal))
                {
                    this.matchAnyUnless = value;
                    this.anyUnlessPhrases = Topic.ParseSearchTerms(value);
                }
            }
        }

        #endregion

        public int AnyCount => this.anyPhrases?.Count ?? 0;

        public int UnlessCount => this.anyUnlessPhrases?.Count ?? 0;

        public static List<Phrase> ParseSearchTerms(string text)
        {
            List<Phrase> result = new();

            if (!string.IsNullOrWhiteSpace(text))
            {
                foreach (string line in text.ToLower().Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    Tokenizer tokenizer = new();
                    tokenizer.GetTokens(line);

                    Phrase phrase = new Phrase(tokenizer.OrderedTokens.ToArray());

                    if (!string.IsNullOrWhiteSpace(phrase.Text))
                    {
                        result.Add(phrase);
                    }
                }
            }

            return result;
        }

        public MatchInfo GetMatchInfo(TripReport tripReport)
        {
            if (tripReport == null)
            {
                return null;
            }

            int lastBreak = 0;
            MatchInfo found = new() { Topic = this };
            Tokenizer tokenizer = new();
            tokenizer.GetTokens(tripReport.GetSearchableReportText());

            found.WordCount = tokenizer.OrderedTokens.Count(m => m.IsWord);

            for (int i = 0; i < tokenizer.OrderedTokens.Count; i++)
            {
                Token token = tokenizer.OrderedTokens[i];

                if (token.Length == 1)
                {
                    char t = token.Text[0];

                    if (Topic.ClauseBoundaries.Contains(t))
                    {
                        lastBreak = i;
                    }

                    continue;
                }

                foreach (Phrase phrase in this.anyPhrases)
                {
                    bool isMatch = tokenizer.IsMatchAt(phrase, i);

                    if (isMatch && this.anyUnlessPhrases != null)
                    {
                        bool falseMatch = false;
                        foreach (Phrase unless in this.anyUnlessPhrases)
                        {
                            int unlessPhraseLength = (int)phrase.Length;
                            for (int j = -unlessPhraseLength; j < unlessPhraseLength; j++)
                            {
                                if (tokenizer.IsMatchAt(unless, j + i))
                                {
                                    falseMatch = true;
                                    break;
                                }
                            }

                            if (falseMatch)
                            {
                                break;
                            }
                        }

                        if (falseMatch)
                        {
                            isMatch = false;
                        }
                    }

                    if (isMatch)
                    {
                        if (found.MatchCounts.ContainsKey(phrase.Text))
                        {
                            found.MatchCounts[phrase.Text] += 1;
                        }
                        else
                        {
                            found.MatchCounts.Add(phrase.Text, 1);
                        }

                        StringBuilder context = new();
                        for (int j = lastBreak + 1; j < tokenizer.OrderedTokens.Count; j++)
                        {
                            Token ahead = tokenizer.OrderedTokens[j];
                            Token next = j + 1 < tokenizer.OrderedTokens.Count ? tokenizer.OrderedTokens[j + 1] : null;

                            context.Append(ahead.Text);

                            if (ahead.Length == 1)
                            {
                                char a = ahead.Text[0];

                                if (Topic.ClauseBoundaries.Contains(a))
                                {
                                    break;
                                }
                            }

                            if ((next?.IsWord ?? false) || (next?.IsNumber ?? false))
                            {
                                context.Append(' ');
                            }
                        }

                        string exerpt = context.ToString();

                        // If two terms appear in the same sentence, the above code
                        // will capture them twice.
                        if(!found.MatchQuotes.Contains(exerpt))
                        {
                            found.MatchQuotes.Add(exerpt);
                        }
                    }
                }
            }

            return found;
        }
    }
}
