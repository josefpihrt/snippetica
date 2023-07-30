// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;

namespace Snippetica;

public class ModifierDefinition
{
    public ModifierDefinition(string name, string keyword, string shortcut, string[] tags)
    {
        Name = name;
        Keyword = keyword;
        Shortcut = shortcut;
        Tags = new List<string>(tags);
        Kind = (ModifierKind)Enum.Parse(typeof(ModifierKind), Name);
    }

    public ModifierDefinition()
    {
    }

    public string Name { get; init; }
    public string Keyword { get; init; }
    public string Shortcut { get; init; }
    public List<string> Tags { get; init; } = new();
    public ModifierKind Kind { get; init; }
}
