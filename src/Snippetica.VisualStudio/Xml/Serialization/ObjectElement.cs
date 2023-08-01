// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Snippetica.VisualStudio.Xml.Serialization;

/// <summary>
/// Represents Object element in a serialized <see cref="Snippet"/>. This class cannot be inherited.
/// </summary>
public sealed class ObjectElement : LiteralElement
{
    /// <summary>
    /// Gets or sets Type element value.
    /// </summary>
    public string? Type { get; set; }
}
