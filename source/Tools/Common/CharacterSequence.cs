// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using Pihrtsoft.Records;

namespace Pihrtsoft.Snippets.CodeGeneration
{
    public class CharacterSequence
    {
        public CharacterSequence(
            string value,
            string description,
            string comment,
            IEnumerable<Language> languages,
            IEnumerable<string> tags)
        {
            Value = value;
            Description = description;
            Comment = comment;
            Languages = new ReadOnlyCollection<Language>(languages.ToArray());
            Tags = new ReadOnlyCollection<string>(tags.ToArray());
        }

        public string Value { get; }
        public string Description { get; }
        public string Comment { get; }
        public ReadOnlyCollection<Language> Languages { get; }
        public ReadOnlyCollection<string> Tags { get; }

        public static IEnumerable<CharacterSequence> LoadFromFile(string uri)
        {
            return Document.ReadRecords(uri)
                .Where(f => !f.HasTag(KnownTags.Disabled))
                .Select(record =>
                {
                    return new CharacterSequence(
                        record.GetString("Value"),
                        record.GetString("Description"),
                        record.GetStringOrDefault("Comment", "-"),
                        record.GetItems("Languages").Select(f => (Language)Enum.Parse(typeof(Language), f)),
                        record.Tags);
                });
        }

        public static void SerializeToXml(string path, IEnumerable<CharacterSequence> sequences)
        {
            var doc = new XDocument(
                new XElement(
                    new XElement(
                        "CharacterSequences",
                        sequences.Select(f =>
                            new XElement(nameof(CharacterSequence),
                                new XAttribute(nameof(f.Value), f.Value),
                                new XAttribute(nameof(f.Description), f.Description),
                                new XAttribute(nameof(f.Comment), f.Comment),
                                new XElement(nameof(f.Languages), f.Languages.Select(language => new XElement(nameof(Language), language.ToString()))),
                                new XElement(nameof(f.Tags), f.Tags.Select(tag => new XElement("Tag", tag)))
                            )
                        )
                    )
                )
            );

            using (var stringWriter = new Utf8StringWriter())
            {
                var xmlWriterSettings = new XmlWriterSettings()
                {
                    OmitXmlDeclaration = false,
                    NewLineChars = "\r\n",
                    IndentChars = "  ",
                    Indent = true
                };

                using (XmlWriter xmlWriter = XmlWriter.Create(stringWriter, xmlWriterSettings))
                    doc.WriteTo(xmlWriter);

                IOUtility.WriteAllText(path, stringWriter.ToString());
            }
        }
    }
}
