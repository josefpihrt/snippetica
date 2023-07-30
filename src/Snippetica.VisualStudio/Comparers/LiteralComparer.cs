// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections;
using System.Collections.Generic;

namespace Snippetica.VisualStudio.Comparers;

/// <summary>
/// Represents a <see cref="Literal"/> comparison operation.
/// </summary>
public abstract class LiteralComparer : IComparer<Literal>, IEqualityComparer<Literal>, IComparer, IEqualityComparer
{
    private static readonly StringComparer _stringComparer = StringComparer.Ordinal;

    /// <summary>
    /// Gets a <see cref="LiteralComparer"/> that performs a case-sensitive ordinal <see cref="Literal.Identifier"/> comparison.
    /// </summary>
    public static LiteralComparer Identifier => new LiteralIdentifierComparer();

    /// <summary>
    /// Returns a value to compare.
    /// </summary>
    /// <param name="literal">A literal.</param>
    /// <returns>A string that will be used to compare two literals.</returns>
    protected abstract string GetValue(Literal literal);

    /// <summary>
    /// Compares two literals and returns an indication of their relative sort order.
    /// </summary>
    /// <param name="x">A <see cref="Literal"/> to compare to <paramref name="y"/>.</param>
    /// <param name="y">A <see cref="Literal"/> to compare to <paramref name="x"/>.</param>
    /// <returns>A signed integer that indicates the relative values of <paramref name="x"/> and <paramref name="y"/>.</returns>
    public int Compare(Literal x, Literal y)
    {
        return _stringComparer.Compare(GetValue(x), GetValue(y));
    }

    /// <summary>
    /// Indicates whether two literals are equal.
    /// </summary>
    /// <param name="x">A <see cref="Literal"/> to compare to <paramref name="y"/>.</param>
    /// <param name="y">A <see cref="Literal"/> to compare to <paramref name="x"/>.</param>
    /// <returns><c>true</c> if <paramref name="x"/> and <paramref name="y"/> refer to the same object, or <paramref name="x"/> and <paramref name="y"/> are <c>null</c>, or <paramref name="x"/> and <paramref name="y"/> are equal; otherwise, <c>false</c>.</returns>
    public bool Equals(Literal x, Literal y)
    {
        return _stringComparer.Equals(GetValue(x), GetValue(y));
    }

    /// <summary>
    /// Gets the hash code for the specified <see cref="Literal"/>.
    /// </summary>
    /// <param name="obj">A literal.</param>
    /// <returns>A 32-bit signed hash code calculated from the value of the <paramref name="obj"/>.</returns>
    public int GetHashCode(Literal obj)
    {
        return _stringComparer.GetHashCode(GetValue(obj));
    }

    /// <summary>
    /// Compares two objects and returns an indication of their relative sort order.
    /// </summary>
    /// <param name="x">An object to compare to <paramref name="y"/>.</param>
    /// <param name="y">An object to compare to <paramref name="x"/>.</param>
    /// <returns>A signed integer that indicates the relative values of <paramref name="x"/> and <paramref name="y"/>.</returns>
    public int Compare(object x, object y)
    {
        return _stringComparer.Compare(GetValue((Literal)x), GetValue((Literal)y));
    }

    /// <summary>
    /// Indicates whether two objects are equal.
    /// </summary>
    /// <param name="x">An object to compare to <paramref name="y"/>.</param>
    /// <param name="y">An object to compare to <paramref name="x"/>.</param>
    /// <returns><c>true</c> if <paramref name="x"/> and <paramref name="y"/> refer to the same object, or <paramref name="x"/> and <paramref name="y"/> are both the same type of object and those objects are equal; otherwise, <c>false</c>.</returns>
    new public bool Equals(object x, object y)
    {
        return _stringComparer.Equals(GetValue((Literal)x), GetValue((Literal)y));
    }

    /// <summary>
    /// Gets the hash code for the specified literal.
    /// </summary>
    /// <param name="obj">An object.</param>
    /// <returns>A 32-bit signed hash code calculated from the value of the <paramref name="obj"/>.</returns>
    public int GetHashCode(object obj)
    {
        return _stringComparer.GetHashCode(GetValue((Literal)obj));
    }
}
