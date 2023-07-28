// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Pihrtsoft.Snippets;
using Snippetica.Records;

namespace Snippetica.CodeGeneration;

public static class Helpers
{
    public static ShortcutInfo MapShortcutInfo(Record record)
    {
        return new ShortcutInfo(
            record.GetString("Value"),
            record.GetString("Description"),
            record.GetStringOrDefault("Comment", "-"),
            record.GetEnumOrDefault("Kind", ShortcutKind.None),
            record.GetItems("Languages").Select(f => ParseLanguage(f)),
            record.GetItems("IDE").Select(f => (EnvironmentKind)Enum.Parse(typeof(EnvironmentKind), f)),
            record.GetTags());
    }

    public static SnippetDirectory MapSnippetDirectory(Record record, string baseDirectoryPath)
    {
        return new SnippetDirectory(
            Path.Combine(baseDirectoryPath, record.GetString("Path")),
            ParseLanguage(record.GetString("Language")),
            record.GetTags());
    }

    public static Dictionary<Language, LanguageDefinition> LoadLanguages(string directoryPath)
    {
        var definitions = new Dictionary<Language, LanguageDefinition>()
        {
            [Language.CSharp] = new LanguageDefinition.CSharpDefinition(),
            [Language.VisualBasic] = new LanguageDefinition.VisualBasicDefinition(),
            [Language.Cpp] = new LanguageDefinition.CppDefinition()
        };

        foreach (IGrouping<string, Record> grouping in Document.ReadRecords(Path.Combine(directoryPath, "Languages.xml"))
            .Where(f => !f.HasTag(KnownTags.Disabled))
            .GroupBy(f => f.GetString(PropertyNames.Language)))
        {
            LanguageDefinition language = definitions[ParseLanguage(grouping.Key)];

            foreach (Record record in grouping)
            {
                switch (record.EntityName)
                {
                    case PropertyNames.Modifier:
                        language.Modifiers.Add(MapModifierDefinition(record));
                        break;

                    case PropertyNames.Type:
                        language.Types.Add(MapTypeDefinition(record));
                        break;

                    case PropertyNames.Keyword:
                        language.Keywords.Add(MapKeywordDefinition(record));
                        break;
                }
            }
        }

        foreach (TypeDefinition typeDefinition in Document.ReadRecords(Path.Combine(directoryPath, "Types.xml"))
            .Where(f => !f.HasTag(KnownTags.Disabled))
            .Select(f => MapTypeDefinition(f)))
        {
            definitions[Language.CSharp].Types.Add(typeDefinition);
            definitions[Language.VisualBasic].Types.Add(typeDefinition);
        }

        return definitions;
    }

    public static ModifierDefinition MapModifierDefinition(Record record)
    {
        return new ModifierDefinition(
            record.Id,
            record.GetStringOrDefault(PropertyNames.Keyword),
            record.GetStringOrDefault(PropertyNames.Shortcut),
            record.GetTags());
    }

    public static TypeDefinition MapTypeDefinition(Record record)
    {
        string keyword = record.GetStringOrDefault(PropertyNames.Keyword);

        return new TypeDefinition(
            record.Id,
            record.GetStringOrDefault(PropertyNames.Title, keyword),
            keyword,
            record.GetStringOrDefault(PropertyNames.Shortcut),
            record.GetStringOrDefault(PropertyNames.DefaultValue),
            record.GetStringOrDefault(PropertyNames.DefaultIdentifier),
            record.GetStringOrDefault(PropertyNames.Namespace),
            record.GetIntOrDefault(PropertyNames.Arity),
            record.GetTags());
    }

    public static KeywordDefinition MapKeywordDefinition(Record record)
    {
        string name = record.GetString(PropertyNames.Name);

        return new KeywordDefinition(
            name,
            record.GetStringOrDefault(PropertyNames.Value),
            record.GetStringOrDefault(PropertyNames.Title, name),
            record.GetStringOrDefault(PropertyNames.Shortcut),
            record.GetBooleanOrDefault(PropertyNames.IsDevelopment),
            record.GetTags());
    }

    private static Language ParseLanguage(string value)
    {
        return value switch
        {
            "Cpp" => Language.Cpp,
            "C#" or "CSharp" => Language.CSharp,
            "Html" => Language.Html,
            "VB" or "VisualBasic" => Language.VisualBasic,
            "Xaml" => Language.Xaml,
            "Xml" => Language.Xml,
            "Json" => Language.Json,
            "Markdown" => Language.Markdown,
            _ => throw new InvalidOperationException(),
        };
    }

    private static class PropertyNames
    {
        public const string Modifier = nameof(Modifier);
        public const string Language = nameof(Language);
        public const string Keyword = nameof(Keyword);
        public const string Shortcut = nameof(Shortcut);
        public const string Value = nameof(Value);
        public const string DefaultValue = nameof(DefaultValue);
        public const string DefaultIdentifier = nameof(DefaultIdentifier);
        public const string Name = nameof(Name);
        public const string Namespace = nameof(Namespace);
        public const string Arity = nameof(Arity);
        public const string Type = nameof(Type);
        public const string Title = nameof(Title);
        public const string IsDevelopment = nameof(IsDevelopment);
    }
}
