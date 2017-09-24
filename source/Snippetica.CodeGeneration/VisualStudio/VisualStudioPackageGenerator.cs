// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using Pihrtsoft.Snippets;
using Snippetica.CodeGeneration.Markdown;
using Snippetica.IO;

namespace Snippetica.CodeGeneration.VisualStudio
{
    public class VisualStudioPackageGenerator : PackageGenerator
    {
        public VisualStudioPackageGenerator(SnippetEnvironment environment)
            : base(environment)
        {
        }

        public override List<Snippet> GeneratePackageFiles(string directoryPath, IEnumerable<SnippetGeneratorResult> results)
        {
            List<Snippet> snippets = base.GeneratePackageFiles(directoryPath, results);

            IOUtility.WriteAllText(
                Path.Combine(directoryPath, "description.html"),
                HtmlGenerator.GenerateVisualStudioMarketplaceDescription(results));

            IOUtility.WriteAllText(
                Path.Combine(directoryPath, "regedit.pkgdef"),
                PkgDefGenerator.GeneratePkgDefFile(results));

            return snippets;
        }

        protected override void SaveSnippets(List<Snippet> snippets, SnippetGeneratorResult result)
        {
            base.SaveSnippets(snippets, result);

            DirectoryReadmeSettings settings = Environment.CreateDirectoryReadmeSettings(result);

            MarkdownWriter.WriteDirectoryReadme(result.Path, snippets, settings);
        }

        protected override void SaveAllSnippets(string projectPath, List<Snippet> allSnippets)
        {
            base.SaveAllSnippets(projectPath, allSnippets);

            string projectName = Path.GetFileName(projectPath);

            string csprojPath = Path.Combine(projectPath, $"{projectName}.{ProjectDocument.CSharpProjectExtension}");

            var document = new ProjectDocument(csprojPath);

            document.RemoveSnippetFiles();

            XElement newItemGroup = document.AddItemGroup();

            document.AddSnippetFiles(allSnippets.Select(f => f.FilePath), newItemGroup);

            document.Save();
        }

        protected override IEnumerable<Snippet> PostProcess(IEnumerable<Snippet> snippets)
        {
            snippets = PostProcessCore(snippets);
            snippets = base.PostProcess(snippets);

            return snippets;
        }

        private IEnumerable<Snippet> PostProcessCore(IEnumerable<Snippet> snippets)
        {
            foreach (Snippet snippet in snippets)
            {
                if (snippet.TryGetTag(KnownTags.ShortcutSuffix, out TagInfo info))
                    snippet.Keywords.RemoveAt(info.KeywordIndex);

                Snippet obsoleteSnippet = GetObsoleteSnippetOrDefault(snippet);

                if (obsoleteSnippet != null)
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
            snippet.AddTag(KnownTags.ExcludeFromReadme);

            snippet.SuffixFileName("_Obsolete");

            return snippet;
        }
    }
}
