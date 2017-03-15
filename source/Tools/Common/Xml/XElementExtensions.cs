using System;
using System.Xml.Linq;

namespace Pihrtsoft.Snippets.Xml
{
    public static class XElementExtensions
    {
        public static string LocalName(this XElement element)
        {
            return element.Name.LocalName;
        }

        public static string AttributeValue(this XElement element, string attributeName)
        {
            return element.Attribute(attributeName).Value;
        }

        public static string AttributeValueOrDefault(this XElement element, string attributeName)
        {
            return element.Attribute(attributeName)?.Value;
        }

        public static TEnum AttributeValueAsEnumOrDefault<TEnum>(this XElement element, string attributeName, TEnum defaultValue) where TEnum : struct
        {
            string s = AttributeValueOrDefault(element, attributeName);

            if (s != null)
                return (TEnum)Enum.Parse(typeof(TEnum), s);

            return defaultValue;
        }
    }
}
