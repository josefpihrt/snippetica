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
        GenerateSnippets(new VisualStudioEnvironment(), directories, languageDefinitions, Path.Combine(sourcePath, "Snippetica.VisualStudio.Vsix"));
        GenerateSnippets(new VisualStudioCodeEnvironment(), directories, languageDefinitions, Path.Combine(sourcePath, "Snippetica.VisualStudioCode.Vsix"));

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

            foreach ((string shortcut, List<Snippet> snippets2) in FindDuplicateShortcuts(grouping))
            {
                if (snippets2.Any(f => !f.HasTag(KnownTags.NonUniqueShortcut)))
                {
                    Console.WriteLine($"DUPLICATE SHORTCUT: {shortcut}");

                    foreach (Snippet item in snippets2)
                        Console.WriteLine($"  {item.GetFileNameWithoutExtension()}");
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

    private static IEnumerable<(string, List<Snippet>)> FindDuplicateShortcuts(IEnumerable<Snippet> snippets)
    {
        if (snippets is null)
            throw new ArgumentNullException(nameof(snippets));

        return FindDuplicateShortcuts();

        IEnumerable<(string, List<Snippet>)> FindDuplicateShortcuts()
        {
            foreach (var grouping in snippets
                .SelectMany(snippet => snippet.Shortcuts()
                    .Select(shortcut => new { Shortcut = shortcut, Snippet = snippet }))
                .GroupBy(f => f.Snippet, SnippetComparer.Shortcut))
            {
                if (grouping.CountExceeds(1))
                    yield return (grouping.Key.Shortcut, grouping.Select(f => f.Snippet).ToList());
            }
        }
    }
}
