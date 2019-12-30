// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using Pihrtsoft.Snippets;

namespace Snippetica.CodeGeneration
{
    public class SnippetGeneratorResult
    {
        public SnippetGeneratorResult(
            List<Snippet> snippets,
            string name,
            Language language,
            bool isDevelopment = false,
            params string[] tags)
        {
            Snippets.AddRange(snippets);
            Path = name;
            Language = language;
            IsDevelopment = isDevelopment;

            if (tags != null)
                Tags.AddRange(tags);
        }

        public List<Snippet> Snippets { get; } = new List<Snippet>();

        public string Path { get; set; }

        public bool IsDevelopment { get; set; }

        public Language Language { get; set; }

        public List<string> Tags { get; } = new List<string>();

        public bool HasTag(string tag)
        {
            return Tags.Any(f => f.Equals(tag, StringComparison.Ordinal));
        }

        public string DirectoryName
        {
            get
            {
                string name = System.IO.Path.GetFileName(Path);

                if (!string.IsNullOrEmpty(name))
                    return name;

                return Path;
            }
        }
    }
}
