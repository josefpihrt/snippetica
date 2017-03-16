// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Diagnostics;

namespace Pihrtsoft.Records.Commands
{
    [DebuggerDisplay("{Kind} {PropertyName,nq} = {Value,nq}")]
    internal class SetCommand : Command
    {
        public SetCommand(string propertyName, string value)
        {
            PropertyName = propertyName;
            Value = value;
        }

        public string PropertyName { get; }

        public string Value { get; }

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
