// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Diagnostics;

namespace Pihrtsoft.Records.Operations
{
    [DebuggerDisplay("{Kind} {PropertyName,nq} = {Value,nq}")]
    internal struct AddRangeOperation : IPropertyOperation
    {
        public AddRangeOperation(PropertyDefinition propertyDefinition, string value, char separator, int depth)
        {
            PropertyDefinition = propertyDefinition;
            Value = value;
            Separator = separator;
            Depth = depth;
        }

        public PropertyDefinition PropertyDefinition { get; }

        public char Separator { get; }

        public string Value { get; }

        public int Depth { get; }

        public OperationKind Kind
        {
            get { return OperationKind.AddRange; }
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
                    record.Tags.Add(tag);

                return;
            }

            List<object> items = null;

            if (record.TryGetValue(PropertyName, out object value))
            {
                items = (List<object>)value;
            }
            else
            {
                items = new List<object>();
                record[PropertyName] = items;
            }

            foreach (string item in Value.Split(Separator))
                items.Add(item);
        }

        string IKey<string>.GetKey()
        {
            return PropertyName;
        }
    }
}
