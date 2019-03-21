// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Pihrtsoft.Snippets;

namespace Snippetica.CodeGeneration.Commands
{
    public abstract class ModifierCommand : SnippetCommand
    {
        public virtual bool ShouldRemoveLiteral => false;

        protected override void Execute(ExecutionContext context, Snippet snippet)
        {
            LanguageDefinition language = ((LanguageExecutionContext)context).Language;

            ModifierDefinition modifier = GetModifier(language);

            snippet.PrefixTitle($"{modifier.Keyword} ");

            snippet.PrefixShortcut(modifier.Shortcut);

            snippet.PrefixDescription($"{modifier.Keyword} ");

            if (ShouldRemoveLiteral)
            {
                snippet.RemoveLiteralAndReplacePlaceholders(LiteralIdentifiers.Modifiers, modifier.Keyword);
            }
            else
            {
                snippet.ReplacePlaceholders(LiteralIdentifiers.Modifiers, $"${LiteralIdentifiers.Modifiers}$ {modifier.Keyword}");
            }

            snippet.PrefixFileName(modifier.Name);
        }

        protected abstract ModifierDefinition GetModifier(LanguageDefinition language);
    }
}
