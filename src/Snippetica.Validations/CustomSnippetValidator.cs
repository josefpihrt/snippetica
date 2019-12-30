// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Pihrtsoft.Snippets.Validations;

namespace Snippetica.Validations
{
    public class CustomSnippetValidator : SnippetValidator
    {
        private static readonly SnippetValidator _defaultValidator = CreateDefaultValidator();

        protected override IEnumerable<SnippetValidationResult> Validate(SnippetValidationContext context)
        {
            foreach (SnippetValidationResult result in _defaultValidator.Validate(context.Snippet))
                yield return result;

            if (context.Snippet.Author != "Josef Pihrt")
            {
                yield return new SnippetValidationResult(
                    context.Snippet,
                    "",
                    "Snippet author is not 'Josef Pihrt'.",
                    ResultImportance.Information);
            }

            if (context.Snippet.Shortcut.Any(f => char.IsWhiteSpace(f)))
            {
                yield return new SnippetValidationResult(
                    context.Snippet,
                    "",
                    "Snippet shortcut contains white-space.",
                    ResultImportance.Information);
            }

            if (RegexHelper.TrimEnd.IsMatch(context.Snippet.CodeText))
            {
                yield return new SnippetValidationResult(
                    context.Snippet,
                    "",
                    "Snippet code contains trailing white-space.",
                    ResultImportance.Information);
            }

            Match match = RegexHelper.InvalidLeadingSpaces.Match(context.Snippet.CodeText);

            if (match.Success)
            {
                Console.WriteLine(match.Value);

                yield return new SnippetValidationResult(
                    context.Snippet,
                    "",
                    "Snippet code contains invalid leading spaces.",
                    ResultImportance.Information);
            }
        }
    }
}
