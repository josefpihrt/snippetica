// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Snippetica.CodeGeneration.Markdown
{
    public class MarkdownTableWriter : StringWriter
    {
        public MarkdownTableWriter(ColumnDefinition[] definitions)
        {
            ColumnDefinitions = definitions;
        }

        public IEnumerable<ColumnDefinition> ColumnDefinitions { get; }

        public void WriteTable(IEnumerable<object> values)
        {
            WriteHeader();
            WriteRows(values);
        }

        public void WriteHeader()
        {
            WriteHeaderTitles();
            WriteHeaderHyphens();
        }

        private void WriteHeaderTitles()
        {
            bool isFirst = true;

            foreach (ColumnDefinition definition in ColumnDefinitions)
            {
                if (isFirst)
                {
                    isFirst = false;
                }
                else
                {
                    Write(" ");
                    WriteColumnSeparator();
                    Write(" ");
                }

                Write(MarkdownHelper.Escape(definition.Title));
            }

            WriteLine();
        }

        private void WriteHeaderHyphens()
        {
            bool isFirst = true;

            foreach (ColumnDefinition definition in ColumnDefinitions)
            {
                if (isFirst)
                {
                    isFirst = false;
                }
                else
                {
                    Write(" ");
                    WriteColumnSeparator();
                    Write(" ");
                }

                Write(new string('-', MarkdownHelper.Escape(definition.Title).Length));
            }

            WriteLine();
        }

        public void WriteColumnSeparator()
        {
            Write("|");
        }

        public void WriteRows(IEnumerable<object> values)
        {
            foreach (object value in Sort(values))
            {
                WriteRow(value);
            }
        }

        protected virtual IEnumerable<object> Sort(IEnumerable<object> values)
        {
            bool isFirst = true;

            foreach (ColumnDefinition definition in ColumnDefinitions)
            {
                if (isFirst)
                {
                    values = values.OrderBy(f => definition.GetValue(f));

                    isFirst = false;
                }
                else
                {
                    values = ((IOrderedEnumerable<object>)values).ThenBy(f => definition.GetValue(f));
                }
            }

            return values;
        }

        public void WriteRow(object value)
        {
            bool isFirst = true;

            foreach (ColumnDefinition definition in ColumnDefinitions)
            {
                if (isFirst)
                {
                    isFirst = false;
                }
                else
                {
                    WriteColumnSeparator();
                }

                Write(definition.GetValue(value));
            }

            WriteLine();
        }
    }
}
