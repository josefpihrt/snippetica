﻿// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Snippetica.VisualStudio;

namespace Snippetica.CodeGeneration.VisualStudio;

public class VisualStudioSnippetGenerator : EnvironmentSnippetGenerator
{
    public VisualStudioSnippetGenerator(SnippetEnvironment environment, LanguageDefinition languageDefinition)
        : base(environment, languageDefinition)
    {
    }

    protected override Snippet PostProcess(Snippet snippet)
    {
        SnippetLiteral typeLiteral = snippet.Literals.Find(LiteralIdentifiers.Type);

        if (typeLiteral is not null)
            typeLiteral.DefaultValue = "T";

        base.PostProcess(snippet);

        snippet.RemoveTag(KnownTags.ExcludeFromVisualStudioCode);

        return snippet;
    }
}
