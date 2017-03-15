using System.Linq;
using System.Xml.Linq;

namespace Pihrtsoft.Records
{
    internal static class XContainerExtensions
    {
        public static XElement FirstElement(this XContainer container)
        {
            return container.Elements().FirstOrDefault();
        }
    }
}
