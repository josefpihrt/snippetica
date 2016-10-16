using System;
using System.Diagnostics;
using System.Xml.Linq;
using Pihrtsoft.Records.Utilities;

namespace Pihrtsoft.Records
{
    internal static class XElementExtensions
    {
        public static ElementKind Kind(this XElement element)
        {
            switch (element.LocalName())
            {
                case ElementNames.Document:
                    return ElementKind.Document;
                case ElementNames.Entities:
                    return ElementKind.Entities;
                case ElementNames.Entity:
                    return ElementKind.Entity;
                case ElementNames.Declarations:
                    return ElementKind.Declarations;
                case ElementNames.Property:
                    return ElementKind.Property;
                case ElementNames.Variable:
                    return ElementKind.Variable;
                case ElementNames.Records:
                    return ElementKind.Records;
                case ElementNames.BaseRecords:
                    return ElementKind.BaseRecords;
                case ElementNames.New:
                    return ElementKind.New;
                case ElementNames.Tag:
                case ElementNames.Set:
                case ElementNames.Add:
                case ElementNames.Append:
                case ElementNames.Prefix:
                    return ElementKind.Command;
                default:
                    {
                        Debug.Assert(false, element.ToString());
                        return ElementKind.None;
                    }
            }
        }

        public static string LocalName(this XElement element)
        {
            return element.Name.LocalName;
        }

        public static string AttributeValue(this XElement element, string attributeName)
        {
            return element.Attribute(attributeName).Value;
        }

        public static string AttributeValueOrThrow(this XElement element, string attributeName)
        {
            XAttribute attribute = element.Attribute(attributeName);

            if (attribute == null)
                ThrowHelper.ThrowInvalidOperation($"Element '{element.LocalName()}' must define attribute '{attributeName}'.", element);

            return attribute.Value;
        }

        public static string AttributeValueOrDefault(this XElement element, string attributeName, string defaultValue = default(string))
        {
            return element.Attribute(attributeName)?.Value ?? defaultValue;
        }

        public static bool AttributeValueAsBooleanOrDefault(this XElement element, string attributeName, bool defaultValue = default(bool))
        {
            string value = element.AttributeValueOrDefault(attributeName);

            if (value != null)
                return bool.Parse(value);

            return defaultValue;
        }

        public static TEnum AttributeValueAsEnumOrDefault<TEnum>(this XElement element, string attributeName, TEnum defaultValue = default(TEnum)) where TEnum : struct
        {
            string value = element.AttributeValueOrDefault(attributeName);

            if (value != null)
                return (TEnum)Enum.Parse(typeof(TEnum), value);

            return defaultValue;
        }
    }
}
