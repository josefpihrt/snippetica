// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Text.RegularExpressions;

namespace Pihrtsoft.Snippets.CodeGeneration
{
    public static class StringExtensions
    {
        public static string ReplacePlaceholder(this string value, string placeholder, string replacement, bool includeWhitespace = false)
        {
            if (includeWhitespace)
            {
                return Regex.Replace(value, $@"\s*{Regex.Escape(Placeholders.Delimiter + placeholder + Placeholders.Delimiter) }\s*", replacement);
            }
            else
            {
                return value.Replace(Placeholders.Delimiter + placeholder + Placeholders.Delimiter, replacement);
            }
        }
    }
}
