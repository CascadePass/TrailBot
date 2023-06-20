using System.Text;

namespace CascadePass.TrailBot.TextAnalysis
{
    public class Phrase : Token
    {
        private long? length;

        protected Phrase() : base() { }

        public Phrase(Token[] tokens)
        {
            this.Parts = tokens;

            StringBuilder stringBuilder = new();

            for(int i = 0; i < tokens.Length; i++)
            {
                Token t = tokens[i];
                Token next = i < tokens.Length - 1 ? tokens[i + 1] : null;

                stringBuilder.Append(t.Text);

                if (next?.IsWord ?? false)
                {
                    stringBuilder.Append(' ');
                }
            }

            this.Text = stringBuilder.ToString();
        }

        public Token[] Parts { get; protected set; }

        public override bool IsWord => false;

        public override bool IsNumber => false;

        public override long Length => this.length ??= this.Parts.Length;
    }
}
