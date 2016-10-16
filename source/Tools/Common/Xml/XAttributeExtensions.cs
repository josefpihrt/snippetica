using System.Xml.Linq;

namespace Pihrtsoft.Snippets.Xml
{
    public static class XAttributeExtensions
    {
        public static string LocalName(this XAttribute attribute)
        {
            return attribute.Name.LocalName;
        }
    }
}
