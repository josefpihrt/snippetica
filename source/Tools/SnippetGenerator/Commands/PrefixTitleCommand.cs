
namespace Pihrtsoft.Snippets.CodeGeneration.Commands
{
    public class PrefixTitleCommand : BaseCommand
    {
        public PrefixTitleCommand(TypeDefinition type)
        {
            Type = type;
        }

        public TypeDefinition Type { get; }

        public override CommandKind Kind
        {
            get { return CommandKind.PrefixTitle; }
        }

        protected override void Execute(ExecutionContext context, Snippet snippet)
        {
            string prefix = (Type != null) ? Type.Shortcut : "a";

            snippet.PrefixTitle($"{prefix} ");
        }
    }
}
