// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;

namespace Snippetica.VisualStudio.Comparers;

/// <summary>
/// Represents a deep comparison for <see cref="SnippetAssemblyReference"/>.
/// </summary>
internal class AssemblyReferenceDeepEqualityComparer : EqualityComparer<SnippetAssemblyReference>
{
    /// <summary>
    /// Indicates whether two assembly references are equal.
    /// </summary>
    /// <param name="x">A <see cref="SnippetAssemblyReference"/> to compare to <paramref name="y"/>.</param>
    /// <param name="y">A <see cref="SnippetAssemblyReference"/> to compare to <paramref name="x"/>.</param>
    /// <returns><c>true</c> if <paramref name="x"/> and <paramref name="y"/> refer to the same object, or <paramref name="x"/> and <paramref name="y"/> are <c>null</c>, or <paramref name="x"/> and <paramref name="y"/> are equal; otherwise, <c>false</c>.</returns>
    public override bool Equals(SnippetAssemblyReference? x, SnippetAssemblyReference? y)
    {
        if (object.ReferenceEquals(x, y))
            return true;

        if (x is null || y is null)
            return false;

        if (!string.Equals(x.AssemblyName, y.AssemblyName, StringComparison.Ordinal))
            return false;

        if (!string.Equals(x.Url?.OriginalString, y.Url?.OriginalString, StringComparison.OrdinalIgnoreCase))
            return false;

        return true;
    }

    /// <summary>
    /// Gets the hash code for the specified <see cref="SnippetAssemblyReference"/>.
    /// </summary>
    /// <param name="obj">An assembly reference.</param>
    /// <returns>A 32-bit signed hash code calculated from the value of the <paramref name="obj"/>.</returns>
    public override int GetHashCode(SnippetAssemblyReference obj) => 0;

    /// <summary>
    /// Gets the instance of the <see cref="AssemblyReferenceDeepEqualityComparer"/>.
    /// </summary>
    internal static AssemblyReferenceDeepEqualityComparer Instance { get; } = new();
}
