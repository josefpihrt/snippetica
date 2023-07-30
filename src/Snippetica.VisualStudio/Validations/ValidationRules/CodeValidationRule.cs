// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using Snippetica.VisualStudio.Comparers;

namespace Snippetica.VisualStudio.Validations;

/// <summary>
/// Represents a validation rule for the snippet code.
/// </summary>
public class CodeValidationRule : ValidationRule
{
    private static readonly StringComparer _stringComparer = StringComparer.Ordinal;

    /// <summary>
    /// Validates a code of the specified <see cref="Snippet"/>.
    /// </summary>
    /// <param name="snippet">A snippet to be validated.</param>
    /// <returns>Enumerable collection of validation results.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="snippet"/> is <c>null</c>.</exception>
    public override IEnumerable<SnippetValidationResult> Validate(Snippet snippet)
    {
        if (snippet is null)
            throw new ArgumentNullException(nameof(snippet));

        return Validate();

        IEnumerable<SnippetValidationResult> Validate()
        {
            if (string.IsNullOrEmpty(snippet.CodeText))
            {
                yield return new SnippetValidationResult(
                    snippet,
                    ErrorCode.MissingCode,
                    "Snippet code is missing.",
                    ResultImportance.Error);
            }

            if (TextUtility.ContainsCDataEnd(snippet.CodeText))
            {
                yield return new SnippetValidationResult(
                    snippet,
                    ErrorCode.InvalidCode,
                    "Snippet code may not contain CData end sequence.",
                    ResultImportance.Error);
            }

            if (snippet.Code.ContainsUnclosedDelimiter)
            {
                yield return new SnippetValidationResult(
                    snippet,
                    ErrorCode.UnclosedDelimiter,
                    "Snippet code contains unclosed delimiter.",
                    ResultImportance.Warning);
            }

            foreach (Placeholder placeholder in snippet.Placeholders)
            {
                if (!placeholder.IsSystemPlaceholder
                    && !snippet.Literals
                        .Distinct(LiteralComparer.Identifier)
                        .Select(f => f.Identifier)
                        .Contains(placeholder.Identifier, _stringComparer))
                {
                    yield return new SnippetValidationResult(
                        snippet,
                        ErrorCode.PlaceholderWithoutLiteral,
                        $"Snippet code placeholder '{placeholder.Identifier}' does not have a corresponding literal.",
                        ResultImportance.Warning);
                }
            }

            if (!snippet.Placeholders.ContainsEnd())
            {
                yield return new SnippetValidationResult(
                    snippet,
                    ErrorCode.MissingEndPlaceholder,
                    "Snippet code does not contain end placeholder.",
                    ResultImportance.Warning);
            }
            else if (snippet.Placeholders.FindAll(Placeholder.EndIdentifier).CountExceeds(1))
            {
                yield return new SnippetValidationResult(
                    snippet,
                    ErrorCode.MultipleEndPlaceholders,
                    "Snippet code contain multiple end placeholders.",
                    ResultImportance.Warning);
            }

            if (snippet.IsSurroundsWith && !snippet.Placeholders.ContainsSelected())
            {
                yield return new SnippetValidationResult(
                    snippet,
                    ErrorCode.MissingSelectedPlaceholder,
                    "Snippet code does not contains selected placeholder.",
                    ResultImportance.Warning);
            }
            else if (snippet.IsSurroundsWith && snippet.Placeholders.FindAll(Placeholder.SelectedIdentifier).CountExceeds(1))
            {
                yield return new SnippetValidationResult(
                    snippet,
                    ErrorCode.MultipleSelectedPlaceholders,
                    "Snippet code contains multiple selected placeholders.",
                    ResultImportance.Warning);
            }

            if (snippet.Language == Language.None)
            {
                yield return new SnippetValidationResult(
                    snippet,
                    ErrorCode.MissingLanguage,
                    "Snippet language is missing.",
                    ResultImportance.Error);
            }
        }
    }
}
