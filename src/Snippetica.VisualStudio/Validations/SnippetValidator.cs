// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Snippetica.VisualStudio.Validations;

/// <summary>
/// Represents a validator that validates <see cref="Snippet"/>.
/// </summary>
public class SnippetValidator
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SnippetValidator"/> class.
    /// </summary>
    public SnippetValidator()
    {
        ValidationRules = new Collection<ValidationRule>();
    }

    /// <summary>
    /// Validates the specified <see cref="Snippet"/> according to the code snippet schema.
    /// </summary>
    /// <param name="snippet">A <see cref="Snippet"/> that is being validated.</param>
    /// <returns>A <see cref="SnippetValidationContext"/> that stores data about the validation.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="snippet"/> is <c>null</c>.</exception>
    public IEnumerable<SnippetValidationResult> Validate(Snippet snippet)
    {
        if (snippet is null)
            throw new ArgumentNullException(nameof(snippet));

        return Validate();

        IEnumerable<SnippetValidationResult> Validate()
        {
            var context = new SnippetValidationContext(snippet);

            foreach (SnippetValidationResult result in this.Validate(context))
                yield return result;
        }
    }

    /// <summary>
    /// Validates the specified <see cref="Snippet"/> according to the code snippet schema.
    /// </summary>
    /// <param name="context">A <see cref="SnippetValidationContext"/> that stores data about the validation.</param>
    /// <exception cref="ArgumentNullException"><paramref name="context"/> is <c>null</c>.</exception>
    protected virtual IEnumerable<SnippetValidationResult> Validate(SnippetValidationContext context)
    {
        if (context is null)
            throw new ArgumentNullException(nameof(context));

        return Validate();

        IEnumerable<SnippetValidationResult> Validate()
        {
            foreach (ValidationRule rule in ValidationRules)
            {
                foreach (SnippetValidationResult result in rule.Validate(context.Snippet))
                    yield return result;
            }
        }
    }

    /// <summary>
    /// Creates <see cref="SnippetValidator"/> that contains validation rules defines in <see cref="Validations"/> namespace.
    /// </summary>
    /// <returns><see cref="SnippetValidator"/> with predefined validation rules.</returns>
    public static SnippetValidator CreateDefaultValidator()
    {
        var validator = new SnippetValidator();

        foreach (ValidationRule rule in ValidationRule.PredefinedRules)
            validator.ValidationRules.Add(rule);

        return validator;
    }

    /// <summary>
    /// Gets a collection of validation rules.
    /// </summary>
    public Collection<ValidationRule> ValidationRules { get; }

    /// <summary>
    /// Gets a value indicating whether the specified <paramref name="value"/> is valid snippet shortcut.
    /// </summary>
    /// <param name="value">A value to validate.</param>
    /// <returns><c>true</c> if <paramref name="value"/> is valid snippet shortcut; Otherwise, <c>false</c>.</returns>
    public static bool IsValidShortcut(string value)
    {
        if (string.IsNullOrEmpty(value))
            return false;

        for (int i = 0; i < value.Length; i++)
        {
            int ch = value[i];

            if (!((ch >= 65 && ch <= 90)
                || (ch >= 97 && ch <= 122)
                || (ch >= 48 && ch <= 57)
                || ch == 95
                || ch == 35))
            {
                return false;
            }
        }

        return true;
    }

    /// <summary>
    /// Gets a value indicating whether the specified <paramref name="value"/> is a valid literal identifier.
    /// </summary>
    /// <param name="value">A value to validate.</param>
    /// <returns><c>true</c> if <paramref name="value"/> is valid literal identifier; Otherwise, <c>false</c>.</returns>
    public static bool IsValidLiteralIdentifier(string value)
    {
        if (string.IsNullOrEmpty(value))
            return false;

        if (SnippetLiteral.IdentifierComparer.Equals(value, SnippetPlaceholder.EndIdentifier))
            return false;

        if (SnippetLiteral.IdentifierComparer.Equals(value, SnippetPlaceholder.SelectedIdentifier))
            return false;

        return true;
    }

    /// <summary>
    /// Gets a value indicating whether the specified <see cref="Version"/> is valid snippet format version.
    /// </summary>
    /// <param name="version">A <see cref="Version"/> to validate.</param>
    /// <returns><c>true</c> if <paramref name="version"/> is valid snippet format version; Otherwise, <c>false</c>.</returns>
    public static bool IsValidVersion(Version? version)
    {
        if (version is null)
            return false;

        if (version.Major == -1)
            return false;

        if (version.Minor == -1)
            return false;

        if (version.Build == -1)
            return false;

        if (version.Revision != -1)
            return false;

        return true;
    }
}
