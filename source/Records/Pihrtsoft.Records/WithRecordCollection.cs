// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;

namespace Pihrtsoft.Records
{
    internal class WithRecordCollection : ReadOnlyKeyedCollection<string, Record>
    {
        public WithRecordCollection(IEnumerable<Record> records)
            : this(new List<Record>(records))
        {
        }

        public WithRecordCollection(IList<Record> list)
            : base(list)
        {
        }
    }
}
