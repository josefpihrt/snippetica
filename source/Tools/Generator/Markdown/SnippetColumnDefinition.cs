
namespace Pihrtsoft.Snippets.CodeGeneration.Markdown
{
    public abstract class SnippetColumnDefinition
    {
        public abstract string Title { get; }

        public abstract string GetValue(Snippet snippet);
    }
}
