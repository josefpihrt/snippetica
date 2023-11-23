// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;

namespace Snippetica.VisualStudio;

public static class SnippetExtensions
{
    /// <summary>
    /// Renames snippet literal including its occurrences in the code.
    /// </summary>
    /// <param name="snippet"><see cref="Snippet"/> that contains the literal to rename.</param>
    /// <param name="oldIdentifier">Old identifier.</param>
    /// <param name="newIdentifier">New identifier.</param>
    /// <exception cref="ArgumentNullException"><paramref name="snippet"/> is <c>null</c>.</exception>
    public static void RenameLiteral(this Snippet snippet, string oldIdentifier, string newIdentifier)
    {
        if (snippet is null)
            throw new ArgumentNullException(nameof(snippet));

        SnippetLiteral literal = snippet.Literals[oldIdentifier];

        if (literal is not null)
        {
            literal.Identifier = newIdentifier;

            snippet.CodeText = snippet.Code.RenamePlaceholder(oldIdentifier, newIdentifier);
        }
    }

    internal static bool CodeContainsCDataEnd(this Snippet snippet)
    {
        if (snippet is null)
            throw new ArgumentNullException(nameof(snippet));

        return snippet.CodeText.Contains("]]>");
    }
}
