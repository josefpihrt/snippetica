using System;
using System.Runtime.Serialization;
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
