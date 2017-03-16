// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Diagnostics;

namespace Pihrtsoft.Records.Commands
{
    [DebuggerDisplay("{Kind} {Tag,nq}")]
    internal class AddTagCommand : Command
    {
        public AddTagCommand(string tag)
        {
            Tag = tag;
        }

        public string Tag { get; }

        public override CommandKind Kind
        {
            get { return CommandKind.AddTag; }
        }

        public override void Execute(Record record)
        {
            record.Tags.Add(Tag);
        }
    }
}
