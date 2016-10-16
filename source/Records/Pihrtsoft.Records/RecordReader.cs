using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Xml.Linq;
using Pihrtsoft.Records.Utilities;

namespace Pihrtsoft.Records
{
    internal class RecordReader : RecordReaderBase
    {
        public RecordReader(XElement element, EntityDefinition entity, DocumentReaderSettings settings, IEnumerable<Record> baseRecords = null)
            : base(element, entity, settings)
        {
            BaseRecords = (baseRecords != null) ? new BaseRecordCollection(baseRecords) : Empty.BaseRecordCollection;
        }

        public BaseRecordCollection BaseRecords { get; }

        private Collection<Record> Records { get; set; }

        public override IEnumerable<Record> ReadRecords()
        {
            Collect(Element.Elements());

            return Records;
        }

        protected override void AddRecord(Record record)
        {
            if (Records == null)
                Records = new Collection<Record>();

            Records.Add(record);
        }

        protected override Record CreateRecord(string id)
        {
            if (id != null && BaseRecords != null)
            {
                Record record = BaseRecords.Find(id);

                if (record != null)
                    return record.WithEntity(Entity);
            }

            return new Record(Entity, id);
        }
    }
}
