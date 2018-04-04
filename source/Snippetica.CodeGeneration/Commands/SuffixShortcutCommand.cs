// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Pihrtsoft.Snippets;

namespace Snippetica.CodeGeneration.Commands
{
    //TODO: AppendToShortcut
    public class SuffixShortcutCommand : SnippetCommand
    {
        public SuffixShortcutCommand(string suffix)
        {
            Suffix = suffix;
        }

        public string Suffix { get; }

        public override CommandKind Kind
        {
            get { return CommandKind.SuffixShortcut; }
        }

        protected override void Execute(ExecutionContext context, Snippet snippet)
        {
            snippet.SuffixShortcut(Suffix);
        }
    }
}
