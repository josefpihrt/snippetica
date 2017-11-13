// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using Pihrtsoft.Records;
using Pihrtsoft.Snippets;

namespace Snippetica.CodeGeneration
{
    public static class LanguageDefinitionMapper
    {
        public static IEnumerable<LanguageDefinition> ToLanguageDefinitions(this IEnumerable<Record> records)
        {
            foreach (IGrouping<string, Record> grouping in records
                .Where(f => f.ContainsProperty("Language"))
                .GroupBy(f => f.GetStringOrDefault(Identifiers.Language)))
            {
                LanguageDefinition language = CreateLanguageDefinition((Language)Enum.Parse(typeof(Language), grouping.Key));

                foreach (ModifierDefinition modifier in grouping.ToModifierDefinitions())
                    language.Modifiers.Add(modifier);

                foreach (TypeDefinition type in grouping
                    .Concat(records.Where(f => !f.ContainsProperty("Language")))
                    .ToTypeDefinitions())
                {
                    language.Types.Add(type);
                }

                yield return language;
            }
        }

        private static LanguageDefinition CreateLanguageDefinition(Language language)
        {
            switch (language)
            {
                case Language.CSharp:
                    return new CSharpDefinition();
                case Language.VisualBasic:
                    return new VisualBasicDefinition();
                default:
                    throw new NotSupportedException();
            }
        }

        public static IEnumerable<ModifierDefinition> ToModifierDefinitions(this IEnumerable<Record> records)
        {
            foreach (Record record in records.Where(f => f.Entity.Name == Identifiers.Modifier))
            {
                yield return new ModifierDefinition(
                    record.Id,
                    record.GetStringOrDefault(Identifiers.Keyword),
                    record.GetStringOrDefault(Identifiers.Shortcut),
                    record.GetTags());
            }
        }

        public static IEnumerable<TypeDefinition> ToTypeDefinitions(this IEnumerable<Record> records)
        {
            foreach (Record record in records.Where(f => f.Entity.Name == Identifiers.Type))
            {
                yield return new TypeDefinition(
                    record.Id,
                    record.GetStringOrDefault(Identifiers.Title, record.Id),
                    record.GetStringOrDefault(Identifiers.Keyword),
                    record.GetStringOrDefault(Identifiers.Shortcut),
                    record.GetStringOrDefault(Identifiers.DefaultValue),
                    record.GetStringOrDefault(Identifiers.DefaultIdentifier),
                    record.GetStringOrDefault(Identifiers.Namespace),
                    record.GetTags());
            }
        }

        private static class Identifiers
        {
            public const string Modifier = nameof(Modifier);
            public const string Language = nameof(Language);
            public const string Keyword = nameof(Keyword);
            public const string Shortcut = nameof(Shortcut);
            public const string DefaultValue = nameof(DefaultValue);
            public const string DefaultIdentifier = nameof(DefaultIdentifier);
            public const string Namespace = nameof(Namespace);
            public const string Type = nameof(Type);
            public const string Title = nameof(Title);
        }
    }
}
