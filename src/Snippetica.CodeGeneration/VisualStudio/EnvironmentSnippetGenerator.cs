// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Pihrtsoft.Snippets;

namespace Snippetica.CodeGeneration
{
    public class EnvironmentSnippetGenerator : LanguageSnippetGenerator
    {
        public EnvironmentSnippetGenerator(SnippetEnvironment environment, LanguageDefinition languageDefinition)
            : base(languageDefinition)
        {
            Environment = environment;
        }

        public SnippetEnvironment Environment { get; }

        protected override ExecutionContext CreateExecutionContext(Snippet snippet)
        {
            return new EnvironmentExecutionContext((Snippet)snippet.Clone(), LanguageDefinition, Environment);
        }
    }
}
