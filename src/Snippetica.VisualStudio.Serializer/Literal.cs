// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Diagnostics;

namespace Snippetica.VisualStudio.Serializer;

/// <summary>
/// Represents a code snippet literal.
/// </summary>
[DebuggerDisplay("{Identifier,nq} Default = {DefaultValue,nq} ToolTip = {ToolTip,nq}")]
public class Literal
#if NETFRAMEWORK
    : ICloneable
#endif
{
    private string _identifier;
    private string _defaultValue;

    internal static readonly StringComparer IdentifierComparer = StringComparer.Ordinal;

    /// <summary>
    /// Initializes a new instance of the <see cref="Literal"/> class with a specified identifier, tooltip and default value.
    /// </summary>
    /// <param name="identifier">The <see cref="Literal"/> identifier.</param>
    /// <param name="toolTip">The <see cref="Literal"/> description.</param>
    /// <param name="defaultValue">The <see cref="Literal"/>default value.</param>
    /// <exception cref="ArgumentNullException"><paramref name="identifier"/> is <c>null</c>.</exception>
    public Literal(string identifier, string toolTip = null, string defaultValue = "")
    {
        Identifier = identifier ?? throw new ArgumentNullException(nameof(identifier));
        ToolTip = toolTip;
        DefaultValue = defaultValue;
        IsEditable = true;
    }

    /// <summary>
    /// Create new <see cref="Literal"/> with function that returns containing type name.
    /// </summary>
    /// <param name="identifier">The <see cref="Literal"/> identifier.</param>
    /// <param name="toolTip">The <see cref="Literal"/> description.</param>
    /// <param name="defaultValue">The <see cref="Literal"/>default value.</param>
    /// <exception cref="ArgumentNullException"><paramref name="identifier"/> is <c>null</c>.</exception>
    /// <returns><see cref="Literal"/> with function that returns containing type name.</returns>
    public static Literal CreateClassNameLiteral(string identifier, string toolTip = null, string defaultValue = "")
    {
        return new Literal(identifier, toolTip, defaultValue)
        {
            Function = "ClassName()",
            IsEditable = false
        };
    }

    /// <summary>
    /// Create new <see cref="Literal"/> with function generates switch cases.
    /// </summary>
    /// <param name="identifier">The <see cref="Literal"/> identifier.</param>
    /// <param name="expressionIdentifier">Identifier of the literal that represent switch expression.</param>
    /// <param name="toolTip">The <see cref="Literal"/> description.</param>
    /// <param name="defaultValue">The <see cref="Literal"/>default value.</param>
    /// <exception cref="ArgumentNullException"><paramref name="identifier"/> is <c>null</c>.</exception>
    /// <returns><see cref="Literal"/> with function that generates switch cases.</returns>
    public static Literal CreateSwitchCasesLiteral(string identifier, string expressionIdentifier, string toolTip = null, string defaultValue = "default:")
    {
        return new Literal(identifier, toolTip, defaultValue)
        {
            Function = $"GenerateSwitchCases(${expressionIdentifier}$)",
            IsEditable = false
        };
    }

    /// <summary>
    /// Creates a new <see cref="Literal"/> that is a deep copy of the current instance.
    /// </summary>
    /// <returns>A new <see cref="Literal"/> that is a deep copy of the current instance</returns>
    public object Clone()
    {
        return new Literal(Identifier)
        {
            DefaultValue = DefaultValue,
            Function = Function,
            ToolTip = ToolTip,
            TypeName = TypeName,
            IsEditable = IsEditable
        };
    }

    /// <summary>
    /// Gets or sets a value indicating whether literal can be edited by a user when inserting a snippet into code.
    /// </summary>
    public bool IsEditable { get; set; }

    /// <summary>
    /// Gets or sets literal identifier.
    /// </summary>
    /// <exception cref="ArgumentNullException"></exception>
    public string Identifier
    {
        get { return _identifier; }
        set { _identifier = value ?? throw new ArgumentNullException(nameof(value)); }
    }

    /// <summary>
    /// Gets or sets literal tooltip.
    /// </summary>
    public string ToolTip { get; set; }

    /// <summary>
    /// Gets or sets literal default value.
    /// </summary>
    public string DefaultValue
    {
        get { return _defaultValue; }
        set { _defaultValue = value ?? ""; }
    }

    /// <summary>
    /// Gets or sets literal function name.
    /// </summary>
    public string Function { get; set; }

    /// <summary>
    /// Gets or sets the type name of the object.
    /// </summary>
    public string TypeName { get; set; }
}
