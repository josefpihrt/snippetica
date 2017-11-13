// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.IO;
using System.Linq;
using Pihrtsoft.Snippets;

namespace Snippetica.CodeGeneration.VisualStudio
{
    public static class PkgDefGenerator
    {
        public static string GeneratePkgDefFile(IEnumerable<SnippetGeneratorResult> results)
        {
            using (var sw = new StringWriter())
            {
                foreach (IGrouping<Language, SnippetGeneratorResult> grouping in results.GroupBy(f => f.Language))
                {
                    sw.WriteLine($"// {grouping.Key.GetTitle()}");

                    foreach (SnippetGeneratorResult result in grouping)
                    {
                        sw.WriteLine($@"[$RootKey$\Languages\CodeExpansions\{result.Language.GetRegistryCode()}\Paths]");
                        sw.WriteLine($"\"{result.DirectoryName}\" = \"$PackageFolder$\\{result.DirectoryName}\"");
                    }

                    sw.WriteLine();
                }

                return sw.ToString();
            }
        }
    }
}
