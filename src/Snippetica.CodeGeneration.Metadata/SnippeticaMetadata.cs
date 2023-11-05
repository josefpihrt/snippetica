// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using Snippetica.VisualStudio;

namespace Snippetica.CodeGeneration;

public class SnippeticaMetadata
{
    public SnippetDirectory[] Directories { get; init; }

    public ShortcutInfo[] Shortcuts { get; init; }

    public Dictionary<Language, LanguageDefinition> Languages { get; init; }

    public static SnippeticaMetadata Load(string filePath, string sourcePath)
    {
        JsonSnippeticaMetadata jsonMetadata = JsonSerializer.Deserialize<JsonSnippeticaMetadata>(
            File.ReadAllText(filePath),
            new JsonSerializerOptions()
            {
                WriteIndented = true,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                Converters = { new JsonStringEnumConverter() },
                ReadCommentHandling = JsonCommentHandling.Skip,
            });

        var metadata = new SnippeticaMetadata()
        {
            Directories = jsonMetadata.Directories.Select(f =>
            {
                return new SnippetDirectory()
                {
                    Path = Path.Combine(sourcePath, f.Path),
                    Language = f.Language,
                    Tags = f.Tags,
                };
            })
                .ToArray(),
            Shortcuts = jsonMetadata.Shortcuts,
            Languages = jsonMetadata.Languages.ToDictionary(
                f => f.Language,
                f =>
                {
                    return (LanguageDefinition)(f.Language switch
                    {
                        Language.CSharp => new LanguageDefinition.CSharpDefinition()
                        {
                            Modifiers = f.Modifiers,
                            Types = f.Types,
                            Keywords = f.Keywords ?? new KeywordDefinitionCollection(),
                        },
                        Language.VisualBasic => new LanguageDefinition.VisualBasicDefinition()
                        {
                            Modifiers = f.Modifiers,
                            Types = f.Types,
                            Keywords = f.Keywords ?? new KeywordDefinitionCollection(),
                        },
                        Language.Cpp => new LanguageDefinition.CppDefinition()
                        {
                            Modifiers = f.Modifiers,
                            Types = f.Types,
                            Keywords = f.Keywords ?? new KeywordDefinitionCollection(),
                        },
                        _ => throw new InvalidOperationException()
                    });
                }),
        };

        foreach (TypeDefinition typeDefinition in jsonMetadata.Types)
        {
            metadata.Languages[Language.CSharp].Types.Add(typeDefinition);
            metadata.Languages[Language.VisualBasic].Types.Add(typeDefinition);
        }

        return metadata;
    }

    public class JsonSnippeticaMetadata
    {
        public SnippetDirectory[] Directories { get; set; }

        public ShortcutInfo[] Shortcuts { get; set; }

        public TypeDefinition[] Types { get; set; }

        public JsonLanguageDefinition[] Languages { get; set; }
    }

    public class JsonLanguageDefinition
    {
        public Language Language { get; init; }

        public ModifierDefinitionCollection Modifiers { get; init; }

        public TypeDefinitionCollection Types { get; init; }

        public KeywordDefinitionCollection Keywords { get; init; }
    }
}
