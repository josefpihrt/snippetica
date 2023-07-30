// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using Snippetica.VisualStudio.Serializer.Comparers;

namespace Snippetica.VisualStudio.Serializer;

/// <summary>
/// Represents the set of <see cref="Literal"/>s.
/// </summary>
[DebuggerDisplay("Count = {Count} {Identifiers,nq}")]
public class LiteralCollection : Collection<Literal>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="LiteralCollection"/> class that is empty.
    /// </summary>
    public LiteralCollection()
        : base(new List<Literal>())
    {
    }

    /// <summary>
    /// Determines whether a <see cref="Literal"/> with a specified identifier is in the <see cref="LiteralCollection"/>.
    /// </summary>
    /// <param name="identifier">A literal identifier to search for.</param>
    /// <returns><c>true</c> if item is found in the <see cref="LiteralCollection"/>; otherwise, <c>false</c>.</returns>
    public bool Contains(string identifier)
    {
        foreach (Literal literal in Items)
        {
            if (Literal.IdentifierComparer.Equals(literal.Identifier, identifier))
                return true;
        }

        return false;
    }

    /// <summary>
    /// Returns first literal with the specified identifier
    /// </summary>
    /// <param name="identifier">A literal identifier.</param>
    /// <returns>First found literal with the specified identifier.</returns>
    public Literal Find(string identifier)
    {
        foreach (Literal literal in Items)
        {
            if (Literal.IdentifierComparer.Equals(literal.Identifier, identifier))
                return literal;
        }

        return null;
    }

    /// <summary>
    /// Removes the first occurrence of a literal with a specific identifier
    /// </summary>
    /// <param name="identifier">A literal identifier.</param>
    /// <returns><c>true</c> if item was successfully removed from <see cref="LiteralCollection"/>; otherwise, <c>false</c>.</returns>
    public bool Remove(string identifier)
    {
        foreach (Literal literal in Items)
        {
            if (Literal.IdentifierComparer.Equals(literal.Identifier, identifier))
                return Items.Remove(literal);
        }

        return false;
    }

    /// <summary>
    /// Sorts the elements in the entire <see cref="LiteralCollection"/> using the <see cref="LiteralComparer.Identifier"/> comparer.
    /// </summary>
    public void Sort()
    {
        Sort(LiteralComparer.Identifier);
    }

    /// <summary>
    /// Sorts the elements in the entire <see cref="LiteralCollection"/> using the specified comparer.
    /// </summary>
    /// <param name="comparer">The <see cref="IComparer{Literal}"/> implementation to use when comparing elements.</param>
    public void Sort(IComparer<Literal> comparer)
    {
        List.Sort(comparer);
    }

    /// <summary>
    /// Returns first literal with the specified identifier
    /// </summary>
    /// <param name="identifier">A literal identifier.</param>
    /// <returns>First found literal with the specified identifier.</returns>
    public Literal this[string identifier] => Find(identifier);

    private List<Literal> List => (List<Literal>)Items;

    private string Identifiers
    {
        get { return string.Join(", ", Items.Select(f => f.Identifier)); }
    }
}
