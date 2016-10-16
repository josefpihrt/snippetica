using System.Text;

namespace Pihrtsoft.Snippets.CodeGeneration
{
    public class SnippetCodeWriter
    {
        public SnippetCodeWriter()
        {
            StringBuilder = new StringBuilder();
        }

        public StringBuilder StringBuilder { get; }

        public char Delimiter { get; set; } = '$';

        public override string ToString()
        {
            return StringBuilder.ToString();
        }

        public void WritePlaceholder(string identifier)
        {
            WriteDelimiter();
            Write(identifier);
            WriteDelimiter();
        }

        public void WriteEndPlaceholder()
        {
            WritePlaceholder("end");
        }

        internal void WriteSelectedPlaceholder()
        {
            WritePlaceholder("selected");
        }

        public void WriteDelimiter()
        {
            Write(Delimiter);
        }

        public void Write(string value)
        {
            StringBuilder.Append(value);
        }

        public void Write(char value)
        {
            StringBuilder.Append(value);
        }
    }
}
