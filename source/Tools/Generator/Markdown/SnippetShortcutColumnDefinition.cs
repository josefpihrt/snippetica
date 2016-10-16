
namespace Pihrtsoft.Snippets.CodeGeneration.Markdown
{
    internal class SnippetShortcutColumnDefinition : SnippetColumnDefinition
    {
        public override string Title
        {
            get { return "Shortcut"; }
        }

        public override string GetValue(Snippet snippet)
        {
            return snippet.Shortcut;
        }
    }
}
