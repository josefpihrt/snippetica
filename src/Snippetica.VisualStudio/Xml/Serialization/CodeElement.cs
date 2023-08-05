// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace Snippetica.VisualStudio.Xml.Serialization;

/// <summary>
/// Intended for internal use only.
/// </summary>
public sealed class CodeElement : IXmlSerializable
{
    /// <summary>
    /// Gets or sets Delimiter attribute value.
    /// </summary>
    public string? Delimiter { get; set; }

    /// <summary>
    /// Gets or sets Kind attribute value.
    /// </summary>
    public string? Kind { get; set; }

    /// <summary>
    /// Gets or sets Language attribute value.
    /// </summary>
    public string? Language { get; set; }

    /// <summary>
    /// Gets or sets Code element value.
    /// </summary>
    public string? Code { get; set; }

    /// <summary>
    /// This method is reserved and should not be used.
    /// </summary>
    public XmlSchema? GetSchema() => null;

    /// <summary>
    /// Generates an object from its XML representation.
    /// </summary>
    /// <param name="reader"></param>
    public void ReadXml(XmlReader reader)
    {
        Delimiter = reader.GetAttribute(nameof(Delimiter));

        Kind = reader.GetAttribute(nameof(Kind));

        Language = reader.GetAttribute(nameof(Language));

        Code = reader.ReadElementContentAsString();
    }

    /// <summary>
    /// Converts an object into its XML representation.
    /// </summary>
    /// <param name="writer"></param>
    public void WriteXml(XmlWriter writer)
    {
        if (Delimiter is not null)
            writer.WriteAttributeString(nameof(Delimiter), Delimiter);

        if (Kind is not null)
            writer.WriteAttributeString(nameof(Kind), Kind);

        if (Language is not null)
            writer.WriteAttributeString(nameof(Language), Language);

        if (Code is not null)
            writer.WriteCData(Code);
    }
}
