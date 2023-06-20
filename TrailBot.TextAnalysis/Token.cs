using System;

namespace CascadePass.TrailBot.TextAnalysis
{
    public class Token
    {
        private bool? isWord, isNumber, isPunctuation, isSymbol;
        private long? length;

        protected Token() { }

        public Token(string text)
        {
            this.Text = text;
        }

        public string Text { get; protected set; }

        public virtual bool IsWord
        {
            get
            {
                if (!this.isWord.HasValue)
                {
                    this.isWord = true;

                    foreach (char c in this.Text)
                    {
                        if (!Char.IsLetter(c) && c != '\'')
                        {
                            this.isWord = false;
                            break;
                        }
                    }
                }

                return this.isWord.Value;
            }
        }

        public virtual bool IsNumber
        {
            get
            {
                if (!this.isNumber.HasValue)
                {
                    this.isNumber = true;

                    bool foundDiget = false;
                    foreach (char c in this.Text)
                    {
                        foundDiget |= char.IsDigit(c);
                        if (!Char.IsDigit(c) && c != '.' && c != ',')
                        {
                            this.isNumber = false;
                            break;
                        }
                    }

                    //if (this.isNumber.Value && (this.Text.Contains(".") || this.Text.Contains(",")))
                    //{
                    //    this.isNumber = this.Text.Any(chr => Char.IsDigit(chr));
                    //}

                    this.isNumber = this.isNumber.Value && foundDiget;
                }

                return this.isNumber.Value;
            }
        }

        public bool IsPunctuation
        {
            get
            {
                if (!this.isPunctuation.HasValue)
                {
                    this.isPunctuation = true;

                    foreach (char c in this.Text)
                    {
                        this.isPunctuation = c switch
                        {
                            '.' => true,
                            '!' => true,
                            '?' => true,
                            ',' => true,
                            ';' => true,
                            ':' => true,
                            _ => false,
                        };

                        if (!this.isPunctuation.Value)
                        {
                            break;
                        }
                    }
                }

                return this.isPunctuation.Value;
            }
        }

        public virtual long Length => this.length ??= this.Text.Length;

        public override string ToString()
        {
            return this.Text;
        }
    }
}
