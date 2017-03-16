// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Pihrtsoft.Records.Commands
{
    internal class GroupCommand : Command
    {
        public GroupCommand(IEnumerable<Command> commands)
        {
            Commands = new ReadOnlyCollection<Command>(commands.ToArray());
        }

        public ReadOnlyCollection<Command> Commands { get; }

        public override CommandKind Kind
        {
            get { return CommandKind.Group; }
        }

        public override void Execute(Record record)
        {
            foreach (Command command in Commands)
                command.Execute(record);
        }
    }
}
