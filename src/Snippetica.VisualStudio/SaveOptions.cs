// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Snippetica.VisualStudio;

/// <summary>
/// Specifies a set of options that enables to modify code snippet serialization process.
/// </summary>
public class SaveOptions
{
    /// <summary>
    /// Represents default indentation when serializing code snippet into xml. This field is read-only.
    /// </summary>
    internal const string DefaultIndentChars = "    ";

    /// <summary>
    /// Initializes a new instance of the <see cref="SaveOptions"/> class.
    /// </summary>
    public SaveOptions()
    {
    }

    //TODO: IncludeDefaultFormat
    /// <summary>
    /// Gets or sets a value indicating whether default format version set is when <see cref="Snippet.FormatVersion"/> value is <c>null</c>.
    /// </summary>
    public bool SetDefaultFormat { get; set; } = true;

    /// <summary>
    /// Gets or sets the string to use when indenting.
    /// </summary>
    public string IndentChars { get; set; } = DefaultIndentChars;

    /// <summary>
    /// Gets or sets a value indicating whether Delimiter attribute with value equal to <see cref="SnippetPlaceholder.DefaultDelimiter"/> will be included.
    /// </summary>
    public bool IncludeDefaultDelimiter { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether to write an XML declaration.
    /// </summary>
    public bool OmitXmlDeclaration { get; set; }

    /// <summary>
    /// Gets or sets an XML comment that will be added to the snippet file.
    /// </summary>
    public string? Comment { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether to write 'CodeSnippets' XML element. This option is relevant only if a single snippet is saved to a file.
    /// </summary>
    public bool OmitCodeSnippetsElement { get; set; }
}
