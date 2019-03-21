// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Pihrtsoft.Snippets;

namespace Snippetica.CodeGeneration.Commands
{
    public class ShortcutToLowercaseCommand : SnippetCommand
    {
        public override CommandKind Kind => CommandKind.ShortcutToLowercase;

        protected override void Execute(ExecutionContext context, Snippet snippet)
        {
            snippet.Shortcut = snippet.Shortcut.ToLowerInvariant();
        }
    }
}
