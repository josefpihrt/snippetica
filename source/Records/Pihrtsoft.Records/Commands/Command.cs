using System.Diagnostics;

namespace Pihrtsoft.Records.Commands
{
    [DebuggerDisplay("{Kind}")]
    public abstract class Command
    {
        protected Command()
        {
        }

        public abstract CommandKind Kind { get; }

        public abstract void Execute(Record record);
    }
}
