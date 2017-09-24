// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Linq;
using Pihrtsoft.Snippets;

namespace Snippetica.CodeGeneration.Commands
{
    internal static class CommandUtility
    {
        public static Command StaticCommand { get; } = new StaticCommand();
        public static Command VirtualCommand { get; } = new VirtualCommand();
        public static Command InitializerCommand { get; } = new InitializerCommand();
        public static Command ParametersCommand { get; } = new ParametersCommand();
        public static Command ArgumentsCommand { get; } = new ArgumentsCommand();
        public static Command ShortcutToLowercase { get; } = new ShortcutToLowercaseCommand();
        public static Command SuffixFileNameWithUnderscore { get; } = new SuffixFileNameCommand("_");
        public static Command GenerateAlternativeShortcuts { get; } = new GenerateAlternativeShortcutCommand();

        public static IEnumerable<Command> GetTypeCommands(Snippet snippet, LanguageDefinition languageDefinition)
        {
            bool flg = false;

            foreach (TypeDefinition type in languageDefinition
                .Types
                .Where(f => !f.HasTag(KnownTags.Collection) && snippet.RequiresTypeGeneration(f.Name)))
            {
                yield return new TypeCommand(type);

                if (!flg)
                {
                    yield return new TypeCommand(TypeDefinition.Default);
                    flg = true;
                }
            }
        }

        public static IEnumerable<Command> GetNonImmutableCollectionCommands(LanguageDefinition languageDefinition)
        {
            return languageDefinition
                .Types
                .Where(f => f.HasTag(KnownTags.Collection) && !f.HasTag(KnownTags.Immutable))
                .Select(f => new CollectionTypeCommand(f));
        }

        public static IEnumerable<Command> GetImmutableCollectionCommands(LanguageDefinition languageDefinition)
        {
            return languageDefinition
                .Types
                .Where(f => f.HasTag(KnownTags.Collection) && f.HasTag(KnownTags.Immutable))
                .Select(f => new ImmutableCollectionTypeCommand(f));
        }

        public static IEnumerable<Command> GetAccessModifierCommands(Snippet snippet, LanguageDefinition languageDefinition)
        {
            return languageDefinition
                .Modifiers
                .Where(modifier => modifier.Tags.Contains(KnownTags.AccessModifier) && snippet.RequiresModifierGeneration(modifier.Name))
                .Select(modifier => new AccessModifierCommand(modifier));
        }
    }
}
