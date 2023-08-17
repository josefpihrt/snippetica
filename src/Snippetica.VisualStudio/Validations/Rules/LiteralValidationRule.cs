﻿// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;

namespace Snippetica.VisualStudio.Validations.Rules;

/// <summary>
/// Represents a validation rule for the snippet literals.
/// </summary>
public class LiteralValidationRule : ValidationRule
{
    private static readonly StringComparer _stringComparer = StringComparer.Ordinal;

    /// <summary>
    /// Validates literals of the specified <see cref="Snippet"/>.
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
            foreach (SnippetLiteral literal in snippet.Literals)
            {
                if (string.IsNullOrEmpty(literal.Identifier))
                {
                    yield return new SnippetValidationResult(
                        snippet,
                        "Snippet literal identifier is missing.",
                        ResultImportance.Error,
                        literal);
                }
                else if (!snippet.Placeholders.Contains(literal.Identifier))
                {
                    yield return new SnippetValidationResult(
                        snippet,
                        "Snippet literal does not have corresponding placeholder in code.",
                        ResultImportance.Error,
                        literal);
                }

                if (!SnippetValidator.IsValidLiteralIdentifier(literal.Identifier))
                {
                    yield return new SnippetValidationResult(
                        snippet,
                        "Snippet literal identifier is invalid.",
                        ResultImportance.Error,
                        literal);
                }

                if (string.IsNullOrEmpty(literal.DefaultValue))
                {
                    yield return new SnippetValidationResult(
                        snippet,
                        "Snippet literal default value is missing.",
                        ResultImportance.Error,
                        literal);
                }
            }

            foreach (IGrouping<string, SnippetLiteral> grp in snippet.Literals.GroupBy(f => f.Identifier, _stringComparer))
            {
                if (grp.CountExceeds(1))
                {
                    yield return new SnippetValidationResult(
                        snippet,
                        $"Snippet literal identifier '{grp.Key}' is duplicated.",
                        ResultImportance.Warning);
                }
            }
        }
    }
}
