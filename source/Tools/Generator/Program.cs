// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Pihrtsoft.Records;
using Pihrtsoft.Snippets.CodeGeneration.Markdown;

namespace Pihrtsoft.Snippets.CodeGeneration
{
    internal static class Program
    {
        private static void Main(string[] args)
        {
            var settings = new GeneralSettings() { SolutionDirectoryPath = @"..\..\..\..\.." };

            SnippetDirectory[] snippetDirectories = SnippetDirectory.LoadFromFile(@"..\..\SnippetDirectories.xml").ToArray();

            CharacterSequence[] characterSequences = CharacterSequence.LoadFromFile(@"..\..\CharacterSequences.xml").ToArray();

            CharacterSequence.SerializeToXml(Path.Combine(settings.ExtensionProjectPath, "CharacterSequences.xml"), characterSequences);

            GenerateSnippets(snippetDirectories);
            SnippetGenerator.GenerateHtmlSnippets(snippetDirectories);
            SnippetGenerator.GenerateXamlSnippets(snippetDirectories);
            SnippetGenerator.GenerateXmlSnippets(snippetDirectories);

            SnippetDirectory[] releaseDirectories = snippetDirectories
                .Where(f => f.HasTag(KnownTags.Release) && !f.HasTag(KnownTags.Dev))
                .ToArray();

            MarkdownGenerator.WriteSolutionReadMe(releaseDirectories, settings);

            MarkdownGenerator.WriteProjectReadMe(releaseDirectories, Path.GetFullPath(settings.ProjectPath));

            MarkdownGenerator.WriteDirectoryReadMe(
                snippetDirectories
                    .Where(f => f.HasAnyTag(KnownTags.Release, KnownTags.Dev) && !f.HasAnyTag(KnownTags.AutoGenerationSource, KnownTags.AutoGenerationDestination))
                    .ToArray(),
                characterSequences,
                settings);

            SnippetPackageGenerator.GenerateVisualStudioPackageFiles(
                releaseDirectories: releaseDirectories,
                characterSequences: characterSequences,
                releases: Release.LoadFromDocument(@"..\..\ChangeLog.xml").ToArray(),
                settings: settings);

            settings.ExtensionProjectName += ".Dev";

            SnippetPackageGenerator.GenerateVisualStudioPackageFiles(
                releaseDirectories: snippetDirectories
                    .Where(f => f.HasTags(KnownTags.Release, KnownTags.Dev))
                    .ToArray(),
                characterSequences: null,
                releases: null,
                settings: settings);

            SnippetChecker.CheckSnippets(snippetDirectories);

            Console.WriteLine("*** END ***");
            Console.ReadKey();
        }

        private static void GenerateSnippets(SnippetDirectory[] snippetDirectories)
        {
            IEnumerable<Record> records = Document.ReadRecords(@"..\..\Records.xml")
                .Where(f => !f.HasTag(KnownTags.Disabled));

            foreach (LanguageDefinition language in records
                .Where(f => f.ContainsProperty(KnownTags.Language))
                .ToLanguageDefinitions())
            {
                var settings = new SnippetGeneratorSettings(language);

                foreach (TypeDefinition typeInfo in records
                    .Where(f => f.HasTag(KnownTags.Collection))
                    .ToTypeDefinitions())
                {
                    settings.Types.Add(typeInfo);
                }

                language.GenerateSnippets(snippetDirectories, settings);
            }
        }
    }
}
