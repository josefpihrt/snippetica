using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using Pihrtsoft.Records.Utilities;

namespace Pihrtsoft.Records
{
    public class Document
    {
        public Document(XDocument document)
        {
            if (document == null)
                throw new ArgumentNullException(nameof(document));

            XDocument = document;
        }

        public XDocument XDocument { get; }

        private static LoadOptions LoadOptions { get; } = LoadOptions.SetLineInfo;

        private static Version SchemaVersion { get; } = new Version(0, 1, 0);

        public static Document Create(string uri)
        {
            return new Document(XDocument.Load(uri, LoadOptions));
        }

        public static Document Create(Stream stream)
        {
            return new Document(XDocument.Load(stream, LoadOptions));
        }

        public static Document Create(TextReader textReader)
        {
            return new Document(XDocument.Load(textReader, LoadOptions));
        }

        public static Document Create(XmlReader xmlReader)
        {
            return new Document(XDocument.Load(xmlReader, LoadOptions));
        }

        public IEnumerable<Record> ReadRecords(DocumentReaderSettings settings = null)
        {
            XElement element = DocumentElement();

            Version version = DocumentVersion(element);

            if (version != null && version > SchemaVersion)
            {
                ThrowHelper.ThrowInvalidOperation(ExceptionMessages.DocumentVersionIsNotSupported(version, SchemaVersion));
            }

            var reader = new DocumentReader(element, settings ?? new DocumentReaderSettings());

            return reader.ReadRecords();
        }

        private XElement DocumentElement()
        {
            XElement element = XDocument.Elements().FirstOrDefault();

            if (element == null || !DefaultComparer.NameEquals(element, ElementNames.Document))
            {
                ThrowHelper.ThrowInvalidOperation(ExceptionMessages.MissingElement(ElementNames.Document));
            }

            return element;
        }

        public Version DocumentVersion(XElement element)
        {
            string value = element.AttributeValueOrDefault(AttributeNames.Version);

            if (value != null)
            {
                Version version;

                if (!Version.TryParse(value, out version))
                    ThrowHelper.ThrowInvalidOperation(ExceptionMessages.InvalidDocumentVersion());

                return version;
            }

            return null;
        }
    }
}
