using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using Pihrtsoft.Snippets.CodeGeneration.Commands;

namespace Pihrtsoft.Snippets.CodeGeneration
{
    public class Job
    {
        public Job()
        {
            Commands = new Collection<Command>();
        }

        public Job(Command command)
            : this()
        {
            Commands.Add(command);
        }

        public Job(IEnumerable<Command> commands)
            : this()
        {
            foreach (Command command in commands)
                Commands.Add(command);
        }

        public Collection<Command> Commands { get; }

        public void Execute(ExecutionContext context)
        {
            var commands = new List<Command>(Commands);

            Stack<Command> stack = null;

            foreach (Command command in commands)
            {
                if (command.ChildCommand != null)
                {
                    if (stack == null)
                        stack = new Stack<Command>();

                    stack.Push(command.ChildCommand);
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
                    if (IsMutuallyExclusive(commands[i], commands[j]))
                    {
                        Debug.WriteLine(commands[i]);
                        Debug.WriteLine(commands[j]);

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
                case CommandKind.StaticModifier:
                    {
                        if (command2.Kind == CommandKind.VirtualModifier)
                            return true;

                        break;
                    }
                case CommandKind.VirtualModifier:
                    {
                        if (command2.Kind == CommandKind.StaticModifier
                            || IsPrivateModifier(command2))
                        {
                            return true;
                        }

                        break;
                    }
                case CommandKind.AccessModifier:
                    {
                        if (IsPrivateModifier(command1)
                            && command2.Kind == CommandKind.VirtualModifier)
                        {
                            return true;
                        }

                        break;
                    }
            }

            return false;
        }

        private static bool IsPrivateModifier(Command command)
        {
            return command.Kind == CommandKind.AccessModifier
                && ((AccessModifierCommand)command).Modifier.Kind == ModifierKind.Private;
        }
    }
}
