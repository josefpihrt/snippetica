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
        public static Command InlineCommand { get; } = new InlineCommand();
        public static Command ConstCommand { get; } = new ConstCommand();
        public static Command ConstExprCommand { get; } = new ConstExprCommand();
        public static Command InitializerCommand { get; } = new InitializerCommand();
        public static Command ShortcutToLowercaseCommand { get; } = new ShortcutToLowercaseCommand();
        public static Command SuffixFileNameWithUnderscoreCommand { get; } = new SuffixFileNameCommand("_");
        public static Command DeclarationCommand { get; } = new DeclarationCommand();
        public static Command DefinitionCommand { get; } = new DefinitionCommand();
        public static Command GenerateAlternativeShortcutCommand { get; } = new GenerateAlternativeShortcutCommand();

        public static IEnumerable<Command> GetBasicTypeCommands(Snippet snippet, LanguageDefinition languageDefinition)
        {
            bool flg = false;

            foreach (TypeDefinition type in languageDefinition
                .Types
                .Where(f => f.HasTag(KnownTags.BasicType) && snippet.RequiresBasicTypeGeneration(f.Name)))
            {
                yield return new BasicTypeCommand(type);

                if (!flg)
                {
                    yield return new BasicTypeCommand(TypeDefinition.Default);
                    flg = true;
                }
            }
        }

        public static IEnumerable<Command> GetTypeCommands(LanguageDefinition languageDefinition)
        {
            return languageDefinition
                .Types
                .Where(f => !f.HasTag(KnownTags.BasicType))
                .Select(f => new TypeCommand(f));
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
