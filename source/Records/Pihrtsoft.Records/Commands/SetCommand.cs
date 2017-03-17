// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Diagnostics;

namespace Pihrtsoft.Records.Commands
{
    [DebuggerDisplay("{Kind} {PropertyName,nq} = {Value,nq}")]
    internal class SetCommand : PropertyCommand
    {
        public SetCommand(PropertyDefinition property, string value)
            : base(property, value)
        {
        }

        public override CommandKind Kind
        {
            get { return CommandKind.Set; }
        }

        public override void Execute(Record record)
        {
            record[PropertyName] = Value;
        }
    }
}
