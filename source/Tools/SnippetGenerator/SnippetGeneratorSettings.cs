// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.ObjectModel;

namespace Pihrtsoft.Snippets.CodeGeneration
{
    public class SnippetGeneratorSettings
    {
        public SnippetGeneratorSettings(LanguageDefinition language)
        {
            Language = language;

            Types = new Collection<TypeDefinition>();
        }

        public LanguageDefinition Language { get; }

        public Collection<TypeDefinition> Types { get; }
    }
}
