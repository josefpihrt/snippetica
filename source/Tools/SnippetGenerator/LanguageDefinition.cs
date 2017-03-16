// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;

namespace Pihrtsoft.Snippets.CodeGeneration
{
    public abstract class LanguageDefinition
    {
        protected LanguageDefinition()
        {
            Modifiers = new ModifierDefinitionCollection();
            Types = new TypeDefinitionCollection();
        }

        public abstract Language Language { get; }

        public ModifierDefinitionCollection Modifiers { get; }
        public TypeDefinitionCollection Types { get; }

        public TypeDefinition Object
        {
            get { return Types["Object"]; }
        }

        public ModifierDefinition Static
        {
            get { return Modifiers["Static"]; }
        }

        public ModifierDefinition Virtual
        {
            get { return Modifiers["Virtual"]; }
        }

        public string DefaultValue
        {
            get { return Object.DefaultValue; }
        }

        public void GenerateSnippets(SnippetDirectory[] snippetDirectories, SnippetGeneratorSettings settings)
        {
            GenerateSnippets(snippetDirectories, settings, f => !f.HasTag(KnownTags.Dev));
            GenerateSnippets(snippetDirectories, settings, f => f.HasTag(KnownTags.Dev));
        }

        private void GenerateSnippets(SnippetDirectory[] snippetDirectories, SnippetGeneratorSettings settings, Func<SnippetDirectory, bool> predicate)
        {
            IEnumerable<SnippetDirectory> items = snippetDirectories.Where(f => f.Language == Language);

            if (predicate != null)
                items = items.Where(predicate);

            snippetDirectories = items.ToArray();

            if (snippetDirectories.Length > 0)
            {
                string source = items
                    .Where(f => f.HasTag(KnownTags.AutoGenerationSource))
                    .Select(f => f.Path)
                    .FirstOrDefault();

                if (source != null)
                {
                    string destination = items
                        .Where(f => f.HasTag(KnownTags.AutoGenerationDestination))
                        .Select(f => f.Path)
                        .FirstOrDefault();

                    if (destination != null)
                    {
                        var generator = new SnippetGenerator(settings);
                        generator.GenerateSnippets(source, destination);
                    }
                }
            }
        }

        public abstract string GetTypeParameterList(string typeName);
        public abstract string GetDefaultParameter();
        public abstract string GetCollectionInitializer(string value);
        public abstract string GetDictionaryInitializer(string value);
        public abstract string GetArrayInitializer(string value);
    }
}
