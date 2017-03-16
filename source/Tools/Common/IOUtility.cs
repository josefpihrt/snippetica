// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace Pihrtsoft.Snippets
{
    public static class IOUtility
    {
        private static readonly StringComparer _stringComparer = StringComparer.OrdinalIgnoreCase;

        public static void SaveSnippets(Snippet[] snippets, string directoryPath)
        {
            foreach (Snippet snippet in snippets)
                snippet.FilePath = Path.Combine(directoryPath, Path.GetFileName(snippet.FilePath));

            if (!Directory.Exists(directoryPath))
                Directory.CreateDirectory(directoryPath);

            string[] filePaths = Directory.GetFiles(directoryPath, "*.snippet", SearchOption.TopDirectoryOnly);

            foreach (string path in filePaths.Except(snippets.Select(f => f.FilePath), _stringComparer))
                DeleteFile(path);

            foreach (Snippet snippet in snippets)
                SaveSnippet(snippet);
        }

        public static void SaveSnippet(Snippet snippet, bool onlyIfChanged = true)
        {
            SaveSnippet(snippet, snippet.FilePath, onlyIfChanged);
        }

        public static void SaveSnippet(Snippet snippet, string filePath, bool onlyIfChanged = true)
        {
            if (snippet == null)
                throw new ArgumentNullException(nameof(snippet));

            SaveSettings settings = CreateSaveSettings();

            if (!onlyIfChanged
                || !File.Exists(filePath)
                || !string.Equals(
                    File.ReadAllText(filePath, Encoding.UTF8),
                    SnippetSerializer.CreateXml(snippet, settings),
                    StringComparison.Ordinal))
            {
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    Console.WriteLine($"saving {filePath}");
                    SnippetSerializer.Serialize(fileStream, snippet, settings);
                }

                Console.WriteLine();
            }
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

        public static void SaveSnippetsToSingleFile(IEnumerable<Snippet> snippets, string filePath)
        {
            if (snippets == null)
                throw new ArgumentNullException(nameof(snippets));

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                Console.WriteLine($"saving {filePath}");
                SnippetSerializer.Serialize(fileStream, snippets, CreateSaveSettings());
            }

            Console.WriteLine();
        }

        public static void WriteAllText(string filePath, string content, bool onlyIfChanged = true)
        {
            if (!onlyIfChanged
                || !File.Exists(filePath)
                || !string.Equals(
                    File.ReadAllText(filePath, Encoding.UTF8),
                    content,
                    StringComparison.Ordinal))
            {
                Console.WriteLine($"saving {filePath}");

                File.WriteAllText(filePath, content, Encoding.UTF8);
            }
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
}
