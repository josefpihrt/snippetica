// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Pihrtsoft.Snippets;

namespace Snippetica.CodeGeneration.Commands
{
    public class DefinitionCommand : SnippetCommand
    {
        public override CommandKind Kind
        {
            get { return CommandKind.Definition; }
        }

        protected override void Execute(ExecutionContext context, Snippet snippet)
        {
            snippet.SuffixShortcut("x");
            snippet.SuffixTitle(" definition");
            snippet.SuffixDescription(" definition");
            snippet.SnippetTypes |= SnippetTypes.SurroundsWith;
            snippet.SuffixFileName("Definition");

            PlaceholderCollection placeholders = snippet.Code.Placeholders;

            if (placeholders.Contains("_definition"))
            {
                snippet.CodeText = snippet.Code.ReplacePlaceholders("_definition", @" {
	$selected$$end$
}");
            }
        }
    }
}
