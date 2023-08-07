﻿// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Diagnostics;

namespace Snippetica.VisualStudio;

/// <summary>
/// Represents a literal placeholder.
/// </summary>
[DebuggerDisplay("{Identifier,nq}, Index = {Index}")]
public class SnippetPlaceholder
{
    /// <summary>
    /// Represents a default delimiter in a snippet code.
    /// </summary>
    public static readonly char DefaultDelimiter = '$';

    /// <summary>
    /// Represents 'end' identifier.
    /// </summary>
    public static readonly string EndIdentifier = "end";

    /// <summary>
    /// Represents 'selected' identifier.
    /// </summary>
    public static readonly string SelectedIdentifier = "selected";

    /// <summary>
    /// Initializes a new instance of the <see cref="SnippetPlaceholder"/> class with a specified index and identifier.
    /// </summary>
    /// <param name="index">The position at which a placeholder is found.</param>
    /// <param name="identifier">The placeholder identifier.</param>
    /// <param name="delimiter">A character that encloses the placeholder.</param>
    internal SnippetPlaceholder(int index, string identifier, char delimiter)
    {
        Index = index;
        Identifier = identifier;
        Delimiter = delimiter;
    }

    /// <summary>
    /// Gets literal placeholder index.
    /// </summary>
    public int Index { get; }

    /// <summary>
    /// Gets literal placeholder Identifier.
    /// </summary>
    public string Identifier { get; }

    /// <summary>
    /// Gets literal placeholder delimiter.
    /// </summary>
    public char Delimiter { get; }

    /// <summary>
    /// Gets literal placeholder length.
    /// </summary>
    public int Length => Identifier.Length;

    /// <summary>
    /// Gets literal placeholder length including delimiters.
    /// </summary>
    public int FullLength => Identifier.Length + 2;

    /// <summary>
    /// Gets literal placeholder end index.
    /// </summary>
    public int EndIndex => Index + Length;

    /// <summary>
    /// Gets a value indicating whether the current instance is "end" or "selected" placeholder.
    /// </summary>
    public bool IsSystemPlaceholder => IsEndPlaceholder || IsSelectedPlaceholder;

    /// <summary>
    /// Gets a value indicating whether the current instance has identifier "end".
    /// </summary>
    public bool IsEndPlaceholder => SnippetLiteral.IdentifierComparer.Equals(Identifier, EndIdentifier);

    /// <summary>
    /// Gets a value indicating whether the current instance has identifier "selected".
    /// </summary>
    public bool IsSelectedPlaceholder => SnippetLiteral.IdentifierComparer.Equals(Identifier, SelectedIdentifier);
}
