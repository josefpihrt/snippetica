﻿// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Diagnostics;
using System.IO;
using Snippetica.VisualStudio;

namespace Snippetica.CodeGeneration.Commands;

public class TypeCommand : BasicTypeCommand
{
    public TypeCommand(TypeDefinition type)
        : base(type)
    {
    }

    public override CommandKind Kind => CommandKind.Collection;

    protected override void Execute(ExecutionContext context, Snippet snippet)
    {
        if (snippet.HasTag(KnownTags.Initializer)
            && (Type.IsImmutable || Type.IsInterface))
        {
            context.IsCanceled = true;
            return;
        }

        LanguageDefinition language = ((LanguageExecutionContext)context).Language;

        var typeName = "";
        var fileName = "";

        if (Type.IsDictionary)
        {
            typeName = language.GetTypeParameterList("TKey, TValue");
            fileName = "OfTKeyTValue";
        }
        else if (Type.Arity == 1)
        {
            typeName = language.GetTypeParameterList("T");
            fileName = "OfT";
        }

        typeName = Type.Name + typeName;
        fileName = Type.Name + fileName;

        snippet.Title = snippet.Title.Replace(Placeholders.Type, typeName);
        snippet.Description = snippet.Description.Replace(Placeholders.Type, typeName);

        snippet.AddNamespace(Type.Namespace);

        snippet.AddTags(KnownTags.NonUniqueShortcut);
        snippet.AddTags(KnownTags.TitleStartsWithShortcut);
        snippet.AddTags(KnownTags.ExcludeFromDocs);

        snippet.RemoveLiteralAndReplacePlaceholders(LiteralIdentifiers.Type, Type.Name);

        if (Type.IsDictionary)
        {
            snippet.RemoveLiteralAndReplacePlaceholders(LiteralIdentifiers.TypeParameterList, language.GetTypeParameterList($"${LiteralIdentifiers.KeyType}$, ${LiteralIdentifiers.ValueType}$"));
            snippet.AddLiteral(LiteralIdentifiers.KeyType, null, language.ObjectType.Keyword);
            snippet.AddLiteral(LiteralIdentifiers.ValueType, null, language.ObjectType.Keyword);

            SnippetLiteral literal = snippet.Literals.Find(LiteralIdentifiers.Identifier);

            if (literal is not null)
                literal.DefaultValue = "dic";
        }
        else if (Type.Arity == 1)
        {
            snippet.RemoveLiteralAndReplacePlaceholders(LiteralIdentifiers.TypeParameterList, language.GetTypeParameterList($"${LiteralIdentifiers.TypeParameter}$"));
            snippet.AddLiteral(LiteralIdentifiers.TypeParameter, null, "T");
        }
        else
        {
            snippet.RemoveLiteralAndPlaceholders(LiteralIdentifiers.TypeParameterList);
        }

        if (!Tags.Contains(KnownTags.Arguments)
            && !(Type.IsReadOnly && Tags.Contains(KnownTags.Collection)))
        {
            snippet.RemoveLiteralAndPlaceholders(LiteralIdentifiers.Arguments);
        }

        snippet.SetFileName(fileName + snippet.GetFileName());

        if (snippet.HasTag(KnownTags.Initializer)
            && Tags.Contains(KnownTags.Initializer))
        {
            var clone = (Snippet)snippet.Clone();
            InitializerCommand.AddInitializer(context, clone, GetInitializer(language), language.GetDefaultValue());
            context.Snippets.Add(clone);
        }
        else
        {
            snippet.RemoveLiteralAndPlaceholders(LiteralIdentifiers.Initializer);
        }
    }

    private string GetInitializer(LanguageDefinition language)
    {
        if (Type.IsDictionary)
            return language.GetDictionaryInitializer($"${LiteralIdentifiers.Value}$");

        if (Tags.Contains(KnownTags.Collection))
            return language.GetCollectionInitializer($"${LiteralIdentifiers.Value}$");

        if (Tags.Contains(KnownTags.Array))
            return language.GetArrayInitializer($"${LiteralIdentifiers.Value}$");

        Debug.Fail("");

        return null;
    }
}
