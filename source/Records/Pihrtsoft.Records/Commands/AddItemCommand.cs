using System.Collections.Generic;
using System.Diagnostics;

namespace Pihrtsoft.Records.Commands
{
    [DebuggerDisplay("{Kind} {PropertyName,nq} {Item,nq}")]
    public class AddItemCommand : Command
    {
        public AddItemCommand(string propertyName, string item)
        {
            PropertyName = propertyName;
            Item = item;
        }

        public string PropertyName { get; }

        public string Item { get; }

        public override CommandKind Kind
        {
            get { return CommandKind.AddItem; }
        }

        public override void Execute(Record record)
        {
            List<object> items = null;

            object value;
            if (record.Properties.TryGetValue(PropertyName, out value))
            {
                items = (List<object>)value;
            }
            else
            {
                items = new List<object>();
                record[PropertyName] = items;
            }

            items.Add(Item);
        }
    }
}
