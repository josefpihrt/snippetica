// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Diagnostics;

namespace Pihrtsoft.Records
{
    [DebuggerDisplay("{Name,nq} = {Value,nq}")]
    public class Variable : IKey<string>
    {
        public Variable(string name, string value)
        {
            Name = name;
            Value = value;
        }

        public string Name { get; }
        public string Value { get; }

        public string GetKey()
        {
            return Name;
        }
    }
}
