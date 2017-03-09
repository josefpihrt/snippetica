namespace Pihrtsoft.Snippets.CodeGeneration.Commands
{
    public class VirtualCommand : BaseCommand
    {
        public override CommandKind Kind
        {
            get { return CommandKind.VirtualModifier; }
        }

        protected override void Execute(ExecutionContext context, Snippet snippet)
        {
            LanguageDefinition language = ((LanguageExecutionContext)context).Language;

            ModifierDefinition @virtual = language.Virtual;

            snippet.PrefixTitle($"{@virtual.Keyword} ");

            snippet.PrefixShortcut(@virtual.Shortcut);

            snippet.PrefixDescription($"{@virtual.Keyword} ");

            snippet.ReplacePlaceholders(LiteralIdentifiers.Modifiers, $"${LiteralIdentifiers.Modifiers}$ {@virtual.Keyword}");

            snippet.PrefixFileName(@virtual.Name);
        }
    }
}
