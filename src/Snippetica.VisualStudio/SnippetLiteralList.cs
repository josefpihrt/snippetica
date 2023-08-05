// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;

namespace Snippetica.VisualStudio;

/// <summary>
/// Represents the set of <see cref="SnippetLiteral"/>s.
/// </summary>
public class SnippetLiteralList : List<SnippetLiteral>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SnippetLiteralList"/> class that is empty.
    /// </summary>
    public SnippetLiteralList()
    {
    }

    /// <summary>
    /// Determines whether a <see cref="SnippetLiteral"/> with a specified identifier is in the <see cref="SnippetLiteralList"/>.
    /// </summary>
    /// <param name="identifier">A literal identifier to search for.</param>
    /// <returns><c>true</c> if item is found in the <see cref="SnippetLiteralList"/>; otherwise, <c>false</c>.</returns>
    public bool Contains(string identifier)
    {
        foreach (SnippetLiteral literal in this)
        {
            if (SnippetLiteral.IdentifierComparer.Equals(literal.Identifier, identifier))
                return true;
        }

        return false;
    }

    /// <summary>
    /// Returns first literal with the specified identifier
    /// </summary>
    /// <param name="identifier">A literal identifier.</param>
    /// <returns>First found literal with the specified identifier.</returns>
    public SnippetLiteral? Find(string identifier)
    {
        foreach (SnippetLiteral literal in this)
        {
            if (SnippetLiteral.IdentifierComparer.Equals(literal.Identifier, identifier))
                return literal;
        }

        return null;
    }

    /// <summary>
    /// Removes the first occurrence of a literal with a specific identifier
    /// </summary>
    /// <param name="identifier">A literal identifier.</param>
    /// <returns><c>true</c> if item was successfully removed from <see cref="SnippetLiteralList"/>; otherwise, <c>false</c>.</returns>
    public bool Remove(string identifier)
    {
        foreach (SnippetLiteral literal in this)
        {
            if (SnippetLiteral.IdentifierComparer.Equals(literal.Identifier, identifier))
                return Remove(literal);
        }

        return false;
    }

    /// <summary>
    /// Returns first literal with the specified identifier
    /// </summary>
    /// <param name="identifier">A literal identifier.</param>
    /// <returns>First found literal with the specified identifier.</returns>
    public SnippetLiteral this[string identifier] => Find(identifier) ?? throw new KeyNotFoundException($"Identifier '{identifier}' was not found.");
}
