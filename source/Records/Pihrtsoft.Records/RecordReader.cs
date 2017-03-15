using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Xml.Linq;
using Pihrtsoft.Records.Utilities;

namespace Pihrtsoft.Records
{
    internal class RecordReader : AbstractRecordReader
    {
        public RecordReader(XElement element, EntityDefinition entity, DocumentSettings settings, IEnumerable<Record> baseRecords = null)
            : base(element, entity, settings)
        {
            BaseRecords = (baseRecords != null)
                ? new BaseRecordCollection(baseRecords)
                : Empty.BaseRecordCollection;
        }

        public BaseRecordCollection BaseRecords { get; }

        private Collection<Record> Records { get; set; }

        public override bool ShouldCheckRequiredProperty
        {
            get { return true; }
        }

        public override Collection<Record> ReadRecords()
        {
            Collect(Element.Elements());

            return Records;
        }

        protected override void AddRecord(Record record)
        {
            (Records ?? (Records = new Collection<Record>())).Add(record);
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
