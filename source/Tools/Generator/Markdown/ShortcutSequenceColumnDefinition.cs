
namespace Pihrtsoft.Snippets.CodeGeneration.Markdown
{
    internal class ShortcutSequenceColumnDefinition : ColumnDefinition
    {
        public override string Title
        {
            get { return "Character(s)"; }
        }

        public override string GetValue(object value)
        {
            return MarkdownHelper.Escape(((CharacterSequence)value).Value);
        }
    }
}
