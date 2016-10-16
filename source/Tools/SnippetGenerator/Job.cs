using System.Collections.Generic;
using System.Collections.ObjectModel;
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

            foreach (Command command in commands.OrderBy(f => f.Kind))
                command.Execute(context);
        }
    }
}
