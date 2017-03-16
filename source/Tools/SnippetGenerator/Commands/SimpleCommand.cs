// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;

namespace Pihrtsoft.Snippets.CodeGeneration.Commands
{
    public class SimpleCommand : BaseCommand
    {
        private readonly Action<Snippet> _action;

        public SimpleCommand(Action<Snippet> action, CommandKind kind)
        {
            _action = action;
            Kind = kind;
        }

        public override CommandKind Kind { get; }

        protected override void Execute(ExecutionContext context, Snippet snippet)
        {
            _action?.Invoke(snippet);
        }
    }
}
