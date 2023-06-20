using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace CascadePass.TrailBot.TextAnalysis
{
    public class Tokenizer
    {
        /// <summary>
        /// Regular expression used to identify word boundaries in a string.
        /// </summary>
        private static readonly Regex wordBoundaries;

        /// <summary>
        /// Static type constructor creates and compiles the regular expression used
        /// to break text apart into words (and other token types).
        /// </summary>
        /// <remarks>
        /// This method runs once during runtime, when the type <see cref="Tokenizer"/>
        /// is first used.
        /// </remarks>
        static Tokenizer()
        {
            // RegexOptions.Compiled slows startup by creating and compiling an
            // assembly in memory, but is much faster to use.

            Tokenizer.wordBoundaries = new Regex(@"(?<!')\b(?!')", RegexOptions.Compiled);
        }

        /// <summary>
        /// Gets the text that was parsed.
        /// </summary>
        public string SourceText { get; private set;  }

        /// <summary>
        /// Gets the maximum phrase length (in <see cref="Token"/>s) used
        /// during parsing of <see cref="SourceText"/>.
        /// </summary>
        public int MaximumPhraseLength { get; set; }

        public List<Token> OrderedTokens { get; private set; }

        public List<Phrase> Phrases { get; private set; }

        public List<TokenCount> TokenCounts { get; private set; }


        public void GetTokens(string text)
        {
            this.GetTokens(text, int.MinValue);
        }

        public void GetTokens(string text, int maximumPhraseLength)
        {
            this.SourceText = text;
            this.MaximumPhraseLength = maximumPhraseLength;
            this.OrderedTokens = new();

            foreach (string token in wordBoundaries.Split(text))
            {
                if (!string.IsNullOrWhiteSpace(token))
                {
                    this.OrderedTokens.Add(new Token(token.Trim()));
                }
            }

            if (maximumPhraseLength > 1)
            {
                this.GetPhrases(maximumPhraseLength);
            }

            this.GetCounts();
        }

        private void GetPhrases(int maximumPhraseLength)
        {
            int max = this.OrderedTokens.Count;
            List<Phrase> foundPhrases = new();

            for (int startingPosition = 0; startingPosition <= max; startingPosition++)
            {
                for (int currentPhraseLength = 2; currentPhraseLength < maximumPhraseLength; currentPhraseLength++)
                {
                    List<Token> partsOfPhrase = new();

                    for (int currentSearchDepth = 0; currentSearchDepth < currentPhraseLength; currentSearchDepth++)
                    {
                        int index = startingPosition + currentSearchDepth;

                        if (index >= max)
                        {
                            break;
                        }

                        partsOfPhrase.Add(this.OrderedTokens[index]);
                    }

                    foundPhrases.Add(new(partsOfPhrase.ToArray()));
                }
            }

            this.Phrases = foundPhrases;
        }

        private void GetCounts()
        {
            Dictionary<string, int> counts = new();
            Dictionary<string, Token> map = new();
            this.TokenCounts = new();

            foreach (Token t in this.OrderedTokens)
            {
                if (!counts.ContainsKey(t.Text))
                {
                    counts.Add(t.Text, 1);
                    map.Add(t.Text, t);
                }
                else
                {
                    counts[t.Text] += 1;
                }
            }

            if (this.Phrases != null)
            {
                foreach (Token t in this.Phrases)
                {
                    if (!counts.ContainsKey(t.Text))
                    {
                        counts.Add(t.Text, 1);
                        map.Add(t.Text, t);
                    }
                    else
                    {
                        counts[t.Text] += 1;
                    }
                }
            }

            foreach (var item in counts)
            {
                Token t = map[item.Key];

                TokenCount result = new() { Token = t, Count = item.Value };
                this.TokenCounts.Add(result);
            }
        }

        public bool IsMatchAt(Phrase phrase, int position)
        {
            #region Sanity checks

            if (phrase == null)
            {
                return false;
            }

            if (position < 0 || position + phrase.Length > this.OrderedTokens.Count)
            {
                return false;
            }

            #endregion

            bool isMatch = true;
            int currentPhraseLength = (int)phrase.Length;

            for (int i = 0; i < currentPhraseLength; i++)
            {
                if (position + i >= this.OrderedTokens.Count)
                {
                    return isMatch;
                }

                if (!string.Equals(this.OrderedTokens[position + i].Text, phrase.Parts[i].Text, StringComparison.OrdinalIgnoreCase))
                {
                    return false;
                }
            }

            return isMatch;
        }
    }
}
