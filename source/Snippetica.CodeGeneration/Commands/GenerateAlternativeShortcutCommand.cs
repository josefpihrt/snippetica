// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using Pihrtsoft.Snippets;
using static Pihrtsoft.Text.RegularExpressions.Linq.Patterns;

namespace Snippetica.CodeGeneration.Commands
{
    public class GenerateAlternativeShortcutCommand : SnippetCommand
    {
        private static readonly Regex _regex = AssertBack(LetterLower()).Assert(LetterUpper()).ToRegex();

        public override CommandKind Kind => CommandKind.AlternativeShortcut;

        public override Command ChildCommand => CommandUtility.SuffixFileNameWithUnderscoreCommand;

        protected override void Execute(ExecutionContext context, Snippet snippet)
        {
            snippet.Shortcut = CreateAlternativeShortcut(snippet);

            snippet.AddTag(KnownTags.NonUniqueTitle);
        }

        private static string CreateAlternativeShortcut(Snippet snippet)
        {
            IEnumerable<string> values = _regex.Split(snippet.Shortcut)
                .Select(f => f.Substring(0, 1) + f.Substring(f.Length - 1, 1))
                .Select(f => f.ToLower(CultureInfo.InvariantCulture));

            return string.Concat(values);
        }
    }
}
