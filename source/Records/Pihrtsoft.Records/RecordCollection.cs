using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Xml.Linq;
using Pihrtsoft.Records.Utilities;

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
