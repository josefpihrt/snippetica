// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using Pihrtsoft.Snippets.Comparers;
using Pihrtsoft.Snippets.Validations;

namespace Pihrtsoft.Snippets
{
    public static class SnippetChecker
    {
        private static readonly SnippetDeepEqualityComparer _snippetEqualityComparer = new SnippetDeepEqualityComparer();

        public static void CheckSnippets(SnippetDirectory[] snippetDirectories)
        {
            foreach (IGrouping<Language, SnippetDirectory> grouping in snippetDirectories
                .Where(f => !f.HasAnyTag(KnownTags.AutoGenerationSource, KnownTags.AutoGenerationDestination))
                .GroupBy(f => f.Language))
            {
                Console.WriteLine();
                Console.WriteLine($"***** {grouping.Key} *****");

                SnippetDirectory[] directories = grouping.ToArray();

                CheckDuplicateShortcuts(directories);

                directories = directories
                    .Where(f => !f.HasTag(KnownTags.VisualStudio))
                    .ToArray();

                CheckLanguageSnippets(directories);
            }
        }

        private static void CheckLanguageSnippets(SnippetDirectory[] snippetDirectories)
        {
            List<Snippet> snippets = snippetDirectories
                .SelectMany(f => f.EnumerateSnippets())
                .ToList();

            Console.WriteLine();
            Console.WriteLine($"number of snippets: {snippets.Count}");

            foreach (SnippetValidationResult result in Validate(snippets))
            {
                Console.WriteLine();
                Console.WriteLine($"{result.Importance}: \"{result.Description}\" in \"{result.Snippet.FilePath}\"");
            }

            foreach (IGrouping<string, Snippet> snippet in snippets
                .Where(f => f.HasTag(KnownTags.NonUniqueShortcut))
                .GroupBy(f => f.Shortcut)
                .Where(f => f.Count() == 1))
            {
                Console.WriteLine();
                Console.WriteLine($"unused tag {KnownTags.NonUniqueShortcut} in \"{snippet.First().FilePath}\"");
            }

            foreach (Snippet snippet in snippets.Select(f => CloneAndSortCollections(f)))
            {
                IOUtility.SaveSnippet(snippet);
            }
        }

        public static void CheckDuplicateShortcuts(SnippetDirectory[] snippetDirectories)
        {
            List<Snippet> snippets = snippetDirectories
                .SelectMany(f => f.EnumerateSnippets())
                .ToList();

            foreach (DuplicateShortcutInfo info in FindDuplicateShortcuts(snippets, KnownTags.NonUniqueShortcut))
            {
                Console.WriteLine();
                Console.WriteLine($"shortcut duplicate: {info.Shortcut}");

                    foreach (Snippet item in info.Snippets)
                    Console.WriteLine($"  {item.FilePath}");
            }
        }

        public static IEnumerable<DuplicateShortcutInfo> FindDuplicateShortcuts(IEnumerable<Snippet> snippets, string allowDuplicateKeyword)
        {
            foreach (DuplicateShortcutInfo info in SnippetUtility.FindDuplicateShortcuts(snippets))
            {
                if (!string.IsNullOrEmpty(info.Shortcut))
                {
                    if (allowDuplicateKeyword == null
                        || info.Snippets.Any(f => !f.HasTag(allowDuplicateKeyword)))
                    {
                        yield return info;
                    }
                }
            }
        }

        public static IEnumerable<SnippetValidationResult> Validate(IEnumerable<Snippet> snippets)
        {
            var validator = new CustomSnippetValidator();

            foreach (Snippet snippet in snippets)
            {
                foreach (SnippetValidationResult result in validator.Validate(snippet))
                    yield return result;
            }
        }

        private static Snippet CloneAndSortCollections(Snippet snippet)
        {
            var clone = (Snippet)snippet.Clone();

            clone.Literals.Sort();
            clone.Keywords.Sort();
            clone.Namespaces.Sort();
            clone.AlternativeShortcuts.Sort();

            return clone;
        }

        private static Snippet GetChangedSnippetOrDefault(Snippet snippet)
        {
            Snippet snippet2 = CloneAndSortCollections(snippet);

            if (!_snippetEqualityComparer.Equals(snippet, snippet2))
                return snippet2;

            return null;
        }
    }
}
