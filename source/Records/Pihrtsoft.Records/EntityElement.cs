// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Xml.Linq;
using Pihrtsoft.Records.Utilities;
using static Pihrtsoft.Records.Utilities.ThrowHelper;

namespace Pihrtsoft.Records
{
    internal class EntityElement
    {
        private XElement _declarationsElement;
        private XElement _baseRecordsElement;
        private XElement _recordsElement;
        private XElement _entitiesElement;

        public EntityElement(XElement element, DocumentSettings settings)
            : this(element, settings, baseEntity: null)
        {
        }

        public EntityElement(XElement element, DocumentSettings settings, EntityDefinition baseEntity = null)
        {
            Settings = settings;

            Scan(element.Elements());

            ExtendedKeyedCollection<string, PropertyDefinition> properties = null;
            ExtendedKeyedCollection<string, Variable> variables = null;

            if (_declarationsElement != null)
                ScanDeclarations(_declarationsElement.Elements(), out properties, out variables);

            Entity = new EntityDefinition(element, baseEntity, properties, variables);
        }

        public DocumentSettings Settings { get; }

        public EntityDefinition Entity { get; }

        private void Scan(IEnumerable<XElement> elements)
        {
            foreach (XElement element in elements)
            {
                switch (element.Kind())
                {
                    case ElementKind.Declarations:
                        {
                            if (_declarationsElement != null)
                                ThrowOnMultipleElementsWithEqualName(element);

                            _declarationsElement = element;
                            break;
                        }
                    case ElementKind.BaseRecords:
                        {
                            if (_baseRecordsElement != null)
                                ThrowOnMultipleElementsWithEqualName(element);

                            _baseRecordsElement = element;
                            break;
                        }
                    case ElementKind.Records:
                        {
                            if (_recordsElement != null)
                                ThrowOnMultipleElementsWithEqualName(element);

                            _recordsElement = element;
                            break;
                        }
                    case ElementKind.Entities:
                        {
                            if (_entitiesElement != null)
                                ThrowOnMultipleElementsWithEqualName(element);

                            _entitiesElement = element;
                            break;
                        }
                    default:
                        {
                            ThrowOnUnknownElement(element);
                            break;
                        }
                }
            }
        }

        private static void ScanDeclarations(IEnumerable<XElement> elements, out ExtendedKeyedCollection<string, PropertyDefinition> properties, out ExtendedKeyedCollection<string, Variable> variables)
        {
            properties = null;
            variables = null;

            foreach (XElement element in elements)
            {
                switch (element.Kind())
                {
                    case ElementKind.Variable:
                        {
                            variables = variables ?? new ExtendedKeyedCollection<string, Variable>(DefaultComparer.StringComparer);

                            string variableName = element.AttributeValueOrThrow(AttributeNames.Name);

                            if (variables.Contains(variableName))
                                Throw(ErrorMessages.ItemAlreadyDefined(ElementNames.Variable, variableName), element);

                            var variable = new Variable(
                                variableName,
                                element.AttributeValueOrThrow(AttributeNames.Value));

                            variables.Add(variable);
                            break;
                        }
                    case ElementKind.Property:
                        {
                            properties = properties ?? new ExtendedKeyedCollection<string, PropertyDefinition>();

                            string name = null;
                            bool isCollection = false;
                            bool isRequired = false;
                            string defaultValue = null;
                            string description = null;

                            foreach (XAttribute attribute in element.Attributes())
                            {
                                switch (attribute.LocalName())
                                {
                                    case AttributeNames.Name:
                                        {
                                            name = attribute.Value;
                                            break;
                                        }
                                    case AttributeNames.IsCollection:
                                        {
                                            isCollection = bool.Parse(attribute.Value);
                                            break;
                                        }
                                    case AttributeNames.IsRequired:
                                        {
                                            isRequired = bool.Parse(attribute.Value);
                                            break;
                                        }
                                    case AttributeNames.DefaultValue:
                                        {
                                            defaultValue = attribute.Value;
                                            break;
                                        }
                                    case AttributeNames.Description:
                                        {
                                            description = attribute.Value;
                                            break;
                                        }
                                    default:
                                        {
                                            Throw(ErrorMessages.UnknownAttribute(attribute), element);
                                            break;
                                        }
                                }
                            }

                            if (properties.Contains(name))
                                Throw(ErrorMessages.ItemAlreadyDefined(ElementNames.Property, name), element);

                            if (isCollection
                                && defaultValue != null)
                            {
                                Throw(ErrorMessages.CollectionPropertyCannotDefineDefaultValue(), element);
                            }

                            var property = new PropertyDefinition(
                                name,
                                isCollection,
                                isRequired,
                                defaultValue,
                                description,
                                element);

                            properties.Add(property);
                            break;
                        }
                    default:
                        {
                            ThrowOnUnknownElement(element);
                            break;
                        }
                }
            }
        }

        private Collection<Record> ReadBaseRecords()
        {
            if (_baseRecordsElement != null)
            {
                var reader = new BaseRecordReader(_baseRecordsElement, Entity, Settings);

                IEnumerable<Record> records = reader.ReadRecords();

                if (records != null)
                    return new ExtendedKeyedCollection<string, Record>(records.ToArray(), DefaultComparer.StringComparer);
            }

            return null;
        }

        public Collection<Record> Records()
        {
            if (_recordsElement != null)
            {
                var reader = new RecordReader(_recordsElement, Entity, Settings, ReadBaseRecords());

                return reader.ReadRecords();
            }

            return null;
        }

        public IEnumerable<EntityElement> EntityElements()
        {
            if (_entitiesElement != null)
            {
                foreach (XElement element in _entitiesElement.Elements())
                {
                    if (element.Kind() != ElementKind.Entity)
                        ThrowOnUnknownElement(element);

                    yield return new EntityElement(element, Settings, Entity);
                }
            }
        }

        private static void Throw(string message, XObject @object)
        {
            ThrowInvalidOperation(message, @object);
        }
    }
}
