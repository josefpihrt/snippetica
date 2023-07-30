// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Snippetica.CodeGeneration.Markdown;
using Snippetica.CodeGeneration.VisualStudio;
using Snippetica.CodeGeneration.VisualStudioCode;
using Snippetica.IO;
using Snippetica.VisualStudio.Serializer;

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

        SnippeticaMetadata metadata = SnippeticaMetadata.Load(Path.Combine(dataDirectoryPath, "metadata.json"), sourcePath);
        ShortcutInfo[] shortcuts = metadata.Shortcuts;
        SnippetDirectory[] directories = metadata.Directories;
        Dictionary<Language, LanguageDefinition> languageDefinitions = metadata.Languages;

        GenerateDocumentation(new VisualStudioEnvironment(), directories, shortcuts, languageDefinitions, Path.Combine(destinationPath, "snippets"));
        GenerateDocumentation(new VisualStudioCodeEnvironment(), directories, shortcuts, languageDefinitions, Path.Combine(destinationPath, "snippets"));
        GenerateQuickReference(destinationPath, shortcuts);

        Console.WriteLine("DONE");
    }

    private static void GenerateDocumentation(
        SnippetEnvironment environment,
        SnippetDirectory[] directories,
        ShortcutInfo[] shortcuts,
        Dictionary<Language, LanguageDefinition> languages,
        string destinationPath)
    {
        environment.Shortcuts.AddRange(shortcuts.Where(f => f.Environments.Contains(environment.Kind)));

        List<SnippetGeneratorResult> results = environment.GenerateSnippets(directories, languages, includeDevelopment: false).ToList();

        IOUtility.WriteAllText(
            Path.Combine(destinationPath, $"{environment.Kind.GetIdentifier()}.md"),
            MarkdownGenerator.GenerateEnvironmentMarkdown(environment, results),
            onlyIfChanged: false,
            createDirectory: true);

        foreach (SnippetGeneratorResult result in results)
        {
            if (result.Tags.Contains(KnownTags.ExcludeFromDocs))
                continue;

            IOUtility.WriteAllText(
                Path.Combine(destinationPath, result.Environment.Kind.GetIdentifier(), $"{result.Language.GetIdentifier()}.md"),
                MarkdownGenerator.GenerateSnippetsMarkdown(result),
                onlyIfChanged: false,
                createDirectory: true);
        }
    }

    private static void GenerateQuickReference(string destinationPath, ShortcutInfo[] shortcuts)
    {
        string quickReference = MarkdownGenerator.GenerateQuickReferenceForCSharpAndVisualBasic(
            shortcuts
                .Where(f => f.Languages.Contains(Language.CSharp)
                    || f.Languages.Contains(Language.VisualBasic))
                .ToList());

        IOUtility.WriteAllText(
            Path.Combine(destinationPath, "quick-reference-cs-vb.md"),
            quickReference,
            onlyIfChanged: false,
            createDirectory: true);
    }
}
