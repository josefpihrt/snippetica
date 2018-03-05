// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using DotMarkdown;
using DotMarkdown.Linq;
using Pihrtsoft.Snippets;
using static System.Environment;
using static DotMarkdown.Linq.MFactory;
using static Snippetica.CodeGeneration.CodeGenerationUtility;
using static Snippetica.KnownNames;
using static Snippetica.KnownPaths;

namespace Snippetica.CodeGeneration.Markdown
{
    public static class MarkdownGenerator
    {
        private static string GetString(this MDocument document, bool addFootnote = true)
        {
            if (addFootnote)
                document.Add(NewLine, Italic("(Generated with ", Link("DotMarkdown", "http://github.com/JosefPihrt/DotMarkdown"), ")"));

            return document.ToString(MarkdownFormat.Default.WithTableOptions(TableOptions.FormatHeader));
        }

        public static string GenerateProjectReadme(
            IEnumerable<SnippetGeneratorResult> results,
            ProjectReadmeSettings settings)
        {
            MDocument document = Document();

            return GenerateProjectReadme(results, document, settings);
        }

        public static string GenerateProjectReadme(
            IEnumerable<SnippetGeneratorResult> results,
            MDocument document,
            ProjectReadmeSettings settings,
            bool addFootnote = true)
        {
            document.Add(
                (!string.IsNullOrEmpty(settings.Header)) ? Heading2(settings.Header) : null,
                BulletItem("Browse all available snippets with ", Link("Snippet Browser", GetSnippetBrowserUrl(settings.Environment.Kind)), "."),
                BulletItem("Download extension from ", Link("Marketplace", $"http://marketplace.visualstudio.com/search?term=publisher%3A\"Josef%20Pihrt\"%20{ProductName}&target={settings.Environment.Kind.GetIdentifier()}&sortBy=Name"), "."),
                Heading3("Snippets"),
                Table(
                    TableRow("Group", "Count", TableColumn(HorizontalAlignment.Right)),
                    results.OrderBy(f => f.DirectoryName).Select(f =>
                    {
                        return TableRow(
                            Link(f.DirectoryName, $"{VisualStudioExtensionGitHubUrl}/{f.DirectoryName}/{ReadMeFileName}"),
                            f.Snippets.Count,
                            Link("Browse", GetSnippetBrowserUrl(settings.Environment.Kind, f.Language)));
                    })));

            return document.GetString(addFootnote: addFootnote);
        }

        public static string GenerateDirectoryReadme(
            IEnumerable<Snippet> snippets,
            DirectoryReadmeSettings settings)
        {
            MDocument document = Document();

            return GenerateDirectoryReadme(snippets, document, settings);
        }

        public static string GenerateDirectoryReadme(
            IEnumerable<Snippet> snippets,
            MDocument document,
            DirectoryReadmeSettings settings)
        {
            document.Add((!string.IsNullOrEmpty(settings.Header)) ? Heading2(settings.Header) : null);

            if (!settings.IsDevelopment)
            {
                document.Add(
                    Heading3("Snippet Browser"),
                    BulletItem("Browse all available snippets with ", Link("Snippet Browser", GetSnippetBrowserUrl(settings.Environment.Kind, settings.Language)), "."));
            }

            if (!settings.IsDevelopment
                && settings.AddQuickReference)
            {
                document.Add(GetQuickReference());
            }

            document.Add(Heading3("List of Selected Snippets"));

            document.Add(Table(
                TableRow("Shortcut", "Title"),
                snippets
                    .Where(f => !f.HasTag(KnownTags.ExcludeFromReadme))
                    .OrderBy(f => f.Shortcut)
                    .ThenBy(f => f.GetTitle())
                    .Select(f =>
                    {
                        return TableRow(
                            f.Shortcut,
                            LinkOrText(f.GetTitle(), GetSnippetPath(f)));
                    })));

            return document.GetString();

            IEnumerable<object> GetQuickReference()
            {
                List<ShortcutInfo> shortcuts = settings.Shortcuts
                    .Where(f => f.Languages.Contains(settings.Language))
                    .ToList();

                if (shortcuts.Count > 0)
                {
                    yield return Heading3("Quick Reference");

                    if (settings.QuickReferenceText != null)
                        yield return Raw(settings.QuickReferenceText);

                    if (settings.GroupShortcuts)
                    {
                        foreach (IGrouping<ShortcutKind, ShortcutInfo> grouping in shortcuts
                            .GroupBy(f => f.Kind)
                            .OrderBy(f => f.Key))
                        {
                            string title = grouping.Key.GetTitle();

                            if (!string.IsNullOrEmpty(title))
                                yield return Heading4(title);

                            yield return TableWithShortcutInfoValueDesriptionComment(grouping);
                        }
                    }
                    else
                    {
                        yield return NewLine;
                        yield return TableWithShortcutInfoValueDesriptionComment(shortcuts);
                    }
                }
            }

            string GetSnippetPath(Snippet snippet)
            {
                if (!settings.AddLinkToTitle)
                    return null;

                string _pattern = $"^{Regex.Escape(settings.DirectoryPath)}{Regex.Escape(Path.DirectorySeparatorChar.ToString())}?";

                string path = Regex.Replace(
                    snippet.FilePath,
                    _pattern,
                    "",
                    RegexOptions.IgnoreCase);

                return path.Replace('\\', '/');
            }

            MTable TableWithShortcutInfoValueDesriptionComment(IEnumerable<ShortcutInfo> shortcuts)
            {
                return Table(
                    TableRow("Shortcut", "Description", "Comment"),
                    shortcuts
                        .OrderBy(f => f.Value)
                        .ThenBy(f => f.Description)
                        .Select(f => TableRow(f.Value, f.Description, f.Comment)));
            }
        }

        public static string GenerateVisualStudioMarketplaceOverview(IEnumerable<SnippetGeneratorResult> results)
        {
            MDocument document = Document(
                Heading3("Introduction"),
                BulletItem(GetProjectSubtitle(results)),
                Heading3("Links"),
                BulletList(
                    Link("Project Website", GitHubUrl),
                    Link("Release Notes", $"{MasterGitHubUrl}/{ChangeLogFileName}"),
                    BulletItem("Browse all available snippets with ", Link("Snippet Browser", GetSnippetBrowserUrl(EnvironmentKind.VisualStudio)))),
                Heading3("Snippets"),
                Table(
                    TableRow("Language", TableColumn(HorizontalAlignment.Right, "Count"), TableColumn(HorizontalAlignment.Center, "Snippet Browser")),
                    results.Select(f =>
                    {
                        return TableRow(
                            Link(f.DirectoryName, VisualStudioExtensionGitHubUrl + "/" + f.DirectoryName + "/" + ReadMeFileName),
                            f.Snippets.Count,
                            Link("browse", GetSnippetBrowserUrl(EnvironmentKind.VisualStudio, f.Language)));
                    })),
                NewLine);

            return document.GetString(addFootnote: false);
        }
    }
}
