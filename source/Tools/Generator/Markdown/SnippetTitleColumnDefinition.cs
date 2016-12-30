
namespace Pihrtsoft.Snippets.CodeGeneration.Markdown
{
    internal class SnippetTitleColumnDefinition : ColumnDefinition
    {
        public override string Title
        {
            get { return "Title"; }
        }

        public override string GetValue(object value)
        {
            var snippet = (Snippet)value;

            return MarkdownHelper.Escape(snippet.GetTitleWithoutShortcut());
        }
    }
}
