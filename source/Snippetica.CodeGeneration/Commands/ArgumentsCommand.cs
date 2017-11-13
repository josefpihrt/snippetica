// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Pihrtsoft.Snippets;

namespace Snippetica.CodeGeneration.Commands
{
    public class ArgumentsCommand : SnippetCommand
    {
        public override CommandKind Kind
        {
            get { return CommandKind.Arguments; }
        }

        protected override void Execute(ExecutionContext context, Snippet snippet)
        {
            snippet.SuffixTitle(" (with arguments)");
            snippet.SuffixShortcut(ShortcutChars.WithArguments);
            snippet.SuffixDescription(" (with arguments)");
            snippet.SuffixFileName("WithArguments");

            snippet.RemoveLiteralAndReplacePlaceholders(LiteralIdentifiers.ArgumentList, "($arguments$)");
            snippet.AddLiteral("arguments", "Arguments", "arguments");

            snippet.AddTag(KnownTags.ExcludeFromReadme);
        }
    }
}
