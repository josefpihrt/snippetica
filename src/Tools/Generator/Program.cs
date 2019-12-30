// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using DotMarkdown;
using DotMarkdown.Linq;
using Pihrtsoft.Records;
using Pihrtsoft.Snippets;
using Pihrtsoft.Snippets.Comparers;
using Snippetica.CodeGeneration.Markdown;
using Snippetica.CodeGeneration.VisualStudio;
using Snippetica.CodeGeneration.VisualStudioCode;
using Snippetica.IO;
using static DotMarkdown.Linq.MFactory;
using static Snippetica.KnownNames;
using static Snippetica.KnownPaths;

namespace Snippetica.CodeGeneration
{
    internal static class Program
    {
        private static readonly SnippetDeepEqualityComparer _snippetEqualityComparer = new SnippetDeepEqualityComparer();

        private static ShortcutInfo[] _shortcuts;

        private static readonly Regex _regexReplaceSpacesWithTabs = new Regex(@"(?<=^(\ {4})*)(?<x>\ {4})(?=(\ {4})*\S)", RegexOptions.Multiline);

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Redundancy", "RCS1163:Unused parameter.")]
        private static void Main(string[] args)
        {
            _shortcuts = Pihrtsoft.Records.Document.ReadRecords(@"..\..\Data\Shortcuts.xml")
                .Where(f => !f.HasTag(KnownTags.Disabled))
                .Select(Mapper.MapShortcutInfo)
                .ToArray();

            SnippetDirectory[] directories = LoadDirectories(@"..\..\Data\Directories.xml");

            ShortcutInfo.SerializeToXml(Path.Combine(VisualStudioExtensionProjectPath, "Shortcuts.xml"), _shortcuts);

            LoadLanguages();

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

            IEnumerable<Language> languages = visualStudioResults
                .Concat(visualStudioCodeResults)
                .Select(f => f.Language).Distinct();

            var document = new MDocument(
                Heading1(ProductName),
                BulletList(
                    CodeGenerationUtility.GetProjectSubtitle(languages),
                    BulletItem(Link("Release Notes", $"{MasterGitHubUrl}/{ChangeLogFileName}"), ".")));

#if !DEBUG
            MarkdownGenerator.GenerateProjectReadme(visualStudioResults, document, visualStudio.CreateProjectReadmeSettings(), addFootnote: false);

            MarkdownGenerator.GenerateProjectReadme(visualStudioCodeResults, document, visualStudioCode.CreateProjectReadmeSettings());

            IOUtility.WriteAllText(Path.Combine(SolutionDirectoryPath, ReadMeFileName), document.ToString(MarkdownFormat.Default.WithTableOptions(TableOptions.FormatHeader)), IOUtility.UTF8NoBom);
#endif

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

#if !DEBUG
            MarkdownFileWriter.WriteProjectReadme(projectPath, results, environment.CreateProjectReadmeSettings());
#endif

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
            return Pihrtsoft.Records.Document.ReadRecords(url)
                .Where(f => !f.HasTag(KnownTags.Disabled))
                .Select(Mapper.MapSnippetDirectory)
                .ToArray();
        }

        private static void LoadLanguages()
        {
            Pihrtsoft.Records.Document.ReadRecords(@"..\..\Data\Languages.xml")
                .Where(f => !f.HasTag(KnownTags.Disabled))
                .LoadLanguages();

            foreach (TypeDefinition typeDefinition in Pihrtsoft.Records.Document.ReadRecords(@"..\..\Data\Types.xml")
                .Where(f => !f.HasTag(KnownTags.Disabled))
                .Select(Mapper.MapTypeDefinition))
            {
                LanguageDefinitions.CSharp.Types.Add(typeDefinition);
                LanguageDefinitions.VisualBasic.Types.Add(typeDefinition);
            }
        }
    }
}
