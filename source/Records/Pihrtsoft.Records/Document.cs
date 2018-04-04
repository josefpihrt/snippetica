// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Xml;
using System.Xml.Linq;
using Pihrtsoft.Records.Utilities;
using static Pihrtsoft.Records.Utilities.ThrowHelper;

namespace Pihrtsoft.Records
{
    public class Document
    {
        private Document(XDocument document, DocumentSettings settings)
        {
            XDocument = document;
            Settings = settings;
        }

        private static Version SchemaVersion { get; } = new Version(0, 1, 0);

        private XDocument XDocument { get; }

        public DocumentSettings Settings { get; }

        internal static Document Create(string uri, DocumentSettings settings = null)
        {
            if (settings == null)
                settings = new DocumentSettings();

            return new Document(XDocument.Load(uri, settings.LoadOptions), settings);
        }

        internal static Document Create(Stream stream, DocumentSettings settings = null)
        {
            if (settings == null)
                settings = new DocumentSettings();

            return new Document(XDocument.Load(stream, settings.LoadOptions), settings);
        }

        internal static Document Create(TextReader textReader, DocumentSettings settings = null)
        {
            if (settings == null)
                settings = new DocumentSettings();

            return new Document(XDocument.Load(textReader, settings.LoadOptions), settings);
        }

        internal static Document Create(XmlReader xmlReader, DocumentSettings settings = null)
        {
            if (settings == null)
                settings = new DocumentSettings();

            return new Document(XDocument.Load(xmlReader, settings.LoadOptions), settings);
        }

        public static RecordCollection ReadRecords(string uri, DocumentSettings settings = null)
        {
            return Create(uri, settings).Records();
        }

        public static RecordCollection ReadRecords(Stream stream, DocumentSettings settings = null)
        {
            return Create(stream, settings).Records();
        }

        public static RecordCollection ReadRecords(TextReader textReader, DocumentSettings settings = null)
        {
            return Create(textReader, settings).Records();
        }

        public static RecordCollection ReadRecords(XmlReader xmlReader, DocumentSettings settings = null)
        {
            return Create(xmlReader, settings).Records();
        }

        public RecordCollection Records()
        {
            XElement documentElement = XDocument.FirstElement();

            if (documentElement == null
                || !DefaultComparer.NameEquals(documentElement, ElementNames.Document))
            {
                ThrowInvalidOperation(ErrorMessages.MissingElement(ElementNames.Document));
            }

            string versionText = documentElement.AttributeValueOrDefault(AttributeNames.Version);

            if (versionText != null)
            {
                if (!Version.TryParse(versionText, out Version version))
                {
                    ThrowInvalidOperation(ErrorMessages.InvalidDocumentVersion());
                }
                else if (version > SchemaVersion)
                {
                    ThrowInvalidOperation(ErrorMessages.DocumentVersionIsNotSupported(version, SchemaVersion));
                }
            }

            XElement entitiesElement = EntitiesElement(documentElement);

            if (entitiesElement != null)
            {
                return ReadRecords(entitiesElement.Elements());
            }
            else
            {
                return Empty.RecordCollection;
            }
        }

        private RecordCollection ReadRecords(IEnumerable<XElement> elements)
        {
            var records = new Collection<Record>();

            Queue<EntityElement> entityElements = null;

            foreach (XElement element in elements)
            {
                if (element.Kind() != ElementKind.Entity)
                    ThrowOnUnknownElement(element);

                (entityElements ?? (entityElements = new Queue<EntityElement>())).Enqueue(new EntityElement(element, Settings));
            }

            if (entityElements != null)
            {
                while (entityElements.Any())
                {
                    EntityElement entityElement = entityElements.Dequeue();

                    Collection<Record> entityRecords = entityElement.Records();

                    if (entityRecords != null)
                        records.AddRange(entityRecords);

                    entityElements.EnqueueRange(entityElement.EntityElements());
                }
            }

            return new RecordCollection(records);
        }

        private static XElement EntitiesElement(XElement rootElement)
        {
            XElement entitiesElement = null;

            foreach (XElement element in rootElement.Elements())
            {
                switch (element.Kind())
                {
                    case ElementKind.Entities:
                        {
                            if (entitiesElement != null)
                                ThrowOnMultipleElementsWithEqualName(element);

                            entitiesElement = element;
                            break;
                        }
                    default:
                        {
                            ThrowOnUnknownElement(element);
                            break;
                        }
                }
            }

            return entitiesElement;
        }
    }
}
