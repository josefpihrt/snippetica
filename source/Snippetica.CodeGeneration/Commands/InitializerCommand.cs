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

            Debug.Fail("");
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
