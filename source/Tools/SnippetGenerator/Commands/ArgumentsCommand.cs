namespace Pihrtsoft.Snippets.CodeGeneration.Commands
{
    public class ArgumentsCommand : BaseCommand
    {
        public override CommandKind Kind
        {
            get { return CommandKind.Arguments; }
        }

        protected override void Execute(ExecutionContext context, Snippet snippet)
        {
            snippet.SuffixTitle(" (with arguments)");
            snippet.SuffixShortcut(ShortcutChars.WithArguments);
            snippet.SuffixDescription(" (with arguments)");

            snippet.RemoveLiteralAndReplacePlaceholders(LiteralIdentifiers.ArgumentList, "($arguments$)");

            snippet.AddLiteral("arguments", "Arguments", "arguments");

            snippet.AddTag(KnownTags.ExcludeFromReadme);

            snippet.SuffixFileName("WithArguments");
        }
    }
}
