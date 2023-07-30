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
    /// Represents a default delimiter in a snippet code. This field is a constant.
    /// </summary>
    public const char DefaultDelimiter = '$';

    /// <summary>
    /// Represents code snippet default format version. This field is read-only.
    /// </summary>
    public static readonly Version DefaultFormatVersion = new(1, 0, 0);

    /// <summary>
    /// Represents code snippet default format version converted to string. This field is read-only.
    /// </summary>
    internal static readonly string DefaultFormatVersionText = DefaultFormatVersion.ToString(3);

    private KeywordCollection _keywords;
    private ShortcutCollection _alternativeShortcuts;
    private NamespaceCollection _namespaces;
    private Collection<AssemblyReference> _assemblyReferences;
    private LiteralCollection _literals;
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
        Delimiter = DefaultDelimiter;
        Code = new SnippetCode(this);
    }

#if !PORTABLE
    /// <summary>
    /// Serializes the current instance to the specified file.
    /// </summary>
    /// <param name="filePath">The absolute or relative path to the file.</param>
    public void Save(string filePath)
    {
        Save(filePath, new SaveSettings());
    }

    /// <summary>
    /// Serializes the current instance to the specified file.
    /// </summary>
    /// <param name="filePath">The absolute or relative path to the file.</param>
    /// <param name="settings">A <see cref="SaveSettings"/> that modify serialization process.</param>
    public void Save(string filePath, SaveSettings settings)
    {
        SnippetSerializer.Serialize(filePath, this, settings);
    }
#endif

    /// <summary>
    /// Serializes the current instance to the specified <see cref="Stream"/>.
    /// </summary>
    /// <param name="stream">The stream to output this <see cref="Snippet"/> to.</param>
    public void Save(Stream stream)
    {
        Save(stream, new SaveSettings());
    }

    /// <summary>
    /// Serializes the current instance to the specified <see cref="Stream"/>, optionally specifying serialization process.
    /// </summary>
    /// <param name="stream">The stream to output this <see cref="Snippet"/> to.</param>
    /// <param name="settings">A <see cref="SaveSettings"/> that modify serialization process.</param>
    public void Save(Stream stream, SaveSettings settings)
    {
        SnippetSerializer.Serialize(stream, this, settings);
    }

    /// <summary>
    /// Returns snippet shortcut and all alternative shortcuts, if any.
    /// </summary>
    /// <returns>Sequence of all defined shortcut.</returns>
    public IEnumerable<string> Shortcuts()
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
    public bool IsExpansion
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
    public bool IsSurroundsWith
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
    public bool IsRefactoring
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
            Delimiter = Delimiter,
            Description = Description,
            FormatVersion = FormatVersion,
            Language = Language,
            Shortcut = Shortcut,
            SnippetTypes = SnippetTypes,
            FilePath = FilePath,
            Title = Title
        };

        if (HelpUrl is not null)
            clone.HelpUrl = new Uri(HelpUrl.OriginalString);

        foreach (AssemblyReference item in AssemblyReferences)
            clone.AssemblyReferences.Add((AssemblyReference)item.Clone());

        foreach (string shortcut in AlternativeShortcuts)
            clone.AlternativeShortcuts.Add(shortcut);

        foreach (string keyword in Keywords)
            clone.Keywords.Add(keyword);

        foreach (Literal item in Literals)
            clone.Literals.Add((Literal)item.Clone());

        foreach (string @namespace in Namespaces)
            clone.Namespaces.Add(@namespace);

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
    public Version FormatVersion { get; set; }

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
    public Uri HelpUrl { get; set; }

    /// <summary>
    /// Gets or sets snippet type.
    /// </summary>
    public SnippetTypes SnippetTypes { get; set; }

    /// <summary>
    /// Gets a collection of snippet keywords.
    /// </summary>
    public KeywordCollection Keywords
    {
        get { return _keywords ??= new KeywordCollection(); }
    }

    /// <summary>
    /// Gets a collection of alternative shortcuts.
    /// </summary>
    public ShortcutCollection AlternativeShortcuts
    {
        get { return _alternativeShortcuts ??= new ShortcutCollection(); }
    }

    /// <summary>
    /// Gets a value indicating whether snippet contains alternative shortcut.
    /// </summary>
    public bool HasAlternativeShortcuts
    {
        get { return _alternativeShortcuts?.Count > 0; }
    }

    /// <summary>
    /// Gets a collection of snippet namespaces.
    /// </summary>
    public NamespaceCollection Namespaces
    {
        get { return _namespaces ??= new NamespaceCollection(); }
    }

    /// <summary>
    /// Gets a collection of snippet assembly references.
    /// </summary>
    public Collection<AssemblyReference> AssemblyReferences
    {
        get { return _assemblyReferences ??= new Collection<AssemblyReference>(); }
    }

    /// <summary>
    /// Gets a collection of snippet literals.
    /// </summary>
    public LiteralCollection Literals
    {
        get { return _literals ??= new LiteralCollection(); }
    }

    /// <summary>
    /// Gets or sets snippet code context.
    /// </summary>
    public ContextKind ContextKind { get; set; }

    /// <summary>
    /// Gets or sets code snippet programming language.
    /// </summary>
    public Language Language { get; set; }

    /// <summary>
    /// Gets or sets a delimiter that encloses placeholder in the code.
    /// </summary>
    public char Delimiter { get; set; }

    /// <summary>
    /// Gets or sets full path to the snippet file.
    /// </summary>
    public string FilePath { get; set; }

    /// <summary>
    /// Gets or sets index of a snippet in a snippet file.
    /// </summary>
    public int Index { get; set; }

    /// <summary>
    /// Gets a value indicating whether the current instance has default delimiter.
    /// </summary>
    public bool HasDefaultDelimiter => Delimiter == DefaultDelimiter;

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
    public PlaceholderCollection Placeholders => Code.Placeholders;

    /// <summary>
    /// Occurs when the snippet text changes.
    /// </summary>
    public event EventHandler TextChanged;
}
