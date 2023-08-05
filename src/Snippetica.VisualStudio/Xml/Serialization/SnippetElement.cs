// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Xml.Serialization;

#pragma warning disable CA1819 // Properties should not return arrays

namespace Snippetica.VisualStudio.Xml.Serialization;

/// <summary>
/// Intended for internal use only.
/// </summary>
public sealed class SnippetElement
{
    /// <summary>
    /// Gets or sets Imports element.
    /// </summary>
    [XmlArrayItem("Import")]
    public ImportElement[]? Imports { get; set; }

    /// <summary>
    /// Gets or sets References element.
    /// </summary>
    [XmlArrayItem("Reference")]
    public ReferenceElement[]? References { get; set; }

    /// <summary>
    /// Gets or sets Declarations element.
    /// </summary>
    public DeclarationsElement? Declarations { get; set; }

    /// <summary>
    /// Gets or sets Code element.
    /// </summary>
    public CodeElement? Code { get; set; }
}
