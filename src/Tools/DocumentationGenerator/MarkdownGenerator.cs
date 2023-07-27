// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using DotMarkdown;
using DotMarkdown.Linq;
using Pihrtsoft.Snippets;
using static DotMarkdown.Linq.MFactory;
using static DotMarkdown.Docusaurus.DocusaurusMarkdownFactory;

namespace Snippetica.CodeGeneration.Markdown;

public static class MarkdownGenerator
{
    private static readonly MarkdownFormat _markdownFormat = new(
        tableOptions: MarkdownFormat.Default.TableOptions | TableOptions.FormatContent,
        angleBracketEscapeStyle: AngleBracketEscapeStyle.EntityRef);

    public static string GenerateEnvironmentMarkdown(
        SnippetEnvironment environment,
        IEnumerable<SnippetGeneratorResult> results)
    {
        MDocument document = Document(
            FrontMatter(("sidebar_label", environment.Kind.GetTitle())),
            Heading1($"Snippets for {environment.Kind.GetTitle()}"),
            Heading2("Languages"),
            results
                .Where(f => !f.Tags.Contains(KnownTags.ExcludeFromDocs))
                .OrderBy(f => f.DirectoryName)
                .Select(f => BulletItem(Link(f.Language.GetTitle(), f.Language.GetIdentifier()))));

        return document.ToString(_markdownFormat);
    }

    public static string GenerateSnippetsMarkdown(
        SnippetGeneratorResult result,
        DirectoryReadmeSettings settings)
    {
        MDocument document = Document(
            FrontMatter(("sidebar_label", result.Language.GetTitle())));

        document.Add(Heading1(result.Language.GetTitle() + " Snippets for " + result.Environment.Kind.GetTitle()));

        document.Add(Heading2("List of Selected Snippets"));

        document.Add(Table(
            TableRow("Shortcut", "Title"),
            result.Snippets
                .Where(f => !f.HasTag(KnownTags.ExcludeFromDocs))
                .OrderBy(f => f.Shortcut)
                .ThenBy(f => f.GetTitle())
                .Select(f =>
                {
                    return TableRow(
                        InlineCode(f.Shortcut),
                        f.GetTitle());
                })));

        if (!settings.IsDevelopment
            && settings.AddQuickReference)
        {
            document.Add(GetQuickReference());
        }

        return document.ToString(_markdownFormat);

        IEnumerable<object> GetQuickReference()
        {
            List<ShortcutInfo> shortcuts = settings.Shortcuts
                .Where(f => f.Languages.Contains(settings.Language))
                .ToList();

            if (shortcuts.Count > 0)
            {
                yield return Heading2("Quick Reference");

                if (settings.QuickReferenceText is not null)
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

                        yield return TableWithShortcutInfoValueDescriptionComment(grouping);
                    }
                }
                else
                {
                    yield return Environment.NewLine;
                    yield return TableWithShortcutInfoValueDescriptionComment(shortcuts);
                }
            }
        }

        MTable TableWithShortcutInfoValueDescriptionComment(IEnumerable<ShortcutInfo> shortcuts)
        {
            return Table(
                TableRow("Shortcut", "Description", "Comment"),
                shortcuts
                    .OrderBy(f => f.Value)
                    .ThenBy(f => f.Description)
                    .Select(f => TableRow(InlineCode(f.Value), f.Description, f.Comment)));
        }
    }
}
