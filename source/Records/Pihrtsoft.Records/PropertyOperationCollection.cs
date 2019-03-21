// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.ObjectModel;

namespace Pihrtsoft.Records
{
    internal class PropertyOperationCollection : Collection<Operation>, IKey<string>
    {
        public PropertyOperationCollection(PropertyDefinition propertyDefinition)
        {
            PropertyDefinition = propertyDefinition;
        }

        public PropertyDefinition PropertyDefinition { get; }

        public virtual string PropertyName => PropertyDefinition.Name;

        string IKey<string>.GetKey() => PropertyName;
    }
}
