
namespace Pihrtsoft.Snippets.CodeGeneration.Markdown
{
    internal class ShortcutCommentColumnDefinition : ColumnDefinition
    {
        public override string Title
        {
            get { return "Comment"; }
        }

        public override string GetValue(object value)
        {
            return ((CharacterSequence)value).Comment;
        }
    }
}
