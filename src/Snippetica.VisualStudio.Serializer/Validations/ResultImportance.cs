// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Snippetica.VisualStudio.Serializer.Validations;

/// <summary>
/// Defines an importance of <see cref="SnippetValidationResult"/>.
/// </summary>
public enum ResultImportance
{
    /// <summary>
    /// Specifies information level.
    /// </summary>
    Information = 0,

    /// <summary>
    /// Specifies warning level.
    /// </summary>
    Warning = 1,

    /// <summary>
    /// Specifies error level.
    /// </summary>
    Error = 2,
}
