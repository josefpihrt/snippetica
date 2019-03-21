// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Pihrtsoft.Snippets;

namespace Snippetica.CodeGeneration.Commands
{
    public class HtmlWithContentCommand : SnippetCommand
    {
        public override CommandKind Kind => CommandKind.None;

        protected override void Execute(ExecutionContext context, Snippet snippet)
        {
            snippet.SuffixTitle(" (with content)");
            snippet.SuffixDescription(" (with content)");
            snippet.SuffixShortcut(XmlSnippetGenerator.ContentShortcut);
            snippet.SuffixFileName("_with_content");

            snippet.AddTag(KnownTags.ExcludeFromReadme);
        }
    }
}
