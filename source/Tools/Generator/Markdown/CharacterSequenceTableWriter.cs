// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

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
