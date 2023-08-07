// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Diagnostics;

namespace Snippetica.VisualStudio.Validations;

/// <summary>
/// Represents a result of the validation of a snippet.
/// </summary>
[DebuggerDisplay("{Code,nq} {Importance} {Description,nq}")]
public class SnippetValidationResult
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SnippetValidationResult"/> class using the specified snippet, code, description, importance and content.
    /// </summary>
    /// <param name="snippet">A snippet.</param>
    /// <param name="description">Result description.</param>
    /// <param name="importance">Result importance.</param>
    /// <param name="content">Additional result content. The value can be <c>null</c>.</param>
    /// <exception cref="ArgumentNullException"><paramref name="snippet"/> is <c>null</c>.</exception>
    public SnippetValidationResult(
        Snippet snippet,
        string description,
        ResultImportance importance,
        object? content = null)
    {
        Snippet = snippet ?? throw new ArgumentNullException(nameof(snippet));
        Description = description ?? throw new ArgumentNullException(nameof(description));
        Importance = importance;
        Content = content;
    }

    /// <summary>
    /// Gets the importance level.
    /// </summary>
    public ResultImportance Importance { get; }

    /// <summary>
    /// Gets the description text.
    /// </summary>
    public string Description { get; }

    /// <summary>
    /// Gets the snippet the is the source of the result.
    /// </summary>
    public Snippet Snippet { get; }

    /// <summary>
    /// Gets the additional content of the result. The value can be <c>null</c>.
    /// </summary>
    public object? Content { get; }
}
