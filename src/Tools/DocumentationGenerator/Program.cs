// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Pihrtsoft.Snippets;
using Snippetica.CodeGeneration.Markdown;
using Snippetica.CodeGeneration.VisualStudio;
using Snippetica.CodeGeneration.VisualStudioCode;
using static Snippetica.KnownPaths;

namespace Snippetica.CodeGeneration.DocumentationGenerator;

internal static class Program
{
    //TODO: data directory
    private static void Main(string[] args)
    {
        string outputDirectoryPath = args[0];
        string dataDirectoryPath = args[1];

        ShortcutInfo[] shortcuts = Records.Document.ReadRecords(Path.Combine(dataDirectoryPath, "Shortcuts.xml"))
            .Where(f => !f.HasTag(KnownTags.Disabled))
            .Select(f => Mapper.MapShortcutInfo(f))
            .ToArray();

        SnippetDirectory[] directories = Records.Document.ReadRecords(Path.Combine(dataDirectoryPath, "Directories.xml"))
            .Where(f => !f.HasTag(KnownTags.Disabled))
            .Select(f => Mapper.MapSnippetDirectory(f))
            .ToArray();

        Dictionary<Language, LanguageDefinition> languageDefinitions = Mapper.LoadLanguages(dataDirectoryPath);

        var visualStudio = new VisualStudioEnvironment();

        List<SnippetGeneratorResult> visualStudioResults = GenerateSnippets(
            visualStudio,
            directories,
            shortcuts,
            languageDefinitions,
            VisualStudioExtensionProjectPath);

        var visualStudioCode = new VisualStudioCodeEnvironment();

        List<SnippetGeneratorResult> visualStudioCodeResults = GenerateSnippets(
            visualStudioCode,
            directories,
            shortcuts,
            languageDefinitions,
            VisualStudioCodeExtensionProjectPath);

        Console.WriteLine("DONE");
    }

    private static List<SnippetGeneratorResult> GenerateSnippets(
        SnippetEnvironment environment,
        SnippetDirectory[] directories,
        ShortcutInfo[] shortcuts,
        Dictionary<Language, LanguageDefinition> languages,
        string projectPath)
    {
        environment.Shortcuts.AddRange(shortcuts.Where(f => f.Environments.Contains(environment.Kind)));

        List<SnippetGeneratorResult> results = environment.GenerateSnippets(directories, languages, includeDevelopment: false).ToList();

        MarkdownFileWriter.WriteProjectReadme(projectPath, results, environment.CreateProjectReadmeSettings());

        return results;
    }
}
