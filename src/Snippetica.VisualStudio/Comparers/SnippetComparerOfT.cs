// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections;
using System.Collections.Generic;

namespace Snippetica.VisualStudio.Comparers;

/// <summary>
/// Represents a <see cref="Snippet"/> comparison operation.
/// </summary>
/// <typeparam name="T"></typeparam>
public abstract class SnippetComparer<T> : SnippetComparer
{
    internal abstract IComparer Comparer { get; }

    internal abstract IEqualityComparer EqualityComparer { get; }

    internal abstract IComparer<T> GenericComparer { get; }

    internal abstract IEqualityComparer<T> GenericEqualityComparer { get; }

    /// <summary>
    /// Returns a value to compare.
    /// </summary>
    /// <param name="snippet">A snippet.</param>
    /// <returns>A string that will be used to compare two snippets.</returns>
    protected abstract T GetValue(Snippet snippet);

    /// <summary>
    /// Compares two snippets and returns an indication of their relative sort order.
    /// </summary>
    /// <param name="x">A <see cref="Snippet"/> to compare to <paramref name="y"/>.</param>
    /// <param name="y">A <see cref="Snippet"/> to compare to <paramref name="x"/>.</param>
    /// <returns>A signed integer that indicates the relative values of <paramref name="x"/> and <paramref name="y"/>.</returns>
    public override int Compare(Snippet x, Snippet y)
    {
        return GenericComparer.Compare(GetValue(x), GetValue(y));
    }

    /// <summary>
    /// Indicates whether two snippets are equal.
    /// </summary>
    /// <param name="x">A <see cref="Snippet"/> to compare to <paramref name="y"/>.</param>
    /// <param name="y">A <see cref="Snippet"/> to compare to <paramref name="x"/>.</param>
    /// <returns><c>true</c> if <paramref name="x"/> and <paramref name="y"/> refer to the same object, or <paramref name="x"/> and <paramref name="y"/> are <c>null</c>, or <paramref name="x"/> and <paramref name="y"/> are equal; otherwise, <c>false</c>.</returns>
    public override bool Equals(Snippet x, Snippet y)
    {
        return GenericEqualityComparer.Equals(GetValue(x), GetValue(y));
    }

    /// <summary>
    /// Gets the hash code for the specified <see cref="Snippet"/>.
    /// </summary>
    /// <param name="obj">A snippet.</param>
    /// <returns>A 32-bit signed hash code calculated from the value of the <paramref name="obj"/>.</returns>
    public override int GetHashCode(Snippet obj)
    {
        return GenericEqualityComparer.GetHashCode(GetValue(obj));
    }

    /// <summary>
    /// Compares two objects and returns an indication of their relative sort order.
    /// </summary>
    /// <param name="x">An object to compare to <paramref name="y"/>.</param>
    /// <param name="y">An object to compare to <paramref name="x"/>.</param>
    /// <returns>A signed integer that indicates the relative values of <paramref name="x"/> and <paramref name="y"/>.</returns>
    public override int Compare(object x, object y)
    {
        return Comparer.Compare(GetValue((Snippet)x), GetValue((Snippet)y));
    }

    /// <summary>
    /// Indicates whether two objects are equal.
    /// </summary>
    /// <param name="x">An object to compare to <paramref name="y"/>.</param>
    /// <param name="y">An object to compare to <paramref name="x"/>.</param>
    /// <returns><c>true</c> if <paramref name="x"/> and <paramref name="y"/> refer to the same object, or <paramref name="x"/> and <paramref name="y"/> are both the same type of object and those objects are equal; otherwise, <c>false</c>.</returns>
    public override bool Equals(object x, object y)
    {
        return EqualityComparer.Equals(GetValue((Snippet)x), GetValue((Snippet)y));
    }

    /// <summary>
    /// Gets the hash code for the specified object.
    /// </summary>
    /// <param name="obj">An object.</param>
    /// <returns>A 32-bit signed hash code calculated from the value of the <paramref name="obj"/>.</returns>
    public override int GetHashCode(object obj)
    {
        return EqualityComparer.GetHashCode(GetValue((Snippet)obj));
    }
}
