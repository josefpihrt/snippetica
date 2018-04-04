// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Snippetica.CodeGeneration.Commands
{
    public class EmptyCommand : MultiCommand
    {
        public static EmptyCommand Instance { get; } = new EmptyCommand();

        protected EmptyCommand()
        {
        }

        public override CommandKind Kind
        {
            get { return CommandKind.Empty; }
        }

        public override void Execute(ExecutionContext context)
        {
        }

        public override Command ChildCommand
        {
            get { return null; }
        }
    }
}
