// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using Pihrtsoft.Snippets;
using Snippetica.CodeGeneration.Commands;

namespace Snippetica.CodeGeneration.VisualStudioCode
{
    public class VisualStudioCodeSnippetGenerator : EnvironmentSnippetGenerator
    {
        public VisualStudioCodeSnippetGenerator(SnippetEnvironment environment, LanguageDefinition languageDefinition)
            : base(environment, languageDefinition)
        {
        }

        protected override Snippet PostProcess(Snippet snippet)
        {
            LiteralCollection literals = snippet.Literals;

            Literal typeLiteral = literals[LiteralIdentifiers.Type];

            if (typeLiteral != null)
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
}
