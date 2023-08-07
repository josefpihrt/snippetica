// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Snippetica.VisualStudio;
using Snippetica.VisualStudio.Comparers;

namespace Snippetica;

public static class SnippetExtensions
{
    private static readonly int _metaPrefixLength = KnownTags.MetaPrefix.Length;

    public static string GetFilePath(this Snippet snippet)
    {
        return snippet.Properties["FilePath"];
    }

    public static void SetFilePath(this Snippet snippet, string filePath)
    {
        snippet.Properties["FilePath"] = filePath;
    }

    public static string GetShortcutFromTitle(this Snippet snippet)
    {
        if (!snippet.HasTag(KnownTags.TitleStartsWithShortcut))
            return null;

        string s = snippet.Title;

        int i = 0;

        while (i < s.Length
            && s[i] != ' ')
        {
            i++;
        }

        return s.Substring(0, i);
    }

    public static Snippet RemoveShortcutFromTitle(this Snippet snippet)
    {
        snippet.Title = snippet.GetTitle(trimLeadingShortcut: true, trimTrailingUnderscore: false);

        return snippet;
    }

    public static string GetTitle(
        this Snippet snippet,
        bool trimLeadingShortcut = true,
        bool trimTrailingUnderscore = true)
    {
        string s = snippet.Title;

        int i = 0;

        if (trimLeadingShortcut
            && snippet.HasTag(KnownTags.TitleStartsWithShortcut))
        {
            while (i < s.Length
                && s[i] != ' ')
            {
                i++;
            }

            while (i < s.Length
                && s[i] == ' ')
            {
                i++;
            }
        }

        int j = s.Length - 1;

        if (trimTrailingUnderscore
            && snippet.HasTag(KnownTags.TitleEndsWithUnderscore))
        {
            while (j >= 0
                && s[j] == '_')
            {
                j--;
            }

            while (j >= 0
                && s[j] == ' ')
            {
                j--;
            }
        }

        return s.Substring(i, j - i + 1);
    }

    public static void RemoveMetaKeywords(this Snippet snippet)
    {
        List<string> keywords = snippet.Keywords;

        for (int i = keywords.Count - 1; i >= 0; i--)
        {
            if (keywords[i].StartsWith(KnownTags.MetaPrefix))
                keywords.RemoveAt(i);
        }
    }

    public static string GetFileName(this Snippet snippet)
    {
        return Path.GetFileName(snippet.GetFilePath());
    }

    public static string GetFileNameWithoutExtension(this Snippet snippet)
    {
        return Path.GetFileNameWithoutExtension(snippet.GetFilePath());
    }

    public static Snippet SortCollections(this Snippet snippet)
    {
        snippet.AlternativeShortcuts.Sort(StringComparer.CurrentCulture);
        snippet.Literals.Sort(SnippetLiteralComparer.Identifier);
        snippet.Keywords.Sort(StringComparer.CurrentCulture);
        snippet.Namespaces.Sort(NamespaceComparer.SystemFirst);

        return snippet;
    }

    public static void AddTag(this Snippet snippet, string tag)
    {
        if (!tag.StartsWith(KnownTags.MetaPrefix))
            tag = KnownTags.MetaPrefix + tag;

        AddKeyword(snippet, tag);
    }

    public static void AddTags(this Snippet snippet, params string[] tags)
    {
        if (tags is null)
            return;

        foreach (string tag in tags)
            AddTag(snippet, tag);
    }

    public static Snippet RemoveTag(this Snippet snippet, string tag, string value = null)
    {
        if (TryGetTag(snippet, tag, value, out TagInfo info))
            snippet.Keywords.RemoveAt(info.KeywordIndex);

        return snippet;
    }

    public static Snippet RemoveTags(this Snippet snippet, params string[] tags)
    {
        if (tags is null)
            return snippet;

        foreach (string tag in tags)
            RemoveTag(snippet, tag);

        return snippet;
    }

    public static bool HasTag(this Snippet snippet, string tag)
    {
        return FindTag(snippet, tag).Success;
    }

    public static bool TryGetTag(this Snippet snippet, string name, out TagInfo info)
    {
        return TryGetTag(snippet, name, null, out info);
    }

    public static bool TryGetTag(this Snippet snippet, string name, string value, out TagInfo info)
    {
        info = FindTag(snippet, name, value);

        return info.Success;
    }

    public static TagInfo FindTag(this Snippet snippet, string name, string value = null)
    {
        if (string.IsNullOrEmpty(name))
            return TagInfo.Default;

        List<string> keywords = snippet.Keywords;

        for (int i = 0; i < keywords.Count; i++)
        {
            string keyword = keywords[i];

            if (!keyword.StartsWith(KnownTags.MetaPrefix, StringComparison.Ordinal))
                continue;

            int length = name.Length;

            if (keyword.Length < _metaPrefixLength + length)
                continue;

            if (string.Compare(keyword, _metaPrefixLength, name, 0, length, StringComparison.Ordinal) != 0)
                continue;

            int start = _metaPrefixLength + length;

            while (start < keyword.Length
                && char.IsWhiteSpace(keyword[start]))
            {
                start++;
            }

            int end = start;

            while (end < keyword.Length
                && !char.IsWhiteSpace(keyword[end]))
            {
                end++;
            }

            if (value is null)
            {
                return new TagInfo(name, keyword.Substring(start, end - start), i);
            }

            if (end > start
                && string.Compare(keyword, start, value, 0, end - start, StringComparison.Ordinal) == 0)
            {
                return new TagInfo(name, value, i);
            }
        }

        return TagInfo.Default;
    }

    public static void AddNamespace(this Snippet snippet, string @namespace)
    {
        if (@namespace is null)
            return;

        if (snippet.Namespaces.Contains(@namespace))
            return;

        snippet.Namespaces.Add(@namespace);
    }

    public static void AddLiteral(this Snippet snippet, SnippetLiteral literal)
    {
        snippet.Literals.Add(literal);
    }

    public static SnippetLiteral AddLiteral(this Snippet snippet, string identifier, string toolTip = null, string defaultValue = "")
    {
        var literal = new SnippetLiteral(identifier, toolTip, defaultValue);

        snippet.Literals.Add(literal);

        return literal;
    }

    public static bool RequiresBasicTypeGeneration(this Snippet snippet, string typeName)
    {
        if (!snippet.HasTag(KnownTags.GenerateBasicType)
            && !snippet.HasTag(KnownTags.GenerateTypeTag(typeName)))
        {
            return false;
        }

        return KnownTags.GenerateTypeTag(typeName) != KnownTags.GenerateVoidType
            || snippet.HasTag(KnownTags.GenerateVoidType);
    }

    public static bool RequiresModifierGeneration(this Snippet snippet, string modifierName)
    {
        return snippet.HasTag(KnownTags.GenerateAccessModifier)
            || snippet.HasTag(KnownTags.GenerateModifierTag(modifierName));
    }

    public static void PrefixTitle(this Snippet snippet, string value)
    {
        snippet.Title = value + snippet.Title;
    }

    public static void PrefixShortcut(this Snippet snippet, string value)
    {
        snippet.Shortcut = value + snippet.Shortcut;
    }

    public static void PrefixDescription(this Snippet snippet, string value)
    {
        snippet.Description = value + snippet.Description;
    }

    public static void PrefixFileName(this Snippet snippet, string value)
    {
        SetFileName(snippet, value + snippet.GetFileName());
    }

    public static void SuffixTitle(this Snippet snippet, string value)
    {
        snippet.Title += value;
    }

    public static void SuffixShortcut(this Snippet snippet, string value)
    {
        snippet.Shortcut += value;
    }

    public static void SuffixDescription(this Snippet snippet, string value)
    {
        snippet.Description += value;
    }

    public static void SuffixFileName(this Snippet snippet, string value)
    {
        SetFileName(snippet, snippet.GetFileNameWithoutExtension() + value + Path.GetExtension(snippet.GetFilePath()));
    }

    public static void AppendCode(this Snippet snippet, string code)
    {
        snippet.CodeText += code;
    }

    public static void SetFileName(this Snippet snippet, string fileName)
    {
        snippet.SetFilePath(Path.Combine(Path.GetDirectoryName(snippet.GetFilePath()), fileName));
    }

    public static bool ContainsKeyword(this Snippet snippet, string keyword)
    {
        return snippet.Keywords.Contains(keyword);
    }

    public static bool ContainsAnyKeyword(this Snippet snippet, params string[] keywords)
    {
        foreach (string keyword in keywords)
        {
            if (snippet.ContainsKeyword(keyword))
                return true;
        }

        return false;
    }

    public static bool ContainsAnyKeyword(this Snippet snippet, IEnumerable<string> keywords)
    {
        foreach (string keyword in keywords)
        {
            if (snippet.ContainsKeyword(keyword))
                return true;
        }

        return false;
    }

    public static void RemoveKeywords(this Snippet snippet, IEnumerable<string> keywords)
    {
        foreach (string keyword in keywords)
            snippet.Keywords.Remove(keyword);
    }

    public static void RemoveKeywords(this Snippet snippet, params string[] keywords)
    {
        foreach (string keyword in keywords)
            snippet.Keywords.Remove(keyword);
    }

    public static bool RemoveKeyword(this Snippet snippet, string keyword)
    {
        return snippet.Keywords.Remove(keyword);
    }

    public static void AddKeyword(this Snippet snippet, string keyword)
    {
        if (!snippet.Keywords.Contains(keyword))
            snippet.Keywords.Add(keyword);
    }

    public static void AddKeywords(this Snippet snippet, params string[] keywords)
    {
        foreach (string keyword in keywords)
            AddKeyword(snippet, keyword);
    }

    public static bool RemoveLiteral(this Snippet snippet, string identifier)
    {
        return snippet.Literals.Remove(identifier);
    }

    public static void RemoveLiteralAndReplacePlaceholders(this Snippet snippet, string identifier, string replacement)
    {
        snippet.RemoveLiteral(identifier);
        snippet.ReplacePlaceholders(identifier, replacement);
    }

    public static void RemoveLiteralAndPlaceholders(this Snippet snippet, SnippetLiteral literal)
    {
        RemoveLiteralAndPlaceholders(snippet, literal.Identifier);
    }

    public static void RemoveLiteralAndPlaceholders(this Snippet snippet, string identifier)
    {
        SnippetLiteral literal = snippet.Literals.FirstOrDefault(f => f.Identifier == identifier);

        if (literal is null)
            return;

        snippet.Literals.Remove(literal);
        snippet.ReplacePlaceholders(literal.Identifier, "");
    }

    public static void RemoveLiterals(this Snippet snippet, params string[] identifiers)
    {
        foreach (string identifier in identifiers)
            RemoveLiteral(snippet, identifier);
    }

    public static void ReplacePlaceholders(this Snippet snippet, string identifier, string replacement)
    {
        string s = snippet.CodeText;

        if (string.IsNullOrEmpty(s))
            return;

        bool startsWithWhitespace = char.IsWhiteSpace(s[0]);

        bool endsWithWhitespace = char.IsWhiteSpace(s[s.Length - 1]);

        s = snippet.Code.ReplacePlaceholder(identifier, replacement);

        if (!startsWithWhitespace
            && char.IsWhiteSpace(s[0]))
        {
            s = s.TrimStart();
        }

        if (!endsWithWhitespace
            && char.IsWhiteSpace(s[s.Length - 1]))
        {
            s = s.TrimEnd();
        }

        snippet.CodeText = s;
    }

    public static void RemovePlaceholders(this Snippet snippet, string identifier)
    {
        ReplacePlaceholders(snippet, identifier, "");
    }

    public static void ReplaceSubOrFunctionLiteral(this Snippet snippet, string replacement)
    {
        SnippetLiteral literal = snippet.Literals.FirstOrDefault(f => f.Identifier == LiteralIdentifiers.SubOrFunction);

        if (literal is null)
            return;

        snippet.CodeText = snippet.CodeText.Replace($"${LiteralIdentifiers.SubOrFunction}$", replacement);
        snippet.Literals.Remove(literal);
    }
}
