// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Diagnostics;

namespace Pihrtsoft.Records.Commands
{
    [DebuggerDisplay("{Kind} {PropertyName,nq} = {Value,nq}")]
    internal abstract class PropertyCommand : Command
    {
        protected PropertyCommand(PropertyDefinition property, string value)
        {
            Property = property;
            Value = value;
        }

        public PropertyDefinition Property { get; }

        public string Value { get; }

        public string PropertyName
        {
            get { return Property.Name; }
        }
    }
}
