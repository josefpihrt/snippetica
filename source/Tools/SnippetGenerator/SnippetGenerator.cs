// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.IO;
using System.Linq;
using Pihrtsoft.Snippets.CodeGeneration.Commands;

namespace Pihrtsoft.Snippets.CodeGeneration
{
    public class SnippetGenerator
    {
        public SnippetGenerator(LanguageDefinition languageDefinition)
        {
            LanguageDefinition = languageDefinition;
        }

        private LanguageDefinition LanguageDefinition { get; }

        private static Command StaticCommand { get; } = new StaticCommand();
        private static Command VirtualCommand { get; } = new VirtualCommand();
        private static Command InitializerCommand { get; } = new InitializerCommand();
        private static Command ParametersCommand { get; } = new ParametersCommand();
        private static Command ArgumentsCommand { get; } = new ArgumentsCommand();

        public static void GenerateSnippets(SnippetDirectory[] snippetDirectories, LanguageDefinition[] languageDefinitions)
        {
            foreach (LanguageDefinition languageDefinition in languageDefinitions)
                GenerateSnippets(snippetDirectories.Where(f => f.Language == languageDefinition.Language), languageDefinition);
        }

        private static void GenerateSnippets(IEnumerable<SnippetDirectory> snippetDirectories, LanguageDefinition languageDefinition)
        {
            GenerateSnippets2(snippetDirectories.Where(f => !f.HasTag(KnownTags.Dev)).ToArray(), languageDefinition);
            GenerateSnippets2(snippetDirectories.Where(f => f.HasTag(KnownTags.Dev)).ToArray(), languageDefinition);
        }

        private static void GenerateSnippets2(SnippetDirectory[] snippetDirectories, LanguageDefinition languageDefinition)
        {
            if (snippetDirectories.Length > 0)
            {
                string source = snippetDirectories
                    .Where(f => f.HasTag(KnownTags.AutoGenerationSource))
                    .Select(f => f.Path)
                    .FirstOrDefault();

                if (source != null)
                {
                    string destination = snippetDirectories
                        .Where(f => f.HasTag(KnownTags.AutoGenerationDestination))
                        .Select(f => f.Path)
                        .FirstOrDefault();

                    if (destination != null)
                    {
                        var generator = new SnippetGenerator(languageDefinition);
                        generator.GenerateSnippets(source, destination);
                    }
                }
            }
        }

        public static void GenerateXamlSnippets(SnippetDirectory[] snippetDirectories)
        {
            IEnumerable<SnippetDirectory> directories = snippetDirectories
                .Where(f => f.Language == Language.Xaml);

            string sourceDirPath = directories.First(f => f.HasTag(KnownTags.AutoGenerationSource)).Path;
            string destinationDirPath = directories.First(f => f.HasTag(KnownTags.AutoGenerationDestination)).Path;

            var snippets = new List<Snippet>();

            snippets.AddRange(XmlSnippetGenerator.GenerateSnippets(destinationDirPath, Language.Xaml));

            var generator = new XamlSnippetGenerator();
            snippets.AddRange(generator.GenerateSnippets(sourceDirPath));

            IOUtility.SaveSnippets(snippets.ToArray(), destinationDirPath);
        }

        public static void GenerateXmlSnippets(SnippetDirectory[] snippetDirectories)
        {
            string destinationDirPath = snippetDirectories.First(f => f.Language == Language.Xml && f.HasTag(KnownTags.AutoGenerationDestination)).Path;

            Snippet[] snippets = XmlSnippetGenerator.GenerateSnippets(destinationDirPath, Language.Xml).ToArray();

            IOUtility.SaveSnippets(snippets, destinationDirPath);
        }

        public static void GenerateHtmlSnippets(SnippetDirectory[] snippetDirectories)
        {
            string sourceDirPath = snippetDirectories.First(f => f.Language == Language.Html && f.HasTag(KnownTags.AutoGenerationSource)).Path;
            string destinationDirPath = snippetDirectories.First(f => f.Language == Language.Html && f.HasTag(KnownTags.AutoGenerationDestination)).Path;

            var snippets = new List<Snippet>();
            snippets.AddRange(XmlSnippetGenerator.GenerateSnippets(destinationDirPath, Language.Html));
            snippets.AddRange(HtmlSnippetGenerator.GenerateSnippets(sourceDirPath));

            IOUtility.SaveSnippets(snippets.ToArray(), destinationDirPath);
        }

        public void GenerateSnippets(string sourceDirectoryPath, string destinationDirectoryPath)
        {
            Snippet[] snippets = SnippetSerializer.Deserialize(sourceDirectoryPath, SearchOption.AllDirectories)
                .SelectMany(snippet => GenerateSnippets(snippet))
                .ToArray();

            IOUtility.SaveSnippets(snippets, destinationDirectoryPath);
        }

        private IEnumerable<Snippet> GenerateSnippets(Snippet snippet)
        {
            var jobs = new JobCollection();

            jobs.AddCommands(GetTypeCommands(snippet));

            if (snippet.HasTag(KnownTags.GenerateCollection))
                jobs.AddCommands(GetNonImmutableCollectionCommands());

            if (snippet.HasTag(KnownTags.GenerateImmutableCollection))
                jobs.AddCommands(GetImmutableCollectionCommands());

            jobs.AddCommands(GetAccessModifierCommands(snippet));

            if (snippet.HasTag(KnownTags.GenerateStaticModifier))
                jobs.AddCommand(StaticCommand);

            if (snippet.HasTag(KnownTags.GenerateVirtualModifier))
                jobs.AddCommand(VirtualCommand);

            if (snippet.HasTag(KnownTags.GenerateInitializer))
                jobs.AddCommand(InitializerCommand);

            if (snippet.HasTag(KnownTags.GenerateParameters))
                jobs.AddCommand(ParametersCommand);

            if (snippet.HasTag(KnownTags.GenerateArguments))
                jobs.AddCommand(ArgumentsCommand);

            if (snippet.HasTag(KnownTags.GenerateUnchanged))
                jobs.Add(new Job());

            foreach (Job job in jobs)
            {
                var context = new LanguageExecutionContext((Snippet)snippet.Clone(), LanguageDefinition);

                job.Execute(context);

                if (!context.IsCanceled)
                {
                    foreach (Snippet snippet2 in context.Snippets)
                    {
                        PostProcess(snippet2);
                        yield return snippet2;
                    }
                }
            }
        }

        private IEnumerable<Command> GetTypeCommands(Snippet snippet)
        {
            bool flg = false;

            foreach (TypeDefinition type in LanguageDefinition
                .Types
                .Where(f => !f.HasTag(KnownTags.Collection) && snippet.RequiresTypeGeneration(f.Name)))
            {
                yield return new TypeCommand(type);

                if (!flg)
                {
                    yield return new TypeCommand(null);
                    flg = true;
                }
            }
        }

        private IEnumerable<Command> GetNonImmutableCollectionCommands()
        {
            return LanguageDefinition
                .Types
                .Where(f => f.HasTag(KnownTags.Collection) && !f.HasTag(KnownTags.Immutable))
                .Select(f => new CollectionTypeCommand(f));
        }

        private IEnumerable<Command> GetImmutableCollectionCommands()
        {
            return LanguageDefinition
                .Types
                .Where(f => f.HasTag(KnownTags.Collection) && f.HasTag(KnownTags.Immutable))
                .Select(f => new ImmutableCollectionTypeCommand(f));
        }

        private IEnumerable<Command> GetAccessModifierCommands(Snippet snippet)
        {
            return LanguageDefinition
                .Modifiers
                .Where(modifier => modifier.Tags.Contains(KnownTags.AccessModifier) && snippet.RequiresModifierGeneration(modifier.Name))
                .Select(modifier => new AccessModifierCommand(modifier));
        }

        private void PostProcess(Snippet snippet)
        {
            ReplacePlaceholders(snippet);

            if (snippet.Language == Language.VisualBasic)
                snippet.ReplaceSubOrFunctionLiteral("Function");

            Literal typeLiteral = snippet.Literals[LiteralIdentifiers.Type];

            if (typeLiteral != null)
                typeLiteral.DefaultValue = "T";

            RemoveUnusedLiterals(snippet);

            RemoveKeywords(snippet);

            snippet.AddTag(KnownTags.AutoGenerated);

            snippet.SortCollections();

            snippet.Author = "Josef Pihrt";

            if (snippet.SnippetTypes == SnippetTypes.None)
                snippet.SnippetTypes = SnippetTypes.Expansion;
        }

        private void ReplacePlaceholders(Snippet snippet)
        {
            snippet.Title = snippet.Title
                .ReplacePlaceholder(Placeholders.Type, " ", true)
                .ReplacePlaceholder(Placeholders.OfType, " ", true)
                .ReplacePlaceholder(Placeholders.GenericType, LanguageDefinition.GetTypeParameterList("T"));

            snippet.Description = snippet.Description
                .ReplacePlaceholder(Placeholders.Type, " ", true)
                .ReplacePlaceholder(Placeholders.OfType, " ", true)
                .ReplacePlaceholder(Placeholders.GenericType, LanguageDefinition.GetTypeParameterList("T"));
        }

        private void RemoveKeywords(Snippet snippet)
        {
            snippet.RemoveTags(LanguageDefinition.Types.Select(f => KnownTags.GenerateTypeTag(f.Name)));
            snippet.RemoveTags(LanguageDefinition.Modifiers.Select(f => KnownTags.GenerateModifierTag(f.Name)));

            snippet.RemoveTags(
                KnownTags.GenerateType,
                KnownTags.GenerateAccessModifier,
                KnownTags.GenerateInitializer,
                KnownTags.GenerateUnchanged,
                KnownTags.GenerateParameters,
                KnownTags.GenerateArguments,
                KnownTags.GenerateCollection,
                KnownTags.GenerateImmutableCollection,
                KnownTags.Array,
                KnownTags.Collection,
                KnownTags.Dictionary,
                KnownTags.TryParse,
                KnownTags.Initializer);
        }

        private static void RemoveUnusedLiterals(Snippet snippet)
        {
            for (int i = snippet.Literals.Count - 1; i >= 0; i--)
            {
                Literal literal = snippet.Literals[i];

                if (!literal.IsEditable
                    && string.IsNullOrEmpty(literal.DefaultValue))
                {
                    snippet.RemoveLiteralAndPlaceholders(literal);
                }
            }
        }
    }
}
