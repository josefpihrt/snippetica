
namespace Pihrtsoft.Snippets.CodeGeneration.Markdown
{
    internal class SnippetShortcutColumnDefinition : ColumnDefinition
    {
        public override string Title
        {
            get { return "Shortcut"; }
        }

        public override string GetValue(object value)
        {
            return ((Snippet)value).Shortcut;
        }
    }
}
