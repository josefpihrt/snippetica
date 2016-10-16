namespace Pihrtsoft.Snippets.CodeGeneration.Commands
{
    public class ParametersCommand : BaseCommand
    {
        public override CommandKind Kind
        {
            get { return CommandKind.Parameters; }
        }

        protected override void Execute(ExecutionContext context, Snippet snippet)
        {
            LanguageDefinition language = ((LanguageExecutionContext)context).Language;

            snippet.SuffixTitle(" (with parameters)");
            snippet.SuffixShortcut(ShortcutChars.WithParameters);
            snippet.SuffixDescription(" (with parameters)");

            snippet.RemoveLiteralAndReplacePlaceholders(LiteralIdentifiers.ParameterList, "($parameters$)");

            snippet.AddLiteral("parameters", "Parameters", language.GetDefaultParameter());

            snippet.AddTag(KnownTags.ExcludeFromReadme);

            snippet.SuffixFileName("WithParameters");
        }
    }
}
