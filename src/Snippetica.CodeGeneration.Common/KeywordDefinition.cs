// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using Snippetica.VisualStudio;

namespace Snippetica;

[DebuggerDisplay("{Name,nq}")]
public class KeywordDefinition
{
    private static readonly Version _formatVersion = new(1, 1, 0);

    public static KeywordDefinition Default { get; } = new(null, null, null, null, false, Array.Empty<string>());

    public KeywordDefinition(
        string name,
        string value,
        string title,
        string shortcut,
        bool isDevelopment,
        string[] tags)
    {
        Name = name;
        Value = value;
        Title = title;
        Shortcut = shortcut;
        IsDevelopment = isDevelopment;
        Tags = new List<string>(tags);
    }

    public KeywordDefinition()
    {
    }

    public string Name { get; init; }

    public string Value { get; init; }

    public string Title { get; init; }

    public string Shortcut { get; init; }

    public bool IsDevelopment { get; init; }

    public List<string> Tags { get; init; }

    public bool HasTag(string tag) => Tags.Contains(tag);

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
