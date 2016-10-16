using System.IO;
using System.Text.RegularExpressions;

namespace Pihrtsoft.Snippets.CodeGeneration.Markdown
{
    internal class SnippetTitleColumnDefinition : SnippetColumnDefinition
    {
        private readonly string _pattern;

        public SnippetTitleColumnDefinition(string directoryPath)
        {
            DirectoryPath = directoryPath;

            _pattern = $"^{Regex.Escape(DirectoryPath)}{Regex.Escape(Path.DirectorySeparatorChar.ToString())}?";
        }

        public override string Title
        {
            get { return "Title"; }
        }

        public string DirectoryPath { get; }

        public override string GetValue(Snippet snippet)
        {
            string path = Regex.Replace(
                snippet.FilePath,
                _pattern,
                "",
                RegexOptions.IgnoreCase);

            return $"[{snippet.GetTitleWithoutShortcut()}]({path})";
        }
    }
}
