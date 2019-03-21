// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Pihrtsoft.Snippets;

namespace Snippetica.CodeGeneration.Commands
{
    public class DeclarationCommand : SnippetCommand
    {
        public override CommandKind Kind => CommandKind.Declaration;

        protected override void Execute(ExecutionContext context, Snippet snippet)
        {
            snippet.SuffixTitle(" declaration");
            snippet.SuffixDescription(" declaration");
            snippet.SuffixFileName("Declaration");

            PlaceholderCollection placeholders = snippet.Code.Placeholders;

            if (placeholders.Contains("_definitionStart"))
            {
                int index = placeholders.Find("_definitionStart").Index - 1;
                int endIndex = placeholders.Find("_definitionEnd").EndIndex + 1;

                string s = snippet.CodeText;

                s = s.Insert(endIndex, ";");
                s = s.Remove(index, endIndex - index);

                snippet.CodeText = s;
            }

            snippet.AppendCode(snippet.Delimiter + Placeholder.EndIdentifier + snippet.Delimiter);
        }
    }
}
