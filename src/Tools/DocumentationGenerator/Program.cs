// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Pihrtsoft.Snippets;
using Snippetica.CodeGeneration.Markdown;
using Snippetica.CodeGeneration.VisualStudio;
using Snippetica.CodeGeneration.VisualStudioCode;
using Snippetica.IO;

namespace Snippetica.CodeGeneration.DocumentationGenerator;

internal static class Program
{
    private static void Main(string[] args)
    {
        if (args.Length < 3)
        {
            Console.WriteLine("Invalid number of arguments");
            return;
        }

        string sourcePath = args[0];
        string destinationPath = args[1];
        string dataDirectoryPath = args[2];

        ShortcutInfo[] shortcuts = Records.Document.ReadRecords(Path.Combine(dataDirectoryPath, "Shortcuts.xml"))
            .Where(f => !f.HasTag(KnownTags.Disabled))
            .Select(f => Helpers.MapShortcutInfo(f))
            .ToArray();

        SnippetDirectory[] directories = Records.Document.ReadRecords(Path.Combine(dataDirectoryPath, "Directories.xml"))
            .Where(f => !f.HasTag(KnownTags.Disabled))
            .Select(f => Helpers.MapSnippetDirectory(f, sourcePath))
            .ToArray();

        Dictionary<Language, LanguageDefinition> languageDefinitions = Helpers.LoadLanguages(dataDirectoryPath);

        GenerateDocumentation(new VisualStudioEnvironment(), directories, shortcuts, languageDefinitions, destinationPath, dataDirectoryPath);
        GenerateDocumentation(new VisualStudioCodeEnvironment(), directories, shortcuts, languageDefinitions, destinationPath, dataDirectoryPath);

        Console.WriteLine("DONE");
    }

    private static void GenerateDocumentation(
        SnippetEnvironment environment,
        SnippetDirectory[] directories,
        ShortcutInfo[] shortcuts,
        Dictionary<Language, LanguageDefinition> languages,
        string destinationPath,
        string dataDirectoryPath)
    {
        environment.Shortcuts.AddRange(shortcuts.Where(f => f.Environments.Contains(environment.Kind)));

        List<SnippetGeneratorResult> results = environment.GenerateSnippets(directories, languages, includeDevelopment: false).ToList();

        IOUtility.WriteAllText(
            Path.Combine(destinationPath, environment.Kind.GetIdentifier(), "index.md"),
            MarkdownGenerator.GenerateEnvironmentMarkdown(environment, results),
            onlyIfChanged: false,
            createDirectory: true);

        foreach (SnippetGeneratorResult result in results)
        {
            if (result.Tags.Contains(KnownTags.ExcludeFromDocs))
                continue;

            DirectoryReadmeSettings settings = environment.CreateSnippetsMarkdownSettings(result);

            string filePath = Path.Combine(dataDirectoryPath, $"{result.Language.GetIdentifier()}.md");

            if (File.Exists(filePath))
                settings.QuickReferenceText = File.ReadAllText(filePath, Encoding.UTF8);

            IOUtility.WriteAllText(
                Path.Combine(destinationPath, result.Environment.Kind.GetIdentifier(), $"{result.Language.GetIdentifier()}.md"),
                MarkdownGenerator.GenerateSnippetsMarkdown(result, settings),
                onlyIfChanged: false,
                createDirectory: true);
        }
    }
}
