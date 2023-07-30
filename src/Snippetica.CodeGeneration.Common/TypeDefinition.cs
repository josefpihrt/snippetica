// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text.Json.Serialization;

namespace Snippetica;

[DebuggerDisplay("{Name,nq}")]
public class TypeDefinition
{
    public static TypeDefinition Default { get; } = new(null, null, null, "a", null, null, null, 0, Array.Empty<string>());

    public TypeDefinition(
        string name,
        string title,
        string keyword,
        string shortcut,
        string defaultValue,
        string defaultIdentifier,
        string @namespace,
        int arity,
        string[] tags)
    {
        Name = name;
        Title = title;
        Keyword = keyword;
        Shortcut = shortcut;
        DefaultValue = defaultValue;
        DefaultIdentifier = defaultIdentifier;
        Namespace = @namespace;
        Arity = arity;
        Tags = new List<string>(tags);
    }

    public TypeDefinition()
    {
    }

    public string Name { get; init; }

    public string Title { get; init; }

    public string Keyword { get; init; }

    public string Shortcut { get; init; }

    public string DefaultValue { get; init; }

    public string DefaultIdentifier { get; init; }

    public string Namespace { get; init; }

    public int Arity { get; init; }

    public List<string> Tags { get; init; }

    [JsonIgnore]
    public bool IsDictionary => Name.EndsWith("Dictionary", StringComparison.Ordinal);

    [JsonIgnore]
    public bool IsReadOnly => Name.StartsWith("ReadOnly", StringComparison.Ordinal);

    [JsonIgnore]
    public bool IsImmutable => Name.StartsWith("Immutable", StringComparison.Ordinal);

    [JsonIgnore]
    public bool IsInterface
    {
        get
        {
            return Name.Length > 2
                && Name[0] == 'I'
                && char.IsUpper(Name[1])
                && char.IsLower(Name[2]);
        }
    }

    public bool HasTag(string tag) => Tags.Contains(tag);
}
