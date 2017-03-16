// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Pihrtsoft.Records
{
    public class RecordCollection : ReadOnlyCollection<Record>
    {
        public RecordCollection(IList<Record> list)
            : base(list)
        {
        }
    }
}
