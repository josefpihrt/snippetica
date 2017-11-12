// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Pihrtsoft.Records;
using Pihrtsoft.Snippets;
using Pihrtsoft.Snippets.Comparers;
using Snippetica.CodeGeneration.Markdown;
using Snippetica.CodeGeneration.VisualStudio;
using Snippetica.CodeGeneration.VisualStudioCode;
using Snippetica.IO;
using static Snippetica.KnownNames;
using static Snippetica.KnownPaths;

namespace Snippetica.CodeGeneration
{
    internal static class Program
    {
        private static readonly SnippetDeepEqualityComparer _snippetEqualityComparer = new SnippetDeepEqualityComparer();

        private static ShortcutInfo[] _shortcuts;

        private static readonly Regex _regexReplaceSpacesWithTabs = new Regex(@"(?<=^(\ {4})*)(?<x>\ {4})(?=(\ {4})*\S)", RegexOptions.Multiline);

        private static void Main(string[] args)
        {
            _shortcuts = ShortcutInfo.LoadFromFile(@"..\..\Data\Shortcuts.xml").ToArray();

            SnippetDirectory[] directories = LoadDirectories(@"..\..\Data\Directories.xml");

            ShortcutInfo.SerializeToXml(Path.Combine(VisualStudioExtensionProjectPath, "Shortcuts.xml"), _shortcuts);

            LoadLanguageDefinitions();

            SaveChangedSnippets(directories);

            var visualStudio = new VisualStudioEnvironment();

            (List<SnippetGeneratorResult> visualStudioResults, List<Snippet> visualStudioSnippets) = GenerateSnippets(
                visualStudio,
                directories,
                VisualStudioExtensionProjectPath);

            var visualStudioCode = new VisualStudioCodeEnvironment();

            (List<SnippetGeneratorResult> visualStudioCodeResults, List<Snippet> visualStudioCodeSnippets) = GenerateSnippets(
                visualStudioCode,
                directories,
                VisualStudioCodeExtensionProjectPath);

            CheckDuplicateShortcuts(visualStudioSnippets, visualStudio);
            CheckDuplicateShortcuts(visualStudioCodeSnippets, visualStudioCode);

            using (var sw = new StringWriter())
            {
                sw.WriteLine($"# {ProductName}");
                sw.WriteLine();

                IEnumerable<Language> languages = visualStudioResults
                    .Concat(visualStudioCodeResults)
                    .Select(f => f.Language).Distinct();

                sw.WriteLine($"* {CodeGenerationUtility.GetProjectSubtitle(languages)}");
                sw.WriteLine($"* [Release Notes]({MasterGitHubUrl}/{$"{ChangeLogFileName}"}).");
                sw.WriteLine();

                MarkdownGenerator.GenerateProjectReadme(visualStudioResults, sw, visualStudio.CreateProjectReadmeSettings());

                sw.WriteLine();

                MarkdownGenerator.GenerateProjectReadme(visualStudioCodeResults, sw, visualStudioCode.CreateProjectReadmeSettings());

                IOUtility.WriteAllText(Path.Combine(SolutionDirectoryPath, ReadMeFileName), sw.ToString(), IOUtility.UTF8NoBom);
            }

            Console.WriteLine("*** END ***");
            Console.ReadKey();
        }

        private static (List<SnippetGeneratorResult> results, List<Snippet> snippets) GenerateSnippets(
            SnippetEnvironment environment,
            SnippetDirectory[] directories,
            string projectPath)
        {
            environment.Shortcuts.AddRange(_shortcuts.Where(f => f.Environments.Contains(environment.Kind)));

            PackageGenerator generator = environment.CreatePackageGenerator();

            var results = new List<SnippetGeneratorResult>();
            var devResults = new List<SnippetGeneratorResult>();

            foreach (SnippetGeneratorResult result in environment.GenerateSnippets(directories))
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

            snippets.AddRange(generator.GeneratePackageFiles(projectPath, results));

            snippets.AddRange(generator.GeneratePackageFiles(projectPath + DevSuffix, devResults));

            MarkdownWriter.WriteProjectReadme(projectPath, results, environment.CreateProjectReadmeSettings());

            return (results, snippets);
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
            foreach (SnippetDirectory directory in directories)
            {
                foreach (Snippet snippet in directory.EnumerateSnippets())
                {
                    var clone = (Snippet)snippet.Clone();

                    clone.SortCollections();

                    clone.CodeText = _regexReplaceSpacesWithTabs.Replace(clone.CodeText, "\t");

                    if (!_snippetEqualityComparer.Equals(snippet, clone))
                        IOUtility.SaveSnippet(clone, onlyIfChanged: false);
                }
            }
        }

        private static SnippetDirectory[] LoadDirectories(string url)
        {
            return Document.ReadRecords(url)
                .Where(f => !f.HasTag(KnownTags.Disabled))
                .Select(SnippetDirectoryMapper.MapFromRecord)
                .ToArray();
        }

        private static void LoadLanguageDefinitions()
        {
            LanguageDefinition[] languageDefinitions = Document.ReadRecords(@"..\..\Data\Languages.xml")
                .Where(f => !f.HasTag(KnownTags.Disabled))
                .ToLanguageDefinitions()
                .ToArray();

            LanguageDefinition.CSharp = languageDefinitions.First(f => f.Language == Language.CSharp);
            LanguageDefinition.VisualBasic = languageDefinitions.First(f => f.Language == Language.VisualBasic);
        }
    }
}
