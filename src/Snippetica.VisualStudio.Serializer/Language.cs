// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;

namespace Snippetica.VisualStudio.Serializer;

/// <summary>
/// Specifies programming language of a <see cref="Snippet"/>.
/// </summary>
public enum Language
{
    /// <summary>
    /// No language.
    /// </summary>
    None = 0,

    /// <summary>
    /// Visual Basic language.
    /// </summary>
    VisualBasic = 1,

    /// <summary>
    /// C# language.
    /// </summary>
    CSharp = 2,

    /// <summary>
    /// C++ language.
    /// </summary>
    Cpp = 3,

    /// <summary>
    /// C++ language.
    /// </summary>
    [Obsolete("Use Language.Cpp instead.")]
    CPlusPlus = Cpp,

    /// <summary>
    /// XML language.
    /// </summary>
    Xml = 4,

    /// <summary>
    /// XAML language.
    /// </summary>
    Xaml = 5,

    /// <summary>
    /// JavaScript language.
    /// </summary>
    JavaScript = 6,

    /// <summary>
    /// SQL language.
    /// </summary>
    Sql = 7,

    /// <summary>
    /// HTML language.
    /// </summary>
    Html = 8,

    /// <summary>
    /// CSS language.
    /// </summary>
    Css = 9,

    /// <summary>
    /// CoffeeScript language.
    /// </summary>
    CoffeeScript = 10,

    /// <summary>
    /// C language.
    /// </summary>
    C = 11,

    /// <summary>
    /// F# language.
    /// </summary>
    FSharp = 12,

    /// <summary>
    /// Go language.
    /// </summary>
    Go = 13,

    /// <summary>
    /// Groovy language.
    /// </summary>
    Groovy = 14,

    /// <summary>
    /// Java language.
    /// </summary>
    Java = 15,

    /// <summary>
    /// JSON language.
    /// </summary>
    Json = 16,

    /// <summary>
    /// Less language.
    /// </summary>
    Less = 17,

    /// <summary>
    /// Markdown language.
    /// </summary>
    Markdown = 18,

    /// <summary>
    /// Objective C language.
    /// </summary>
    ObjectiveC = 19,

    /// <summary>
    /// Objective C++ language.
    /// </summary>
    ObjectiveCpp = 20,

    /// <summary>
    /// Perl language.
    /// </summary>
    Perl = 21,

    /// <summary>
    /// PHP language.
    /// </summary>
    Php = 22,

    /// <summary>
    /// PowerShell language.
    /// </summary>
    PowerShell = 23,

    /// <summary>
    /// Python language.
    /// </summary>
    Python = 24,

    /// <summary>
    /// R language.
    /// </summary>
    R = 25,

    /// <summary>
    /// Razor language.
    /// </summary>
    Razor = 26,

    /// <summary>
    /// Ruby language.
    /// </summary>
    Ruby = 27,

    /// <summary>
    /// Rust language.
    /// </summary>
    Rust = 28,

    /// <summary>
    /// Sass language.
    /// </summary>
    Sass = 29,

    /// <summary>
    /// Swift language.
    /// </summary>
    Swift = 30,

    /// <summary>
    /// TypeScript language.
    /// </summary>
    TypeScript = 31,

    /// <summary>
    /// XSL language.
    /// </summary>
    Xsl = 32,

    /// <summary>
    /// YAML language.
    /// </summary>
    Yaml = 33,
}
