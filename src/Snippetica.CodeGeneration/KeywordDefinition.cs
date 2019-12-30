// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using Pihrtsoft.Snippets;

namespace Snippetica.CodeGeneration
{
    [DebuggerDisplay("{Name,nq}")]
    public class KeywordDefinition
    {
        private static readonly Version _formatVersion = new Version(1, 1, 0);

        public static KeywordDefinition Default { get; } = new KeywordDefinition(null, null, null, null, false, Array.Empty<string>());

        public KeywordDefinition(
            string name,
            string value,
            string title,
            string shortcut,
            bool isDevelopment,
            string[] tags)
        {
            Name = name;
            Value = value;
            Title = title;
            Shortcut = shortcut;
            IsDevelopment = isDevelopment;
            Tags = new ReadOnlyCollection<string>(tags);
        }

        public string Name { get; }

        public string Value { get; }

        public string Title { get; }

        public string Shortcut { get; }

        public bool IsDevelopment { get; }

        public ReadOnlyCollection<string> Tags { get; }

        public bool HasTag(string tag)
        {
            return Tags.Contains(tag);
        }

        public Snippet ToSnippet()
        {
            string title = $"{Value} keyword";

            return new Snippet()
            {
                FormatVersion = _formatVersion,
                Title = title,
                Shortcut = Shortcut,

                Description = title,

                CodeText = Value + "$end$",

                FilePath = $"{Name}Keyword.{SnippetFile.Extension}"
            };
        }
    }
}
