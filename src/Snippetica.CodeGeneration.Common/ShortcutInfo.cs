// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Linq;
using Snippetica.VisualStudio;

namespace Snippetica;

public class ShortcutInfo
{
    public ShortcutInfo(
        string value,
        string description,
        string comment,
        ShortcutKind kind,
        IEnumerable<Language> languages,
        IEnumerable<EnvironmentKind> environments,
        IEnumerable<string> tags)
    {
        Value = value;
        Description = description;
        Comment = comment;
        Kind = kind;
        Environments = new List<EnvironmentKind>(environments.ToArray());
        Languages = new List<Language>(languages.ToArray());
        Tags = new List<string>(tags.ToArray());
    }

    public ShortcutInfo()
    {
    }

    public string Value { get; set; }

    public string Description { get; set; }

    public string Comment { get; set; }

    public ShortcutKind Kind { get; set; }

    public List<Language> Languages { get; set; }

    public List<EnvironmentKind> Environments { get; set; }

    public List<string> Tags { get; set; }

    public bool HasTag(string value) => Tags.Contains(value);
}
