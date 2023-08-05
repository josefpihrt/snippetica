// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;

namespace Snippetica.VisualStudio.Comparers;

/// <summary>
/// Represents a deep comparison for <see cref="Snippet"/>.
/// </summary>
public class SnippetDeepEqualityComparer : EqualityComparer<Snippet>
{
    private static readonly StringComparer _stringComparer = StringComparer.Ordinal;

    /// <summary>
    /// Indicates whether two snippets are equal.
    /// </summary>
    /// <param name="x">A <see cref="Snippet"/> to compare to <paramref name="y"/>.</param>
    /// <param name="y">A <see cref="Snippet"/> to compare to <paramref name="x"/>.</param>
    /// <returns><c>true</c> if <paramref name="x"/> and <paramref name="y"/> refer to the same object, or <paramref name="x"/> and <paramref name="y"/> are <c>null</c>, or <paramref name="x"/> and <paramref name="y"/> are equal; otherwise, <c>false</c>.</returns>
    public override bool Equals(Snippet? x, Snippet? y)
    {
        if (object.ReferenceEquals(x, y))
            return true;

        if (x is null || y is null)
            return false;

        if (x.Delimiter != y.Delimiter)
            return false;

        if (x.ContextKind != y.ContextKind)
            return false;

        if (x.Language != y.Language)
            return false;

        if (x.SnippetTypes != y.SnippetTypes)
            return false;

        if (x.FormatVersion != y.FormatVersion)
            return false;

        if (!string.Equals(x.Shortcut, y.Shortcut, StringComparison.Ordinal))
            return false;

        if (!string.Equals(x.Title, y.Title, StringComparison.Ordinal))
            return false;

        if (!string.Equals(x.Author, y.Author, StringComparison.Ordinal))
            return false;

        if (!string.Equals(x.Description, y.Description, StringComparison.Ordinal))
            return false;

        if (!string.Equals(x.HelpUrl?.OriginalString, y.HelpUrl?.OriginalString, StringComparison.OrdinalIgnoreCase))
            return false;

        if (!string.Equals(x.CodeText, y.CodeText, StringComparison.Ordinal))
            return false;

        if ((x.HasAlternativeShortcuts || y.HasAlternativeShortcuts)
            && !x.AlternativeShortcuts.SequenceEqual(y.AlternativeShortcuts, StringComparer.CurrentCulture))
        {
            return false;
        }

        if (!x.Keywords.SequenceEqual(y.Keywords, _stringComparer))
            return false;

        if (!x.Namespaces.SequenceEqual(y.Namespaces, _stringComparer))
            return false;

        if (!x.AssemblyReferences.SequenceEqual(y.AssemblyReferences, AssemblyReferenceDeepEqualityComparer.Instance))
            return false;

        if (!x.Literals.SequenceEqual(y.Literals, SnippetLiteralDeepEqualityComparer.Instance))
            return false;

        return true;
    }

    /// <summary>
    /// Gets the hash code for the specified <see cref="Snippet"/>.
    /// </summary>
    /// <param name="obj">A snippet.</param>
    /// <returns>A 32-bit signed hash code calculated from the value of the <paramref name="obj"/>.</returns>
    public override int GetHashCode(Snippet obj) => 0;
}
