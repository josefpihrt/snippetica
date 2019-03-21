// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using Pihrtsoft.Snippets;
using Snippetica.IO;

namespace Snippetica.CodeGeneration
{
    public class ShortcutInfo
    {
        public ShortcutInfo(
            string value,
            string description,
            string comment,
            ShortcutKind kind,
            IEnumerable<Language> languages,
            IEnumerable<EnvironmentKind> environments,
            IEnumerable<string> tags)
        {
            Value = value;
            Description = description;
            Comment = comment;
            Kind = kind;
            Environments = new ReadOnlyCollection<EnvironmentKind>(environments.ToArray());
            Languages = new ReadOnlyCollection<Language>(languages.ToArray());
            Tags = new ReadOnlyCollection<string>(tags.ToArray());
        }

        public string Value { get; }

        public string Description { get; }

        public string Comment { get; }

        public ShortcutKind Kind { get; }

        public ReadOnlyCollection<Language> Languages { get; }

        public ReadOnlyCollection<EnvironmentKind> Environments { get; }

        public ReadOnlyCollection<string> Tags { get; }

        public bool HasTag(string value)
        {
            return Tags.Contains(value);
        }

        public static void SerializeToXml(string path, IEnumerable<ShortcutInfo> shortcuts)
        {
            var doc = new XDocument(
                new XElement(
                    new XElement(
                        "Shortcuts",
                        shortcuts.Select(f =>
                            new XElement(nameof(ShortcutInfo),
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
