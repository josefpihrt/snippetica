// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.IO;
using System.Linq;
using Pihrtsoft.Snippets;
using static Snippetica.CodeGeneration.CodeGenerationUtility;
using static Snippetica.KnownNames;
using static Snippetica.KnownPaths;

namespace Snippetica.CodeGeneration.Markdown
{
    public static class MarkdownGenerator
    {
        public static string GenerateProjectReadme(
            IEnumerable<SnippetGeneratorResult> results,
            ProjectReadmeSettings settings)
        {
            using (var sw = new StringWriter())
            {
                GenerateProjectReadme(results, sw, settings);

                return sw.ToString();
            }
        }

        public static void GenerateProjectReadme(
            IEnumerable<SnippetGeneratorResult> results,
            TextWriter writer,
            ProjectReadmeSettings settings)
        {
            if (!string.IsNullOrEmpty(settings.Header))
            {
                writer.WriteLine($"## {settings.Header}");
                writer.WriteLine();
            }

            writer.WriteLine($"* Browse all available snippets with [Snippet Browser]({GetSnippetBrowserUrl(settings.Environment.Kind)}).");
            writer.WriteLine($"* Download extension from [Marketplace](http://marketplace.visualstudio.com/search?term=publisher%3A\"Josef%20Pihrt\"%20{ProductName}&target={settings.Environment.Kind.GetIdentifier()}&sortBy=Name).");
            writer.WriteLine();

            writer.WriteLine("### Snippets");
            writer.WriteLine();

            writer.WriteLine("Group|Count| |");
            writer.WriteLine("--- | --- | ---:");

            foreach (SnippetGeneratorResult result in results.OrderBy(f => f.DirectoryName))
            {
                writer.WriteLine($"[{result.DirectoryName}]({VisualStudioExtensionGitHubUrl}/{result.DirectoryName}/{ReadMeFileName})|{result.Snippets.Count}|[Browse]({GetSnippetBrowserUrl(settings.Environment.Kind, result.Language)})");
            }
        }

        public static string GenerateDirectoryReadme(
            IEnumerable<Snippet> snippets,
            DirectoryReadmeSettings settings)
        {
            using (var sw = new StringWriter())
            {
                snippets = GenerateDirectoryReadme(snippets, sw, settings);

                return sw.ToString();
            }
        }

        public static IEnumerable<Snippet> GenerateDirectoryReadme(
            IEnumerable<Snippet> snippets,
            TextWriter writer,
            DirectoryReadmeSettings settings)
        {
            if (!string.IsNullOrEmpty(settings.Header))
            {
                writer.WriteLine($"## {settings.Header}");
                writer.WriteLine();
            }

            if (!settings.IsDevelopment)
            {
                writer.WriteLine("### Snippet Browser");
                writer.WriteLine($"* Browse all available snippets with [Snippet Browser]({GetSnippetBrowserUrl(settings.Environment.Kind, settings.Language)}).");
                writer.WriteLine();
            }

            if (!settings.IsDevelopment
                && settings.AddQuickReference)
            {
                List<ShortcutInfo> shortcuts = settings.Shortcuts
                    .Where(f => f.Languages.Contains(settings.Language))
                    .ToList();

                if (shortcuts.Count > 0)
                {
                    writer.WriteLine("### Quick Reference");
                    writer.WriteLine();

                    if (settings.QuickReferenceText != null)
                    {
                        writer.WriteLine(settings.QuickReferenceText);
                    }

                    if (settings.GroupShortcuts)
                    {
                        foreach (IGrouping<ShortcutKind, ShortcutInfo> grouping in shortcuts
                            .GroupBy(f => f.Kind)
                            .OrderBy(f => f.Key))
                        {
                            writer.WriteLine();
                            writer.WriteLine($"#### {grouping.Key.GetTitle()}");
                            writer.WriteLine();

                            using (ShortcutInfoTableWriter tableWriter = ShortcutInfoTableWriter.Create())
                            {
                                tableWriter.WriteTable(grouping);
                                writer.Write(tableWriter.ToString());
                            }
                        }
                    }
                    else
                    {
                        writer.WriteLine();

                        using (ShortcutInfoTableWriter tableWriter = ShortcutInfoTableWriter.Create())
                        {
                            tableWriter.WriteTable(shortcuts);
                            writer.Write(tableWriter.ToString());
                        }
                    }


                    writer.WriteLine();
                }
            }

            writer.WriteLine("### List of Selected Snippets");
            writer.WriteLine();

            using (SnippetTableWriter tableWriter = (settings.AddLinkToTitle)
                ? SnippetTableWriter.CreateShortcutThenTitleWithLink(settings.DirectoryPath)
                : SnippetTableWriter.CreateShortcutThenTitle())
            {
                snippets = snippets.Where(f => !f.HasTag(KnownTags.ExcludeFromReadme));

                tableWriter.WriteTable(snippets);
                writer.Write(tableWriter.ToString());
            }

            return snippets;
        }
    }
}
