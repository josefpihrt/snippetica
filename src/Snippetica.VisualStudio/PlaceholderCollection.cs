// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;

namespace Snippetica.VisualStudio;

/// <summary>
/// Represents the set of <see cref="Placeholder"/>s. This collection is read-only.
/// </summary>
[DebuggerDisplay("Count = {Count} {Identifiers,nq}")]
public class PlaceholderCollection : ReadOnlyCollection<Placeholder>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="PlaceholderCollection"/> class that is a read-only wrapper around the specified list.
    /// </summary>
    /// <param name="list">The list to wrap.</param>
    public PlaceholderCollection(IList<Placeholder> list)
        : base(list)
    {
    }

    /// <summary>
    /// Determines whether the <see cref="PlaceholderCollection"/> contains an item with a specified <paramref name="identifier"/>.
    /// </summary>
    /// <param name="identifier">A <see cref="Placeholder.Identifier"/> to search for.</param>
    /// <returns><c>true</c> if item is found in the <see cref="PlaceholderCollection"/>; otherwise, <c>false</c>.</returns>
    public bool Contains(string identifier)
    {
        foreach (Placeholder placeholder in Items)
        {
            if (Literal.IdentifierComparer.Equals(placeholder.Identifier, identifier))
                return true;
        }

        return false;
    }

    /// <summary>
    /// Determines whether the <see cref="PlaceholderCollection"/> contains an item with identifier equal to <see cref="Placeholder.EndIdentifier"/>.
    /// </summary>
    /// <returns><c>true</c> if item is found in the <see cref="PlaceholderCollection"/>; otherwise, <c>false</c>.</returns>
    public bool ContainsEnd()
    {
        return Contains(Placeholder.EndIdentifier);
    }

    /// <summary>
    /// Determines whether the <see cref="PlaceholderCollection"/> contains an item with identifier equal to <see cref="Placeholder.SelectedIdentifier"/>.
    /// </summary>
    /// <returns><c>true</c> if item is found in the <see cref="PlaceholderCollection"/>; otherwise, <c>false</c>.</returns>
    public bool ContainsSelected()
    {
        return Contains(Placeholder.SelectedIdentifier);
    }

    /// <summary>
    /// Returns first placeholder with the specified identifier
    /// </summary>
    /// <param name="identifier">A placeholder identifier.</param>
    /// <returns>First found placeholder with the specified identifier.</returns>
    public Placeholder Find(string identifier)
    {
        foreach (Placeholder placeholder in Items)
        {
            if (Literal.IdentifierComparer.Equals(placeholder.Identifier, identifier))
                return placeholder;
        }

        return null;
    }

    /// <summary>
    /// Returns all placeholders with the specified identifier
    /// </summary>
    /// <param name="identifier">A placeholder identifier.</param>
    /// <returns>Enumerable collection of placeholders with the specified identifier.</returns>
    public IEnumerable<Placeholder> FindAll(string identifier)
    {
        foreach (Placeholder placeholder in Items)
        {
            if (Literal.IdentifierComparer.Equals(placeholder.Identifier, identifier))
                yield return placeholder;
        }
    }

    /// <summary>
    /// Returns first placeholder with the specified identifier
    /// </summary>
    /// <param name="identifier">A placeholder identifier.</param>
    /// <returns>First found placeholder with the specified identifier.</returns>
    public Placeholder this[string identifier] => Find(identifier);

    private string Identifiers
    {
        get { return string.Join(", ", Items.Select(f => f.Identifier)); }
    }
}
