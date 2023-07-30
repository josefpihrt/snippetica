// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;

namespace Snippetica.VisualStudio.Serializer;

/// <summary>
/// Supports renaming of snippet literal including its occurrences in the code.
/// </summary>
public static class LiteralRenamer
{
    /// <summary>
    /// Renames snippet literal including its occurrences in the code.
    /// </summary>
    /// <param name="snippet"><see cref="Snippet"/> that contains the literal to rename.</param>
    /// <param name="oldIdentifier">Old identifier.</param>
    /// <param name="newIdentifier">New identifier.</param>
    /// <exception cref="ArgumentNullException"><paramref name="snippet"/> is <c>null</c>.</exception>
    public static void Rename(Snippet snippet, string oldIdentifier, string newIdentifier)
    {
        if (snippet is null)
            throw new ArgumentNullException(nameof(snippet));

        Literal literal = snippet.Literals[oldIdentifier];

        if (literal is not null)
        {
            literal.Identifier = newIdentifier;

            snippet.CodeText = snippet.Code.RenamePlaceholder(oldIdentifier, newIdentifier);
        }
    }
}
