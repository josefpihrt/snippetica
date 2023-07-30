// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Snippetica.VisualStudio.Serializer.Xml.Serialization;

/// <summary>
/// Represents Import element in a serialized <see cref="Snippet"/>. This class cannot be inherited.
/// </summary>
public sealed class ImportElement
{
    /// <summary>
    /// Gets or sets Namespace element value.
    /// </summary>
    public string Namespace { get; set; }
}
