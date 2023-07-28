﻿// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.IO;
using Pihrtsoft.Snippets;

namespace Snippetica.CodeGeneration.Commands;

public class XamlPropertyCommand : SnippetCommand
{
    public override CommandKind Kind => CommandKind.XamlProperty;

    protected override void Execute(ExecutionContext context, Snippet snippet)
    {
        snippet.SuffixTitle(" property");
        snippet.SuffixShortcut("py");
        snippet.SuffixDescription(" property");
        snippet.RemoveTag(KnownTags.GenerateXamlProperty);
        snippet.RemoveTag(KnownTags.NonUniqueShortcut);
        snippet.AddTag(KnownTags.AutoGenerated);

        snippet.Literals.Clear();
        snippet.AddLiteral("property", "Property name", ".");

        string name = Path.GetFileNameWithoutExtension(snippet.FilePath);

        snippet.CodeText = $"<{name}$property$>$end$</{name}$property$>";

        snippet.SuffixFileName("Property");
    }
}
