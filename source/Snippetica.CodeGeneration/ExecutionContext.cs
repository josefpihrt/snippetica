// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.ObjectModel;
using Pihrtsoft.Snippets;

namespace Snippetica.CodeGeneration
{
    public class ExecutionContext
    {
        public ExecutionContext(Snippet snippet)
        {
            Snippets = new Collection<Snippet>() { snippet };
        }

        public bool IsCanceled { get; set; }

        public Collection<Snippet> Snippets { get; }
    }
}