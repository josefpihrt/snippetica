// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Xml.Serialization;

#pragma warning disable CA1819 // Properties should not return arrays

namespace Snippetica.VisualStudio.Xml.Serialization;

/// <summary>
/// Represents CodeSnippets element in a serialized <see cref="Snippet"/>. This class cannot be inherited.
/// </summary>
[XmlRoot("CodeSnippets")]
public sealed class CodeSnippetsElement
{
    /// <summary>
    /// Gets or sets array of CodeSnippet elements.
    /// </summary>
    [XmlElement("CodeSnippet")]
    public CodeSnippetElement[]? Snippets { get; set; }
}
