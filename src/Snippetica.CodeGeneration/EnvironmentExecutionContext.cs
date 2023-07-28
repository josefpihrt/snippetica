// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Pihrtsoft.Snippets;

namespace Snippetica.CodeGeneration;

public class EnvironmentExecutionContext : LanguageExecutionContext
{
    public EnvironmentExecutionContext(Snippet snippet, LanguageDefinition language, SnippetEnvironment environment)
        : base(snippet, language)
    {
        Environment = environment;
    }

    public SnippetEnvironment Environment { get; }

    public override string WithInitializerSuffix(Snippet snippet)
    {
        return "x";
    }
}
