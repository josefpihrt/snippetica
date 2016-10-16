using System.Collections.Generic;
using System.Xml.Linq;
using Pihrtsoft.Records.Utilities;

namespace Pihrtsoft.Records
{
    internal class DocumentReader
    {
        private XElement _entitiesElement;

        public DocumentReader(XElement element, DocumentReaderSettings settings)
        {
            Element = element;
            Settings = settings;
        }

        public XElement Element { get; }

        public DocumentReaderSettings Settings { get; }

        public IEnumerable<Record> ReadRecords()
        {
            Scan();

            if (_entitiesElement != null)
            {
                Queue<EntityReader> readers = GetEntityReaders(_entitiesElement.Elements());

                if (readers != null)
                {
                    while (readers.Count > 0)
                    {
                        EntityReader reader = readers.Dequeue();

                        IEnumerable<Record> records = reader.ReadRecords();

                        if (records != null)
                        {
                            foreach (Record record in records)
                                yield return record;
                        }

                        readers.EnqueueRange(reader.GetEntityReaders());
                    }
                }
            }

            _entitiesElement = null;
        }

        private void Scan()
        {
            foreach (XElement element in Element.Elements())
            {
                switch (element.Kind())
                {
                    case ElementKind.Entities:
                        {
                            if (_entitiesElement != null)
                                ThrowHelper.MultipleElementsWithEqualName(element);

                            _entitiesElement = element;
                            break;
                        }
                    default:
                        {
                            ThrowHelper.UnknownElement(element);
                            break;
                        }
                }
            }
        }

        private Queue<EntityReader> GetEntityReaders(IEnumerable<XElement> elements)
        {
            Queue<EntityReader> readers = null;

            foreach (XElement element in elements)
            {
                switch (element.Kind())
                {
                    case ElementKind.Entity:
                        {
                            if (readers == null)
                                readers = new Queue<EntityReader>();

                            readers.Enqueue(new EntityReader(element, Settings));

                            break;
                        }
                    default:
                        {
                            ThrowHelper.UnknownElement(element);
                            break;
                        }
                }
            }

            return readers;
        }
    }
}
