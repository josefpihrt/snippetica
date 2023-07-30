// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Snippetica.VisualStudio.Validations;
using Snippetica.VisualStudio.Xml.Serialization;

namespace Snippetica.VisualStudio;

/// <summary>
/// Maps <see cref="Snippet"/> from and to <see cref="CodeSnippetElement"/>.
/// </summary>
internal static class SnippetMapper
{
    private static Dictionary<string, ContextKind> _contextKinds;

    /// <summary>
    /// Maps a specified <see cref="CodeSnippetElement"/> to the newly created <see cref="Snippet"/>.
    /// </summary>
    /// <param name="element">A <see cref="CodeSnippetElement"/> that contains deserialized snippet data.</param>
    /// <returns>Newly created <see cref="Snippet"/>.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="element"/> is <c>null</c>.</exception>
    public static Snippet MapFromElement(CodeSnippetElement element)
    {
        if (element is null)
            throw new ArgumentNullException(nameof(element));

        var snippet = new Snippet();

        if (element.Format is not null
            && Version.TryParse(element.Format, out Version version)
            && ValidationHelper.IsValidVersion(version))
        {
            snippet.FormatVersion = version;
        }

        if (element.Header is not null)
            LoadHeaderElement(element.Header, snippet);

        if (element.Snippet is not null)
            LoadSnippetElement(element.Snippet, snippet);

        return snippet;
    }

    /// <summary>
    /// Maps a specified <see cref="Snippet"/> to the newly created <see cref="CodeSnippetElement"/>.
    /// </summary>
    /// <param name="snippet">A <see cref="Snippet"/> to be serialized.</param>
    /// <returns>Newly created <see cref="CodeSnippetElement"/>.</returns>
    public static CodeSnippetElement MapToElement(Snippet snippet)
    {
        return MapToElement(snippet, new SaveSettings());
    }

    /// <summary>
    /// Maps a specified <see cref="Snippet"/> to the newly created <see cref="CodeSnippetElement"/>, optionally modifying serialization process.
    /// </summary>
    /// <param name="snippet">A <see cref="Snippet"/> to be serialized.</param>
    /// <param name="settings">A <see cref="SaveSettings"/> that modify serialization process.</param>
    /// <returns>Newly created <see cref="CodeSnippetElement"/>.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="snippet"/> or <paramref name="settings"/> is <c>null</c>.</exception>
    public static CodeSnippetElement MapToElement(Snippet snippet, SaveSettings settings)
    {
        if (snippet is null)
            throw new ArgumentNullException(nameof(snippet));

        if (settings is null)
            throw new ArgumentNullException(nameof(settings));

        var context = new SerializationContext(snippet, settings);

        SerializeVersion(snippet.FormatVersion, context);

        context.Element.Header = CreateHeaderElement(context);
        context.Element.Snippet = CreateSnippetElement(context);

        return context.Element;
    }

    /// <summary>
    /// Maps each element of <see cref="CodeSnippetElement"/> sequence to the newly created <see cref="CodeSnippetElement"/>.
    /// </summary>
    /// <param name="snippets">An enumerable collection of code snippets to be serialized.</param>
    /// <returns>An enumerable collection of <see cref="CodeSnippetElement"/>.</returns>
    public static IEnumerable<CodeSnippetElement> MapToElement(IEnumerable<Snippet> snippets)
    {
        return MapToElement(snippets, new SaveSettings());
    }

    /// <summary>
    /// Maps each element of <see cref="CodeSnippetElement"/> sequence to the newly created <see cref="CodeSnippetElement"/>, optionally modifying serialization process.
    /// </summary>
    /// <param name="snippets">An enumerable collection of code snippets to be serialized.</param>
    /// <param name="settings">A <see cref="SaveSettings"/> that modify serialization process.</param>
    /// <returns>An enumerable collection of <see cref="CodeSnippetElement"/>.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="snippets"/> is <c>null</c>.</exception>
    public static IEnumerable<CodeSnippetElement> MapToElement(IEnumerable<Snippet> snippets, SaveSettings settings)
    {
        if (snippets is null)
            throw new ArgumentNullException(nameof(snippets));

        if (settings is null)
            throw new ArgumentNullException(nameof(settings));

        return MapToElement();

        IEnumerable<CodeSnippetElement> MapToElement()
        {
            foreach (Snippet snippet in snippets)
                yield return SnippetMapper.MapToElement(snippet, settings);
        }
    }

    private static void SerializeVersion(Version version, SerializationContext context)
    {
        if (ValidationHelper.IsValidVersion(version))
        {
            context.Element.Format = version.ToString(3);
        }
        else if (context.Settings.SetDefaultFormat)
        {
            context.Element.Format = Snippet.DefaultFormatVersionText;
        }
    }

    private static SnippetElement CreateSnippetElement(SerializationContext context)
    {
        var element = new SnippetElement() { Code = CreateCodeElement(context) };

        if (context.Snippet.Literals.Count > 0)
            element.Declarations = CreateDeclarationsElement(context);

        element.Imports = CreateImportsElements(context.Snippet.Namespaces);
        element.References = CreateReferenceElements(context.Snippet.AssemblyReferences);

        return element;
    }

    private static ImportElement[] CreateImportsElements(NamespaceCollection namespaces)
    {
        if (namespaces.Count > 0)
        {
            var elements = new ImportElement[namespaces.Count];

            for (int i = 0; i < namespaces.Count; i++)
                elements[i] = new ImportElement() { Namespace = namespaces[i] };

            return elements;
        }

        return null;
    }

    private static ReferenceElement[] CreateReferenceElements(Collection<AssemblyReference> references)
    {
        if (references.Count > 0)
        {
            var elements = new ReferenceElement[references.Count];

            for (int i = 0; i < references.Count; i++)
            {
                var element = new ReferenceElement() { Assembly = references[i].AssemblyName };

                if (references[i].Url is not null)
                    element.Url = references[i].Url.ToString();

                elements[i] = element;
            }

            return elements;
        }

        return null;
    }

    private static DeclarationsElement CreateDeclarationsElement(SerializationContext context)
    {
        var elements = new DeclarationsElement();

        LiteralElement[] literals = CreateLiteralElements(context).ToArray();

        ObjectElement[] objects = CreateObjectElements(context).ToArray();

        if (literals.Length > 0)
            elements.Literals = literals;

        if (objects.Length > 0)
            elements.Objects = objects;

        return elements;
    }

    private static IEnumerable<LiteralElement> CreateLiteralElements(SerializationContext context)
    {
        foreach (Literal literal in context.Snippet.Literals.Where(f => string.IsNullOrEmpty(f.TypeName)))
        {
            var element = new LiteralElement();

            if (!string.IsNullOrEmpty(literal.DefaultValue))
                element.Default = literal.DefaultValue;

            if (!string.IsNullOrEmpty(literal.Identifier))
                element.ID = literal.Identifier;

            element.Editable = literal.IsEditable;
            element.Function = literal.Function;
            element.ToolTip = literal.ToolTip;

            yield return element;
        }
    }

    private static IEnumerable<ObjectElement> CreateObjectElements(SerializationContext context)
    {
        foreach (Literal literal in context.Snippet.Literals.Where(f => !string.IsNullOrEmpty(f.TypeName)))
        {
            var element = new ObjectElement();

            if (!string.IsNullOrEmpty(literal.DefaultValue))
                element.Default = literal.DefaultValue;

            if (!string.IsNullOrEmpty(literal.Identifier))
                element.ID = literal.Identifier;

            element.Editable = literal.IsEditable;
            element.Function = literal.Function;
            element.ToolTip = literal.ToolTip;
            element.Type = literal.TypeName;

            yield return element;
        }
    }

    private static CodeElement CreateCodeElement(SerializationContext context)
    {
        var element = new CodeElement();
        Snippet snippet = context.Snippet;

        if (snippet.CodeText is not null)
        {
            if (TextUtility.ContainsCDataEnd(snippet.CodeText))
                throw new InvalidOperationException("Snippet code cannot contain CDATA ending sequence ']]>'.");

            element.Code = snippet.CodeText;
        }

        if (!snippet.HasDefaultDelimiter || !context.Settings.OmitDefaultDelimiter)
            element.Delimiter = new string(snippet.Delimiter, 1);

        if (snippet.ContextKind != ContextKind.None)
            element.Kind = snippet.ContextKind.ToString();

        element.Language = LanguageMapper.MapEnumToText(snippet.Language);

        return element;
    }

    private static HeaderElement CreateHeaderElement(SerializationContext context)
    {
        var element = new HeaderElement();
        Snippet snippet = context.Snippet;

        if (!string.IsNullOrEmpty(snippet.Title))
            element.Title = snippet.Title;

        if (!string.IsNullOrEmpty(snippet.Author))
            element.Author = snippet.Author;

        if (!string.IsNullOrEmpty(snippet.Description))
            element.Description = snippet.Description;

        if (!string.IsNullOrEmpty(snippet.Shortcut))
            element.Shortcut = snippet.Shortcut;

        if (snippet.HelpUrl is not null)
            element.HelpUrl = snippet.HelpUrl.ToString();

        if (snippet.SnippetTypes != SnippetTypes.None)
            element.SnippetTypes = GetSnippetTypes(snippet.SnippetTypes).ToArray();

        if (snippet.Keywords.Count > 0)
            element.Keywords = snippet.Keywords.ToArray();

        if (snippet.HasAlternativeShortcuts)
            element.AlternativeShortcuts = snippet.AlternativeShortcuts.ToArray();

        return element;
    }

    private static IEnumerable<string> GetSnippetTypes(SnippetTypes value)
    {
        if ((value & SnippetTypes.Expansion) != 0)
            yield return SnippetTypes.Expansion.ToString();

        if ((value & SnippetTypes.SurroundsWith) != 0)
            yield return SnippetTypes.SurroundsWith.ToString();

        if ((value & SnippetTypes.Refactoring) != 0)
            yield return SnippetTypes.Refactoring.ToString();
    }

    private static void LoadHeaderElement(HeaderElement element, Snippet snippet)
    {
        snippet.Author = element.Author;
        snippet.Description = element.Description;
        snippet.Shortcut = element.Shortcut;
        snippet.Title = element.Title;

        if (element.AlternativeShortcuts is not null)
        {
            foreach (string shortcut in element.AlternativeShortcuts)
                snippet.AlternativeShortcuts.Add(shortcut);
        }

        if (element.HelpUrl is not null
            && Uri.TryCreate(element.HelpUrl, UriKind.RelativeOrAbsolute, out Uri uri))
        {
            snippet.HelpUrl = uri;
        }

        if (element.Keywords is not null)
        {
            foreach (string keyword in element.Keywords)
                snippet.Keywords.Add(keyword);
        }

        if (element.SnippetTypes is not null)
        {
            foreach (string value in element.SnippetTypes)
            {
                if (Enum.TryParse(value, out SnippetTypes snippetTypes))
                    snippet.SnippetTypes |= snippetTypes;
            }
        }
    }

    private static void LoadSnippetElement(SnippetElement element, Snippet snippet)
    {
        if (element.Code is not null)
            LoadCodeElement(element.Code, snippet);

        if (element.Declarations is not null)
            LoadDeclarationsElement(element.Declarations, snippet);

        if (element.Imports is not null)
            LoadImports(element.Imports, snippet);

        if (element.References is not null)
            LoadReferences(element.References, snippet);
    }

    private static void LoadCodeElement(CodeElement element, Snippet snippet)
    {
        if (element.Delimiter?.Length == 1)
            snippet.Delimiter = element.Delimiter[0];

        if (element.Kind is not null
            && ContextKinds.TryGetValue(element.Kind, out ContextKind kind))
        {
            snippet.ContextKind = kind;
        }

        if (element.Language is not null)
            snippet.Language = LanguageMapper.MapTextToEnum(element.Language);

        if (element.Code is not null)
            snippet.CodeText = element.Code;
    }

    private static void LoadImports(ImportElement[] elements, Snippet snippet)
    {
        foreach (ImportElement element in elements)
        {
            if (!string.IsNullOrEmpty(element.Namespace))
                snippet.Namespaces.Add(element.Namespace);
        }
    }

    private static void LoadReferences(ReferenceElement[] references, Snippet snippet)
    {
        foreach (ReferenceElement element in references)
        {
            if (!string.IsNullOrEmpty(element.Assembly))
            {
                var reference = new AssemblyReference() { AssemblyName = element.Assembly };

                if (!string.IsNullOrEmpty(element.Url)
                    && Uri.TryCreate(element.Url, UriKind.RelativeOrAbsolute, out Uri url))
                {
                    reference.Url = url;
                }

                snippet.AssemblyReferences.Add(reference);
            }
        }
    }

    private static void LoadDeclarationsElement(DeclarationsElement element, Snippet snippet)
    {
        if (element.Literals is not null)
            LoadLiterals(element.Literals, snippet);

        if (element.Objects is not null)
            LoadObjects(element.Objects, snippet);
    }

    private static void LoadObjects(ObjectElement[] elements, Snippet snippet)
    {
        foreach (ObjectElement element in elements)
        {
            var literal = new Literal(element.ID ?? "")
            {
                DefaultValue = element.Default,
                IsEditable = element.Editable,
                Function = element.Function,
                ToolTip = element.ToolTip,
                TypeName = element.Type
            };

            snippet.Literals.Add(literal);
        }
    }

    private static void LoadLiterals(LiteralElement[] elements, Snippet snippet)
    {
        foreach (LiteralElement element in elements)
        {
            var literal = new Literal(element.ID ?? "")
            {
                DefaultValue = element.Default,
                IsEditable = element.Editable,
                Function = element.Function,
                ToolTip = element.ToolTip
            };

            snippet.Literals.Add(literal);
        }
    }

    private static Dictionary<string, ContextKind> ContextKinds
    {
        get
        {
            return _contextKinds
                ??= new Dictionary<string, ContextKind>(StringComparer.OrdinalIgnoreCase)
                {
                    ["method body"] = ContextKind.MethodBody,
                    ["method decl"] = ContextKind.MethodDeclaration,
                    ["type decl"] = ContextKind.TypeDeclaration,
                    ["file"] = ContextKind.File,
                    ["any"] = ContextKind.Any
                };
        }
    }
}
