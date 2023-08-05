// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;

namespace Snippetica.VisualStudio.Validations.Rules;

/// <summary>
/// Represents the validation rule for a snippet title.
/// </summary>
public class TitleValidationRule : ValidationRule
{
    /// <summary>
    /// Validates a title of the specified <see cref="Snippet"/>.
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
            if (snippet.Title.Length == 0)
            {
                yield return new SnippetValidationResult(
                    snippet,
                    ErrorCode.MissingTitle,
                    "Snippet title is missing.",
                    ResultImportance.Error);
            }

            if (string.IsNullOrWhiteSpace(snippet.Title))
            {
                yield return new SnippetValidationResult(
                    snippet,
                    ErrorCode.TitleTitleContainsWhiteSpaceOnly,
                    "Snippet title contains white-space only.",
                    ResultImportance.Error);
            }
        }
    }
}
