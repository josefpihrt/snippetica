// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Snippetica.VisualStudio;

/// <summary>
/// Represents the set of <see cref="SnippetPlaceholder"/>s. This collection is read-only.
/// </summary>
public class SnippetPlaceholderList : Collection<SnippetPlaceholder>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SnippetPlaceholderList"/> class.
    /// </summary>
    public SnippetPlaceholderList()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="SnippetPlaceholderList"/> class.
    /// </summary>
    /// <param name="list">The list to wrap.</param>
    public SnippetPlaceholderList(IList<SnippetPlaceholder> list)
        : base(list)
    {
    }

    /// <summary>
    /// Determines whether the <see cref="SnippetPlaceholderList"/> contains an item with a specified <paramref name="identifier"/>.
    /// </summary>
    /// <param name="identifier">A <see cref="SnippetPlaceholder.Identifier"/> to search for.</param>
    /// <returns><c>true</c> if item is found in the <see cref="SnippetPlaceholderList"/>; otherwise, <c>false</c>.</returns>
    public bool Contains(string identifier)
    {
        foreach (SnippetPlaceholder placeholder in Items)
        {
            if (SnippetLiteral.IdentifierComparer.Equals(placeholder.Identifier, identifier))
                return true;
        }

        return false;
    }

    /// <summary>
    /// Determines whether the <see cref="SnippetPlaceholderList"/> contains an item with identifier equal to <see cref="SnippetPlaceholder.EndIdentifier"/>.
    /// </summary>
    /// <returns><c>true</c> if item is found in the <see cref="SnippetPlaceholderList"/>; otherwise, <c>false</c>.</returns>
    public bool ContainsEndIdentifier()
    {
        return Contains(SnippetPlaceholder.EndIdentifier);
    }

    /// <summary>
    /// Determines whether the <see cref="SnippetPlaceholderList"/> contains an item with identifier equal to <see cref="SnippetPlaceholder.SelectedIdentifier"/>.
    /// </summary>
    /// <returns><c>true</c> if item is found in the <see cref="SnippetPlaceholderList"/>; otherwise, <c>false</c>.</returns>
    public bool ContainsSelectedIdentifier()
    {
        return Contains(SnippetPlaceholder.SelectedIdentifier);
    }

    /// <summary>
    /// Returns first placeholder with the specified identifier
    /// </summary>
    /// <param name="identifier">A placeholder identifier.</param>
    /// <returns>First found placeholder with the specified identifier.</returns>
    public SnippetPlaceholder? Find(string identifier)
    {
        foreach (SnippetPlaceholder placeholder in Items)
        {
            if (SnippetLiteral.IdentifierComparer.Equals(placeholder.Identifier, identifier))
                return placeholder;
        }

        return null;
    }

    /// <summary>
    /// Returns all placeholders with the specified identifier
    /// </summary>
    /// <param name="identifier">A placeholder identifier.</param>
    /// <returns>Enumerable collection of placeholders with the specified identifier.</returns>
    public IEnumerable<SnippetPlaceholder> FindAll(string identifier)
    {
        foreach (SnippetPlaceholder placeholder in Items)
        {
            if (SnippetLiteral.IdentifierComparer.Equals(placeholder.Identifier, identifier))
                yield return placeholder;
        }
    }

    /// <summary>
    /// Returns first placeholder with the specified identifier
    /// </summary>
    /// <param name="identifier">A placeholder identifier.</param>
    /// <returns>First found placeholder with the specified identifier.</returns>
    public SnippetPlaceholder this[string identifier] => Find(identifier) ?? throw new KeyNotFoundException($"Identifier '{identifier}' was not found.");
}
