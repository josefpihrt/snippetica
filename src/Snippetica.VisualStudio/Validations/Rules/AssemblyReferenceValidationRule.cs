// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;

namespace Snippetica.VisualStudio.Validations.Rules;

/// <summary>
/// Represents a validation rule for the <see cref="SnippetAssemblyReference"/>.
/// </summary>
public class AssemblyReferenceValidationRule : ValidationRule
{
    /// <summary>
    /// Validates assembly references of the specified <see cref="Snippet"/>.
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
            foreach (SnippetAssemblyReference reference in snippet.AssemblyReferences)
            {
                if (string.IsNullOrEmpty(reference.AssemblyName))
                {
                    yield return new SnippetValidationResult(
                        snippet,
                        ErrorCode.MissingAssemblyReferenceName,
                        "Snippet assembly reference name is missing.",
                        ResultImportance.Error,
                        reference);
                }
            }
        }
    }
}
