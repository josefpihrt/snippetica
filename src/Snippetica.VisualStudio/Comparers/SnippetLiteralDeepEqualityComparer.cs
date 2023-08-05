﻿// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;

namespace Snippetica.VisualStudio.Comparers;

/// <summary>
/// Represents a deep comparison for <see cref="SnippetLiteral"/>.
/// </summary>
internal class SnippetLiteralDeepEqualityComparer : EqualityComparer<SnippetLiteral>
{
    /// <summary>
    /// Indicates whether two literals are equal.
    /// </summary>
    /// <param name="x">A <see cref="SnippetLiteral"/> to compare to <paramref name="y"/>.</param>
    /// <param name="y">A <see cref="SnippetLiteral"/> to compare to <paramref name="x"/>.</param>
    /// <returns><c>true</c> if <paramref name="x"/> and <paramref name="y"/> refer to the same object, or <paramref name="x"/> and <paramref name="y"/> are <c>null</c>, or <paramref name="x"/> and <paramref name="y"/> are equal; otherwise, <c>false</c>.</returns>
    public override bool Equals(SnippetLiteral? x, SnippetLiteral? y)
    {
        if (object.ReferenceEquals(x, y))
            return true;

        if (x is null || y is null)
            return false;

        if (x.IsEditable != y.IsEditable)
            return false;

        if (!string.Equals(x.DefaultValue, y.DefaultValue, StringComparison.Ordinal))
            return false;

        if (!string.Equals(x.Function, y.Function, StringComparison.Ordinal))
            return false;

        if (!string.Equals(x.Identifier, y.Identifier, StringComparison.Ordinal))
            return false;

        if (!string.Equals(x.ToolTip, y.ToolTip, StringComparison.Ordinal))
            return false;

        if (!string.Equals(x.TypeName, y.TypeName, StringComparison.Ordinal))
            return false;

        return true;
    }

    /// <summary>
    /// Gets the hash code for the specified <see cref="SnippetLiteral"/>.
    /// </summary>
    /// <param name="obj">A literal.</param>
    /// <returns>A 32-bit signed hash code calculated from the value of the <paramref name="obj"/>.</returns>
    public override int GetHashCode(SnippetLiteral obj) => 0;

    /// <summary>
    /// Gets the instance of the <see cref="SnippetLiteralDeepEqualityComparer"/>.
    /// </summary>
    internal static SnippetLiteralDeepEqualityComparer Instance { get; } = new();
}
