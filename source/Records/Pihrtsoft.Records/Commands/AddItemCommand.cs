// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;

namespace Pihrtsoft.Records.Commands
{
    internal class AddItemCommand : PropertyCommand
    {
        public AddItemCommand(PropertyDefinition propertyName, string value)
            : base(propertyName, value)
        {
        }

        public override CommandKind Kind
        {
            get { return CommandKind.AddItem; }
        }

        public override void Execute(Record record)
        {
            List<object> items = null;

            object value;
            if (record.TryGetValue(PropertyName, out value))
            {
                items = (List<object>)value;
            }
            else
            {
                items = new List<object>();
                record[PropertyName] = items;
            }

            items.Add(Value);
        }
    }
}
