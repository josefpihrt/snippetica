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
