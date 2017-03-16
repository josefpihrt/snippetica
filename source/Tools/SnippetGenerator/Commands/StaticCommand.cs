// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Pihrtsoft.Snippets.CodeGeneration.Commands
{
    public class StaticCommand : BaseCommand
    {
        public override CommandKind Kind
        {
            get { return CommandKind.StaticModifier; }
        }

        protected override void Execute(ExecutionContext context, Snippet snippet)
        {
            LanguageDefinition language = ((LanguageExecutionContext)context).Language;

            ModifierDefinition @static = language.Static;

            snippet.PrefixTitle($"{@static.Keyword} ");

            snippet.PrefixShortcut(@static.Shortcut);

            snippet.PrefixDescription($"{@static.Keyword} ");

            snippet.ReplacePlaceholders(LiteralIdentifiers.Modifiers, $"${LiteralIdentifiers.Modifiers}$ {@static.Keyword}");

            snippet.PrefixFileName(@static.Name);
        }
    }
}
