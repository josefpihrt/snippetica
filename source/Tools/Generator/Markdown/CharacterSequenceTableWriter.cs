namespace Pihrtsoft.Snippets.CodeGeneration.Markdown
{
    public class CharacterSequenceTableWriter : MarkdownTableWriter
    {
        public CharacterSequenceTableWriter(ColumnDefinition[] definitions)
            : base(definitions)
        {
        }

        public static CharacterSequenceTableWriter Create()
        {
            return new CharacterSequenceTableWriter(new ColumnDefinition[]
            {
                new ShortcutSequenceColumnDefinition(),
                new ShortcutDescriptionColumnDefinition(),
                new ShortcutCommentColumnDefinition()
            });
        }
    }
}
