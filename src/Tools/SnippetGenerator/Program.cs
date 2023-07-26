// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Pihrtsoft.Snippets;
using Pihrtsoft.Snippets.Comparers;
using Snippetica.CodeGeneration.VisualStudio;
using Snippetica.CodeGeneration.VisualStudioCode;
using Snippetica.IO;
using static Snippetica.KnownNames;
using static Snippetica.KnownPaths;

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

        ShortcutInfo[] shortcuts = Records.Document.ReadRecords(Path.Combine(dataDirectoryPath, "Shortcuts.xml"))
            .Where(f => !f.HasTag(KnownTags.Disabled))
            .Select(f => Mapper.MapShortcutInfo(f))
            .ToArray();

        SnippetDirectory[] directories = Records.Document.ReadRecords(Path.Combine(dataDirectoryPath, "Directories.xml"))
            .Where(f => !f.HasTag(KnownTags.Disabled))
            .Select(f => Mapper.MapSnippetDirectory(f, sourcePath))
            .ToArray();

        Dictionary<Language, LanguageDefinition> languageDefinitions = Mapper.LoadLanguages(dataDirectoryPath);

        SaveChangedSnippets(directories);

        GenerateSnippets(new VisualStudioEnvironment(), directories, languageDefinitions, VisualStudioExtensionProjectPath);

        GenerateSnippets(new VisualStudioCodeEnvironment(), directories, languageDefinitions, VisualStudioCodeExtensionProjectPath);

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
