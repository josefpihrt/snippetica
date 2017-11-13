// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Xml.Linq;

namespace Snippetica.Xml
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
