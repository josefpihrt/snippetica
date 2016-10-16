using System.Diagnostics;
namespace Pihrtsoft.Snippets.CodeGeneration.Commands
{
    public class InitializerCommand : BaseCommand
    {
        public override CommandKind Kind
        {
            get { return CommandKind.Initializer; }
        }

        protected override void Execute(ExecutionContext context, Snippet snippet)
        {
            LanguageDefinition language = ((LanguageExecutionContext)context).Language;

            AddInitializer(snippet, GetInitializer(snippet, language), language.Object.DefaultValue);
        }

        private string GetInitializer(Snippet snippet, LanguageDefinition language)
        {
            if (snippet.HasTag(KnownTags.Array))
                return language.GetArrayInitializer($"${LiteralIdentifiers.Value}$");

            if (snippet.HasTag(KnownTags.Dictionary))
                return language.GetDictionaryInitializer($"${LiteralIdentifiers.Value}$");

            if (snippet.HasTag(KnownTags.Collection))
                return language.GetCollectionInitializer($"${LiteralIdentifiers.Value}$");

            Debug.Assert(false, "");
            return null;
        }

        internal static Snippet AddInitializer(Snippet snippet, string initializer, string defaultValue)
        {
            snippet.SuffixTitle(" (with initializer)");
            snippet.SuffixShortcut(ShortcutChars.WithInitializer);
            snippet.SuffixDescription(" (with initializer)");

            snippet.ReplacePlaceholders(LiteralIdentifiers.Initializer, initializer);

            snippet.AddLiteral(LiteralIdentifiers.Value, null, defaultValue);

            snippet.RemoveLiteral(LiteralIdentifiers.Initializer);

            snippet.RemoveLiteralAndPlaceholders(LiteralIdentifiers.ArrayLength);

            snippet.AddTag(KnownTags.ExcludeFromReadme);

            snippet.SuffixFileName("WithInitializer");

            return snippet;
        }
    }
}
