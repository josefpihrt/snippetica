// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Snippetica.IO;
using Snippetica.Validations;
using Snippetica.VisualStudio;

namespace Snippetica.CodeGeneration;

public abstract class SnippetEnvironment
{
    public abstract EnvironmentKind Kind { get; }

    public List<ShortcutInfo> Shortcuts { get; } = new();

    public Dictionary<Language, LanguageDefinition> LanguageDefinitions { get; } = new();

    public IEnumerable<SnippetGeneratorResult> GenerateSnippets(
        IEnumerable<SnippetDirectory> directories,
        Dictionary<Language, LanguageDefinition> languages,
        bool includeDevelopment)
    {
        return directories.SelectMany(directory => GenerateSnippets(directory, languages, includeDevelopment)
            .Select(result =>
            {
                List<Snippet> snippets = PostProcess(result.Snippets).ToList();

                result.Snippets.Clear();
                result.Snippets.AddRange(snippets);

                return result;
            }));
    }

    private IEnumerable<SnippetGeneratorResult> GenerateSnippets(
        SnippetDirectory directory,
        Dictionary<Language, LanguageDefinition> languages,
        bool includeDevelopment)
    {
        if (!ShouldGenerateSnippets(directory))
            yield break;

        yield return new SnippetGeneratorResult(
            GenerateSnippetsCore(directory, languages, isDevelopment: false),
            this,
            directory.Name,
            directory.Language,
            isDevelopment: false,
            tags: directory.Tags.ToArray());

        if (includeDevelopment)
        {
            string devPath = Path.Combine(directory.Path, KnownNames.Dev);

            if (!Directory.Exists(devPath))
                yield break;

            SnippetDirectory devDirectory = directory.WithPath(devPath);

            List<Snippet> snippets = GenerateSnippetsCore(devDirectory, languages, isDevelopment: true);

            snippets = PostProcess(snippets).ToList();

            yield return new SnippetGeneratorResult(
                snippets,
                this,
                name: directory.Path + KnownNames.DevSuffix,
                language: directory.Language,
                isDevelopment: true,
                tags: directory.Tags.ToArray());
        }
    }

    private List<Snippet> GenerateSnippetsCore(
        SnippetDirectory directory,
        Dictionary<Language, LanguageDefinition> languages,
        bool isDevelopment = false)
    {
        var snippets = new List<Snippet>();

        snippets.AddRange(EnumerateSnippets(directory.Path));

#if DEBUG
        foreach (Snippet snippet in snippets)
        {
            foreach (string keyword in snippet.Keywords)
            {
                if (keyword.StartsWith(KnownTags.MetaPrefix + KnownTags.GeneratePrefix, StringComparison.OrdinalIgnoreCase))
                {
                    Debug.Fail(keyword + "\r\n" + snippet.GetFilePath());
                    break;
                }
            }
        }
#endif

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

        if (languages.TryGetValue(directory.Language, out LanguageDefinition language))
        {
            foreach (KeywordDefinition keyword in language.Keywords)
            {
                if (keyword.IsDevelopment == isDevelopment)
                {
                    Snippet snippet = keyword.ToSnippet();
                    snippet.Language = directory.Language;
                    snippets.Add(snippet);
                }
            }
        }

        string autoGenerationPath = Path.Combine(directory.Path, KnownNames.AutoGeneration);

        if (Directory.Exists(autoGenerationPath))
        {
            SnippetDirectory autoGenerationDirectory = directory.WithPath(autoGenerationPath);

            SnippetGenerator generator = CreateSnippetGenerator(autoGenerationDirectory, languages);

            snippets.AddRange(generator.GenerateSnippets(autoGenerationDirectory.Path));
        }

        return snippets;
    }

    private static IEnumerable<Snippet> EnumerateSnippets(string directoryPath)
    {
        if (!Directory.Exists(directoryPath))
            yield break;

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

    public DirectoryReadmeSettings CreateSnippetsMarkdownSettings(SnippetGeneratorResult result)
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
            GroupShortcuts = false
        };

        if (!settings.IsDevelopment)
            settings.Shortcuts.AddRange(Shortcuts);

        return settings;
    }

    public ProjectReadmeSettings CreateEnvironmentMarkdownSettings()
    {
        return new ProjectReadmeSettings()
        {
            Environment = this,
            Header = $"{KnownNames.ProductName} for {Kind.GetTitle()}"
        };
    }

    protected abstract SnippetGenerator CreateSnippetGenerator(SnippetDirectory directory, Dictionary<Language, LanguageDefinition> languages);

    public abstract bool IsSupportedLanguage(Language language);

    public abstract string GetVersion(Language language);

    internal virtual IEnumerable<Snippet> PostProcess(IEnumerable<Snippet> snippets)
    {
        foreach (Snippet snippet in snippets)
        {
            for (int i = snippet.Literals.Count - 1; i >= 0; i--)
            {
                SnippetLiteral literal = snippet.Literals[i];

                if (!literal.IsEditable
                    && !string.Equals(literal.Identifier, XmlSnippetGenerator.CDataIdentifier, StringComparison.Ordinal))
                {
                    if (string.IsNullOrEmpty(literal.DefaultValue))
                    {
                        snippet.RemoveLiteralAndPlaceholders(literal);
                    }
                    else if (string.IsNullOrEmpty(literal.Function))
                    {
                        snippet.RemoveLiteralAndReplacePlaceholders(literal.Identifier, literal.DefaultValue);
                    }
                }
                else if (!snippet.Code.Placeholders.Contains(literal.Identifier))
                {
                    snippet.Literals.Remove(literal);
                }
            }

            if (snippet.TryGetTag(KnownTags.Environment, out TagInfo info))
            {
                if (string.Equals(info.Value, Kind.GetIdentifier()))
                {
                    snippet.Keywords.RemoveAt(info.KeywordIndex);
                }
                else
                {
                    continue;
                }
            }

            if (snippet.TryGetTag(KnownTags.ObsoleteShortcut, out info))
                snippet.Keywords.RemoveAt(info.KeywordIndex);

            if (snippet.HasTag(KnownTags.NonUniqueTitle))
            {
                snippet.Title += " _";
                snippet.RemoveTag(KnownTags.NonUniqueTitle);
                snippet.AddTag(KnownTags.TitleEndsWithUnderscore);
            }

            snippet.SortCollections();

            snippet.Author = "Josef Pihrt";

            if (snippet.SnippetTypes == SnippetTypes.None)
                snippet.SnippetTypes = SnippetTypes.Expansion;

            yield return snippet;
        }
    }

    public virtual List<Snippet> GeneratePackageFiles(
        string directoryPath,
        IEnumerable<SnippetGeneratorResult> results)
    {
        var allSnippets = new List<Snippet>();

        foreach (SnippetGeneratorResult result in results)
        {
            ValidateSnippets(result.Snippets);

            result.Path = Path.Combine(directoryPath, result.DirectoryName);

            SaveSnippets(result);

            allSnippets.AddRange(result.Snippets);
        }

#if !DEBUG
        SaveAllSnippets(directoryPath, allSnippets);
#endif

        return allSnippets;
    }

    protected virtual void SaveSnippets(SnippetGeneratorResult result)
    {
        IOUtility.SaveSnippets(result.Snippets, result.Path);
    }

    protected virtual void SaveAllSnippets(string projectPath, List<Snippet> allSnippets)
    {
    }

    protected virtual void ValidateSnippets(List<Snippet> snippets)
    {
        Validator.ValidateSnippets(snippets);

        Validator.ThrowOnDuplicateFileName(snippets);
    }
}
