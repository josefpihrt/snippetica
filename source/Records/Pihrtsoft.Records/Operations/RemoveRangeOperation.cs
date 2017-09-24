// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Diagnostics;

namespace Pihrtsoft.Records.Operations
{
    [DebuggerDisplay("{Kind} {PropertyName,nq} = {Value,nq}")]
    internal struct RemoveRangeOperation : IPropertyOperation
    {
        public RemoveRangeOperation(PropertyDefinition propertyDefinition, string value, char separator, int depth)
        {
            PropertyDefinition = propertyDefinition;
            Value = value;
            Separator = separator;
            Depth = depth;
        }

        public PropertyDefinition PropertyDefinition { get; }

        public string Value { get; }

        public char Separator { get; }

        public int Depth { get; }

        public OperationKind Kind
        {
            get { return OperationKind.RemoveRange; }
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
                foreach (string tag in Value.Split(Separator))
                    record.Tags.Remove(tag);

                return;
            }

            List<object> items = null;

            if (record.TryGetValue(PropertyName, out object value))
            {
                items = (List<object>)value;

                foreach (string item in Value.Split(Separator))
                    items.Remove(item);
            }
        }

        string IKey<string>.GetKey()
        {
            return PropertyName;
        }
    }
}
