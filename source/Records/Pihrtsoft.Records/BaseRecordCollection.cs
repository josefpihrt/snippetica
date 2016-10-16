using System.Collections.Generic;

namespace Pihrtsoft.Records
{
    internal class BaseRecordCollection : ReadOnlyKeyedCollection<string, Record>
    {
        public BaseRecordCollection(IEnumerable<Record> records)
            : this(new List<Record>(records))
        {
        }

        public BaseRecordCollection(IList<Record> list)
            : base(list)
        {
        }
    }
}
