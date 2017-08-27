// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
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

            LanguageDefinition[] languageDefinitions = LanguageDefinition.LoadFromFile(@"..\..\LanguageDefinitions.xml").ToArray();

            CharacterSequence.SerializeToXml(Path.Combine(settings.ExtensionProjectPath, "CharacterSequences.xml"), characterSequences);

            SnippetGenerator.GenerateSnippets(snippetDirectories, languageDefinitions);
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
                settings: settings);

            settings.ExtensionProjectName += ".Dev";

            SnippetPackageGenerator.GenerateVisualStudioPackageFiles(
                releaseDirectories: snippetDirectories
                    .Where(f => f.HasTags(KnownTags.Release, KnownTags.Dev))
                    .ToArray(),
                characterSequences: null,
                settings: settings);

            SnippetChecker.CheckSnippets(snippetDirectories);

            Console.WriteLine("*** END ***");
            Console.ReadKey();
        }
    }
}
