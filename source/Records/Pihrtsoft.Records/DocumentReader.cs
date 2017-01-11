using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using Pihrtsoft.Records.Utilities;

namespace Pihrtsoft.Records
{
    public class DocumentReader
    {
        private DocumentReader(XDocument document, DocumentReaderSettings settings)
        {
            XDocument = document;
            Settings = settings;
        }

        private static Version SchemaVersion { get; } = new Version(0, 1, 0);

        private XDocument XDocument { get; }

        public DocumentReaderSettings Settings { get; }

        public static DocumentReader Create(string uri, DocumentReaderSettings settings = null)
        {
            if (settings == null)
                settings = new DocumentReaderSettings();

            return new DocumentReader(XDocument.Load(uri, settings.LoadOptions), settings);
        }

        public static DocumentReader Create(Stream stream, DocumentReaderSettings settings = null)
        {
            if (settings == null)
                settings = new DocumentReaderSettings();

            return new DocumentReader(XDocument.Load(stream, settings.LoadOptions), settings);
        }

        public static DocumentReader Create(TextReader textReader, DocumentReaderSettings settings = null)
        {
            if (settings == null)
                settings = new DocumentReaderSettings();

            return new DocumentReader(XDocument.Load(textReader, settings.LoadOptions), settings);
        }

        public static DocumentReader Create(XmlReader xmlReader, DocumentReaderSettings settings = null)
        {
            if (settings == null)
                settings = new DocumentReaderSettings();

            return new DocumentReader(XDocument.Load(xmlReader, settings.LoadOptions), settings);
        }

        public IEnumerable<Record> ReadRecords()
        {
            XElement documentElement = XDocument.Elements().FirstOrDefault();

            if (documentElement == null
                || !DefaultComparer.NameEquals(documentElement, ElementNames.Document))
            {
                ThrowHelper.ThrowInvalidOperation(ExceptionMessages.MissingElement(ElementNames.Document));
            }

            string versionText = documentElement.AttributeValueOrDefault(AttributeNames.Version);

            if (versionText != null)
            {
                Version version;

                if (!Version.TryParse(versionText, out version))
                {
                    ThrowHelper.ThrowInvalidOperation(ExceptionMessages.InvalidDocumentVersion());
                }
                else if (version > SchemaVersion)
                {
                    ThrowHelper.ThrowInvalidOperation(ExceptionMessages.DocumentVersionIsNotSupported(version, SchemaVersion));
                }
            }

            return ReadRecords(documentElement);
        }

        private IEnumerable<Record> ReadRecords(XElement documentElement)
        {
            XElement entitiesElement = GetEntitiesElement(documentElement);

            if (entitiesElement != null)
            {
                Queue<EntityReader> readers = GetEntityReaders(entitiesElement.Elements());

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

            entitiesElement = null;
        }

        private XElement GetEntitiesElement(XElement rootElement)
        {
            XElement entitiesElement = null;

            foreach (XElement element in rootElement.Elements())
            {
                switch (element.Kind())
                {
                    case ElementKind.Entities:
                        {
                            if (entitiesElement != null)
                                ThrowHelper.MultipleElementsWithEqualName(element);

                            entitiesElement = element;
                            break;
                        }
                    default:
                        {
                            ThrowHelper.UnknownElement(element);
                            break;
                        }
                }
            }

            return entitiesElement;
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
