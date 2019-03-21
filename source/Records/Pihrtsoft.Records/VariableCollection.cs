// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;

namespace Pihrtsoft.Records
{
    public class VariableCollection : ReadOnlyKeyedCollection<string, Variable>
    {
        public VariableCollection(IList<Variable> list)
            : base(list)
        {
        }

        internal VariableCollection(ExtendedKeyedCollection<string, Variable> collection)
            : base(collection)
        {
        }
    }
}
