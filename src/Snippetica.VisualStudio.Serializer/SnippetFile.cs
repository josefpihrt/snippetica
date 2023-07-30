// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Diagnostics;

namespace Snippetica.VisualStudio.Serializer;

/// <summary>
/// Represents a file that has the extension 'snippet' and contains zero or more snippets serialized into xml.
/// </summary>
[DebuggerDisplay("{Snippets.Count} {FullName,nq}")]
public class SnippetFile
{
    /// <summary>
    /// Specifies snippet file extension. This field is a constant.
    /// </summary>
    public const string Extension = "snippet";

    /// <summary>
    /// Initializes a new instance of the <see cref="SnippetFile"/> class.
    /// </summary>
    public SnippetFile()
        : this(null)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="SnippetFile"/> class with a specified path.
    /// </summary>
    /// <param name="fullName">Full name of the snippet file.</param>
    public SnippetFile(string fullName)
    {
        FullName = fullName;
        Snippets = new SnippetCollection();
    }

    /// <summary>
    /// Gets or sets the full name.
    /// </summary>
    public string FullName { get; set; }

    /// <summary>
    /// Gets a collection of <see cref="Snippet"/>.
    /// </summary>
    public SnippetCollection Snippets { get; }
}
