// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;

namespace Snippetica.VisualStudio;

/// <summary>
/// Represents a code snippet.
/// </summary>
[DebuggerDisplay("{Language} {Shortcut,nq} {Title,nq} {CodeText,nq}")]
public class Snippet
#if NETFRAMEWORK
    : ICloneable
#endif
{
    /// <summary>
    /// Represents code snippet default format version. This field is read-only.
    /// </summary>
    public static readonly Version DefaultFormatVersion = new(1, 0, 0);

    /// <summary>
    /// Represents code snippet default format version converted to string. This field is read-only.
    /// </summary>
    internal static readonly string DefaultFormatVersionText = DefaultFormatVersion.ToString(3);

    private List<string>? _keywords;
    private List<string>? _alternativeShortcuts;
    private List<string>? _namespaces;
    private Collection<SnippetAssemblyReference>? _assemblyReferences;
    private SnippetLiteralList? _literals;
    private string _codeText = "";
    private string _shortcut = "";
    private string _title = "";
    private string _description = "";
    private string _author = "";

    /// <summary>
    /// Initializes a new instance of the <see cref="Snippet"/> class.
    /// </summary>
    public Snippet()
    {
        PlaceholderDelimiter = SnippetPlaceholder.DefaultDelimiter;
        Code = new SnippetCode(this);
    }

    /// <summary>
    /// Returns snippet shortcut and all alternative shortcuts, if any.
    /// </summary>
    /// <returns>Sequence of all defined shortcut.</returns>
    internal IEnumerable<string> Shortcuts()
    {
        yield return Shortcut;

        if (HasAlternativeShortcuts)
        {
            foreach (string shortcut in AlternativeShortcuts)
                yield return shortcut;
        }
    }

    /// <summary>
    /// Gets or sets a value indicating whether the current instance has <see cref="SnippetTypes.Expansion"/> flag set.
    /// </summary>
    internal bool IsExpansion
    {
        get { return (SnippetTypes & SnippetTypes.Expansion) != 0; }
        set
        {
            if (value)
            {
                SnippetTypes |= SnippetTypes.Expansion;
            }
            else
            {
                SnippetTypes &= ~SnippetTypes.Expansion;
            }
        }
    }

    /// <summary>
    /// Gets or sets a value indicating whether the current instance has <see cref="SnippetTypes.SurroundsWith"/> flag set.
    /// </summary>
    internal bool IsSurroundsWith
    {
        get { return (SnippetTypes & SnippetTypes.SurroundsWith) != 0; }
        set
        {
            if (value)
            {
                SnippetTypes |= SnippetTypes.SurroundsWith;
            }
            else
            {
                SnippetTypes &= ~SnippetTypes.SurroundsWith;
            }
        }
    }

    /// <summary>
    /// Gets or sets a value indicating whether the current instance has <see cref="SnippetTypes.Refactoring"/> flag set.
    /// </summary>
    internal bool IsRefactoring
    {
        get { return (SnippetTypes & SnippetTypes.Refactoring) != 0; }
        set
        {
            if (value)
            {
                SnippetTypes |= SnippetTypes.Refactoring;
            }
            else
            {
                SnippetTypes &= ~SnippetTypes.Refactoring;
            }
        }
    }

    /// <summary>
    /// Creates a new <see cref="Snippet"/> that is a deep copy of the current instance.
    /// </summary>
    /// <returns>A new <see cref="Snippet"/> that is a deep copy of the current instance</returns>
    public object Clone()
    {
        var clone = new Snippet()
        {
            Author = Author,
            CodeText = CodeText,
            ContextKind = ContextKind,
            PlaceholderDelimiter = PlaceholderDelimiter,
            Description = Description,
            FormatVersion = FormatVersion,
            Language = Language,
            Shortcut = Shortcut,
            SnippetTypes = SnippetTypes,
            Title = Title,
        };

        foreach (KeyValuePair<string, string> kvp in Properties)
            clone.Properties.Add(kvp.Key, kvp.Value);

        if (HelpUrl is not null)
            clone.HelpUrl = new Uri(HelpUrl.OriginalString);

        foreach (SnippetAssemblyReference item in AssemblyReferences)
            clone.AssemblyReferences.Add((SnippetAssemblyReference)item.Clone());

        clone.AlternativeShortcuts.AddRange(AlternativeShortcuts);

        clone.Keywords.AddRange(Keywords);

        foreach (SnippetLiteral item in Literals)
            clone.Literals.Add((SnippetLiteral)item.Clone());

        clone.Namespaces.AddRange(Namespaces);

        return clone;
    }

    /// <summary>
    /// Raises the <see cref="TextChanged"/> event.
    /// </summary>
    /// <param name="e">An <see cref="EventArgs"/> that contains the event data.</param>
    protected virtual void OnTextChanged(EventArgs e)
    {
        TextChanged?.Invoke(this, e);
    }

    /// <summary>
    /// Gets or sets snippet format version.
    /// </summary>
    public Version? FormatVersion { get; set; }

    /// <summary>
    /// Gets or sets snippet title.
    /// </summary>
    public string Title
    {
        get { return _title; }
        set { _title = value ?? ""; }
    }

    /// <summary>
    /// Gets or sets snippet shortcut.
    /// </summary>
    public string Shortcut
    {
        get { return _shortcut; }
        set { _shortcut = value ?? ""; }
    }

    /// <summary>
    /// Gets or sets snippet description.
    /// </summary>
    public string Description
    {
        get { return _description; }
        set { _description = value ?? ""; }
    }

    /// <summary>
    /// Gets or sets snippet author.
    /// </summary>
    public string Author
    {
        get { return _author; }
        set { _author = value ?? ""; }
    }

    /// <summary>
    /// Gets or sets URL the provides additional information about the current instance.
    /// </summary>
    public Uri? HelpUrl { get; set; }

    /// <summary>
    /// Gets or sets snippet type.
    /// </summary>
    public SnippetTypes SnippetTypes { get; set; }

    /// <summary>
    /// Gets a collection of snippet keywords.
    /// </summary>
    public List<string> Keywords => _keywords ??= new List<string>();

    /// <summary>
    /// Gets a collection of alternative shortcuts.
    /// </summary>
    public List<string> AlternativeShortcuts => _alternativeShortcuts ??= new List<string>();

    /// <summary>
    /// Gets a value indicating whether snippet contains alternative shortcut.
    /// </summary>
    internal bool HasAlternativeShortcuts => _alternativeShortcuts?.Count > 0;

    /// <summary>
    /// Gets a collection of snippet namespaces.
    /// </summary>
    public List<string> Namespaces => _namespaces ??= new List<string>();

    /// <summary>
    /// Gets a collection of snippet assembly references.
    /// </summary>
    public Collection<SnippetAssemblyReference> AssemblyReferences => _assemblyReferences ??= new Collection<SnippetAssemblyReference>();

    /// <summary>
    /// Gets a collection of snippet literals.
    /// </summary>
    public SnippetLiteralList Literals => _literals ??= new SnippetLiteralList();

    /// <summary>
    /// Gets or sets snippet code context.
    /// </summary>
    public SnippetContextKind ContextKind { get; set; }

    /// <summary>
    /// Gets or sets code snippet programming language.
    /// </summary>
    public Language Language { get; set; }

    /// <summary>
    /// Gets or sets a delimiter that encloses placeholder in the code.
    /// </summary>
    public char PlaceholderDelimiter { get; set; }

    /// <summary>
    /// Gets or sets snippet code text.
    /// </summary>
    public string CodeText
    {
        get { return _codeText; }
        set
        {
            value ??= "";
            if (_codeText != value)
            {
                _codeText = value;
                OnTextChanged(EventArgs.Empty);
            }
        }
    }

    /// <summary>
    /// Gets the snippet code.
    /// </summary>
    public SnippetCode Code { get; }

    /// <summary>
    /// Gets a collection of literal placeholders.
    /// </summary>
    public SnippetPlaceholderList Placeholders => Code.Placeholders;

    public Dictionary<string, string> Properties { get; } = new();

    /// <summary>
    /// Occurs when the snippet text changes.
    /// </summary>
    public event EventHandler? TextChanged;
}
