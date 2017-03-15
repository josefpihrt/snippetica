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
