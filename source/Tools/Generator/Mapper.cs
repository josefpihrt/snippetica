// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using Pihrtsoft.Records;
using Pihrtsoft.Snippets;

namespace Snippetica.CodeGeneration
{
    public static class Mapper
    {
        public static void LoadLanguages(this IEnumerable<Record> records)
        {
            foreach (IGrouping<string, Record> grouping in records.GroupBy(f => f.GetString(Identifiers.Language)))
            {
                LanguageDefinition language = LanguageDefinition.FromLanguage((Language)Enum.Parse(typeof(Language), grouping.Key));

                foreach (Record record in grouping)
                {
                    switch (record.EntityName)
                    {
                        case Identifiers.Modifier:
                            {
                                language.Modifiers.Add(CreateModifier(record));
                                break;
                            }
                        case Identifiers.Type:
                            {
                                language.Types.Add(CreateType(record));
                                break;
                            }
                        case Identifiers.Keyword:
                            {
                                language.Keywords.Add(CreateKeyword(record));
                                break;
                            }
                    }
                }
            }
        }

        public static ModifierDefinition CreateModifier(Record record)
        {
            return new ModifierDefinition(
                record.Id,
                record.GetStringOrDefault(Identifiers.Keyword),
                record.GetStringOrDefault(Identifiers.Shortcut),
                record.GetTags());
        }

        public static TypeDefinition CreateType(Record record)
        {
            string keyword = record.GetStringOrDefault(Identifiers.Keyword);

            return new TypeDefinition(
                record.Id,
                record.GetStringOrDefault(Identifiers.Title, keyword),
                keyword,
                record.GetStringOrDefault(Identifiers.Shortcut),
                record.GetStringOrDefault(Identifiers.DefaultValue),
                record.GetStringOrDefault(Identifiers.DefaultIdentifier),
                record.GetStringOrDefault(Identifiers.Namespace),
                record.GetTags());
        }

        public static KeywordDefinition CreateKeyword(Record record)
        {
            string name = record.GetString(Identifiers.Name);

            return new KeywordDefinition(
                name,
                record.GetStringOrDefault(Identifiers.Value),
                record.GetStringOrDefault(Identifiers.Title, name),
                record.GetStringOrDefault(Identifiers.Shortcut),
                record.GetBooleanOrDefault(Identifiers.IsDevelopment),
                record.GetTags());
        }

        private static class Identifiers
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
            public const string Type = nameof(Type);
            public const string Title = nameof(Title);
            public const string IsDevelopment = nameof(IsDevelopment);
        }
    }
}
