// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Diagnostics;
using Snippetica.VisualStudio;

namespace Snippetica;

[DebuggerDisplay("{Name,nq}")]
public class KeywordDefinition
{
    private static readonly Version _formatVersion = new(1, 1, 0);

    public string Name { get; init; }

    public string Value { get; init; }

    public string Shortcut { get; init; }

    public bool IsDevelopment { get; init; }

    public Snippet ToSnippet()
    {
        string title = $"{Value} keyword";

        var snippet = new Snippet()
        {
            FormatVersion = _formatVersion,
            Title = title,
            Shortcut = Shortcut,
            Description = title,
            CodeText = Value + "$end$",
        };

        snippet.SetFilePath($"{Name}Keyword.{SnippetFile.Extension}");

        return snippet;
    }
}
