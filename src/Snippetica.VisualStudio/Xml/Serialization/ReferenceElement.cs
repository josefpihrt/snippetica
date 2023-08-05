// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Snippetica.VisualStudio.Xml.Serialization;

/// <summary>
/// Intended for internal use only.
/// </summary>
public sealed class ReferenceElement
{
    /// <summary>
    /// Gets or sets Assembly element value.
    /// </summary>
    public string? Assembly { get; set; }

    /// <summary>
    /// Gets or sets Url element.
    /// </summary>
    public string? Url { get; set; }
}
