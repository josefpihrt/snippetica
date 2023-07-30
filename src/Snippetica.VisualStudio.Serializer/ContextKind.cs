// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Snippetica.VisualStudio.Serializer;

/// <summary>
/// Specifies the location at which a code snippet can be inserted.
/// </summary>
public enum ContextKind
{
    /// <summary>
    /// Specifies that the code snippet has no context defined.
    /// </summary>
    None = 0,

    /// <summary>
    /// Specifies that the code snippet can be inserted anywhere.
    /// </summary>
    Any = 1,

    /// <summary>
    /// Specifies that the code snippet is a full code file.
    /// </summary>
    File = 2,

    /// <summary>
    /// Specifies that the code snippet is a method body.
    /// </summary>
    MethodBody = 3,

    /// <summary>
    /// Specifies that the code snippet is a method.
    /// </summary>
    MethodDeclaration = 4,

    /// <summary>
    /// Specifies that the code snippet can be inserted anywhere.
    /// </summary>
    TypeDeclaration = 5,
}
