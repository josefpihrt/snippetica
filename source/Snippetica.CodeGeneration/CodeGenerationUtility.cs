// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Linq;
using Pihrtsoft.Snippets;

namespace Snippetica.CodeGeneration
{
    public static class CodeGenerationUtility
    {
        public static string GetProjectSubtitle(IEnumerable<SnippetGeneratorResult> results)
        {
            IEnumerable<Language> languages = results.Select(f => f.Language).Distinct();

            return GetProjectSubtitle(languages);
        }

        public static string GetProjectSubtitle(IEnumerable<Language> languages)
        {
            return $"A collection of snippets for {GetLanguagesSeparatedWithComma(languages)}.";
        }

        private static string GetLanguagesSeparatedWithComma(IEnumerable<Language> languages)
        {
            string[] titles = languages
                .Select(f => f.GetTitle())
                .OrderBy(f => f)
                .ToArray();

            for (int i = 1; i < titles.Length - 1; i++)
            {
                titles[i] = ", " + titles[i];
            }

            titles[titles.Length - 1] = " and " + titles[titles.Length - 1];

            return string.Concat(titles);
        }

        public static string GetSnippetBrowserUrl(EnvironmentKind environmentKind, Language language = Language.None)
        {
            string s = $"?engine={environmentKind.GetIdentifier()}";

            if (language != Language.None)
                s += $"&language={language.GetIdentifier()}";

            return KnownPaths.SnippetBrowserUrl + s;
        }
    }
}
