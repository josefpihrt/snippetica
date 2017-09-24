// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Pihrtsoft.Snippets;

namespace Snippetica.CodeGeneration.Markdown
{
    internal class ShortcutColumnDefinition : ColumnDefinition
    {
        public override string Title
        {
            get { return "Shortcut"; }
        }

        public override string GetValue(object value)
        {
            return MarkdownHelper.Escape(((Snippet)value).Shortcut);
        }
    }
}
