using System;

namespace Pihrtsoft.Snippets.CodeGeneration.Commands
{
    public class SimpleCommand : BaseCommand
    {
        private readonly Action<Snippet> _action;

        public SimpleCommand(Action<Snippet> action, CommandKind kind)
        {
            _action = action;
            Kind = kind;
        }

        public override CommandKind Kind { get; }

        protected override void Execute(ExecutionContext context, Snippet snippet)
        {
            _action?.Invoke(snippet);
        }
    }
}
