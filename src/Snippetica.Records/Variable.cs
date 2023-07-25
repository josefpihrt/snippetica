﻿// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Diagnostics;

namespace Snippetica.Records;

[DebuggerDisplay("{DebuggerDisplay,nq}")]
public readonly struct Variable : IKey<string>
{
    public Variable(string name, string value)
    {
        Name = name ?? throw new ArgumentNullException(nameof(name));
        Value = value;
    }

    public string Name { get; }

    public string Value { get; }

    internal bool IsDefault => Name is null;

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private string DebuggerDisplay => $"{Name} = {Value}";

    string IKey<string>.GetKey() => Name;
}
