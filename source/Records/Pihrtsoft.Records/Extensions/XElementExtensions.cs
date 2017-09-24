// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Xml.Linq;
using Pihrtsoft.Records.Utilities;

namespace Pihrtsoft.Records
{
    internal static class XElementExtensions
    {
        public static bool IsKind(this XElement element, ElementKind kind)
        {
            return element.Kind() == kind;
        }

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
                case ElementNames.Set:
                    return ElementKind.Set;
                case ElementNames.Add:
                    return ElementKind.Add;
                case ElementNames.AddRange:
                    return ElementKind.AddRange;
                case ElementNames.Remove:
                    return ElementKind.Remove;
                case ElementNames.RemoveRange:
                    return ElementKind.RemoveRange;
                case ElementNames.Postfix:
                    return ElementKind.Postfix;
                case ElementNames.PostfixMany:
                    return ElementKind.PostfixMany;
                case ElementNames.Prefix:
                    return ElementKind.Prefix;
                case ElementNames.PrefixMany:
                    return ElementKind.PrefixMany;
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

        public static XAttribute SingleAttributeOrThrow(this XElement element, string attributeName = null)
        {
            using (IEnumerator<XAttribute> en = element.Attributes().GetEnumerator())
            {
                if (en.MoveNext())
                {
                    XAttribute attribute = en.Current;

                    if (!en.MoveNext()
                        && (attributeName == null || en.Current.Name == attributeName))
                    {
                        return attribute;
                    }
                }
            }

            if (attributeName != null)
            {
                ThrowHelper.ThrowInvalidOperation($"Element '{element.Name}' must contains single attribute with name '{attributeName}'.");
            }
            else
            {
                ThrowHelper.ThrowInvalidOperation($"Element '{element.Name}' must contains single attribute.");
            }

            return null;
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
