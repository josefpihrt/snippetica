// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace Pihrtsoft.Snippets
{
    public static class SnippetExtensions
    {
        public static string GetSubmenuShortcut(this Snippet snippet)
        {
            if (snippet.HasTag(KnownTags.TitleStartsWithShortcut))
            {
                string s = snippet.Title;

                int i = 0;

                while (i < s.Length
                    && s[i] != ' ')
                {
                    i++;
                }

                return s.Substring(0, i);
            }

            return null;
        }

        public static void RemoveShortcutFromTitle(this Snippet snippet)
        {
            snippet.Title = snippet.GetTitleWithoutShortcut();
        }

        public static string GetTitleWithoutShortcut(this Snippet snippet)
        {
            if (snippet.HasTag(KnownTags.TitleStartsWithShortcut))
            {
                string s = snippet.Title;

                int i = 0;

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

                return s.Substring(i);
            }

            return snippet.Title;
        }

        public static void RemoveMetaKeywords(this Snippet snippet)
        {
            KeywordCollection keywords = snippet.Keywords;

            for (int i = keywords.Count - 1; i >= 0; i--)
            {
                if (keywords[i].StartsWith(KnownTags.MetaPrefix))
                    keywords.RemoveAt(i);
            }
        }

        public static string FileName(this Snippet snippet)
        {
            return Path.GetFileName(snippet.FilePath);
        }

        public static string FileNameWithoutExtension(this Snippet snippet)
        {
            return Path.GetFileNameWithoutExtension(snippet.FilePath);
        }

        public static void SortCollections(this Snippet snippet)
        {
            snippet.Literals.Sort();
            snippet.Keywords.Sort();
            snippet.Namespaces.Sort();
        }

        public static void AddTag(this Snippet snippet, string tag)
        {
            AddKeyword(snippet, KnownTags.MetaTagPrefix + tag);
        }

        public static void AddTags(this Snippet snippet, IEnumerable<string> tags)
        {
            foreach (string tag in tags)
                AddTag(snippet, tag);
        }

        public static void AddTags(this Snippet snippet, params string[] tags)
        {
            foreach (string tag in tags)
                AddTag(snippet, tag);
        }

        public static bool RemoveTag(this Snippet snippet, string tag)
        {
            string tagWithPrefix = KnownTags.MetaTagPrefix + tag;

            if (RemoveKeyword(snippet, tagWithPrefix))
            {
                return true;
            }
            else
            {
                string keyword = snippet.Keywords.FirstOrDefault(f => f.StartsWith(tagWithPrefix + " ", StringComparison.Ordinal));

                if (keyword != null)
                {
                    return snippet.RemoveKeyword(keyword);
                }
                else
                {
                    return false;
                }
            }
        }

        public static void RemoveTags(this Snippet snippet, IEnumerable<string> tags)
        {
            foreach (string tag in tags)
                RemoveTag(snippet, tag);
        }

        public static void RemoveTags(this Snippet snippet, params string[] tags)
        {
            foreach (string tag in tags)
                RemoveTag(snippet, tag);
        }

        public static bool HasTag(this Snippet snippet, string tag)
        {
            foreach (string keyword in snippet.Keywords)
            {
                if (keyword.StartsWith(KnownTags.MetaTagPrefix))
                {
                    int i = KnownTags.MetaTagPrefix.Length;
                    while (i < keyword.Length
                        && char.IsWhiteSpace(keyword[i]))
                    {
                        i++;
                    }

                    if (string.Equals(keyword.Substring(i, Math.Min(keyword.Length - i, tag.Length)), tag, StringComparison.Ordinal))
                        return true;
                }
            }

            return false;
        }

        public static string GetTagValueOrDefault(this Snippet snippet, string tag)
        {
            foreach (string keyword in snippet.Keywords)
            {
                if (keyword.StartsWith(KnownTags.MetaTagPrefix))
                {
                    int i = KnownTags.MetaTagPrefix.Length;
                    while (i < keyword.Length
                        && char.IsWhiteSpace(keyword[i]))
                    {
                        i++;
                    }

                    if (string.Equals(keyword.Substring(i, Math.Min(keyword.Length - i, tag.Length)), tag, StringComparison.Ordinal))
                        return keyword.Substring(i + tag.Length).Trim();
                }
            }

            return null;
        }

        public static void AddNamespace(this Snippet snippet, string @namespace)
        {
            if (@namespace != null && !snippet.Namespaces.Contains(@namespace))
            {
                snippet.Namespaces.Add(@namespace);
            }
        }

        public static void AddLiteral(this Snippet snippet, Literal literal)
        {
            snippet.Literals.Add(literal);
        }

        public static Literal AddLiteral(this Snippet snippet, string identifier, string toolTip = null, string defaultValue = "")
        {
            var literal = new Literal(identifier, toolTip, defaultValue);

            snippet.Literals.Add(literal);

            return literal;
        }

        public static bool RequiresTypeGeneration(this Snippet snippet, string typeName)
        {
            if (snippet.HasTag(KnownTags.GenerateType)
                || snippet.HasTag(KnownTags.GenerateTypeTag(typeName)))
            {
                if (KnownTags.GenerateTypeTag(typeName) != KnownTags.GenerateVoidType
                    || snippet.HasTag(KnownTags.GenerateVoidType))
                {
                    return true;
                }
            }

            return false;
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
            SetFileName(snippet, value + Path.GetFileName(snippet.FilePath));
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
            SetFileName(snippet, Path.GetFileNameWithoutExtension(snippet.FilePath) + value + Path.GetExtension(snippet.FilePath));
        }

        public static void SetFileName(this Snippet snippet, string fileName)
        {
            snippet.FilePath = Path.Combine(Path.GetDirectoryName(snippet.FilePath), fileName);
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

        public static void RemoveLiteralAndPlaceholders(this Snippet snippet, Literal literal)
        {
            RemoveLiteralAndPlaceholders(snippet, literal.Identifier);
        }

        public static void RemoveLiteralAndPlaceholders(this Snippet snippet, string identifier)
        {
            Literal literal = snippet.Literals.FirstOrDefault(f => f.Identifier == identifier);

            if (literal != null)
            {
                snippet.Literals.Remove(literal);
                snippet.ReplacePlaceholders(literal.Identifier, "");
            }
        }

        public static void RemoveLiterals(this Snippet snippet, params string[] identifiers)
        {
            foreach (string identifier in identifiers)
                RemoveLiteral(snippet, identifier);
        }

        public static void ReplacePlaceholders(this Snippet snippet, string identifier, string replacement)
        {
            snippet.CodeText = snippet.Code.ReplacePlaceholders(identifier, replacement);
        }

        public static void ReplaceSubOrFunctionLiteral(this Snippet snippet, string replacement)
        {
            Literal literal = snippet.Literals.FirstOrDefault(f => f.Identifier == LiteralIdentifiers.SubOrFunction);

            if (literal != null)
            {
                snippet.CodeText = snippet.CodeText.Replace($"${LiteralIdentifiers.SubOrFunction}$", replacement);
                snippet.Literals.Remove(literal);
            }
        }
    }
}
