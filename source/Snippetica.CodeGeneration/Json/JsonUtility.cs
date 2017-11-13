// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Pihrtsoft.Snippets;

namespace Snippetica.CodeGeneration.Json
{
    public static class JsonUtility
    {
        public static JObject ToJson(IEnumerable<Snippet> snippets)
        {
            return new JObject(snippets.Select(ToJProperty));
        }

        public static string ToJsonText(IEnumerable<Snippet> snippets)
        {
            return ToJson(snippets).ToString(Formatting.Indented);
        }

        private static JProperty ToJProperty(Snippet snippet)
        {
            return new JProperty(
                snippet.Title,
                new JObject(
                    new JProperty("prefix", snippet.Shortcut),
                    new JProperty("body", new JArray(GetTextMateBody(snippet).ToLines())),
                    new JProperty("description", snippet.Description)
                )
            );
        }

        private static string GetTextMateBody(Snippet snippet)
        {
            LiteralCollection literals = snippet.Literals;

            string s = snippet.CodeText;

            var sb = new StringBuilder(s.Length);

            int pos = 0;

            PlaceholderCollection placeholders = snippet.Code.Placeholders;

            Dictionary<Literal, int> literalIndexes = literals
                .OrderBy(f => FindMinIndex(f, placeholders))
                .Select((literal, i) => new { Literal = literal, Index = i })
                .ToDictionary(f => f.Literal, f => f.Index + 1);

            var processedIds = new List<string>();

            foreach (Placeholder placeholder in placeholders.OrderBy(f => f.Index))
            {
                sb.Append(s, pos, placeholder.Index - 1 - pos);

                if (placeholder.IsEndPlaceholder)
                {
                    sb.Append("${0}");
                }
                else if (placeholder.IsSelectedPlaceholder)
                {
                    sb.Append("${TM_SELECTED_TEXT}");
                }
                else
                {
                    string id = placeholder.Identifier;

                    Literal literal = literals[id];

                    sb.Append("${");
                    sb.Append(literalIndexes[literal]);

                    if (!processedIds.Contains(id))
                    {
                        sb.Append(":");
                        sb.Append(literal.DefaultValue);
                        processedIds.Add(id);
                    }

                    sb.Append("}");
                }

                pos = placeholder.EndIndex + 1;
            }

            sb.Append(s, pos, s.Length - pos);

            return sb.ToString();
        }

        private static int FindMinIndex(Literal literal, PlaceholderCollection placeholders)
        {
            return placeholders
                .Where(placeholder => placeholder.Identifier == literal.Identifier)
                .Select(placeholder => placeholder.Index)
                .Min();
        }

        private static IEnumerable<string> ToLines(this string value)
        {
            using (var sr = new StringReader(value))
            {
                string line = null;

                while ((line = sr.ReadLine()) != null)
                    yield return line;
            }
        }
    }
}
