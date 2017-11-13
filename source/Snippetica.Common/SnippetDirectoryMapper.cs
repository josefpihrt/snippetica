// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Diagnostics;
using Pihrtsoft.Records;
using Pihrtsoft.Snippets;

namespace Snippetica
{
    public static class SnippetDirectoryMapper
    {
        public static SnippetDirectory MapFromRecord(Record record)
        {
            return new SnippetDirectory(
                record.GetString("Path"),
                ParseEnumValue(record.GetString("Language")),
                record.GetTags());
        }

        private static Language ParseEnumValue(string value)
        {
            switch (value)
            {
                case "Cpp":
                    return Language.Cpp;
                case "CSharp":
                    return Language.CSharp;
                case "Html":
                    return Language.Html;
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
                        Debug.Fail(value);
                        throw new NotSupportedException();
                    }
            }
        }
    }
}
