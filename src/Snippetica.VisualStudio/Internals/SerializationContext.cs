// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Snippetica.VisualStudio.Serialization;

namespace Snippetica.VisualStudio;

/// <summary>
/// Represents the context used when <see cref="Snippet"/> is serialized into xml.
/// </summary>
internal class SerializationContext
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SerializationContext"/> class with a specified code snippet and options.
    /// </summary>
    /// <param name="snippet">A <see cref="Snippet"/> instance to serialize.</param>
    /// <param name="options">A <see cref="SaveOptions"/> that enables to modify code snippet serialization process.</param>
    public SerializationContext(Snippet snippet, SaveOptions options)
    {
        Snippet = snippet;
        Options = options;
        Element = new CodeSnippetElement();
    }

    /// <summary>
    /// Gets the element that the code snippet is transformed into.
    /// </summary>
    public CodeSnippetElement Element { get; }

    /// <summary>
    /// Gets the code snippet being serialized.
    /// </summary>
    public Snippet Snippet { get; }

    /// <summary>
    /// Gets options that enables to modify code snippet serialization process.
    /// </summary>
    public SaveOptions Options { get; }
}
