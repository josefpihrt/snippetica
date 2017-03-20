// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Diagnostics;

namespace Pihrtsoft.Records.Operations
{
    [DebuggerDisplay("{Kind} {PropertyName,nq} = {Value,nq}")]
    internal struct AddTagOperation : IPropertyOperation
    {
        public AddTagOperation(string value, int depth)
        {
            Value = value;
            Depth = depth;
        }

        public string PropertyName
        {
            get { return PropertyDefinition.Name; }
        }

        public string Value { get; }

        public int Depth { get; }

        public OperationKind Kind
        {
            get { return OperationKind.AddTag; }
        }

        public PropertyDefinition PropertyDefinition
        {
            get { return PropertyDefinition.TagProperty; }
        }

        public bool SupportsExecute
        {
            get { return true; }
        }

        public void Execute(Record record)
        {
            record.Tags.Add(Value);
        }

        string IKey<string>.GetKey()
        {
            return PropertyName;
        }
    }
}
