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
    private static ShortcutInfo[] _shortcuts;

    //TODO: data directory
    private static void Main(string[] args)
    {
        _shortcuts = Records.Document.ReadRecords(@"..\..\..\Data\Shortcuts.xml")
            .Where(f => !f.HasTag(KnownTags.Disabled))
            .Select(f => Mapper.MapShortcutInfo(f))
            .ToArray();

        SnippetDirectory[] directories = LoadDirectories(@"..\..\..\Data\Directories.xml");

        ShortcutInfo.SerializeToXml(Path.Combine(VisualStudioExtensionProjectPath, "Shortcuts.xml"), _shortcuts);

        LoadLanguages();

        var visualStudio = new VisualStudioEnvironment();

        List<SnippetGeneratorResult> visualStudioResults = GenerateSnippets(
            visualStudio,
            directories,
            VisualStudioExtensionProjectPath);

        var visualStudioCode = new VisualStudioCodeEnvironment();

        List<SnippetGeneratorResult> visualStudioCodeResults = GenerateSnippets(
            visualStudioCode,
            directories,
            VisualStudioCodeExtensionProjectPath);

        Console.WriteLine("DONE");
    }

    private static List<SnippetGeneratorResult> GenerateSnippets(
        SnippetEnvironment environment,
        SnippetDirectory[] directories,
        string projectPath)
    {
        environment.Shortcuts.AddRange(_shortcuts.Where(f => f.Environments.Contains(environment.Kind)));

        List<SnippetGeneratorResult> results = environment.GenerateSnippets(directories, includeDevelopment: false).ToList();

        MarkdownFileWriter.WriteProjectReadme(projectPath, results, environment.CreateProjectReadmeSettings());

        return results;
    }

    private static SnippetDirectory[] LoadDirectories(string url)
    {
        return Records.Document.ReadRecords(url)
            .Where(f => !f.HasTag(KnownTags.Disabled))
            .Select(f => Mapper.MapSnippetDirectory(f))
            .ToArray();
    }

    private static void LoadLanguages()
    {
        Records.Document.ReadRecords(@"..\..\..\Data\Languages.xml")
            .Where(f => !f.HasTag(KnownTags.Disabled))
            .LoadLanguages();

        foreach (TypeDefinition typeDefinition in Records.Document.ReadRecords(@"..\..\..\Data\Types.xml")
            .Where(f => !f.HasTag(KnownTags.Disabled))
            .Select(f => Mapper.MapTypeDefinition(f)))
        {
            LanguageDefinitions.CSharp.Types.Add(typeDefinition);
            LanguageDefinitions.VisualBasic.Types.Add(typeDefinition);
        }
    }
}
