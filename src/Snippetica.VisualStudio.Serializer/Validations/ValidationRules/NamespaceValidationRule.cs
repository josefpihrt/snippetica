// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;

namespace Snippetica.VisualStudio.Serializer.Validations;

/// <summary>
/// Represents a validation rule for the snippet namespaces.
/// </summary>
public class NamespaceValidationRule : ValidationRule
{
    private static readonly StringComparer _stringComparer = StringComparer.Ordinal;

    /// <summary>
    /// Validates namespaces of the specified <see cref="Snippet"/>.
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
            foreach (IGrouping<string, string> grp in snippet.Namespaces.GroupBy(f => f, _stringComparer))
            {
                if (grp.CountExceeds(1))
                {
                    yield return new SnippetValidationResult(
                        snippet,
                        ErrorCode.NamespaceDuplicate,
                        $"Namespace '{grp.Key}' is duplicated.",
                        ResultImportance.Warning);
                }
            }
        }
    }
}
