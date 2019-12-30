// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using Pihrtsoft.Snippets;

namespace Snippetica.CodeGeneration
{
    internal static class ParseHelpers
    {
        public static Language ParseLanguage(string value)
        {
            switch (value)
            {
                case "Cpp":
                    return Language.Cpp;
                case "C#":
                case "CSharp":
                    return Language.CSharp;
                case "Html":
                    return Language.Html;
                case "VB":
                case "VisualBasic":
                    return Language.VisualBasic;
                case "Xaml":
                    return Language.Xaml;
                case "Xml":
                    return Language.Xml;
                case "Json":
                    return Language.Json;
                case "Markdown":
                    return Language.Markdown;
                default:
                    {
                        throw new InvalidOperationException();
                    }
            }
        }
    }
}
