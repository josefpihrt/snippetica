// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Diagnostics;

namespace Pihrtsoft.Records.Operations
{
    [DebuggerDisplay("{Kind} {PropertyName,nq} = {Value,nq}")]
    internal struct SetOperation : IPropertyOperation
    {
        public SetOperation(PropertyDefinition propertyDefinition, string value, int depth)
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
            get { return OperationKind.Set; }
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
            record[PropertyName] = Value;
        }

        string IKey<string>.GetKey()
        {
            return PropertyName;
        }
    }
}
