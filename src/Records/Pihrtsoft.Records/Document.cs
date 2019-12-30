// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Immutable;
using System.IO;
using System.Xml;
using System.Xml.Linq;
using Pihrtsoft.Records.Xml;

namespace Pihrtsoft.Records
{
    public sealed class Document
    {
        private Document(XDocument document, DocumentOptions options)
        {
            XDocument = document;
            Options = options;
        }

        internal static Version SchemaVersion { get; } = new Version(0, 1, 0);

        private XDocument XDocument { get; }

        public DocumentOptions Options { get; }

        internal static Document Create(string uri, DocumentOptions options = null)
        {
            if (options == null)
                options = new DocumentOptions();

            return new Document(XDocument.Load(uri, options.LoadOptions), options);
        }

        internal static Document Create(Stream stream, DocumentOptions options = null)
        {
            if (options == null)
                options = new DocumentOptions();

            return new Document(XDocument.Load(stream, options.LoadOptions), options);
        }

        internal static Document Create(TextReader textReader, DocumentOptions options = null)
        {
            if (options == null)
                options = new DocumentOptions();

            return new Document(XDocument.Load(textReader, options.LoadOptions), options);
        }

        internal static Document Create(XmlReader xmlReader, DocumentOptions options = null)
        {
            if (options == null)
                options = new DocumentOptions();

            return new Document(XDocument.Load(xmlReader, options.LoadOptions), options);
        }

        public static ImmutableArray<Record> ReadRecords(string uri, DocumentOptions options = null)
        {
            return Create(uri, options).Records();
        }

        public static ImmutableArray<Record> ReadRecords(Stream stream, DocumentOptions options = null)
        {
            return Create(stream, options).Records();
        }

        public static ImmutableArray<Record> ReadRecords(TextReader textReader, DocumentOptions options = null)
        {
            return Create(textReader, options).Records();
        }

        public static ImmutableArray<Record> ReadRecords(XmlReader xmlReader, DocumentOptions options = null)
        {
            return Create(xmlReader, options).Records();
        }

        public ImmutableArray<Record> Records()
        {
            var reader = new XmlRecordReader(XDocument, Options);

            reader.ReadAll();

            return reader.Records.ToImmutableArray();
        }
    }
}
