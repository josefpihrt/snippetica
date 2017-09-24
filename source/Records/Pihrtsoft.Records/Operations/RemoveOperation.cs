// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Diagnostics;

namespace Pihrtsoft.Records.Operations
{
    [DebuggerDisplay("{Kind} {PropertyName,nq} = {Value,nq}")]
    internal struct RemoveOperation : IPropertyOperation
    {
        public RemoveOperation(PropertyDefinition propertyDefinition, string value, int depth)
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
            get { return OperationKind.Remove; }
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
            if (PropertyDefinition == PropertyDefinition.Tags)
            {
                record.Tags.Remove(Value);
                return;
            }

            List<object> items = null;

            if (record.TryGetValue(PropertyName, out object value))
            {
                items = (List<object>)value;

                items.Remove(Value);
            }
        }

        string IKey<string>.GetKey()
        {
            return PropertyName;
        }
    }
}
