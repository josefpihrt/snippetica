// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Pihrtsoft.Snippets;
using Pihrtsoft.Snippets.Validations;

namespace Snippetica.Validations
{
    public static class Validator
    {
        public static void ValidateSnippets(List<Snippet> snippets)
        {
            Console.WriteLine($"number of snippets: {snippets.Count}");

            foreach (SnippetValidationResult result in Validate(snippets))
            {
                Console.WriteLine();
                Console.WriteLine($"{result.Importance.ToString().ToUpper()}: \"{result.Description}\" in \"{result.Snippet.FilePath}\"");
            }

            foreach (IGrouping<string, Snippet> snippet in snippets
                .Where(f => f.HasTag(KnownTags.NonUniqueShortcut))
                .GroupBy(f => f.Shortcut)
                .Where(f => f.Count() == 1))
            {
                Console.WriteLine();
                Console.WriteLine($"UNUSED TAG {KnownTags.NonUniqueShortcut} in \"{snippet.First().FilePath}\"");
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

        public static void ThrowOnDuplicateFileName(IEnumerable<Snippet> snippets)
        {
            foreach (IGrouping<string, Snippet> grouping in snippets
                .GroupBy(f => Path.GetFileNameWithoutExtension(f.FilePath))
                .Where(f => f.CountExceeds(1)))
            {
                throw new InvalidOperationException($"Multiple snippets with same file name '{grouping.Key}'");
            }
        }

        public static void ThrowOnDuplicateShortcut(IEnumerable<Snippet> snippets)
        {
            foreach (IGrouping<string, Snippet> grouping in snippets
                .GroupBy(f => f.Shortcut)
                .Where(f => f.CountExceeds(1)))
            {
                throw new InvalidOperationException($"Multiple snippets with same shortcut '{grouping.Key}'");
            }
        }
    }
}
