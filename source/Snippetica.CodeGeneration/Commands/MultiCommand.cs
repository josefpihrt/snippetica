// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Snippetica.CodeGeneration.Commands
{
    public class MultiCommand : Command
    {
        public MultiCommand()
        {
            Commands = new Collection<Command>();
        }

        public MultiCommand(Command command)
            : this()
        {
            Commands.Add(command);
        }

        public MultiCommand(IEnumerable<Command> commands)
            : this()
        {
            foreach (Command command in commands)
                Commands.Add(command);
        }

        public override CommandKind Kind => CommandKind.Multi;

        public Collection<Command> Commands { get; }

        public override void Execute(ExecutionContext context)
        {
            var commands = new List<Command>(Commands);

            Stack<Command> stack = null;

            foreach (Command command in commands)
            {
                if (command.ChildCommand != null)
                {
                    (stack ?? (stack = new Stack<Command>())).Push(command.ChildCommand);
                }
            }

            if (stack != null)
            {
                while (stack.Count > 0)
                {
                    Command command = stack.Pop();

                    commands.Add(command);

                    if (command.ChildCommand != null)
                        stack.Push(command.ChildCommand);
                }
            }

            if (ContainsMutuallyExclusiveCommands(commands))
            {
                context.IsCanceled = true;
                return;
            }

            foreach (Command command in commands.OrderBy(f => f.Kind))
                command.Execute(context);
        }

        private static bool ContainsMutuallyExclusiveCommands(List<Command> commands)
        {
            for (int i = 0; i < commands.Count; i++)
            {
                for (int j = i + 1; j < commands.Count; j++)
                {
                    if (IsMutuallyExclusive(commands[i], commands[j])
                        || IsMutuallyExclusive(commands[j], commands[i]))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        private static bool IsMutuallyExclusive(Command command1, Command command2)
        {
            switch (command1.Kind)
            {
                case CommandKind.VirtualModifier:
                    {
                        if (command2.Kind == CommandKind.StaticModifier
                            || command2.Kind == CommandKind.ConstModifier
                            || command2.Kind == CommandKind.ConstExprModifier
                            || (command2 as AccessModifierCommand)?.Modifier.Kind == ModifierKind.Private)
                        {
                            return true;
                        }

                        break;
                    }
                case CommandKind.ConstExprModifier:
                    {
                        if (command2.Kind == CommandKind.ConstModifier
                            || command2.Kind == CommandKind.InlineModifier)
                        {
                            return true;
                        }

                        break;
                    }
                case CommandKind.InlineModifier:
                    {
                        if (command2.Kind == CommandKind.Declaration)
                            return true;

                        break;
                    }
            }

            return false;
        }
    }
}
