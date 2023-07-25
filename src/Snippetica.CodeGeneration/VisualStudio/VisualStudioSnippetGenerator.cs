﻿// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Pihrtsoft.Snippets;

namespace Snippetica.CodeGeneration.VisualStudio;

public class VisualStudioSnippetGenerator : EnvironmentSnippetGenerator
{
    public VisualStudioSnippetGenerator(SnippetEnvironment environment, LanguageDefinition languageDefinition)
        : base(environment, languageDefinition)
    {
    }

    protected override Snippet PostProcess(Snippet snippet)
    {
        Literal typeLiteral = snippet.Literals[LiteralIdentifiers.Type];

        if (typeLiteral is not null)
            typeLiteral.DefaultValue = "T";

        base.PostProcess(snippet);

        snippet.RemoveTag(KnownTags.ExcludeFromVisualStudioCode);

        return snippet;
    }
}
