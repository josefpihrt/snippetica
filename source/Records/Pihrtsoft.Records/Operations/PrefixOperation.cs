// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Diagnostics;

namespace Pihrtsoft.Records.Operations
{
    [DebuggerDisplay("{Kind} {PropertyName,nq} = {Value,nq}")]
    internal struct PrefixOperation : IPropertyOperation
    {
        public PrefixOperation(PropertyDefinition propertyDefinition, string value, int depth)
        {
            PropertyDefinition = propertyDefinition;
            Value = value;
            Depth = depth;
        }

        public PropertyDefinition PropertyDefinition { get; }

        public string Value { get; }

        public int Depth { get; }

        public OperationKind Kind
        {
            get { return OperationKind.Prefix; }
        }

        public string PropertyName
        {
            get { return PropertyDefinition.Name; }
        }

        public bool SupportsExecute
        {
            get { return true; }
        }

        public void Execute(Record record)
        {
            if (PropertyDefinition.IsCollection)
            {
                object value;
                if (record.TryGetValue(PropertyName, out value))
                {
                    var items = (List<object>)value;

                    for (int i = 0; i < items.Count; i++)
                        items[i] = Value + items[i];
                }
            }
            else
            {
                record[PropertyName] = Value + record[PropertyName];
            }
        }

        string IKey<string>.GetKey()
        {
            return PropertyName;
        }
    }
}
