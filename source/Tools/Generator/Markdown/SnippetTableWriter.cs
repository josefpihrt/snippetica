using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Pihrtsoft.Snippets.CodeGeneration.Markdown
{
    public class SnippetTableWriter : StringWriter
    {
        public SnippetTableWriter(SnippetColumnDefinition[] definitions)
        {
            ColumnDefinitions = definitions;
        }

        public IEnumerable<SnippetColumnDefinition> ColumnDefinitions { get; }

        public static SnippetTableWriter CreateTitleThenShortcut(string directoryPath)
        {
            return new SnippetTableWriter(new SnippetColumnDefinition[]
            {
                new SnippetTitleColumnDefinition(directoryPath),
                new SnippetShortcutColumnDefinition()
            });
        }

        public static SnippetTableWriter CreateShortcutThenTitle(string directoryPath)
        {
            return new SnippetTableWriter(new SnippetColumnDefinition[]
            {
                new SnippetShortcutColumnDefinition(),
                new SnippetTitleColumnDefinition(directoryPath)
            });
        }

        public static SnippetTableWriter CreateLanguageThenTitleThenShortcut(string directoryPath)
        {
            return new SnippetTableWriter(new SnippetColumnDefinition[]
            {
                new SnippetLanguageColumnDefinition(),
                new SnippetTitleColumnDefinition(directoryPath),
                new SnippetShortcutColumnDefinition(),
            });
        }

        public static SnippetTableWriter CreateLanguageThenShortcutThenTitle(string directoryPath)
        {
            return new SnippetTableWriter(new SnippetColumnDefinition[]
            {
                new SnippetLanguageColumnDefinition(),
                new SnippetShortcutColumnDefinition(),
                new SnippetTitleColumnDefinition(directoryPath),
            });
        }

        public void WriteTable(IEnumerable<Snippet> snippets)
        {
            WriteHeader();
            WriteRows(snippets);
        }

        public void WriteHeader()
        {
            WriteHeaderTitles();
            WriteHeaderHyphens();
        }

        private void WriteHeaderTitles()
        {
            bool isFirst = true;

            foreach (SnippetColumnDefinition definition in ColumnDefinitions)
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

                Write(definition.Title);
            }

            WriteLine();
        }

        private void WriteHeaderHyphens()
        {
            bool isFirst = true;

            foreach (SnippetColumnDefinition definition in ColumnDefinitions)
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

                Write(new string('-', definition.Title.Length));
            }

            WriteLine();
        }

        public void WriteColumnSeparator()
        {
            Write("|");
        }


        public void WriteRows(IEnumerable<Snippet> snippets)
        {
            foreach (Snippet snippet in SortSnippets(snippets))
            {
                WriteRow(snippet);
            }
        }

        protected virtual IEnumerable<Snippet> SortSnippets(IEnumerable<Snippet> snippets)
        {
            bool isFirst = true;

            foreach (SnippetColumnDefinition definition in ColumnDefinitions)
            {
                if (isFirst)
                {
                    snippets = snippets.OrderBy(f => definition.GetValue(f));

                    isFirst = false;
                }
                else
                {
                    snippets = ((IOrderedEnumerable<Snippet>)snippets).ThenBy(f => definition.GetValue(f));
                }
            }

            return snippets;
        }

        public void WriteRow(Snippet snippet)
        {
            bool isFirst = true;

            foreach (SnippetColumnDefinition definitions in ColumnDefinitions)
            {
                if (isFirst)
                {
                    isFirst = false;
                }
                else
                {
                    WriteColumnSeparator();
                }

                Write(definitions.GetValue(snippet));
            }

            WriteLine();
        }
    }
}
