using System.Diagnostics;

namespace Pihrtsoft.Snippets.CodeGeneration.Commands
{
    [DebuggerDisplay("{Kind}")]
    public abstract class Command
    {
        public abstract void Execute(ExecutionContext context);

        public abstract CommandKind Kind { get; }

        public virtual Command ChildCommand { get; }
    }
}
