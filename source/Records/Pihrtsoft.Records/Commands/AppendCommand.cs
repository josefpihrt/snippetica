// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Pihrtsoft.Records.Commands
{
    internal class AppendCommand : SetCommand
    {
        public AppendCommand(string propertyName, string value)
            : base(propertyName, value)
        {
        }

        public override CommandKind Kind
        {
            get { return CommandKind.Append; }
        }

        public override void Execute(Record record)
        {
            record[PropertyName] += Value;
        }
    }
}
