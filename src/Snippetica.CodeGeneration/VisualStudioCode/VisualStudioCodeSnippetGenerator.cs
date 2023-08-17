﻿// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using Snippetica.CodeGeneration.Commands;
using Snippetica.VisualStudio;

namespace Snippetica.CodeGeneration.VisualStudioCode;

public class VisualStudioCodeSnippetGenerator : EnvironmentSnippetGenerator
{
    public VisualStudioCodeSnippetGenerator(SnippetEnvironment environment, LanguageDefinition languageDefinition)
        : base(environment, languageDefinition)
    {
    }

    protected override Snippet PostProcess(Snippet snippet)
    {
        SnippetLiteralList literals = snippet.Literals;

        SnippetLiteral typeLiteral = literals.Find(LiteralIdentifiers.Type);

        if (typeLiteral is not null)
        {
            if (snippet.HasTag(KnownTags.GenerateVoidType))
            {
                typeLiteral.DefaultValue = "void";
            }
            else
            {
                typeLiteral.DefaultValue = "T";
            }
        }

        base.PostProcess(snippet);

        return snippet;
    }

    protected override IEnumerable<Command> GetBasicTypeCommands(Snippet snippet)
    {
        if (snippet.HasTag(KnownTags.GenerateBasicType)
            || snippet.HasTag(KnownTags.GenerateVoidType)
            || snippet.HasTag(KnownTags.GenerateBooleanType)
            || snippet.HasTag(KnownTags.GenerateDateTimeType)
            || snippet.HasTag(KnownTags.GenerateDoubleType)
            || snippet.HasTag(KnownTags.GenerateDecimalType)
            || snippet.HasTag(KnownTags.GenerateInt32Type)
            || snippet.HasTag(KnownTags.GenerateInt64Type)
            || snippet.HasTag(KnownTags.GenerateObjectType)
            || snippet.HasTag(KnownTags.GenerateStringType)
            || snippet.HasTag(KnownTags.GenerateSingleType))
        {
            yield return new BasicTypeCommand(null);
        }
    }

    protected override IEnumerable<Command> GetTypeCommands(Snippet snippet)
    {
        yield break;
    }
}
