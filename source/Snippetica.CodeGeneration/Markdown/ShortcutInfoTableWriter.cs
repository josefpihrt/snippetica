// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Snippetica.CodeGeneration.Markdown
{
    public class ShortcutInfoTableWriter : MarkdownTableWriter
    {
        public ShortcutInfoTableWriter(ColumnDefinition[] definitions)
            : base(definitions)
        {
        }

        public static ShortcutInfoTableWriter Create()
        {
            return new ShortcutInfoTableWriter(new ColumnDefinition[]
            {
                new ShortcutInfoColumnDefinition(),
                new ShortcutDescriptionColumnDefinition(),
                new ShortcutCommentColumnDefinition()
            });
        }
    }
}
