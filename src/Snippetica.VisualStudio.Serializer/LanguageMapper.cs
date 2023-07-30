// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;

namespace Snippetica.VisualStudio.Serializer;

internal static class LanguageMapper
{
    private static readonly Dictionary<string, Language> _mapTextToEnum = new(StringComparer.OrdinalIgnoreCase)
    {
        ["VB"] = Language.VisualBasic,
        ["CSharp"] = Language.CSharp,
        ["Cpp"] = Language.Cpp,
        ["Xml"] = Language.Xml,
        ["Xaml"] = Language.Xaml,
        ["JavaScript"] = Language.JavaScript,
        ["Sql"] = Language.Sql,
        ["Html"] = Language.Html,
        ["Css"] = Language.Css,
        ["CoffeeScript"] = Language.CoffeeScript,
        ["C"] = Language.C,
        ["FSharp"] = Language.FSharp,
        ["Go"] = Language.Go,
        ["Groovy"] = Language.Groovy,
        ["Java"] = Language.Java,
        ["Json"] = Language.Json,
        ["Less"] = Language.Less,
        ["Markdown"] = Language.Markdown,
        ["ObjectiveC"] = Language.ObjectiveC,
        ["ObjectiveCpp"] = Language.ObjectiveCpp,
        ["Perl"] = Language.Perl,
        ["Php"] = Language.Php,
        ["PowerShell"] = Language.PowerShell,
        ["Python"] = Language.Python,
        ["R"] = Language.R,
        ["Razor"] = Language.Razor,
        ["Ruby"] = Language.Ruby,
        ["Rust"] = Language.Rust,
        ["Sass"] = Language.Sass,
        ["Swift"] = Language.Swift,
        ["TypeScript"] = Language.TypeScript,
        ["Xsl"] = Language.Xsl,
        ["Yaml"] = Language.Yaml
    };

    private static readonly Dictionary<Language, string> _mapEnumToText = new()
    {
        [Language.VisualBasic] = "VB",
        [Language.CSharp] = "CSharp",
        [Language.Cpp] = "Cpp",
        [Language.Xml] = "Xml",
        [Language.Xaml] = "Xaml",
        [Language.JavaScript] = "JavaScript",
        [Language.Sql] = "Sql",
        [Language.Html] = "Html",
        [Language.Css] = "Css",
        [Language.CoffeeScript] = "CoffeeScript",
        [Language.C] = "C",
        [Language.FSharp] = "FSharp",
        [Language.Go] = "Go",
        [Language.Groovy] = "Groovy",
        [Language.Java] = "Java",
        [Language.Json] = "Json",
        [Language.Less] = "Less",
        [Language.Markdown] = "Markdown",
        [Language.ObjectiveC] = "ObjectiveC",
        [Language.ObjectiveCpp] = "ObjectiveCpp",
        [Language.Perl] = "Perl",
        [Language.Php] = "Php",
        [Language.PowerShell] = "PowerShell",
        [Language.Python] = "Python",
        [Language.R] = "R",
        [Language.Razor] = "Razor",
        [Language.Ruby] = "Ruby",
        [Language.Rust] = "Rust",
        [Language.Sass] = "Sass",
        [Language.Swift] = "Swift",
        [Language.TypeScript] = "TypeScript",
        [Language.Xsl] = "Xsl",
        [Language.Yaml] = "Yaml",
        [Language.None] = ""
    };

    public static string MapEnumToText(Language value)
    {
        return _mapEnumToText[value];
    }

    public static Language MapTextToEnum(string value)
    {
        if (_mapTextToEnum.TryGetValue(value, out Language result))
            return result;

        return Language.None;
    }
}
