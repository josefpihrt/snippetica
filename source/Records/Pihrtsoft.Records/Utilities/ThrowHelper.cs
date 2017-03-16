// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Xml;
using System.Xml.Linq;

namespace Pihrtsoft.Records.Utilities
{
    internal static class ThrowHelper
    {
        public static void ThrowOnUnknownElement(XElement element)
        {
            ThrowInvalidOperation(ErrorMessages.UnknownElement(element), element);
        }

        public static void ThrowOnMultipleElementsWithEqualName(XElement element)
        {
            ThrowInvalidOperation(ErrorMessages.MultipleElementsWithEqualName(element), element);
        }

        public static void ThrowInvalidOperation(string message, XObject @object = null, Exception innerException = null)
        {
            if (@object != null)
            {
                var info = (IXmlLineInfo)@object;

                if (info.HasLineInfo())
                    message += $" Line: {info.LineNumber}, Position: {info.LinePosition}.";
            }

            throw new DocumentException(message, @object, innerException);
        }
    }
}
