using System.Xml.Linq;

namespace Pihrtsoft.Records
{
    internal static class XAttributeExtensions
    {
        public static string LocalName(this XAttribute attribute)
        {
            return attribute.Name.LocalName;
        }
    }
}
