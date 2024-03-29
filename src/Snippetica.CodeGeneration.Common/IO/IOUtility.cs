﻿// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Snippetica.VisualStudio;

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
            string filePath = Path.Combine(directoryPath, snippet.GetFileName());
            snippet.SetFilePath(filePath);

            SaveSnippet(snippet);

            filePaths.Remove(filePath);
        }

        foreach (string path in filePaths)
            DeleteFile(path);
    }

    public static void SaveSnippet(Snippet snippet, bool onlyIfChanged = true)
    {
        SaveSnippet(snippet, snippet.GetFilePath(), onlyIfChanged);
    }

    public static void SaveSnippet(Snippet snippet, string filePath, bool onlyIfChanged = true)
    {
        ArgumentNullException.ThrowIfNull(snippet);

        SaveOptions settings = CreateSaveSettings();

        if (!ShouldSave(snippet, filePath, settings, onlyIfChanged))
            return;

        using (var fileStream = new FileStream(filePath, FileMode.Create))
        {
            Console.WriteLine($"saving file {filePath}");
            SnippetSerializer.Serialize(fileStream, snippet, settings);
        }
    }

    private static bool ShouldSave(Snippet snippet, string filePath, SaveOptions settings, bool onlyIfChanged)
    {
        if (!onlyIfChanged)
            return true;

        if (!File.Exists(filePath))
            return true;

        string s1 = File.ReadAllText(filePath, Encoding.UTF8);
        string s2 = SnippetSerializer.CreateXml(snippet, settings);

        return !string.Equals(s1, s2, StringComparison.Ordinal);
    }

    private static SaveOptions CreateSaveSettings()
    {
        return new SaveOptions()
        {
            OmitXmlDeclaration = true,
            OmitCodeSnippetsElement = true,
            IndentChars = "  ",
            Comment = "Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0.",
        };
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

    public static void DeleteFile(string path)
    {
        Console.WriteLine($"deleting file {path}");
        File.Delete(path);
    }
}
