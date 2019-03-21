// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Pihrtsoft.Snippets;

namespace Snippetica.CodeGeneration.Commands
{
    public class SuffixFileNameCommand : SnippetCommand
    {
        public SuffixFileNameCommand(string suffix)
        {
            Suffix = suffix;
        }

        public string Suffix { get; }

        public override CommandKind Kind => CommandKind.SuffixFileName;

        protected override void Execute(ExecutionContext context, Snippet snippet)
        {
            snippet.SuffixFileName(Suffix);
        }
    }
}
