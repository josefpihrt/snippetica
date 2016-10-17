
namespace Pihrtsoft.Snippets.CodeGeneration.Markdown
{
    internal class SnippetLanguageColumnDefinition : ColumnDefinition
    {
        public override string Title
        {
            get { return "Language"; }
        }

        public override string GetValue(object value)
        {
            return MarkdownHelper.Escape(LanguageHelper.GetLanguageTitle(((Snippet)value).Language));
        }
    }
}
