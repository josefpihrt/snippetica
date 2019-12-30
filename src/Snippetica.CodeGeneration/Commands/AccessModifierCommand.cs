// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Pihrtsoft.Snippets;

namespace Snippetica.CodeGeneration.Commands
{
    public class AccessModifierCommand : ModifierCommand
    {
        public AccessModifierCommand(ModifierDefinition modifier)
        {
            Modifier = modifier;
        }

        public ModifierDefinition Modifier { get; }

        public override CommandKind Kind => CommandKind.AccessModifier;

        public override bool ShouldRemoveLiteral => true;

        protected override void Execute(ExecutionContext context, Snippet snippet)
        {
            base.Execute(context, snippet);

            if (!Modifier.Tags.Contains(KnownTags.Default))
                snippet.AddTag(KnownTags.ExcludeFromReadme);
        }

        protected override ModifierDefinition GetModifier(LanguageDefinition language)
        {
            return Modifier;
        }
    }
}
