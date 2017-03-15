using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Pihrtsoft.Snippets.CodeGeneration.Commands;

namespace Pihrtsoft.Snippets.CodeGeneration
{
    public class XamlSnippetGenerator
    {
        public IEnumerable<Snippet> GenerateSnippets(string sourceDirectoryPath)
        {
            return SnippetSerializer.Deserialize(sourceDirectoryPath, SearchOption.AllDirectories)
                .SelectMany(snippet => GenerateSnippets(snippet));
        }

        public IEnumerable<Snippet> GenerateSnippets(Snippet snippet)
        {
            var jobs = new JobCollection();

            if (snippet.HasTag(KnownTags.GenerateAlternativeShortcut))
            {
                jobs.AddCommand(new SimpleCommand(f => f.Shortcut = f.Shortcut.ToLowerInvariant(), CommandKind.ShortcutToLowercase));
                jobs.AddCommand(new AlternativeShortcutCommand());
            }

            //if (snippet.HasTag(KnownTags.GenerateXamlProperty))
            //    jobs.AddCommand(new XamlPropertyCommand());

            foreach (Job job in jobs)
            {
                var context = new ExecutionContext((Snippet)snippet.Clone());

                job.Execute(context);

                if (!context.IsCanceled)
                {
                    foreach (Snippet snippet2 in context.Snippets)
                    {
                        snippet2.SortCollections();
                        yield return snippet2;
                    }
                }
            }
        }
    }
}
