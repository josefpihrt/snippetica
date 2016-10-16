using System;
using System.Xml.Linq;

namespace Pihrtsoft.Records.Utilities
{
    internal static class DefaultComparer
    {
        public static StringComparison StringComparison { get; } = StringComparison.Ordinal;

        public static StringComparer StringComparer { get; } = StringComparer.Ordinal;

        public static bool NameEquals(XElement element, string name)
        {
            return NameEquals(element.LocalName(), name);
        }

        public static bool NameEquals(XAttribute attribute, string name)
        {
            return NameEquals(attribute.LocalName(), name);
        }

        public static bool NameEquals(string name1, string name2)
        {
            return string.Equals(name1, name2, StringComparison);
        }
    }
}
