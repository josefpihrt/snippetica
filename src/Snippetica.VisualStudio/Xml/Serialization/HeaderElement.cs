// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Xml.Serialization;

#pragma warning disable CA1819 // Properties should not return arrays

namespace Snippetica.VisualStudio.Xml.Serialization;

/// <summary>
/// Intended for internal use only.
/// </summary>
public sealed class HeaderElement
{
    /// <summary>
    /// Gets or sets Title element value.
    /// </summary>
    public string? Title { get; set; }

    /// <summary>
    /// Gets or sets Shortcut element value.
    /// </summary>
    public string? Shortcut { get; set; }

    /// <summary>
    /// Gets or sets AlternativeShortcuts element.
    /// </summary>
    [XmlArrayItem("Shortcut")]
    public string[]? AlternativeShortcuts { get; set; }

    /// <summary>
    /// Gets or sets Description element value.
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Gets or sets Author element values.
    /// </summary>
    public string? Author { get; set; }

    /// <summary>
    /// Gets or sets HelpUrl element value.
    /// </summary>
    public string? HelpUrl { get; set; }

    /// <summary>
    /// Gets or sets SnippetTypes element.
    /// </summary>
    [XmlArrayItem("SnippetType")]
    public string[]? SnippetTypes { get; set; }

    /// <summary>
    /// Gets or sets Keywords element.
    /// </summary>
    [XmlArrayItem("Keyword")]
    public string[]? Keywords { get; set; }
}
