// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Pihrtsoft.Snippets;

namespace Snippetica.CodeGeneration.Markdown
{
    public class ProjectReadmeSettings
    {
        public SnippetEnvironment Environment { get; set; }

        public string DirectoryPath { get; set; }

        public Language Language { get; set; }

        public bool IsDevelopment { get; set; }

        public string Header { get; set; }
    }
}
