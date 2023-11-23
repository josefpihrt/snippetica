// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;

namespace Snippetica;

public class ModifierDefinition
{
    public string Name { get; init; }
    public string Keyword { get; init; }
    public string Shortcut { get; init; }
    public List<string> Tags { get; init; } = new();
    public ModifierKind Kind { get; init; }
}
