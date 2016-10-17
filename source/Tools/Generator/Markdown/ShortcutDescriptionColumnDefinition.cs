
namespace Pihrtsoft.Snippets.CodeGeneration.Markdown
{
    internal class ShortcutDescriptionColumnDefinition : ColumnDefinition
    {
        public override string Title
        {
            get { return "Description"; }
        }

        public override string GetValue(object value)
        {
            return ((CharacterSequence)value).Description;
        }
    }
}
