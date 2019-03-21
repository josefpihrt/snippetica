// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using Pihrtsoft.Records;

namespace Snippetica.CodeGeneration
{
    public static class Mapper
    {
        public static ShortcutInfo MapShortcutInfo(Record record)
        {
            return new ShortcutInfo(
                record.GetString("Value"),
                record.GetString("Description"),
                record.GetStringOrDefault("Comment", "-"),
                record.GetEnumOrDefault("Kind", ShortcutKind.None),
                record.GetItems("Languages").Select(ParseHelpers.ParseLanguage),
                record.GetItems("IDE").Select(f => (EnvironmentKind)Enum.Parse(typeof(EnvironmentKind), f)),
                record.GetTags());
        }

        public static SnippetDirectory MapSnippetDirectory(Record record)
        {
            return new SnippetDirectory(
                record.GetString("Path"),
                ParseHelpers.ParseLanguage(record.GetString("Language")),
                record.GetTags());
        }

        public static void LoadLanguages(this IEnumerable<Record> records)
        {
            foreach (IGrouping<string, Record> grouping in records.GroupBy(f => f.GetString(PropertyNames.Language)))
            {
                LanguageDefinition language = LanguageDefinition.FromLanguage(ParseHelpers.ParseLanguage(grouping.Key));

                foreach (Record record in grouping)
                {
                    switch (record.EntityName)
                    {
                        case PropertyNames.Modifier:
                            {
                                language.Modifiers.Add(MapModifierDefinition(record));
                                break;
                            }
                        case PropertyNames.Type:
                            {
                                language.Types.Add(MapTypeDefinition(record));
                                break;
                            }
                        case PropertyNames.Keyword:
                            {
                                language.Keywords.Add(MapKeywordDefinition(record));
                                break;
                            }
                    }
                }
            }
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
}
