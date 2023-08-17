// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Snippetica.VisualStudio.Validations;

/// <summary>
/// Represents a context that is used during <see cref="Snippet"/> validation.
/// </summary>
public class SnippetValidationContext
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SnippetValidationContext"/> class with a specified <see cref="Snippet"/> and <see cref="SnippetValidationContext"/>.
    /// </summary>
    /// <param name="snippet">A <see cref="Snippet"/> that is being validated.</param>
    internal SnippetValidationContext(Snippet snippet)
    {
        Snippet = snippet;
    }

    /// <summary>
    /// Gets the code snippet that is being validated.
    /// </summary>
    public Snippet Snippet { get; }
}
