// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Linq;

namespace Pihrtsoft.Snippets.CodeGeneration.Commands
{
    public abstract class BaseCommand : Command
    {
        public override void Execute(ExecutionContext context)
        {
            foreach (Snippet snippet in context.Snippets.ToArray())
                Execute(context, snippet);
        }

        protected abstract void Execute(ExecutionContext context, Snippet snippet);
    }
}
