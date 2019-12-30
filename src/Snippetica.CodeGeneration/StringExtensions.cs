// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Text.RegularExpressions;

namespace Snippetica.CodeGeneration
{
    public static class StringExtensions
    {
        internal static string Replace(
            this string value,
            string placeholder,
            string replacement,
            bool includeWhitespace)
        {
            if (includeWhitespace)
            {
                return Regex.Replace(value, $@"\s*{placeholder}\s*", replacement);
            }
            else
            {
                return value.Replace(placeholder, replacement);
            }
        }
    }
}
