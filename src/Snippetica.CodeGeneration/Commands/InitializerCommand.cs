// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Pihrtsoft.Snippets;

namespace Snippetica.CodeGeneration.Commands
{
    public class InitializerCommand : SnippetCommand
    {
        public override CommandKind Kind => CommandKind.Initializer;

        protected override void Execute(ExecutionContext context, Snippet snippet)
        {
            LanguageDefinition language = ((LanguageExecutionContext)context).Language;

            if (snippet.HasTag(KnownTags.Array))
            {
                AddInitializer(context, snippet, language.GetArrayInitializer($"${LiteralIdentifiers.Value}$"), language.GetDefaultValue());
            }
            else if (snippet.HasTag(KnownTags.Variable))
            {
                AddInitializer(context, snippet, language.GetVariableInitializer($"${LiteralIdentifiers.Value}$"), language.GetDefaultValue());
            }
            else
            {
                AddInitializer(context, snippet, language.GetObjectInitializer($"${LiteralIdentifiers.Value}$"), "x");
            }
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
