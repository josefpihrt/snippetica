// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Pihrtsoft.Snippets;
using Snippetica.CodeGeneration.Commands;

namespace Snippetica.CodeGeneration
{
    public class XamlSnippetGenerator : SnippetGenerator
    {
        protected override MultiCommandCollection CreateCommands(Snippet snippet)
        {
            var commands = new MultiCommandCollection();

            if (snippet.HasTag(KnownTags.GenerateAlternativeShortcut))
            {
                commands.AddMultiCommand(CommandUtility.ShortcutToLowercaseCommand);
                commands.AddMultiCommand(CommandUtility.GenerateAlternativeShortcutCommand);
            }

            return commands;
        }

        protected override Snippet PostProcess(Snippet snippet)
        {
            snippet.RemoveTag(KnownTags.NonUniqueTitle);

            return base.PostProcess(snippet);
        }
    }
}
