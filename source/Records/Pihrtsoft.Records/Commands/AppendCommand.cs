// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;

namespace Pihrtsoft.Records.Commands
{
    internal class AppendCommand : PropertyCommand
    {
        public AppendCommand(PropertyDefinition property, string value)
            : base(property, value)
        {
        }

        public override CommandKind Kind
        {
            get { return CommandKind.Append; }
        }

        public override void Execute(Record record)
        {
            if (Property.IsCollection)
            {
                object value;
                if (record.TryGetValue(PropertyName, out value))
                {
                    var items = (List<object>)value;

                    for (int i = 0; i < items.Count; i++)
                        items[i] += Value;
                }
            }
            else
            {
                record[PropertyName] += Value;
            }
        }
    }
}
