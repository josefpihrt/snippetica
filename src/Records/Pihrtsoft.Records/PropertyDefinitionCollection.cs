// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;

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
