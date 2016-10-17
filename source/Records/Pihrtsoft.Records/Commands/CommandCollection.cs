using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Pihrtsoft.Records.Commands
{
    internal class CommandCollection : Collection<Command>
    {
        public CommandCollection()
        {
        }

        public CommandCollection(IEnumerable<Command> commands)
        {
            foreach (Command command in commands)
                Add(command);
        }

        public void RemoveLast()
        {
            Items.RemoveAt(Items.Count - 1);
        }

        public void ExecuteAll(Record record)
        {
            foreach (Command command in Items)
                command.Execute(record);
        }
    }
}
