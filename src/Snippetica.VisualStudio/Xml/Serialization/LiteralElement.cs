// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.ComponentModel;
using System.Xml.Serialization;

namespace Snippetica.VisualStudio.Xml.Serialization;

/// <summary>
/// Represents Literal element in a serialized <see cref="Snippet"/>. This class cannot be inherited.
/// </summary>
public class LiteralElement
{
    /// <summary>
    /// Initializes a new instance of the <see cref="LiteralElement"/> class.
    /// </summary>
    public LiteralElement()
    {
        Editable = true;
    }

    /// <summary>
    /// Gets or sets Editable attribute value. Default value is <c>true</c>.
    /// </summary>
    [XmlAttribute]
    [DefaultValue(true)]
    public bool Editable { get; set; }

    /// <summary>
    /// Gets or sets ID element value.
    /// </summary>
    public string ID { get; set; }

    /// <summary>
    /// Gets or sets ToolTip element value.
    /// </summary>
    public string ToolTip { get; set; }

    /// <summary>
    /// Gets or sets Default element value.
    /// </summary>
    public string Default { get; set; }

    /// <summary>
    /// Gets or sets Function element value.
    /// </summary>
    public string Function { get; set; }
}
