// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Diagnostics;
using System.Xml.Linq;
using Pihrtsoft.Records.Utilities;
using System.Collections.Generic;

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
                case ElementNames.Tag:
                case ElementNames.Set:
                case ElementNames.Add:
                case ElementNames.Postfix:
                case ElementNames.PostfixMany:
                case ElementNames.Prefix:
                case ElementNames.PrefixMany:
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

        public static XAttribute SingleAttributeOrThrow(this XElement element, string attributeName)
        {
            using (IEnumerator<XAttribute> en = element.Attributes().GetEnumerator())
            {
                if (en.MoveNext())
                {
                    XAttribute attribute = en.Current;

                    if (!en.MoveNext()
                        && en.Current.Name == attributeName)
                    {
                        return attribute;
                    }
                }
            }

            ThrowHelper.ThrowInvalidOperation($"Element '{element.Name}' must contains single attribute with name '{attributeName}'.");
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
