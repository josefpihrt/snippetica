// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Snippetica.CodeGeneration.Markdown
{
    internal class ShortcutCommentColumnDefinition : ColumnDefinition
    {
        public override string Title
        {
            get { return "Comment"; }
        }

        public override string GetValue(object value)
        {
            return MarkdownHelper.Escape(((ShortcutInfo)value).Comment);
        }
    }
}
