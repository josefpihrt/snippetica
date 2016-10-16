using System.Diagnostics;
using System.IO;

namespace Pihrtsoft.Snippets.CodeGeneration.Commands
{
    public class CollectionTypeCommand : TypeCommand
    {
        public CollectionTypeCommand(TypeDefinition type)
            : base(type)
        {
        }

        public override CommandKind Kind
        {
            get { return CommandKind.Collection; }
        }

        protected override void Execute(ExecutionContext context, Snippet snippet)
        {
            LanguageDefinition language = ((LanguageExecutionContext)context).Language;

            snippet.Title = snippet.Title.ReplacePlaceholder(Placeholders.Collection, Type.Name);
            snippet.Description = snippet.Description.ReplacePlaceholder(Placeholders.Collection, Type.Name);

            snippet.AddNamespace(Type.Namespace);

            snippet.AddTags(
                KnownTags.NonUniqueShortcut,
                KnownTags.TitleStartsWithShortcut,
                KnownTags.ExcludeFromReadme);

            snippet.RemoveLiteralAndReplacePlaceholders(LiteralIdentifiers.Collection, Type.Name);

            if (Tags.Contains(KnownTags.Dictionary))
            {
                snippet.RemoveLiteralAndReplacePlaceholders(LiteralIdentifiers.GenericType, $"${LiteralIdentifiers.KeyType}$, ${LiteralIdentifiers.ValueType}$");
                snippet.AddLiteral(LiteralIdentifiers.KeyType, null, language.Object.Keyword);
                snippet.AddLiteral(LiteralIdentifiers.ValueType, null, language.Object.Keyword);

                LiteralRenamer.Rename(snippet, LiteralIdentifiers.CollectionIdentifier, LiteralIdentifiers.DictionaryIdentifier);

                Literal literal = snippet.Literals.Find(LiteralIdentifiers.DictionaryIdentifier);

                if (literal != null)
                    literal.DefaultValue = "dic";
            }

            if (!Tags.Contains(KnownTags.ArgumentList))
                snippet.RemoveLiteralAndPlaceholders(LiteralIdentifiers.ArgumentList);

            ProcessFilePath(context, snippet);

            if (snippet.HasTag(KnownTags.Initializer) && Tags.Contains(KnownTags.Initializer))
            {
                var clone = (Snippet)snippet.Clone();
                InitializerCommand.AddInitializer(clone, GetInitializer(snippet, language), language.DefaultValue);
                context.Snippets.Add(clone);
            }
            else
            {
                snippet.RemoveLiteralAndPlaceholders(LiteralIdentifiers.Initializer);
            }
        }

        private string GetInitializer(Snippet snippet, LanguageDefinition language)
        {
            if (Tags.Contains(KnownTags.Array))
                return language.GetArrayInitializer($"${LiteralIdentifiers.Value}$");

            if (Tags.Contains(KnownTags.Dictionary))
                return language.GetDictionaryInitializer($"${LiteralIdentifiers.Value}$");

            if (Tags.Contains(KnownTags.Collection))
                return language.GetCollectionInitializer($"${LiteralIdentifiers.Value}$");

            Debug.Assert(false, "");

            return null;
        }

        protected virtual void ProcessFilePath(ExecutionContext context, Snippet snippet)
        {
            snippet.SetFileName(Path.GetFileName(snippet.FilePath).Replace("Collection", Type.Name));
        }
    }
}
