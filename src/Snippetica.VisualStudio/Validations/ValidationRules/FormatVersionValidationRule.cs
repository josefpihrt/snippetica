// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;

namespace Snippetica.VisualStudio.Validations;

/// <summary>
/// Represents a validation rule for the snippet format version.
/// </summary>
public class FormatVersionValidationRule : ValidationRule
{
    /// <summary>
    /// Validates a format version of the specified <see cref="Snippet"/>.
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
            if (snippet.FormatVersion is null)
            {
                yield return new SnippetValidationResult(
                    snippet,
                    ErrorCode.MissingVersion,
                    "Snippet format version is missing.",
                    ResultImportance.Error);
            }

            if (!ValidationHelper.IsValidVersion(snippet.FormatVersion))
            {
                yield return new SnippetValidationResult(
                    snippet,
                    ErrorCode.InvalidVersion,
                    "Snippet format version is has invalid format.",
                    ResultImportance.Error);
            }
        }
    }
}
