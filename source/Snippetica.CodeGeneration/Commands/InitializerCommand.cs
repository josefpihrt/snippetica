// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Diagnostics;
using Pihrtsoft.Snippets;

namespace Snippetica.CodeGeneration.Commands
{
    public class InitializerCommand : SnippetCommand
    {
        public override CommandKind Kind
        {
            get { return CommandKind.Initializer; }
        }

        protected override void Execute(ExecutionContext context, Snippet snippet)
        {
            LanguageDefinition language = ((LanguageExecutionContext)context).Language;

            AddInitializer(context, snippet, GetInitializer(snippet, language), language.GetDefaultValue());
        }

        private string GetInitializer(Snippet snippet, LanguageDefinition language)
        {
            if (snippet.HasTag(KnownTags.Array))
                return language.GetArrayInitializer($"${LiteralIdentifiers.Value}$");

            if (snippet.HasTag(KnownTags.Dictionary))
                return language.GetDictionaryInitializer($"${LiteralIdentifiers.Value}$");

            if (snippet.HasTag(KnownTags.Collection))
                return language.GetCollectionInitializer($"${LiteralIdentifiers.Value}$");

            if (snippet.HasTag(KnownTags.Variable))
                return language.GetVariableInitializer($"${LiteralIdentifiers.Value}$");

            Debug.Fail("");
            return null;
        }

        internal static Snippet AddInitializer(ExecutionContext context, Snippet snippet, string initializer, string defaultValue)
        {
            string suffix = (snippet.Language == Language.Cpp) ? " (with initialization)" : " (with initializer)";

            snippet.SuffixTitle(suffix);
            snippet.SuffixShortcut(context.WithInitializerSuffix(snippet));
            snippet.SuffixDescription(suffix);

            snippet.ReplacePlaceholders(LiteralIdentifiers.Initializer, initializer);

            snippet.AddLiteral(LiteralIdentifiers.Value, null, defaultValue);

            snippet.RemoveLiteral(LiteralIdentifiers.Initializer);

            if (snippet.Language == Language.Cpp)
            {
                Literal typeLiteral = snippet.Literals.Find(LiteralIdentifiers.Type);
                typeLiteral.DefaultValue = "auto";

                LiteralRenamer.Rename(snippet, LiteralIdentifiers.Type, "type");
            }
            else
            {
                snippet.RemoveLiteralAndPlaceholders(LiteralIdentifiers.ArrayLength);
            }

            snippet.AddTag(KnownTags.ExcludeFromReadme);

            snippet.SuffixFileName((snippet.Language == Language.Cpp) ? "WithInitialization" : "WithInitializer");

            return snippet;
        }
    }
}
