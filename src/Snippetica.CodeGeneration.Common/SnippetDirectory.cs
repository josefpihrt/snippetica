﻿// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Snippetica.VisualStudio;

namespace Snippetica;

[DebuggerDisplay("{DebuggerDisplay,nq}")]
public class SnippetDirectory
{
    public string Path { get; init; }

    public Language Language { get; init; }

    public List<string> Tags { get; init; } = new();

    public string Name => System.IO.Path.GetFileName(Path);

    public SnippetDirectory WithPath(string path)
    {
        return new SnippetDirectory() { Path = path, Language = Language, Tags = Tags.ToList() };
    }

    public bool HasTag(string tag) => Tags.Any(f => f.Equals(tag, StringComparison.Ordinal));

    public bool HasTags(params string[] tags)
    {
        foreach (string tag in tags)
        {
            if (!HasTag(tag))
                return false;
        }

        return true;
    }

    public bool HasAnyTag(params string[] tags)
    {
        foreach (string tag in tags)
        {
            if (HasTag(tag))
                return true;
        }

        return false;
    }

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private string DebuggerDisplay
    {
        get { return $"{Language} Tags = {string.Join(", ", Tags)} Path = {Path}"; }
    }

    public IEnumerable<Snippet> EnumerateSnippets(SearchOption searchOption = SearchOption.AllDirectories)
    {
        return SnippetSerializer.Deserialize(Path, searchOption);
    }
}
