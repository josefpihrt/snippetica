using System.Collections.Generic;
using Pihrtsoft.Records.Utilities;

namespace Pihrtsoft.Records
{
    public class PropertyDefinitionCollection : ReadOnlyKeyedCollection<string, PropertyDefinition>
    {
        public PropertyDefinitionCollection(IList<PropertyDefinition> list)
            : base(list)
        {
        }

        internal PropertyDefinitionCollection(ExtendedKeyedCollection<string, PropertyDefinition> collection)
            : base(collection)
        {
        }
    }
}
