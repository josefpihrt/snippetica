// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

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
