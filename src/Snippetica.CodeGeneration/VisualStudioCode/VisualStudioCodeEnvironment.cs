﻿// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Pihrtsoft.Snippets;
using Snippetica.CodeGeneration.Json.Package;
using Snippetica.CodeGeneration.Json;
using Snippetica.IO;
using static Snippetica.KnownNames;
using static Snippetica.KnownPaths;

namespace Snippetica.CodeGeneration.VisualStudioCode;

public class VisualStudioCodeEnvironment : SnippetEnvironment
{
    public override EnvironmentKind Kind => EnvironmentKind.VisualStudioCode;

    protected override bool ShouldGenerateSnippets(SnippetDirectory directory)
    {
        return base.ShouldGenerateSnippets(directory)
            && !directory.HasTag(KnownTags.ExcludeFromVisualStudioCode);
    }

    protected override SnippetGenerator CreateSnippetGenerator(SnippetDirectory directory, Dictionary<Language, LanguageDefinition> languages)
    {
        return directory.Language switch
        {
            Language.VisualBasic => new VisualStudioCodeSnippetGenerator(this, languages[Language.VisualBasic]),
            Language.CSharp => new VisualStudioCodeSnippetGenerator(this, languages[Language.CSharp]),
            Language.Cpp => new VisualStudioCodeSnippetGenerator(this, languages[Language.Cpp]),
            Language.Xaml => new XamlSnippetGenerator(),
            Language.Html => new HtmlSnippetGenerator(),
            _ => throw new ArgumentException("", nameof(directory)),
        };
    }

    public override bool IsSupportedLanguage(Language language)
    {
        switch (language)
        {
            case Language.CSharp:
            case Language.VisualBasic:
            case Language.Cpp:
            case Language.Xml:
            case Language.JavaScript:
            case Language.Sql:
            case Language.Html:
            case Language.Css:
            case Language.Json:
            case Language.Markdown:
                return true;
            default:
                return false;
        }
    }

    public override string GetVersion(Language language)
    {
        switch (language)
        {
            case Language.CSharp:
            case Language.VisualBasic:
            case Language.Cpp:
                return "1.0.0";
            case Language.Xml:
            case Language.JavaScript:
            case Language.Sql:
            case Language.Html:
            case Language.Css:
            case Language.Json:
            case Language.Markdown:
                return "0.6.0";
            default:
                throw new ArgumentException("", nameof(language));
        }
    }

    internal override IEnumerable<Snippet> PostProcess(IEnumerable<Snippet> snippets)
    {
        foreach (Snippet snippet in base.PostProcess(snippets))
        {
            if (snippet.HasTag(KnownTags.ExcludeFromVisualStudioCode))
                continue;

            LiteralCollection literals = snippet.Literals;

            for (int i = literals.Count - 1; i >= 0; i--)
            {
                Literal literal = literals[i];

                if (!literal.IsEditable
                    && !string.IsNullOrEmpty(literal.Function))
                {
                    literal.IsEditable = true;
                    literal.Function = null;
                }
            }

            if (snippet.HasTag(KnownTags.TitleStartsWithShortcut))
            {
                string shortcut = Regex.Match(snippet.Title, @"^\S+\s+").Value;

                snippet.Title = snippet.Title.Substring(shortcut.Length);

                shortcut = shortcut.TrimEnd();

                if (shortcut != "-")
                {
                    if (snippet.Shortcut.Last() != '_')
                        snippet.Shortcut += "_";

                    snippet.Shortcut += shortcut.TrimEnd();
                }

                snippet.RemoveTag(KnownTags.TitleStartsWithShortcut);
            }

            if (snippet.HasTag(KnownTags.NonUniqueShortcut))
            {
                if (snippet.TryGetTag(KnownTags.ShortcutSuffix, out TagInfo info))
                {
                    if (snippet.Shortcut.Last() != '_')
                        snippet.Shortcut += "_";

                    snippet.Shortcut += info.Value;

                    snippet.Keywords.RemoveAt(info.KeywordIndex);

                    snippet.AddTag(KnownTags.ExcludeFromReadme);
                }

                snippet.RemoveTag(KnownTags.NonUniqueShortcut);
            }

            yield return snippet;
        }
    }

    protected override void SaveSnippets(List<Snippet> snippets, SnippetGeneratorResult result)
    {
        base.SaveSnippets(snippets, result);

        if (!result.IsDevelopment)
        {
            IOUtility.SaveSnippetBrowserFile(
                snippets,
                Path.Combine(result.Path, $"{result.Language.GetIdentifier()}.xml"));
        }

        Language language = result.Language;

        string languageId = result.Language.GetIdentifier();

        string directoryPath = result.Path;

        string packageDirectoryPath = Path.Combine(directoryPath, "package");

        IOUtility.WriteAllText(
            Path.Combine(packageDirectoryPath, "snippets", Path.ChangeExtension(languageId, "json")),
            JsonUtility.ToJsonText(snippets.OrderBy(f => f.Title)),
            createDirectory: true);

        PackageInfo info = GetDefaultPackageInfo();
        info.Version = GetVersion(result.Language);
        info.Name += "-" + languageId;
        info.DisplayName += " for " + language.GetTitle();
        info.Description += language.GetTitle() + ".";
        info.Homepage += $"/{Path.GetFileName(directoryPath)}/{ReadMeFileName}";
        info.Keywords.AddRange(language.GetKeywords());
        info.Snippets.Add(new SnippetInfo() { Language = languageId, Path = $"./snippets/{languageId}.json" });

        IOUtility.WriteAllText(Path.Combine(packageDirectoryPath, "package.json"), info.ToString(), IOUtility.UTF8NoBom);

        DirectoryReadmeSettings settings = CreateDirectoryReadmeSettings(result);

#if !DEBUG
        MarkdownFileWriter.WriteDirectoryReadme(directoryPath, snippets, settings);
#endif

        settings.AddLinkToTitle = false;
        settings.Header = null;

#if !DEBUG
        MarkdownFileWriter.WriteDirectoryReadme(packageDirectoryPath, snippets, settings);
#endif
    }

    private static PackageInfo GetDefaultPackageInfo()
    {
        var info = new PackageInfo()
        {
            Name = "snippetica",
            Publisher = "josefpihrt-vscode",
            DisplayName = "Snippetica",
            Description = "A collection of snippets for ",
            Icon = "images/icon.png",
            Author = "Josef Pihrt",
            License = "Apache-2.0",
            Homepage = $"{SourceGitHubUrl}/{VisualStudioCodeExtensionProjectName}",
            Repository = new RepositoryInfo()
            {
                Type = "git",
                Url = $"{GitHubUrl}.git"
            },
            Bugs = new BugInfo() { Url = $"{GitHubUrl}/issues" },
            EngineVersion = "^1.0.0"
        };

        info.Categories.Add("Snippets");
        info.Keywords.Add("Snippet");
        info.Keywords.Add("Snippets");

        return info;
    }
}
