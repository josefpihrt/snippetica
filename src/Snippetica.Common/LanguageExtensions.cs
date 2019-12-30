// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using Pihrtsoft.Snippets;

namespace Snippetica
{
    public static class LanguageExtensions
    {
        public static string GetTitle(this Language language)
        {
            switch (language)
            {
                case Language.None:
                    return "";
                case Language.VisualBasic:
                    return "VB";
                case Language.CSharp:
                    return "C#";
                case Language.Cpp:
                    return "C++";
                case Language.Xml:
                    return "XML";
                case Language.Xaml:
                    return "XAML";
                case Language.JavaScript:
                    return "JavaScript";
                case Language.Sql:
                    return "SQL";
                case Language.Html:
                    return "HTML";
                case Language.Css:
                    return "CSS";
                case Language.Json:
                    return "JSON";
                case Language.Markdown:
                    return "Markdown";
                default:
                    {
                        Debug.Fail(language.ToString());
                        return null;
                    }
            }
        }

        public static string GetIdentifier(this Language language)
        {
            switch (language)
            {
                case Language.VisualBasic:
                    return "vb";
                case Language.CSharp:
                    return "csharp";
                case Language.Cpp:
                    return "cpp";
                case Language.Xml:
                    return "xml";
                case Language.JavaScript:
                    return "javascript";
                case Language.Sql:
                    return "sql";
                case Language.Html:
                    return "html";
                case Language.Css:
                    return "css";
                case Language.Xaml:
                    return "xaml";
                case Language.Json:
                    return "json";
                case Language.Markdown:
                    return "markdown";
                default:
                    throw new ArgumentException(language.ToString(), nameof(language));
            }
        }

        public static string GetRegistryCode(this Language language)
        {
            switch (language)
            {
                case Language.VisualBasic:
                    return "Basic";
                case Language.CSharp:
                    return "CSharp";
                case Language.Cpp:
                    return "C/C++";
                case Language.Xml:
                    return "XML";
                case Language.Xaml:
                    return "XAML";
                case Language.JavaScript:
                    return "JavaScript";
                case Language.Sql:
                    return "SQL_SSDT";
                case Language.Html:
                    return "HTML";
                case Language.Css:
                    return "CSS";
                default:
                    throw new ArgumentException(language.ToString(), nameof(language));
            }
        }

        public static IEnumerable<string> GetKeywords(this Language language)
        {
            switch (language)
            {
                case Language.VisualBasic:
                    {
                        yield return "VisualBasic";
                        yield return "VB";
                        break;
                    }
                case Language.CSharp:
                    {
                        yield return "C#";
                        yield return "CSharp";
                        break;
                    }
                case Language.Cpp:
                    {
                        yield return "C++";
                        yield return "Cpp";
                        yield return "CPlusPlus";
                        break;
                    }
                case Language.Xml:
                    {
                        yield return "XML";
                        break;
                    }
                case Language.Xaml:
                    {
                        yield return "XAML";
                        break;
                    }
                case Language.JavaScript:
                    {
                        yield return "JavaScript";
                        yield return "JS";
                        break;
                    }
                case Language.Sql:
                    {
                        yield return "SQL";
                        break;
                    }
                case Language.Html:
                    {
                        yield return "HTML";
                        break;
                    }
                case Language.Css:
                    {
                        yield return "CSS";
                        break;
                    }
                case Language.Json:
                    {
                        yield return "JSON";
                        break;
                    }
                case Language.Markdown:
                    {
                        yield return "Markdown";
                        break;
                    }
                default:
                    {
                        throw new ArgumentException(language.ToString(), nameof(language));
                    }
            }
        }
    }
}
