// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using Snippetica.VisualStudio.Serializer.Comparers;

namespace Snippetica.VisualStudio.Serializer;

/// <summary>
/// Provides a set of static methods that are related to <see cref="Snippet"/>.
/// </summary>
public static class SnippetUtility
{
    /// <summary>
    /// Returns enumerable groups of <see cref="Snippet"/>s that have same shortcut. <see cref="SnippetComparer.Shortcut"/> is used to compare shortcuts.
    /// </summary>
    /// <param name="snippets">Collection of <see cref="Snippet"/>s.</param>
    /// <returns>Enumerable collection of <see cref="DuplicateShortcutInfo"/> where each element contains shortcut and snippets with that shortcut.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="snippets"/> is <c>null</c>.</exception>
    public static IEnumerable<DuplicateShortcutInfo> FindDuplicateShortcuts(IEnumerable<Snippet> snippets)
    {
        if (snippets is null)
            throw new ArgumentNullException(nameof(snippets));

        return FindDuplicateShortcuts();

        IEnumerable<DuplicateShortcutInfo> FindDuplicateShortcuts()
        {
            foreach (var grouping in snippets
                .SelectMany(snippet => snippet.Shortcuts()
                    .Select(shortcut => new { Shortcut = shortcut, Snippet = snippet }))
                .GroupBy(f => f.Shortcut, SnippetComparer.Shortcut.GenericEqualityComparer))
            {
                if (grouping.CountExceeds(1))
                    yield return new DuplicateShortcutInfo(grouping.Key, grouping.Select(f => f.Snippet));
            }
        }
    }

    /// <summary>
    /// Removes all literals that do not have corresponding placeholder (placeholder with same identifier).
    /// </summary>
    /// <param name="snippet"><see cref="Snippet"/> to remove literals from.</param>
    public static void RemoveUnusedLiterals(Snippet snippet)
    {
        List<string> identifiers = null;

        foreach (Literal literal in snippet.Literals)
        {
            if (!snippet.Code.Placeholders.Contains(literal.Identifier))
            {
                (identifiers ??= new List<string>()).Add(literal.Identifier);
            }
        }

        if (identifiers is not null)
        {
            foreach (string identifier in identifiers)
                snippet.CodeText = snippet.Code.ReplacePlaceholders(identifier, "");
        }
    }

    /// <summary>
    /// Removes all placeholders that do not have corresponding literal (literal with same identifier).
    /// </summary>
    /// <param name="snippet"><see cref="Snippet"/> to remove placeholders from.</param>
    public static void RemoveUnusedPlaceholders(Snippet snippet)
    {
        List<string> identifiers = null;

        foreach (Placeholder placeholder in snippet.Code.Placeholders)
        {
            if (!placeholder.IsSystemPlaceholder
                && !snippet.Literals.Contains(placeholder.Identifier))
            {
                (identifiers ??= new List<string>()).Add(placeholder.Identifier);
            }
        }

        if (identifiers is not null)
        {
            foreach (string identifier in identifiers)
                snippet.CodeText = snippet.Code.ReplacePlaceholders(identifier, "");
        }
    }
}
