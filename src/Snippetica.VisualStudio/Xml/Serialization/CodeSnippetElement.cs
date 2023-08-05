// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Xml.Serialization;

namespace Snippetica.VisualStudio.Xml.Serialization;

/// <summary>
/// Intended for internal use only.
/// </summary>
[XmlRoot("CodeSnippet")]
public sealed class CodeSnippetElement
{
    /// <summary>
    /// Gets or sets Format attribute value.
    /// </summary>
    [XmlAttribute]
    public string? Format { get; set; }

    /// <summary>
    /// Gets or sets Header element.
    /// </summary>
    public HeaderElement? Header { get; set; }

    /// <summary>
    /// Gets or sets Snippet element.
    /// </summary>
    public SnippetElement? Snippet { get; set; }
}
