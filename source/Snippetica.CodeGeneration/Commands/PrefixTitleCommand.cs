// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Pihrtsoft.Snippets;

namespace Snippetica.CodeGeneration.Commands
{
    public class PrefixTitleCommand : SnippetCommand
    {
        public PrefixTitleCommand(TypeDefinition type)
        {
            Type = type;
        }

        public TypeDefinition Type { get; }

        public override CommandKind Kind => CommandKind.PrefixTitle;

        protected override void Execute(ExecutionContext context, Snippet snippet)
        {
            snippet.PrefixTitle($"{Type.Shortcut} ");
            snippet.AddTag(KnownTags.TitleStartsWithShortcut);
        }
    }
}
