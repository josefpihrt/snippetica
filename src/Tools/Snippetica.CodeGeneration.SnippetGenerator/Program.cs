// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Snippetica.CodeGeneration.VisualStudio;
using Snippetica.CodeGeneration.VisualStudioCode;
using Snippetica.IO;
using Snippetica.VisualStudio;
using Snippetica.VisualStudio.Comparers;
using static Snippetica.KnownNames;

namespace Snippetica.CodeGeneration.SnippetGenerator;

internal static class Program
{
    private static readonly Regex _regexReplaceSpacesWithTabs = new(@"(?<=^(\ {4})*)(?<x>\ {4})(?=(\ {4})*\S)", RegexOptions.Multiline);

    private static void Main(string[] args)
    {
        if (args.Length < 2)
        {
            Console.WriteLine("Invalid number of arguments");
            return;
        }

        string sourcePath = args[0];
        string dataDirectoryPath = args[1];

        sourcePath = Path.GetFullPath(sourcePath);
        dataDirectoryPath = Path.GetFullPath(dataDirectoryPath);

        SnippeticaMetadata metadata = SnippeticaMetadata.Load(Path.Combine(dataDirectoryPath, "metadata.json"), sourcePath);
        SnippetDirectory[] directories = metadata.Directories;
        Dictionary<Language, LanguageDefinition> languageDefinitions = metadata.Languages;

        SaveChangedSnippets(directories);
        GenerateSnippets(new VisualStudioEnvironment(), directories, languageDefinitions, Path.Combine(sourcePath, "Snippetica.VisualStudio"));
        GenerateSnippets(new VisualStudioCodeEnvironment(), directories, languageDefinitions, Path.Combine(sourcePath, "Snippetica.VisualStudioCode"));

        Console.WriteLine("DONE");
    }

    private static void GenerateSnippets(
        SnippetEnvironment environment,
        SnippetDirectory[] directories,
        Dictionary<Language, LanguageDefinition> languages,
        string projectPath)
    {
        var results = new List<SnippetGeneratorResult>();
        var devResults = new List<SnippetGeneratorResult>();

        foreach (SnippetGeneratorResult result in environment.GenerateSnippets(directories, languages, includeDevelopment: true))
        {
            if (result.IsDevelopment)
            {
                devResults.Add(result);
            }
            else
            {
                results.Add(result);
            }
        }

        var snippets = new List<Snippet>();

        snippets.AddRange(environment.GeneratePackageFiles(projectPath, results));
        snippets.AddRange(environment.GeneratePackageFiles(projectPath + DevSuffix, devResults));

        CheckDuplicateShortcuts(snippets, environment);
    }

    private static void CheckDuplicateShortcuts(IEnumerable<Snippet> snippets, SnippetEnvironment environment)
    {
        foreach (IGrouping<Language, Snippet> grouping in snippets
            .GroupBy(f => f.Language)
            .OrderBy(f => f.Key.GetIdentifier()))
        {
            Console.WriteLine($"checking duplicate shortcuts for '{environment.Kind.GetIdentifier()}.{grouping.Key.GetIdentifier()}'");

            foreach (DuplicateShortcutInfo info in SnippetUtility.FindDuplicateShortcuts(grouping))
            {
                if (info.Snippets.Any(f => !f.HasTag(KnownTags.NonUniqueShortcut)))
                {
                    Console.WriteLine($"DUPLICATE SHORTCUT: {info.Shortcut}");

                    foreach (Snippet item in info.Snippets)
                        Console.WriteLine($"  {item.FileNameWithoutExtension()}");
                }
            }
        }
    }

    private static void SaveChangedSnippets(SnippetDirectory[] directories)
    {
        var snippetEqualityComparer = new SnippetDeepEqualityComparer();

        foreach (SnippetDirectory directory in directories)
        {
            foreach (Snippet snippet in directory.EnumerateSnippets())
            {
                var clone = (Snippet)snippet.Clone();

                clone.SortCollections();

                clone.CodeText = _regexReplaceSpacesWithTabs.Replace(clone.CodeText, "\t");

                if (!snippetEqualityComparer.Equals(snippet, clone))
                    IOUtility.SaveSnippet(clone, onlyIfChanged: false);
            }
        }
    }
}
