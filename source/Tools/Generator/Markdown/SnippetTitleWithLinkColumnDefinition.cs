// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.IO;
using System.Text.RegularExpressions;

namespace Pihrtsoft.Snippets.CodeGeneration.Markdown
{
    internal class SnippetTitleWithLinkColumnDefinition : SnippetTitleColumnDefinition
    {
        private readonly string _pattern;

        public SnippetTitleWithLinkColumnDefinition(string directoryPath)
        {
            DirectoryPath = directoryPath;

            _pattern = $"^{Regex.Escape(DirectoryPath)}{Regex.Escape(Path.DirectorySeparatorChar.ToString())}?";
        }

        public string DirectoryPath { get; }

        public override string GetValue(object value)
        {
            var snippet = (Snippet)value;

            string path = Regex.Replace(
                snippet.FilePath,
                _pattern,
                "",
                RegexOptions.IgnoreCase);

            path = path.Replace('\\', '/');

            return $"[{MarkdownHelper.Escape(snippet.GetTitleWithoutShortcut())}]({path})";
        }
    }
}
