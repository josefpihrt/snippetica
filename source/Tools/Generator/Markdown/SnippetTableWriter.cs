namespace Pihrtsoft.Snippets.CodeGeneration.Markdown
{
    public class SnippetTableWriter : MarkdownTableWriter
    {
        public SnippetTableWriter(ColumnDefinition[] definitions)
            : base(definitions)
        {
        }

        public static SnippetTableWriter CreateTitleThenShortcut(string directoryPath)
        {
            return new SnippetTableWriter(new ColumnDefinition[]
            {
                new SnippetTitleColumnDefinition(directoryPath),
                new SnippetShortcutColumnDefinition()
            });
        }

        public static SnippetTableWriter CreateShortcutThenTitle(string directoryPath)
        {
            return new SnippetTableWriter(new ColumnDefinition[]
            {
                new SnippetShortcutColumnDefinition(),
                new SnippetTitleColumnDefinition(directoryPath)
            });
        }

        public static SnippetTableWriter CreateLanguageThenTitleThenShortcut(string directoryPath)
        {
            return new SnippetTableWriter(new ColumnDefinition[]
            {
                new SnippetLanguageColumnDefinition(),
                new SnippetTitleColumnDefinition(directoryPath),
                new SnippetShortcutColumnDefinition(),
            });
        }

        public static SnippetTableWriter CreateLanguageThenShortcutThenTitle(string directoryPath)
        {
            return new SnippetTableWriter(new ColumnDefinition[]
            {
                new SnippetLanguageColumnDefinition(),
                new SnippetShortcutColumnDefinition(),
                new SnippetTitleColumnDefinition(directoryPath),
            });
        }
    }
}
