// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.ObjectModel;
using Pihrtsoft.Snippets;

namespace Snippetica.CodeGeneration.Commands
{
    public abstract class SnippetCommand : Command
    {
        public override void Execute(ExecutionContext context)
        {
            Collection<Snippet> snippets = context.Snippets;

            int length = snippets.Count;

            for (int i = 0; i < length; i++)
                Execute(context, snippets[i]);
        }

        protected abstract void Execute(ExecutionContext context, Snippet snippet);
    }
}
