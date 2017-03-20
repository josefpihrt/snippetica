// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.ObjectModel;

namespace Pihrtsoft.Records.Operations
{
    internal class PropertyOperationCollection : Collection<IPropertyOperation>, IKey<string>
    {
        public PropertyOperationCollection(PropertyDefinition propertyDefinition)
        {
            PropertyDefinition = propertyDefinition;
        }

        public PropertyDefinition PropertyDefinition { get; }

        public virtual string PropertyName
        {
            get { return PropertyDefinition.Name; }
        }

        string IKey<string>.GetKey()
        {
            return PropertyName;
        }
    }
}
