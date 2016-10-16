
namespace Pihrtsoft.Snippets.CodeGeneration.Markdown
{
    internal class SnippetLanguageColumnDefinition : SnippetColumnDefinition
    {
        public override string Title
        {
            get { return "Language"; }
        }

        public override string GetValue(Snippet snippet)
        {
            return LanguageHelper.GetLanguageTitle(snippet.Language);
        }
    }
}
