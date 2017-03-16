// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Diagnostics;

namespace Pihrtsoft.Records.Commands
{
    [DebuggerDisplay("{Kind}")]
    internal abstract class Command
    {
        protected Command()
        {
        }

        public abstract CommandKind Kind { get; }

        public abstract void Execute(Record record);
    }
}
