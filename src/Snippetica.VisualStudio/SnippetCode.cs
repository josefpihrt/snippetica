// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Snippetica.VisualStudio.Validations;

namespace Snippetica.VisualStudio;

/// <summary>
/// Represents a code of the <see cref="Snippet"/>.
/// </summary>
[DebuggerDisplay("{Text,nq}")]
public class SnippetCode
{
    private SnippetPlaceholderList? _placeholders;
    private Dictionary<int, SnippetPlaceholder?>? _indexes;
    private int _startIndex = -1;

    /// <summary>
    /// Initializes a new instance of the <see cref="SnippetCode"/> class with the specified snippet.
    /// </summary>
    /// <param name="snippet">A snippet</param>
    /// <exception cref="ArgumentNullException"><paramref name="snippet"/> is <c>null</c>.</exception>
    public SnippetCode(Snippet snippet)
    {
        Snippet = snippet ?? throw new ArgumentNullException(nameof(snippet));

        snippet.TextChanged += (object? sender, EventArgs e) => _indexes = null;
    }

    /// <summary>
    /// Gets snippet text where placeholders are replaced with matching literal's value.
    /// System placeholders and placeholders which do not have matching literal are removed.
    /// </summary>
    public string TextWithDefaultValues
    {
        get
        {
            if (Indexes.Count == 0)
                return Text;

            int index = 0;
            int prevIndex = 0;
            var sb = new StringBuilder();
            SnippetPlaceholder? placeholder = null;

            foreach (KeyValuePair<int, SnippetPlaceholder> item in Indexes)
            {
                index = item.Key;
                placeholder = item.Value;

                sb.Append(Text, prevIndex, index - 1 - prevIndex);

                if (placeholder is not null)
                {
                    SnippetLiteral? literal = Snippet.Literals.FirstOrDefault(f => SnippetLiteral.IdentifierComparer.Equals(f.Identifier, placeholder.Identifier));
                    if (literal is not null)
                        sb.Append(literal.DefaultValue);
                }
                else
                {
                    sb.Append(Text, index, 1);
                }

                prevIndex = ((placeholder is not null) ? placeholder.EndIndex : index) + 1;
            }

            sb.Append(Text, prevIndex, Text.Length - prevIndex);

            return sb.ToString();
        }
    }

    /// <summary>
    /// Returns a new string in which all occurrences of a specified identifier in the current instance are replaced with another specified identifier.
    /// </summary>
    /// <param name="oldIdentifier">The identifier to be replaced.</param>
    /// <param name="newIdentifier">The identifier to replace all occurrences of <paramref name="oldIdentifier"/>.</param>
    /// <returns>A string where all occurrences of <paramref name="oldIdentifier"/> are replaced with <paramref name="newIdentifier"/>.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="oldIdentifier"/> is <c>null</c>.</exception>
    /// <exception cref="ArgumentException"><paramref name="oldIdentifier"/> is the empty string or <paramref name="newIdentifier"/> is invalid.</exception>
    public string RenamePlaceholder(string oldIdentifier, string newIdentifier)
    {
        if (oldIdentifier is null)
            throw new ArgumentNullException(nameof(oldIdentifier));

        if (oldIdentifier.Length == 0)
            throw new ArgumentException("Value cannot be empty string.", nameof(oldIdentifier));

        if (string.IsNullOrEmpty(newIdentifier))
        {
            newIdentifier = "";
        }
        else if (!SnippetValidator.IsValidLiteralIdentifier(newIdentifier))
        {
            throw new ArgumentException("Value is not a valid literal identifier.", nameof(newIdentifier));
        }

        int prevIndex = 0;
        StringBuilder? sb = null;

        foreach (SnippetPlaceholder ph in Placeholders)
        {
            if (SnippetLiteral.IdentifierComparer.Equals(ph.Identifier, oldIdentifier))
            {
                sb ??= new StringBuilder();

                sb.Append(Text, prevIndex, ph.Index - prevIndex);
                sb.Append(newIdentifier);
                prevIndex = ph.EndIndex;
            }
        }

        if (sb is not null)
        {
            sb.Append(Text, prevIndex, Text.Length - prevIndex);
            return sb.ToString();
        }

        return Text;
    }

    /// <summary>
    /// Returns a new <see cref="string"/> where all placeholders are removed. Escaped $ characters are left intact.
    /// </summary>
    /// <returns>A new <see cref="string"/> where all placeholders are removed.</returns>
    public string RemoveAllPlaceholders()
    {
        if (Placeholders.Count == 0)
            return Text;

        int prevIndex = 0;
        var sb = new StringBuilder();

        foreach (SnippetPlaceholder ph in Placeholders)
        {
            sb.Append(Text, prevIndex, ph.Index - 1 - prevIndex);
            prevIndex = ph.EndIndex + 1;
        }

        sb.Append(Text, prevIndex, Text.Length - prevIndex);

        return sb.ToString();
    }

    /// <summary>
    /// Returns a new <see cref="string"/> where all placeholders with the specified identifier are replaced. Escaped $ characters are left intact.
    /// </summary>
    /// <param name="identifier">A placeholder identifier.</param>
    /// <param name="replacement">The string to replace all occurrences of placeholder with the specified identifier.</param>
    /// <returns>A new <see cref="string"/> where all placeholders with the specified identifier are replaced.</returns>
    public string ReplacePlaceholders(string identifier, string replacement)
    {
        if (Placeholders.Count > 0)
        {
            int prevIndex = 0;
            StringBuilder? sb = null;

            foreach (SnippetPlaceholder placeholder in Placeholders)
            {
                if (SnippetLiteral.IdentifierComparer.Equals(placeholder.Identifier, identifier))
                {
                    sb ??= new StringBuilder();

                    sb.Append(Text, prevIndex, placeholder.Index - 1 - prevIndex);
                    sb.Append(replacement);
                    prevIndex = placeholder.EndIndex + 1;
                }
            }

            if (sb is not null)
            {
                sb.Append(Text, prevIndex, Text.Length - prevIndex);
                return sb.ToString();
            }
        }

        return Text;
    }

    private void Parse()
    {
        _startIndex = -1;
        _indexes = new Dictionary<int, SnippetPlaceholder?>();
        var placeholders = new List<SnippetPlaceholder>();

        for (int i = 0; i < Text.Length; i++)
        {
            if (Text[i] == Delimiter)
            {
                if (_startIndex == -1)
                {
                    _startIndex = i + 1;
                }
                else
                {
                    if (i > _startIndex)
                    {
                        var placeholder = new SnippetPlaceholder(_startIndex, Text.Substring(_startIndex, i - _startIndex), Delimiter);
                        _indexes.Add(_startIndex, placeholder);
                        placeholders.Add(placeholder);
                    }
                    else
                    {
                        _indexes.Add(_startIndex, null);
                    }

                    _startIndex = -1;
                }
            }
        }

        _placeholders = new SnippetPlaceholderList(placeholders);
    }

    /// <summary>
    /// Gets a collection of literal placeholders.
    /// </summary>
    public SnippetPlaceholderList Placeholders
    {
        get
        {
            if (_indexes is null)
                Parse();

            return _placeholders!;
        }
    }

    /// <summary>
    /// Gets a placeholder with identifier 'end' or <c>null</c> if it is not present.
    /// </summary>
    public SnippetPlaceholder? EndPlaceholder
    {
        get
        {
            foreach (SnippetPlaceholder placeholder in Placeholders)
            {
                if (placeholder.IsEndPlaceholder)
                    return placeholder;
            }

            return null;
        }
    }

    /// <summary>
    /// Gets a placeholder with identifier 'selected' or <c>null</c> if it is not present.
    /// </summary>
    public SnippetPlaceholder? SelectedPlaceholder
    {
        get
        {
            foreach (SnippetPlaceholder placeholder in Placeholders)
            {
                if (placeholder.IsSelectedPlaceholder)
                    return placeholder;
            }

            return null;
        }
    }

    private Dictionary<int, SnippetPlaceholder> Indexes
    {
        get
        {
            if (_indexes is null)
                Parse();

            return _indexes!;
        }
    }

    /// <summary>
    /// Gets a value indicating whether snippet code contains unclosed delimiter.
    /// </summary>
    public bool ContainsUnclosedDelimiter
    {
        get
        {
            if (_indexes is null)
                Parse();

            return _startIndex != -1;
        }
    }

    /// <summary>
    /// Gets the snippet code text.
    /// </summary>
    public string Text => Snippet.CodeText;

    /// <summary>
    /// Gets the placeholder delimiter.
    /// </summary>
    public char Delimiter => Snippet.PlaceholderDelimiter;

    /// <summary>
    /// Gets the snippet that contains current instance.
    /// </summary>
    public Snippet Snippet { get; }
}
