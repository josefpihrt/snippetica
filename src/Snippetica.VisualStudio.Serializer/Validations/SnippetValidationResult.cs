// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Diagnostics;

namespace Snippetica.VisualStudio.Serializer.Validations;

/// <summary>
/// Represents a result of the <see cref="Snippet"/> validation.
/// </summary>
[DebuggerDisplay("{Code,nq} {Importance} {Description,nq}")]
public class SnippetValidationResult
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SnippetValidationResult"/> class using the specified snippet, code, description a and importance.
    /// </summary>
    /// <param name="snippet">A snippet.</param>
    /// <param name="code">Alphanumeric code the identifies the result.</param>
    /// <param name="description">Result description.</param>
    /// <param name="importance">Result importance.</param>
    /// <exception cref="ArgumentNullException"><paramref name="snippet"/> is <c>null</c>.</exception>
    public SnippetValidationResult(Snippet snippet, string code, string description, ResultImportance importance)
        : this(snippet, code, description, importance, null)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="SnippetValidationResult"/> class using the specified snippet, code, description, importance and content.
    /// </summary>
    /// <param name="snippet">A snippet.</param>
    /// <param name="code">Alphanumeric code the identifies the result.</param>
    /// <param name="description">Result description.</param>
    /// <param name="importance">Result importance.</param>
    /// <param name="content">Additional result content. The value can be <c>null</c>.</param>
    /// <exception cref="ArgumentNullException"><paramref name="snippet"/> is <c>null</c>.</exception>
    public SnippetValidationResult(Snippet snippet, string code, string description, ResultImportance importance, object content)
    {
        Snippet = snippet ?? throw new ArgumentNullException(nameof(snippet));
        Code = code;
        Description = description;
        Importance = importance;
        Content = content;
    }

    /// <summary>
    /// Gets the importance level.
    /// </summary>
    public ResultImportance Importance { get; }

    /// <summary>
    /// Gets the alphanumeric code of the result.
    /// </summary>
    public string Code { get; }

    /// <summary>
    /// Gets the description text.
    /// </summary>
    public string Description { get; }

    /// <summary>
    /// Gets the <see cref="Snippet"/> the is the source of the result.
    /// </summary>
    public Snippet Snippet { get; }

    /// <summary>
    /// Gets the additional content of the result. The value can be <c>null</c>.
    /// </summary>
    public object Content { get; }
}
