// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Pihrtsoft.Snippets.CodeGeneration.Markdown
{
    public class SnippetTableWriter : MarkdownTableWriter
    {
        public SnippetTableWriter(ColumnDefinition[] definitions)
            : base(definitions)
        {
        }

        public static SnippetTableWriter CreateTitleWithLinkThenShortcut(string directoryPath)
        {
            return new SnippetTableWriter(new ColumnDefinition[]
            {
                new SnippetTitleWithLinkColumnDefinition(directoryPath),
                new SnippetShortcutColumnDefinition()
            });
        }

        public static SnippetTableWriter CreateShortcutThenTitleWithLink(string directoryPath)
        {
            return new SnippetTableWriter(new ColumnDefinition[]
            {
                new SnippetShortcutColumnDefinition(),
                new SnippetTitleWithLinkColumnDefinition(directoryPath)
            });
        }

        public static SnippetTableWriter CreateLanguageThenTitleWithLinkThenShortcut(string directoryPath)
        {
            return new SnippetTableWriter(new ColumnDefinition[]
            {
                new SnippetLanguageColumnDefinition(),
                new SnippetTitleWithLinkColumnDefinition(directoryPath),
                new SnippetShortcutColumnDefinition(),
            });
        }

        public static SnippetTableWriter CreateLanguageThenShortcutThenTitle()
        {
            return new SnippetTableWriter(new ColumnDefinition[]
            {
                new SnippetLanguageColumnDefinition(),
                new SnippetShortcutColumnDefinition(),
                new SnippetTitleColumnDefinition(),
            });
        }

        public static SnippetTableWriter CreateLanguageThenShortcutThenTitleWithLink(string directoryPath)
        {
            return new SnippetTableWriter(new ColumnDefinition[]
            {
                new SnippetLanguageColumnDefinition(),
                new SnippetShortcutColumnDefinition(),
                new SnippetTitleWithLinkColumnDefinition(directoryPath),
            });
        }
    }
}
