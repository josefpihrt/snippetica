// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Pihrtsoft.Snippets;
using Snippetica.CodeGeneration.Commands;

namespace Snippetica.CodeGeneration
{
    public class HtmlSnippetGenerator : SnippetGenerator
    {
        protected override MultiCommandCollection CreateCommands(Snippet snippet)
        {
            var commands = new MultiCommandCollection();

            if (snippet.Literals.Contains("content"))
            {
                commands.Add(new MultiCommand(new HtmlWithContentCommand()));
                commands.Add(new MultiCommand(new HtmlWithoutContentCommand()));
            }
            else
            {
                commands.Add(new MultiCommand());
            }

            return commands;
        }
    }
}
