using System;
using System.Xml;
using System.Xml.Linq;

namespace Pihrtsoft.Records.Utilities
{
    internal static class ThrowHelper
    {
        public static void UnknownElement(XElement element)
        {
            ThrowInvalidOperation(ExceptionMessages.UnknownElement(element), element);
        }

        public static void MultipleElementsWithEqualName(XElement element)
        {
            ThrowInvalidOperation(ExceptionMessages.MultipleElementsWithEqualName(element), element);
        }

        public static void ThrowInvalidOperation(string message, XObject @object = null, Exception innerException = null)
        {
            if (@object != null)
            {
                var info = (IXmlLineInfo)@object;

                if (info.HasLineInfo())
                    message += $" Line: {info.LineNumber}, Position: {info.LinePosition}.";
            }

            throw new InvalidOperationException(message, innerException);
        }
    }
}
