// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Pihrtsoft.Snippets;
using Snippetica.IO;
using Snippetica.Validations;

namespace Snippetica.CodeGeneration
{
    public class PackageGenerator
    {
        public PackageGenerator(SnippetEnvironment environment)
        {
            Environment = environment;
        }

        public SnippetEnvironment Environment { get; }

        public virtual List<Snippet> GeneratePackageFiles(
            string directoryPath,
            IEnumerable<SnippetGeneratorResult> results)
        {
            var allSnippets = new List<Snippet>();

            foreach (SnippetGeneratorResult result in results)
            {
                result.Path = Path.Combine(directoryPath, result.DirectoryName);

                List<Snippet> snippets = PostProcess(result.Snippets).ToList();

                result.Snippets.Clear();
                result.Snippets.AddRange(snippets);

                ValidateSnippets(snippets);

                SaveSnippets(snippets, result);

                allSnippets.AddRange(snippets);
            }

            SaveAllSnippets(directoryPath, allSnippets);

            return allSnippets;
        }

        protected virtual void SaveSnippets(List<Snippet> snippets, SnippetGeneratorResult result)
        {
            IOUtility.SaveSnippets(snippets, result.Path);
        }

        protected virtual void SaveAllSnippets(string projectPath, List<Snippet> allSnippets)
        {
            IOUtility.SaveSnippetBrowserFile(allSnippets, Path.Combine(projectPath, "snippets.xml"));
        }

        protected virtual void ValidateSnippets(List<Snippet> snippets)
        {
            Validator.ValidateSnippets(snippets);

            Validator.ThrowOnDuplicateFileName(snippets);
        }

        protected virtual IEnumerable<Snippet> PostProcess(IEnumerable<Snippet> snippets)
        {
            foreach (Snippet snippet in snippets)
            {
                for (int i = snippet.Literals.Count - 1; i >= 0; i--)
                {
                    Literal literal = snippet.Literals[i];

                    if (!literal.IsEditable
                        && !string.Equals(literal.Identifier, "__cdataEnd", StringComparison.Ordinal))
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
                    if (string.Equals(info.Value, Environment.Kind.GetIdentifier()))
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
    }
}
