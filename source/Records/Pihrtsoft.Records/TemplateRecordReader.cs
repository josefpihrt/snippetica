using System.Collections.Generic;
using System.Xml.Linq;
using Pihrtsoft.Records.Utilities;

namespace Pihrtsoft.Records
{
    internal class BaseRecordReader : RecordReaderBase
    {
        public BaseRecordReader(XElement element, EntityDefinition entity, DocumentReaderSettings settings)
            : base(element, entity, settings)
        {
        }

        private ExtendedKeyedCollection<string, Record> Records { get; set; }

        public override IEnumerable<Record> ReadRecords()
        {
            Collect(Element.Elements());

            return Records;
        }

        protected override void AddRecord(Record record)
        {
            if (Records == null)
            {
                Records = new ExtendedKeyedCollection<string, Record>();
            }
            else if (Records.Contains(record.Id))
            {
                Throw(ExceptionMessages.ItemAlreadyDefined(PropertyDefinition.Id.Name, record.Id));
            }

            Records.Add(record);
        }

        protected override Record CreateRecord(string id)
        {
            if (id == null)
                Throw(ExceptionMessages.MissingBaseRecordIdentifier());

            return new Record(Entity, id);
        }
    }
}
