// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Pihrtsoft.Snippets;

namespace Snippetica.IO;

public static class IOUtility
{
    private static readonly StringComparer _stringComparer = StringComparer.OrdinalIgnoreCase;

    public static Encoding UTF8NoBom { get; } = new UTF8Encoding(encoderShouldEmitUTF8Identifier: false);

    public static void SaveSnippets(IEnumerable<Snippet> snippets, string directoryPath)
    {
        Console.WriteLine($"saving snippets to {directoryPath}");

        if (!Directory.Exists(directoryPath))
            Directory.CreateDirectory(directoryPath);

        var filePaths = new HashSet<string>(Directory.GetFiles(directoryPath, "*.snippet", SearchOption.TopDirectoryOnly), _stringComparer);

        foreach (Snippet snippet in snippets)
        {
            snippet.FilePath = Path.Combine(directoryPath, Path.GetFileName(snippet.FilePath));

            SaveSnippet(snippet);

            filePaths.Remove(snippet.FilePath);
        }

        foreach (string path in filePaths)
            DeleteFile(path);
    }

    public static void SaveSnippet(Snippet snippet, bool onlyIfChanged = true)
    {
        SaveSnippet(snippet, snippet.FilePath, onlyIfChanged);
    }

    public static void SaveSnippet(Snippet snippet, string filePath, bool onlyIfChanged = true)
    {
        if (snippet is null)
            throw new ArgumentNullException(nameof(snippet));

        SaveSettings settings = CreateSaveSettings();

        if (!ShouldSave(snippet, filePath, settings, onlyIfChanged))
            return;

        using (var fileStream = new FileStream(filePath, FileMode.Create))
        {
            Console.WriteLine($"saving file {filePath}");
            SnippetSerializer.Serialize(fileStream, snippet, settings);
        }
    }

    private static bool ShouldSave(Snippet snippet, string filePath, SaveSettings settings, bool onlyIfChanged)
    {
        if (!onlyIfChanged)
            return true;

        if (!File.Exists(filePath))
            return true;

        string s1 = File.ReadAllText(filePath, Encoding.UTF8);
        string s2 = SnippetSerializer.CreateXml(snippet, settings);

        return !string.Equals(s1, s2, StringComparison.Ordinal);
    }

    private static SaveSettings CreateSaveSettings()
    {
        return new SaveSettings()
        {
            OmitXmlDeclaration = true,
            OmitCodeSnippetsElement = true,
            IndentChars = "  ",
            Comment = "Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0."
        };
    }

    public static void SaveSnippetsToSingleFile(
        IEnumerable<Snippet> snippets,
        string filePath,
        bool onlyIfChanged = true)
    {
        if (snippets is null)
            throw new ArgumentNullException(nameof(snippets));

        SaveSettings settings = CreateSaveSettings();

        string content = SnippetSerializer.CreateXml(snippets, settings);

        if (!ShouldSave(filePath, content, Encoding.UTF8, onlyIfChanged))
            return;

        using (var fileStream = new FileStream(filePath, FileMode.Create))
        {
            Console.WriteLine($"saving file {filePath}");
            SnippetSerializer.Serialize(fileStream, snippets, settings);
        }
    }

    public static void SaveSnippetBrowserFile(IEnumerable<Snippet> snippets, string filePath)
    {
        snippets = snippets
            .Where(f => !f.HasTag(KnownTags.ExcludeFromSnippetBrowser))
            .Select(snippet =>
            {
                snippet = (Snippet)snippet.Clone();

                string submenuShortcut = snippet.GetShortcutFromTitle();

                snippet.RemoveShortcutFromTitle();

                snippet.RemoveMetaKeywords();

                snippet.Keywords.Add($"{KnownTags.MetaPrefix}Name {snippet.FileNameWithoutExtension()}");

                if (!string.IsNullOrEmpty(submenuShortcut))
                    snippet.Keywords.Add($"{KnownTags.MetaPrefix}SubmenuShortcut {submenuShortcut}");

                return snippet;
            })
            .OrderBy(f => f.Language.ToString())
            .ThenBy(f => f.FileNameWithoutExtension());

        SaveSnippetsToSingleFile(snippets, filePath);
    }

    public static void WriteAllText(
        string filePath,
        string content,
        Encoding encoding = null,
        bool onlyIfChanged = true,
        bool createDirectory = false)
    {
        encoding ??= Encoding.UTF8;

        if (!ShouldSave(filePath, content, encoding, onlyIfChanged))
            return;

        if (createDirectory)
            Directory.CreateDirectory(Path.GetDirectoryName(filePath));

        Console.WriteLine($"saving file {filePath}");

        File.WriteAllText(filePath, content, encoding);
    }

    private static bool ShouldSave(string filePath, string content, Encoding encoding, bool onlyIfChanged)
    {
        if (!onlyIfChanged)
            return true;

        if (!File.Exists(filePath))
            return true;

        string content2 = File.ReadAllText(filePath, encoding);

        return !string.Equals(content, content2, StringComparison.Ordinal);
    }

    public static void CleanOrCreateDirectory(string directoryPath)
    {
        if (Directory.Exists(directoryPath))
        {
            CleanDirectory(directoryPath);
        }
        else
        {
            CreateDirectory(directoryPath);
        }
    }

    private static void CleanDirectory(string directoryPath)
    {
        DeleteDirectories(directoryPath);
        DeleteFiles(directoryPath);
    }

    private static void DeleteFiles(string directoryPath)
    {
        foreach (string path in Directory.EnumerateFiles(directoryPath))
            DeleteFile(path);
    }

    private static void DeleteDirectories(string directoryPath)
    {
        foreach (string path in Directory.EnumerateDirectories(directoryPath))
            DeleteDirectory(path);
    }

    public static void DeleteFile(string path)
    {
        Console.WriteLine($"deleting file {path}");
        File.Delete(path);
    }

    private static void DeleteDirectory(string path)
    {
        Console.WriteLine($"deleting directory {path}");
        Directory.Delete(path);
    }

    public static void DeleteAndCreateDirectory(string directoryPath)
    {
        if (Directory.Exists(directoryPath))
            DeleteDirectory(directoryPath);

        CreateDirectory(directoryPath);
    }

    private static void CreateDirectory(string directoryPath)
    {
        Console.WriteLine($"creating directory {directoryPath}");
        Directory.CreateDirectory(directoryPath);
    }
}
