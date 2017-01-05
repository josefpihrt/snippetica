using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using Pihrtsoft.Records;
using Pihrtsoft.Snippets.CodeGeneration.Markdown;
using Pihrtsoft.Snippets.Mappings;
using Pihrtsoft.Snippets.Xml;

namespace Pihrtsoft.Snippets.CodeGeneration
{
    internal static class Program
    {
        private static void Main(string[] args)
        {
            var settings = new GeneralSettings();
            settings.SolutionDirectoryPath = @"..\..\..\..\..";

            SnippetDirectory[] snippetDirectories = LoadSnippetDirectories().ToArray();

            CharacterSequence[] characterSequences = LoadCharacterSequences(settings).ToArray();

            GenerateSnippets(snippetDirectories);
            GenerateHtmlSnippets(snippetDirectories);
            GenerateXamlSnippets(snippetDirectories);
            GenerateXmlSnippets(snippetDirectories);

            SnippetDirectory[] releaseDirectories = snippetDirectories
                .Where(f => f.HasTag(KnownTags.Release))
                .ToArray();

            MarkdownGenerator.WriteSolutionReadMe(releaseDirectories, settings);

            MarkdownGenerator.WriteProjectMarkdownFiles(releaseDirectories, Path.GetFullPath(settings.ProjectPath));

            MarkdownGenerator.WriteDirectoryMarkdownFiles(
                snippetDirectories
                    .Where(f => f.HasAnyTag(KnownTags.Release, KnownTags.Dev) && !f.HasAnyTag(KnownTags.AutoGenerationSource, KnownTags.AutoGenerationDestination))
                    .ToArray(),
                characterSequences);

            CopySnippetsToVisualStudioProject(settings.ExtensionProjectPath, releaseDirectories);

            releaseDirectories = releaseDirectories
                .Select(f => f.WithPath(Path.Combine(settings.ExtensionProjectPath, f.DirectoryName)))
                .ToArray();

            MarkdownGenerator.WriteChangeLog(releaseDirectories, Release.LoadFromDocument(@"..\..\ChangeLog.xml").ToArray(), settings);

            MarkdownGenerator.WriteProjectMarkdownFiles(releaseDirectories, settings.ExtensionProjectPath);

            MarkdownGenerator.WriteDirectoryMarkdownFiles(releaseDirectories, characterSequences);

            WriteVisualStudioGalleryDescription(releaseDirectories, settings);
            WritePkgDefFile(releaseDirectories, settings);

            CheckSnippets(snippetDirectories);

            Console.WriteLine("*** END ***");
            Console.ReadKey();
        }

        private static IEnumerable<SnippetDirectory> LoadSnippetDirectories()
        {
            return Document.Create(@"..\..\SnippetDirectories.xml")
                .ReadRecords()
                .Where(f => !f.HasTag(KnownTags.Disabled))
                .Select(record => SnippetDirectoryMapper.MapFromRecord(record));
        }

        private static IEnumerable<CharacterSequence> LoadCharacterSequences(GeneralSettings settings)
        {
            return Document.Create(@"..\..\CharacterSequences.xml")
                .ReadRecords()
                .Where(f => !f.HasTag(KnownTags.Disabled))
                .Select(record =>
                {
                    return new CharacterSequence(
                        record.GetString("Value"),
                        record.GetString("Description"),
                        record.GetStringOrDefault("Comment", "-"),
                        record.GetItems<string>("Languages").Select(f => settings.DirectoryNamePrefix + f),
                        record.Tags);
                });
        }

        private static void CheckSnippets(SnippetDirectory[] snippetDirectories)
        {
            foreach (IGrouping<Language, SnippetDirectory> grouping in snippetDirectories
                .Where(f => !f.HasAnyTag(KnownTags.AutoGenerationSource, KnownTags.AutoGenerationDestination))
                .GroupBy(f => f.Language))
            {
                Console.WriteLine();
                Console.WriteLine($"***** {grouping.Key} *****");

                SnippetDirectory[] directories = grouping.ToArray();

                SnippetChecker.CheckDuplicateShortcuts(directories);

                directories = directories
                    .Where(f => !f.HasTag(KnownTags.VisualStudio))
                    .ToArray();

                SnippetChecker.CheckSnippets(directories);
            }
        }

        public static void CopySnippetsToVisualStudioProject(string projectDirPath, IEnumerable<SnippetDirectory> snippetDirectories)
        {
            string projectName = Path.GetFileName(projectDirPath);

            string csprojPath = Path.Combine(projectDirPath, $"{projectName}.{ProjectDocument.CSharpProjectExtension}");

            var document = new ProjectDocument(csprojPath);

            document.RemoveSnippetFiles();

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
            }

            document.Save();
        }

        public static void RemoveAllTags(string projectDirPath, IEnumerable<SnippetDirectory> snippetDirectories)
        {
            foreach (SnippetDirectory snippetDirectory in snippetDirectories)
            {
                Snippet[] snippets = snippetDirectory.EnumerateSnippets().ToArray();

                foreach (Snippet snippet in snippets)
                    snippet.RemoveMetaKeywords();

                IOUtility.SaveSnippets(snippets, snippetDirectory.Path);
            }
        }

        public static void GenerateSnippets(SnippetDirectory[] snippetDirectories)
        {
            Document document = Document.Create(@"..\..\Records.xml");

            IEnumerable<Record> records = document
                .ReadRecords()
                .Where(f => !f.HasTag(KnownTags.Disabled));

            LanguageDefinition[] languages = records
                .Where(f => f.ContainsProperty(KnownTags.Language))
                .ToLanguageDefinitions()
                .ToArray();

            foreach (LanguageDefinition language in languages)
            {
                var settings = new SnippetGeneratorSettings(language);

                foreach (TypeDefinition typeInfo in records
                    .Where(f => f.HasTag(KnownTags.Collection))
                    .ToTypeDefinitions())
                {
                    settings.Types.Add(typeInfo);
                }

                language.GenerateSnippets(snippetDirectories, settings);
            }
        }

        public static void GenerateXamlSnippets(SnippetDirectory[] snippetDirectories)
        {
            IEnumerable<SnippetDirectory> directories = snippetDirectories
                .Where(f => f.Language == Language.Xaml);

            string sourceDirPath = directories.First(f => f.HasTag(KnownTags.AutoGenerationSource)).Path;
            string destinationDirPath = directories.First(f => f.HasTag(KnownTags.AutoGenerationDestination)).Path;

            var snippets = new List<Snippet>();

            snippets.AddRange(XmlSnippetGenerator.GenerateSnippets(destinationDirPath, Language.Xaml));

            var generator = new XamlSnippetGenerator();
            snippets.AddRange(generator.GenerateSnippets(sourceDirPath, destinationDirPath));

            IOUtility.SaveSnippets(snippets.ToArray(), destinationDirPath);
        }

        public static void GenerateXmlSnippets(SnippetDirectory[] snippetDirectories)
        {
            string destinationDirPath = snippetDirectories.First(f => f.Language == Language.Xml && f.HasTag(KnownTags.AutoGenerationDestination)).Path;

            Snippet[] snippets = XmlSnippetGenerator.GenerateSnippets(destinationDirPath, Language.Xml).ToArray();

            IOUtility.SaveSnippets(snippets, destinationDirPath);
        }

        public static void GenerateHtmlSnippets(SnippetDirectory[] snippetDirectories)
        {
            string sourceDirPath = snippetDirectories.First(f => f.Language == Language.Html && f.HasTag(KnownTags.AutoGenerationSource)).Path;
            string destinationDirPath = snippetDirectories.First(f => f.Language == Language.Html && f.HasTag(KnownTags.AutoGenerationDestination)).Path;

            var snippets = new List<Snippet>();
            snippets.AddRange(XmlSnippetGenerator.GenerateSnippets(destinationDirPath, Language.Html));
            snippets.AddRange(HtmlSnippetGenerator.GenerateSnippets(sourceDirPath));

            IOUtility.SaveSnippets(snippets.ToArray(), destinationDirPath);
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
                var xmlWriterSettings = new XmlWriterSettings();
                xmlWriterSettings.Indent = true;
                xmlWriterSettings.IndentChars = "  ";
                xmlWriterSettings.ConformanceLevel = ConformanceLevel.Fragment;

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

                    x.WriteEndElement();

                    x.WriteElementString("h3", "List of Snippets");
                    x.WriteStartElement("ul");

                    foreach (SnippetDirectory snippetDirectory in snippetDirectories)
                    {
                        string directoryName = Path.GetFileName(snippetDirectory.Path);

                        x.WriteStartElement("li");
                        x.WriteStartElement("a");
                        x.WriteAttributeString("href", $"{settings.GitHubSourcePath}/{settings.ExtensionProjectName}/{directoryName}/README.md");
                        x.WriteString($"{directoryName} ({snippetDirectory.EnumerateSnippets().Count()} snippets)");
                        x.WriteEndElement();
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
                        Debug.Assert(false, language.ToString());
                        throw new NotSupportedException();
                    }
            }
        }
    }
}
