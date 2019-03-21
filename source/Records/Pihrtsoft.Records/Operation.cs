// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Pihrtsoft.Records
{
    [DebuggerDisplay("{DebuggerDisplay,nq}")]
    internal struct Operation
    {
        public Operation(PropertyDefinition propertyDefinition, string value, int depth, OperationKind kind)
        {
            PropertyDefinition = propertyDefinition;
            Value = value;
            Depth = depth;
            Kind = kind;
        }

        public PropertyDefinition PropertyDefinition { get; }

        public string Value { get; }

        public int Depth { get; }

        public OperationKind Kind { get; }

        public string PropertyName => PropertyDefinition.Name;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string DebuggerDisplay
        {
            get { return $"{Kind} {PropertyName} = {Value}"; }
        }

        public void Execute(Record record)
        {
            Debug.Assert(Kind == OperationKind.With || Kind == OperationKind.Without, Kind.ToString());

            if (Kind == OperationKind.With)
            {
                if (!PropertyDefinition.IsCollection)
                {
                    record[PropertyName] = Value;
                    return;
                }

                char[] separators = PropertyDefinition.SeparatorsArray;

                if (PropertyDefinition == PropertyDefinition.Tags)
                {
                    if (separators.Length > 0)
                    {
                        foreach (string value2 in Value.Split(separators, StringSplitOptions.RemoveEmptyEntries))
                            record.Tags.Add(value2);
                    }
                    else
                    {
                        record.Tags.Add(Value);
                    }

                    return;
                }

                List<object> items = record.GetOrAddCollection(PropertyName);

                if (separators.Length > 0)
                {
                    foreach (string value2 in Value.Split(separators, StringSplitOptions.RemoveEmptyEntries))
                        items.Add(value2);
                }
                else
                {
                    items.Add(Value);
                }
            }
            else if (Kind == OperationKind.Without)
            {
                Debug.Assert(PropertyDefinition.IsCollection, "Property should be a collection.");

                char[] separators = PropertyDefinition.SeparatorsArray;

                if (PropertyDefinition == PropertyDefinition.Tags)
                {
                    if (separators.Length > 0)
                    {
                        foreach (string value2 in Value.Split(separators, StringSplitOptions.RemoveEmptyEntries))
                            record.Tags.Remove(value2);
                    }
                    else
                    {
                        record.Tags.Remove(Value);
                    }
                }
                else if (record.TryGetCollection(PropertyName, out List<object> items))
                {
                    if (separators.Length > 0)
                    {
                        foreach (string value2 in Value.Split(separators, StringSplitOptions.RemoveEmptyEntries))
                            items.Remove(value2);
                    }
                    else
                    {
                        items.Remove(Value);
                    }
                }
            }
        }
    }
}
