// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Diagnostics;

namespace Snippetica.CodeGeneration.Commands
{
    [DebuggerDisplay("{Kind}")]
    public abstract class Command
    {
        public abstract void Execute(ExecutionContext context);

        public abstract CommandKind Kind { get; }

        public virtual Command ChildCommand { get; }
    }
}
