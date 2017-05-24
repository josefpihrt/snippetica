// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using Pihrtsoft.Snippets.CodeGeneration.Markdown;
using Pihrtsoft.Snippets.Xml;

namespace Pihrtsoft.Snippets.CodeGeneration
{
    internal static class SnippetPackageGenerator
    {
        public static void GenerateVisualStudioPackageFiles(
            SnippetDirectory[] releaseDirectories,
            CharacterSequence[] characterSequences,
            Release[] releases,
            GeneralSettings settings)
        {
            CopySnippetsToVisualStudioProject(settings.ExtensionProjectPath, releaseDirectories);

            releaseDirectories = releaseDirectories
                .Select(f => f.WithPath(Path.Combine(settings.ExtensionProjectPath, f.DirectoryName)))
                .ToArray();

            MarkdownGenerator.WriteProjectReadMe(releaseDirectories, settings.ExtensionProjectPath);

            MarkdownGenerator.WriteDirectoryReadMe(releaseDirectories, characterSequences, settings);

            WriteVisualStudioGalleryDescription(releaseDirectories, settings);
            WritePkgDefFile(releaseDirectories, settings);
        }

        public static void CopySnippetsToVisualStudioProject(string projectDirPath, IEnumerable<SnippetDirectory> snippetDirectories)
        {
            string projectName = Path.GetFileName(projectDirPath);

            string csprojPath = Path.Combine(projectDirPath, $"{projectName}.{ProjectDocument.CSharpProjectExtension}");

            var document = new ProjectDocument(csprojPath);

            document.RemoveSnippetFiles();

#if RELEASE
            var allSnippets = new List<Snippet>();
#endif

            XElement newItemGroup = document.AddItemGroup();

            foreach (SnippetDirectory snippetDirectory in snippetDirectories)
            {
                string directoryPath = Path.Combine(projectDirPath, snippetDirectory.DirectoryName);

                Directory.CreateDirectory(directoryPath);

                Snippet[] snippets = snippetDirectory.EnumerateSnippets().ToArray();

                foreach (IGrouping<string, Snippet> grouping in snippets
                    .GroupBy(f => Path.GetFileNameWithoutExtension(f.FilePath))
                    .Where(f => f.Count() > 1))
                {
                    throw new Exception($"multiple files with same name '{grouping.Key}'");
                }

                IOUtility.SaveSnippets(snippets, directoryPath);

                document.AddSnippetFiles(snippets.Select(f => f.FilePath), newItemGroup);

#if RELEASE
                allSnippets.AddRange(snippets);
#endif
            }

            document.Save();

#if RELEASE
            foreach (Snippet snippet in allSnippets)
            {
                string submenuShortcut = snippet.GetSubmenuShortcut();

                snippet.RemoveShortcutFromTitle();

                snippet.RemoveMetaKeywords();
                snippet.Keywords.Add($"{KnownTags.MetaTagPrefix}Name:{snippet.FileNameWithoutExtension()}");

                if (!string.IsNullOrEmpty(submenuShortcut))
                    snippet.Keywords.Add($"{KnownTags.MetaTagPrefix}SubmenuShortcut:{submenuShortcut}");
            }

            IOUtility.SaveSnippetsToSingleFile(
                allSnippets
                    .Where(f => !f.HasTag(KnownTags.ExcludeFromReadme))
                    .OrderBy(f => f.Language.ToString())
                    .ThenBy(f => f.FileNameWithoutExtension()),
                Path.Combine(projectDirPath, "snippets.xml"));
#endif
        }

        public static void WriteVisualStudioGalleryDescription(SnippetDirectory[] snippetDirectories, GeneralSettings settings)
        {
            IOUtility.WriteAllText(
                Path.Combine(settings.ExtensionProjectPath, settings.GalleryDescriptionFileName),
                GenerateVisualStudioGalleryDescription(snippetDirectories, settings));
        }

        public static string GenerateVisualStudioGalleryDescription(SnippetDirectory[] snippetDirectories, GeneralSettings settings)
        {
            using (var sw = new StringWriter())
            {
                var xmlWriterSettings = new XmlWriterSettings()
                {
                    Indent = true,
                    IndentChars = "  ",
                    ConformanceLevel = ConformanceLevel.Fragment
                };

                using (XmlWriter x = XmlWriter.Create(sw, xmlWriterSettings))
                {
                    x.WriteElementString("h3", settings.ProjectTitle);
                    x.WriteElementString("p", settings.GetProjectSubtitle(snippetDirectories));

                    x.WriteElementString("h3", "Links");

                    x.WriteStartElement("ul");

                    x.WriteStartElement("li");
                    x.WriteStartElement("a");
                    x.WriteAttributeString("href", settings.GitHubPath);
                    x.WriteString("Project Website");
                    x.WriteEndElement();
                    x.WriteEndElement();

                    x.WriteStartElement("li");
                    x.WriteStartElement("a");
                    x.WriteAttributeString("href", $"{settings.GitHubMasterPath}/{settings.ChangeLogFileName}");
                    x.WriteString("Release Notes");
                    x.WriteEndElement();
                    x.WriteEndElement();

                    x.WriteStartElement("li");
                    x.WriteStartElement("a");
                    x.WriteAttributeString("href", "http://pihrt.net/Snippetica/Snippets");
                    x.WriteString("Browse and Search All Snippets");
                    x.WriteEndElement();
                    x.WriteEndElement();

                    x.WriteEndElement();

                    x.WriteElementString("h3", "Snippets");
                    x.WriteStartElement("ul");

                    foreach (SnippetDirectory snippetDirectory in snippetDirectories)
                    {
                        string directoryName = Path.GetFileName(snippetDirectory.Path);

                        x.WriteStartElement("li");

                        x.WriteStartElement("a");
                        x.WriteAttributeString("href", $"{settings.GitHubSourcePath}/{settings.ExtensionProjectName}/{directoryName}/README.md");
                        x.WriteString(directoryName);
                        x.WriteEndElement();
                        x.WriteString($" ({snippetDirectory.EnumerateSnippets().Count()} snippets)");

                        x.WriteString(" (");
                        x.WriteStartElement("a");
                        x.WriteAttributeString("href", $"http://pihrt.net/Snippetica/Snippets?Language={snippetDirectory.Language}");
                        x.WriteString("full list");
                        x.WriteEndElement();
                        x.WriteString(")");

                        x.WriteEndElement();
                    }

                    x.WriteEndElement();
                }

                return sw.ToString();
            }
        }

        public static void WritePkgDefFile(SnippetDirectory[] snippetDirectories, GeneralSettings settings)
        {
            IOUtility.WriteAllText(
                Path.Combine(settings.ExtensionProjectPath, settings.PkgDefFileName),
                GeneratePkgDefFile(snippetDirectories));
        }

        public static string GeneratePkgDefFile(SnippetDirectory[] snippetDirectories)
        {
            using (var sw = new StringWriter())
            {
                foreach (IGrouping<Language, SnippetDirectory> grouping in snippetDirectories.GroupBy(f => f.Language))
                {
                    sw.WriteLine($"// {LanguageHelper.GetLanguageTitle(grouping.Key)}");

                    foreach (SnippetDirectory snippetDirectory in grouping)
                    {
                        sw.WriteLine($@"[$RootKey$\Languages\CodeExpansions\{GetRegistryCode(snippetDirectory.Language)}\Paths]");
                        sw.WriteLine($"\"{snippetDirectory.DirectoryName}\" = \"$PackageFolder$\\{snippetDirectory.DirectoryName}\"");
                    }

                    sw.WriteLine();
                }

                return sw.ToString();
            }
        }

        private static string GetRegistryCode(Language language)
        {
            switch (language)
            {
                case Language.VisualBasic:
                    return "Basic";
                case Language.CSharp:
                    return "CSharp";
                case Language.CPlusPlus:
                    return "C/C++";
                case Language.Xml:
                    return "XML";
                case Language.Xaml:
                    return "XAML";
                case Language.JavaScript:
                    return "JavaScript";
                case Language.Sql:
                    return "SQL_SSDT";
                case Language.Html:
                    return "HTML";
                case Language.Css:
                    return "CSS";
                default:
                    {
                        Debug.Fail(language.ToString());
                        throw new NotSupportedException();
                    }
            }
        }
    }
}
