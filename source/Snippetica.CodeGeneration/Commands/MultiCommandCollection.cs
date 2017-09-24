// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Snippetica.CodeGeneration.Commands
{
    public class MultiCommandCollection : Collection<MultiCommand>
    {
        public void AddRange(IEnumerable<MultiCommand> commands)
        {
            foreach (MultiCommand command in commands)
                Add(command);
        }

        public void AddMultiCommands(IEnumerable<Command> commands)
        {
            if (Count == 0)
            {
                foreach (Command command in commands)
                    Add(new MultiCommand(command));
            }
            else
            {
                CartesianProduct(commands);
            }
        }

        private void CartesianProduct(IEnumerable<Command> commands)
        {
            using (IEnumerator<Command> en = commands.GetEnumerator())
            {
                if (en.MoveNext())
                {
                    Command first = en.Current;

                    var multiCommands = new List<MultiCommand>();

                    while (en.MoveNext())
                        multiCommands.AddRange(WithCommand(en.Current));

                    foreach (MultiCommand command in Items)
                        command.Commands.Add(first);

                    AddRange(multiCommands);
                }
            }
        }

        public void AddMultiCommand(Command command)
        {
            if (Count == 0)
            {
                Add(new MultiCommand(command));
            }
            else
            {
                AddRange(WithCommand(command));
            }
        }

        private List<MultiCommand> WithCommand(Command command)
        {
            var multiCommands = new List<MultiCommand>(Count);

            foreach (MultiCommand item in Items)
            {
                var multiCommand = new MultiCommand(item.Commands);
                multiCommand.Commands.Add(command);
                multiCommands.Add(multiCommand);
            }

            return multiCommands;
        }
    }
}
