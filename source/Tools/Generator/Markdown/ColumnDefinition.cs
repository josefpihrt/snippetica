using System;

namespace Pihrtsoft.Snippets.CodeGeneration.Markdown
{
    public abstract class ColumnDefinition
    {
        public abstract string Title { get; }

        public abstract string GetValue(object value);
    }
}
