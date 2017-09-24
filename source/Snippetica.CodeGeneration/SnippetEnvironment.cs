// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Pihrtsoft.Snippets;
using Snippetica.CodeGeneration.Markdown;

namespace Snippetica.CodeGeneration
{
    public abstract class SnippetEnvironment
    {
        public abstract EnvironmentKind Kind { get; }

        public List<ShortcutInfo> Shortcuts { get; } = new List<ShortcutInfo>();

        public IEnumerable<SnippetGeneratorResult> GenerateSnippets(IEnumerable<SnippetDirectory> directories)
        {
            foreach (SnippetDirectory directory in directories)
            {
                foreach (SnippetGeneratorResult result in GenerateSnippets(directory))
                    yield return result;
            }
        }

        public IEnumerable<SnippetGeneratorResult> GenerateSnippets(SnippetDirectory directory)
        {
            if (!ShouldGenerateSnippets(directory))
                yield break;

            yield return new SnippetGeneratorResult(
                GenerateSnippetsCore(directory),
                directory.Name,
                directory.Language,
                isDevelopment: false,
                tags: directory.Tags.ToArray());

            string devPath = Path.Combine(directory.Path, KnownNames.Dev);

            if (Directory.Exists(devPath))
            {
                SnippetDirectory devDirectory = directory.WithPath(devPath);

                yield return new SnippetGeneratorResult(
                    GenerateSnippetsCore(devDirectory, isDevelopment: true),
                    name: directory.Path + KnownNames.DevSuffix,
                    language: directory.Language,
                    isDevelopment: true,
                    tags: directory.Tags.ToArray());
            }
        }

        private List<Snippet> GenerateSnippetsCore(SnippetDirectory directory, bool isDevelopment = false)
        {
            var snippets = new List<Snippet>();

            snippets.AddRange(EnumerateSnippets(directory.Path));

            snippets.AddRange(SnippetGenerator.GenerateAlternativeShortcuts(snippets));

            if (!isDevelopment
                && directory.HasTag(KnownTags.GenerateXmlSnippets))
            {
                switch (directory.Language)
                {
                    case Language.Xml:
                    case Language.Xaml:
                    case Language.Html:
                    case Language.Markdown:
                        {
                            snippets.AddRange(XmlSnippetGenerator.GenerateSnippets(directory.Language));
                            break;
                        }
                }
            }

            string autoGenerationPath = Path.Combine(directory.Path, KnownNames.AutoGeneration);

            if (Directory.Exists(autoGenerationPath))
            {
                SnippetDirectory autoGenerationDirectory = directory.WithPath(autoGenerationPath);

                SnippetGenerator generator = CreateSnippetGenerator(autoGenerationDirectory);

                snippets.AddRange(generator.GenerateSnippets(autoGenerationDirectory.Path));
            }

            return snippets;
        }

        private static IEnumerable<Snippet> EnumerateSnippets(string directoryPath)
        {
            foreach (string path in Directory.EnumerateDirectories(directoryPath, "*", SearchOption.TopDirectoryOnly))
            {
                string name = Path.GetFileName(path);

                if (name == KnownNames.Dev)
                    continue;

                if (name == KnownNames.AutoGeneration)
                    continue;

                foreach (Snippet snippet in SnippetSerializer.Deserialize(path, SearchOption.AllDirectories))
                {
                    yield return snippet;
                }
            }

            foreach (string filePath in Directory.EnumerateFiles(directoryPath, SnippetFileSearcher.Pattern, SearchOption.TopDirectoryOnly))
            {
                foreach (Snippet snippet in SnippetSerializer.DeserializeFile(filePath).Snippets)
                {
                    yield return snippet;
                }
            }
        }

        protected virtual bool ShouldGenerateSnippets(SnippetDirectory directory)
        {
            return IsSupportedLanguage(directory.Language);
        }

        public virtual DirectoryReadmeSettings CreateDirectoryReadmeSettings(SnippetGeneratorResult result)
        {
            var settings = new DirectoryReadmeSettings()
            {
                Environment = this,
                IsDevelopment = result.IsDevelopment,
                Header = result.DirectoryName,
                AddLinkToTitle = true,
                AddQuickReference = !result.IsDevelopment && !result.HasTag(KnownTags.NoQuickReference),
                Language = result.Language,
                DirectoryPath = result.Path,
                GroupShortcuts = true
            };

            if (!settings.IsDevelopment)
            {
                //TODO: 
                string filePath = $@"..\..\..\..\..\text\{result.DirectoryName}.md";

                if (File.Exists(filePath))
                    settings.QuickReferenceText = File.ReadAllText(filePath, Encoding.UTF8);

                settings.Shortcuts.AddRange(Shortcuts);
            }

            return settings;
        }

        public virtual ProjectReadmeSettings CreateProjectReadmeSettings()
        {
            return new ProjectReadmeSettings()
            {
                Environment = this,
                Header = $"{KnownNames.ProductName} for {Kind.GetTitle()}"
            };
        }

        protected abstract SnippetGenerator CreateSnippetGenerator(SnippetDirectory directory);

        public abstract PackageGenerator CreatePackageGenerator();

        public abstract bool IsSupportedLanguage(Language language);
    }
}
