// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Snippetica.VisualStudio.Serializer.Validations;

/// <summary>
/// Represents the rule for a <see cref="Snippet"/> validation. This class is abstract.
/// </summary>
public abstract class ValidationRule
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ValidationRule"/> class.
    /// </summary>
    protected ValidationRule()
    {
    }

    /// <summary>
    /// Validates a specified snippet and returns enumerable collection of <see cref="SnippetValidationResult"/>.
    /// </summary>
    /// <param name="snippet">A snippet to be validated.</param>
    /// <returns>Enumerable collection of <see cref="SnippetValidationResult"/>.</returns>
    public abstract IEnumerable<SnippetValidationResult> Validate(Snippet snippet);

    private static List<ValidationRule> GetValidationRules()
    {
        return new List<ValidationRule>()
        {
            new FormatVersionValidationRule(),
            new TitleValidationRule(),
            new ShortcutValidationRule(),
            new DescriptionValidationRule(),
            new SnippetTypeValidationRule(),
            new NamespaceValidationRule(),
            new AssemblyReferenceValidationRule(),
            new LiteralValidationRule(),
            new CodeValidationRule()
        };
    }

    /// <summary>
    /// Gets a collection of predefined validation rules.
    /// </summary>
    internal static ReadOnlyCollection<ValidationRule> PredefinedRules { get; } = new(GetValidationRules());
}
