// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;

namespace Snippetica.VisualStudio.Serializer;

/// <summary>
/// Specifies how the code snippet can be inserted into code.
/// </summary>
[Flags]
public enum SnippetTypes
{
    /// <summary>
    /// Specifies that no snippet type is set.
    /// </summary>
    None = 0,

    /// <summary>
    /// Specifies that the code snippet can be inserted at the cursor.
    /// </summary>
    Expansion = 1,

    /// <summary>
    /// Specifies that the code snippet can be placed around a block of code.
    /// </summary>
    SurroundsWith = 1 << 1,

    /// <summary>
    /// Specifies that the code snippet can be used during refactoring.
    /// </summary>
    Refactoring = 1 << 2,
}
