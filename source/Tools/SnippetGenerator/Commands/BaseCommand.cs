using System.Linq;

namespace Pihrtsoft.Snippets.CodeGeneration.Commands
{
    public abstract class BaseCommand : Command
    {
        public override void Execute(ExecutionContext context)
        {
            foreach (Snippet snippet in context.Snippets.ToArray())
                Execute(context, snippet);
        }

        protected abstract void Execute(ExecutionContext context, Snippet snippet);
    }
}
