// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using Snippetica.IO;
using Snippetica.VisualStudio;

namespace Snippetica.CodeGeneration.VisualStudio;

public class VisualStudioEnvironment : SnippetEnvironment
{
    public override EnvironmentKind Kind => EnvironmentKind.VisualStudio;

    protected override bool ShouldGenerateSnippets(SnippetDirectory directory)
    {
        return base.ShouldGenerateSnippets(directory)
            && !directory.HasTag(KnownTags.ExcludeFromVisualStudio);
    }

    protected override SnippetGenerator CreateSnippetGenerator(SnippetDirectory directory, Dictionary<Language, LanguageDefinition> languages)
    {
        return directory.Language switch
        {
            Language.VisualBasic => new VisualStudioSnippetGenerator(this, languages[Language.VisualBasic]),
            Language.CSharp => new VisualStudioSnippetGenerator(this, languages[Language.CSharp]),
            Language.Cpp => new VisualStudioSnippetGenerator(this, languages[Language.Cpp]),
            Language.Xaml => new XamlSnippetGenerator(),
            Language.Html => new HtmlSnippetGenerator(),
            _ => throw new ArgumentException("", nameof(directory)),
        };
    }

    public override bool IsSupportedLanguage(Language language)
    {
        switch (language)
        {
            case Language.VisualBasic:
            case Language.CSharp:
            case Language.Cpp:
            case Language.Xml:
            case Language.Xaml:
            case Language.JavaScript:
            case Language.Sql:
            case Language.Html:
            case Language.Css:
                return true;
            default:
                return false;
        }
    }

    public override string GetVersion(Language language)
    {
        throw new InvalidOperationException();
    }

    internal override IEnumerable<Snippet> PostProcess(IEnumerable<Snippet> snippets)
    {
        snippets = PostProcessCore(snippets);

        return base.PostProcess(snippets);
    }

    private static IEnumerable<Snippet> PostProcessCore(IEnumerable<Snippet> snippets)
    {
        foreach (Snippet snippet in snippets)
        {
            if (snippet.TryGetTag(KnownTags.ShortcutSuffix, out TagInfo info))
                snippet.Keywords.RemoveAt(info.KeywordIndex);

            Snippet obsoleteSnippet = GetObsoleteSnippetOrDefault(snippet);

            if (obsoleteSnippet is not null)
                yield return obsoleteSnippet;

            yield return snippet;
        }
    }

    private static Snippet GetObsoleteSnippetOrDefault(Snippet snippet)
    {
        if (!snippet.TryGetTag(KnownTags.ObsoleteShortcut, out TagInfo info))
            return null;

        snippet.Keywords.RemoveAt(info.KeywordIndex);

        snippet = (Snippet)snippet.Clone();

        string s = $"Shortcut '{info.Value}' is obsolete, use '{snippet.Shortcut}' instead.";

        if (snippet.Language == Language.CSharp)
        {
            s = $"/* {s} */";
        }
        else if (snippet.Language == Language.VisualBasic)
        {
            s = $"' {s}\r\n";
        }
        else
        {
            throw new NotSupportedException(snippet.Language.ToString());
        }

        snippet.Title += " [Obsolete]";

        snippet.Shortcut = info.Value;

        snippet.CodeText = s + $"${Placeholder.EndIdentifier}$";

        snippet.Literals.Clear();

        snippet.AddTag(KnownTags.ExcludeFromSnippetBrowser);
        snippet.AddTag(KnownTags.ExcludeFromDocs);

        snippet.SuffixFileName("_Obsolete");

        return snippet;
    }

    public override List<Snippet> GeneratePackageFiles(string directoryPath, IEnumerable<SnippetGeneratorResult> results)
    {
        List<Snippet> snippets = base.GeneratePackageFiles(directoryPath, results);

        IOUtility.WriteAllText(Path.Combine(directoryPath, "regedit.pkgdef"), PkgDefGenerator.GeneratePkgDefFile(results));

        return snippets;
    }

    protected override void SaveAllSnippets(string projectPath, List<Snippet> allSnippets)
    {
#if !DEBUG
        base.SaveAllSnippets(projectPath, allSnippets);
#endif

        string projectName = Path.GetFileName(projectPath);

        string csprojPath = Path.Combine(projectPath, $"{projectName}.{ProjectDocument.CSharpProjectExtension}");

        var document = new ProjectDocument(csprojPath);

        document.RemoveSnippetFiles();

        XElement newItemGroup = document.AddItemGroup();

        document.AddSnippetFiles(allSnippets.Select(f => f.FilePath), newItemGroup);

        document.Save();
    }
}
