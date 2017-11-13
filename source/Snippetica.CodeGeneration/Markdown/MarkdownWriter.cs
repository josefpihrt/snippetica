// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.IO;
using Pihrtsoft.Snippets;
using Snippetica.IO;
using static Snippetica.KnownNames;

namespace Snippetica.CodeGeneration.Markdown
{
    public static class MarkdownWriter
    {
        public static void WriteProjectReadme(
            string directoryPath,
            IEnumerable<SnippetGeneratorResult> results,
            ProjectReadmeSettings settings)
        {
            IOUtility.WriteAllText(
                Path.Combine(directoryPath, ReadMeFileName),
                MarkdownGenerator.GenerateProjectReadme(results, settings),
                IOUtility.UTF8NoBom);
        }

        public static void WriteDirectoryReadme(
            string directoryPath,
            List<Snippet> snippets,
            DirectoryReadmeSettings settings)
        {
            IOUtility.WriteAllText(
                Path.Combine(directoryPath, ReadMeFileName),
                MarkdownGenerator.GenerateDirectoryReadme(snippets, settings),
                IOUtility.UTF8NoBom);
        }
    }
}
