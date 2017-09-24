// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using Pihrtsoft.Snippets;

namespace Snippetica.CodeGeneration.Markdown
{
    public class DirectoryReadmeSettings
    {
        public SnippetEnvironment Environment { get; set; }

        public string DirectoryPath { get; set; }

        public Language Language { get; set; }

        public bool IsDevelopment { get; set; }

        public bool AddQuickReference { get; set; }

        public string Header { get; set; }

        public string QuickReferenceText { get; set; }

        public bool AddLinkToTitle { get; set; }

        public bool GroupShortcuts { get; set; }

        public List<ShortcutInfo> Shortcuts { get; } = new List<ShortcutInfo>();
    }
}
