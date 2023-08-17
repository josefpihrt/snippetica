﻿// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Snippetica.VisualStudio;

/// <summary>
/// Represents a file that has the extension 'snippet' and contains zero or more XML serialized snippets.
/// </summary>
[DebuggerDisplay("{Snippets.Count} {FullName,nq}")]
public class SnippetFile
{
    /// <summary>
    /// Specifies snippet file extension. This field is a constant.
    /// </summary>
    public const string Extension = "snippet";

    /// <summary>
    /// Initializes a new instance of the <see cref="SnippetFile"/> class with a specified path.
    /// </summary>
    /// <param name="fullName">Full name of the snippet file.</param>
    internal SnippetFile(string fullName)
    {
        FullName = fullName ?? throw new ArgumentNullException(nameof(fullName));
    }

    /// <summary>
    /// Gets or sets the full name.
    /// </summary>
    public string FullName { get; set; }

    /// <summary>
    /// Gets a collection of <see cref="Snippet"/>.
    /// </summary>
    public List<Snippet> Snippets { get; } = new();
}
