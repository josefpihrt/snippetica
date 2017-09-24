// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Snippetica.CodeGeneration.Markdown
{
    public class SnippetTableWriter : MarkdownTableWriter
    {
        public SnippetTableWriter(ColumnDefinition[] definitions)
            : base(definitions)
        {
        }

        public static SnippetTableWriter CreateTitleThenShortcut()
        {
            return new SnippetTableWriter(new ColumnDefinition[]
            {
                new TitleColumnDefinition(),
                new ShortcutColumnDefinition()
            });
        }

        public static SnippetTableWriter CreateTitleWithLinkThenShortcut(string directoryPath)
        {
            return new SnippetTableWriter(new ColumnDefinition[]
            {
                new TitleWithLinkColumnDefinition(directoryPath),
                new ShortcutColumnDefinition()
            });
        }

        public static SnippetTableWriter CreateShortcutThenTitle()
        {
            return new SnippetTableWriter(new ColumnDefinition[]
            {
                new ShortcutColumnDefinition(),
                new TitleColumnDefinition()
            });
        }

        public static SnippetTableWriter CreateShortcutThenTitleWithLink(string directoryPath)
        {
            return new SnippetTableWriter(new ColumnDefinition[]
            {
                new ShortcutColumnDefinition(),
                new TitleWithLinkColumnDefinition(directoryPath)
            });
        }

        public static SnippetTableWriter CreateLanguageThenTitleWithLinkThenShortcut(string directoryPath)
        {
            return new SnippetTableWriter(new ColumnDefinition[]
            {
                new LanguageColumnDefinition(),
                new TitleWithLinkColumnDefinition(directoryPath),
                new ShortcutColumnDefinition()
            });
        }

        public static SnippetTableWriter CreateLanguageThenShortcutThenTitle()
        {
            return new SnippetTableWriter(new ColumnDefinition[]
            {
                new LanguageColumnDefinition(),
                new ShortcutColumnDefinition(),
                new TitleColumnDefinition()
            });
        }

        public static SnippetTableWriter CreateLanguageThenShortcutThenTitleWithLink(string directoryPath)
        {
            return new SnippetTableWriter(new ColumnDefinition[]
            {
                new LanguageColumnDefinition(),
                new ShortcutColumnDefinition(),
                new TitleWithLinkColumnDefinition(directoryPath)
            });
        }
    }
}
