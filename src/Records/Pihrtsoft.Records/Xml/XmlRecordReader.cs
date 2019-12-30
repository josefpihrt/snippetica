// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Xml.Linq;
using Pihrtsoft.Records.Utilities;
using static Pihrtsoft.Records.Utilities.ThrowHelper;

namespace Pihrtsoft.Records.Xml
{
    internal class XmlRecordReader
    {
        private XElement _documentElement;
        private XElement _entitiesElement;
        private XElement _entityElement;

        private readonly Queue<EntitiesInfo> _entities = new Queue<EntitiesInfo>();

        private XElement _declarationsElement;
        private XElement _withElement;
        private XElement _recordsElement;
        private XElement _childEntitiesElement;

        private EntityDefinition _entityDefinition;

        private State _state;
        private XElement _current;
        private Dictionary<string, Record> _withRecords;
        private int _depth = -1;
        private StringKeyedCollection<PropertyOperationCollection> _propertyOperations;
        private Stack<Variable> _variables;

        public XmlRecordReader(XDocument document, DocumentOptions options)
        {
            Document = document;
            Options = options;
        }

        public XDocument Document { get; }

        public DocumentOptions Options { get; }

        public ImmutableArray<Record>.Builder Records { get; } = ImmutableArray.CreateBuilder<Record>();

        public void ReadAll()
        {
            _documentElement = Document.FirstElement();

            if (_documentElement == null
                || !DefaultComparer.NameEquals(_documentElement, ElementNames.Document))
            {
                ThrowInvalidOperation(ErrorMessages.MissingElement(ElementNames.Document));
            }

            string versionText = _documentElement.AttributeValueOrDefault(AttributeNames.Version);

            if (versionText != null)
            {
                if (!Version.TryParse(versionText, out Version version))
                {
                    ThrowInvalidOperation(ErrorMessages.InvalidDocumentVersion());
                }
                else if (version > Pihrtsoft.Records.Document.SchemaVersion)
                {
                    ThrowInvalidOperation(ErrorMessages.DocumentVersionIsNotSupported(version, Pihrtsoft.Records.Document.SchemaVersion));
                }
            }

            foreach (XElement element in _documentElement.Elements())
            {
                switch (element.Kind())
                {
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

            if (_entitiesElement == null)
                return;

            _entities.Enqueue(new EntitiesInfo(_entitiesElement));

            while (_entities.Count > 0)
            {
                EntitiesInfo entities = _entities.Dequeue();

                foreach (XElement element in entities.Element.Elements())
                {
                    if (element.Kind() != ElementKind.Entity)
                        ThrowOnUnknownElement(element);

                    _entityElement = element;

                    ScanEntity();

                    ExtendedKeyedCollection<string, PropertyDefinition> properties = null;
                    ExtendedKeyedCollection<string, Variable> variables = null;

                    if (_declarationsElement != null)
                        ScanDeclarations(out properties, out variables);

                    _entityDefinition = CreateEntityDefinition(_entityElement, baseEntity: entities.BaseEntity, properties, variables);

                    if (_recordsElement != null)
                    {
                        if (_withElement != null)
                        {
                            _withRecords?.Clear();
                            _state = State.WithRecords;

                            ReadRecords(_withElement);
                        }

                        _state = State.Records;
                        ReadRecords(_recordsElement);
                        _state = State.None;
                    }

                    if (_childEntitiesElement != null)
                        _entities.Enqueue(new EntitiesInfo(_childEntitiesElement, _entityDefinition));

                    _entityDefinition = null;
                    _entityElement = null;
                    _declarationsElement = null;
                    _withElement = null;
                    _recordsElement = null;
                    _childEntitiesElement = null;
                }
            }
        }

        private void ScanEntity()
        {
            foreach (XElement element in _entityElement.Elements())
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
                    case ElementKind.With:
                        {
                            if (_withElement != null)
                                ThrowOnMultipleElementsWithEqualName(element);

                            _withElement = element;
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
                            if (_childEntitiesElement != null)
                                ThrowOnMultipleElementsWithEqualName(element);

                            _childEntitiesElement = element;
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

        private void ScanDeclarations(out ExtendedKeyedCollection<string, PropertyDefinition> properties, out ExtendedKeyedCollection<string, Variable> variables)
        {
            properties = null;
            variables = null;

            foreach (XElement element in _declarationsElement.Elements())
            {
                switch (element.Kind())
                {
                    case ElementKind.Variable:
                        {
                            variables = variables ?? new ExtendedKeyedCollection<string, Variable>(DefaultComparer.StringComparer);

                            string variableName = element.GetAttributeValueOrThrow(AttributeNames.Name);

                            if (variables.Contains(variableName))
                                Throw(ErrorMessages.ItemAlreadyDefined(ElementNames.Variable, variableName), element);

                            var variable = new Variable(
                                variableName,
                                element.GetAttributeValueOrThrow(AttributeNames.Value));

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
                            char[] separators = PropertyDefinition.Tags.SeparatorsArray;

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
                                    case AttributeNames.Separators:
                                        {
                                            separators = ParseHelpers.ParseSeparators(attribute.Value);
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

                            if (PropertyDefinition.IsReservedName(name))
                                ThrowInvalidOperation(ErrorMessages.PropertyNameIsReserved(name), element);

                            var property = new PropertyDefinition(
                                name,
                                isCollection,
                                isRequired,
                                defaultValue,
                                description,
                                separators);

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

        private static EntityDefinition CreateEntityDefinition(
            XElement element,
            EntityDefinition baseEntity = null,
            ExtendedKeyedCollection<string, PropertyDefinition> properties = null,
            ExtendedKeyedCollection<string, Variable> variables = null)
        {
            string name = element.GetAttributeValueOrThrow(AttributeNames.Name);

            if (baseEntity != null
                && properties != null)
            {
                foreach (PropertyDefinition property in properties)
                {
                    if (baseEntity.FindProperty(property.Name) != null)
                        ThrowInvalidOperation(ErrorMessages.PropertyAlreadyDefined(property.Name, name), element);
                }
            }

            return new EntityDefinition(name, baseEntity ?? EntityDefinition.Global, properties, variables);
        }

        private void AddRecord(Record record)
        {
            if (_state == State.Records)
            {
                Records.Add(record);
            }
            else
            {
                if (_withRecords == null)
                {
                    _withRecords = new Dictionary<string, Record>(DefaultComparer.StringComparer);
                }
                else if (_withRecords.ContainsKey(record.Id))
                {
                    Throw(ErrorMessages.ItemAlreadyDefined(PropertyDefinition.IdName, record.Id));
                }

                _withRecords.Add(record.Id, record);
            }
        }

        private Record CreateRecord(string id)
        {
            if (_state == State.Records)
            {
                if (id != null
                    && _withRecords != null
                    && _withRecords.TryGetValue(id, out Record record))
                {
                    return record.WithEntity(_entityDefinition);
                }

                return new Record(_entityDefinition, id);
            }
            else
            {
                if (id == null)
                    Throw(ErrorMessages.MissingWithRecordIdentifier());

                return new Record(_entityDefinition, id);
            }
        }

        private void ReadRecords(XElement element)
        {
            _propertyOperations?.Clear();
            _variables?.Clear();

            ReadRecords(element.Elements());
        }

        private void ReadRecords(IEnumerable<XElement> elements)
        {
            _depth++;

            foreach (XElement element in elements)
            {
                _current = element;

                switch (element.Kind())
                {
                    case ElementKind.New:
                        {
                            AddRecord(CreateRecord(element));

                            break;
                        }
                    case ElementKind.With:
                    case ElementKind.Without:
                    case ElementKind.Postfix:
                    case ElementKind.Prefix:
                        {
                            if (element.HasElements)
                            {
                                PushOperations(element);
                                ReadRecords(element.Elements());
                                PopOperations();
                            }

                            break;
                        }
                    case ElementKind.Variable:
                        {
                            if (element.HasElements)
                            {
                                AddVariable(element);
                                ReadRecords(element.Elements());
                                _variables.Pop();
                            }

                            break;
                        }
                    default:
                        {
                            ThrowOnUnknownElement(element);
                            break;
                        }
                }

                _current = null;
            }

            _depth--;
        }

        private void PushOperations(XElement element)
        {
            foreach (Operation operation in CreateOperationsFromElement(element))
            {
                _propertyOperations = _propertyOperations ?? new StringKeyedCollection<PropertyOperationCollection>();

                if (!_propertyOperations.TryGetValue(operation.PropertyName, out PropertyOperationCollection propertyOperations))
                {
                    propertyOperations = new PropertyOperationCollection(operation.PropertyDefinition);
                    _propertyOperations.Add(propertyOperations);
                }

                propertyOperations.Add(operation);
            }
        }

        private void PopOperations()
        {
            for (int i = 0; i < _propertyOperations.Count; i++)
            {
                PropertyOperationCollection propertyOperations = _propertyOperations[i];

                for (int j = propertyOperations.Count - 1; j >= 0; j--)
                {
                    if (propertyOperations[j].Depth == _depth)
                        propertyOperations.RemoveAt(j);
                }
            }
        }

        private void AddVariable(XElement element)
        {
            string name = element.GetAttributeValueOrThrow(AttributeNames.Name);
            string value = element.GetAttributeValueOrThrow(AttributeNames.Value);

            (_variables ?? (_variables = new Stack<Variable>())).Push(new Variable(name, value));
        }

        private Record CreateRecord(XElement element)
        {
            string id = null;

            Collection<Operation> operations = null;

            foreach (XAttribute attribute in element.Attributes())
            {
                if (DefaultComparer.NameEquals(attribute, AttributeNames.Id))
                {
                    id = GetValue(attribute);
                }
                else
                {
                    Operation operation = CreateOperationFromAttribute(element, ElementKind.New, attribute);

                    (operations ?? (operations = new Collection<Operation>())).Add(operation);
                }
            }

            Record record = CreateRecord(id);

            if (operations != null)
                ExecuteAll(operations, record);

            ExecuteChildOperations(element, record);

            ExecutePendingOperations(record);

            foreach (PropertyDefinition property in _entityDefinition.AllProperties())
            {
                if (property.DefaultValue != null)
                {
                    if (!record.ContainsProperty(property.Name))
                    {
                        record[property.Name] = property.DefaultValue;
                    }
                }
                else if (_state == State.Records
                    && property.IsRequired
                    && !record.ContainsProperty(property.Name))
                {
                    Throw(ErrorMessages.PropertyIsRequired(property.Name));
                }
            }

            return record;
        }

        private void ExecuteChildOperations(XElement element, Record record)
        {
            foreach (XElement child in element.Elements())
            {
                _current = child;

                ExecuteAll(CreateOperationsFromElement(child), record);
            }

            _current = element;
        }

        private void ExecutePendingOperations(Record record)
        {
            if (_propertyOperations == null)
                return;

            foreach (PropertyOperationCollection propertyOperations in _propertyOperations)
            {
                Dictionary<OperationKind, string> pendingValues = null;

                foreach (Operation operation in propertyOperations)
                {
                    OperationKind kind = operation.Kind;

                    if (kind == OperationKind.With
                        || kind == OperationKind.Without)
                    {
                        operation.Execute(record);

                        if (pendingValues != null)
                            ProcessPendingValues(pendingValues, propertyOperations.PropertyDefinition);
                    }
                    else
                    {
                        pendingValues = pendingValues ?? new Dictionary<OperationKind, string>();

                        if (pendingValues.TryGetValue(kind, out string value))
                        {
                            Debug.Assert(kind == OperationKind.Prefix || kind == OperationKind.Postfix, kind.ToString());

                            pendingValues[kind] += operation.Value;
                        }
                        else
                        {
                            pendingValues[kind] = operation.Value;
                        }
                    }
                }

                if (pendingValues != null)
                    ProcessPendingValues(pendingValues, propertyOperations.PropertyDefinition);
            }

            void ProcessPendingValues(Dictionary<OperationKind, string> pendingValues, PropertyDefinition propertyDefinition)
            {
                string name = propertyDefinition.Name;

                foreach (KeyValuePair<OperationKind, string> kvp in pendingValues)
                {
                    OperationKind kind = kvp.Key;

                    if (kind == OperationKind.Postfix)
                    {
                        if (propertyDefinition.IsCollection)
                        {
                            if (record.TryGetCollection(name, out List<object> items))
                            {
                                for (int i = 0; i < items.Count; i++)
                                    items[i] += kvp.Value;
                            }
                        }
                        else
                        {
                            record[name] += kvp.Value;
                        }
                    }
                    else if (kind == OperationKind.Prefix)
                    {
                        if (propertyDefinition.IsCollection)
                        {
                            if (record.TryGetCollection(name, out List<object> items))
                            {
                                for (int i = 0; i < items.Count; i++)
                                    items[i] = kvp.Value + items[i];
                            }
                        }
                        else
                        {
                            record[name] = kvp.Value + record[name];
                        }
                    }
                    else
                    {
                        Debug.Fail(kind.ToString());
                    }
                }

                pendingValues.Clear();
            }
        }

        private Operation CreateOperationFromAttribute(
            XElement element,
            ElementKind kind,
            XAttribute attribute,
            bool throwOnId = false)
        {
            string attributeName = attribute.LocalName();

            if (throwOnId
                && DefaultComparer.NameEquals(attributeName, AttributeNames.Id))
            {
                Throw(ErrorMessages.CannotUseOperationOnProperty(element, attributeName));
            }

            PropertyDefinition property;

            string name = attribute.LocalName();

            if (name == PropertyDefinition.TagsName)
            {
                property = PropertyDefinition.Tags;
            }
            else
            {
                property = GetProperty(attribute);
            }

            switch (kind)
            {
                case ElementKind.With:
                    {
                        return new Operation(property, GetValue(attribute), _depth, OperationKind.With);
                    }
                case ElementKind.Without:
                    {
                        if (!property.IsCollection)
                            Throw(ErrorMessages.CannotUseOperationOnNonCollectionProperty(element, property.Name));

                        return new Operation(property, GetValue(attribute), _depth, OperationKind.Without);
                    }
                default:
                    {
                        Debug.Assert(kind == ElementKind.New, kind.ToString());

                        return new Operation(property, GetValue(attribute), _depth, OperationKind.With);
                    }
            }
        }

        private IEnumerable<Operation> CreateOperationsFromElement(XElement element)
        {
            Debug.Assert(element.HasAttributes, element.ToString());

            ElementKind kind = element.Kind();

            switch (kind)
            {
                case ElementKind.With:
                case ElementKind.Without:
                    {
                        foreach (XAttribute attribute in element.Attributes())
                            yield return CreateOperationFromAttribute(element, kind, attribute, throwOnId: true);

                        //TODO: Separator
                        //char separator = ',';

                        //foreach (XAttribute attribute in element.Attributes())
                        //{
                        //    switch (attribute.LocalName())
                        //    {
                        //        case AttributeNames.Separator:
                        //            {
                        //                string separatorText = attribute.Value;

                        //                if (separatorText.Length != 1)
                        //                    Throw("Separator must be a single character", attribute);

                        //                separator = separatorText[0];
                        //                break;
                        //            }
                        //        default:
                        //            {
                        //                yield return CreateOperationFromAttribute(element, kind, attribute, separator: separator, throwOnId: true);
                        //                break;
                        //            }
                        //    }
                        //}

                        break;
                    }
                case ElementKind.Postfix:
                    {
                        foreach (XAttribute attribute in element.Attributes())
                            yield return new Operation(GetProperty(attribute), GetValue(attribute), _depth, OperationKind.Postfix);

                        break;
                    }
                case ElementKind.Prefix:
                    {
                        foreach (XAttribute attribute in element.Attributes())
                            yield return new Operation(GetProperty(attribute), GetValue(attribute), _depth, OperationKind.Prefix);

                        break;
                    }
                default:
                    {
                        Throw(ErrorMessages.OperationIsNotDefined(element.LocalName()));
                        break;
                    }
            }
        }

        private PropertyDefinition GetProperty(XAttribute attribute)
        {
            string propertyName = attribute.LocalName();

            if (DefaultComparer.NameEquals(propertyName, PropertyDefinition.TagsName))
                return PropertyDefinition.Tags;

            if (_entityDefinition.TryGetProperty(propertyName, out PropertyDefinition property))
                return property;

            Throw(ErrorMessages.PropertyIsNotDefined(propertyName), attribute);

            return null;
        }

        private string GetValue(XAttribute attribute)
        {
            return GetValue(attribute.Value, attribute);
        }

        private string GetValue(string value, XObject xobject)
        {
            try
            {
                return ParseHelpers.ParseAttributeValue(value, this);
            }
            catch (InvalidValueException ex)
            {
                ThrowInvalidOperation("Error while parsing value.", xobject, ex);
            }

            return null;
        }

        internal Variable FindVariable(string name)
        {
            if (_variables != null)
            {
                Variable variable = _variables.FirstOrDefault(f => DefaultComparer.NameEquals(name, f.Name));

                if (!variable.IsDefault)
                    return variable;
            }

            return _entityDefinition.FindVariable(name);
        }

        private static void ExecuteAll(IEnumerable<Operation> propertyOperations, Record record)
        {
            foreach (Operation propertyOperation in propertyOperations)
                propertyOperation.Execute(record);
        }

        private void Throw(string message, XObject @object = null)
        {
            ThrowInvalidOperation(message, @object ?? _current);
        }

        [DebuggerDisplay("{DebuggerDisplay,nq}")]
        private struct EntitiesInfo
        {
            public EntitiesInfo(XElement element, EntityDefinition baseEntity = null)
            {
                Element = element;
                BaseEntity = baseEntity;
            }

            public XElement Element { get; }

            public EntityDefinition BaseEntity { get; }

            [DebuggerBrowsable(DebuggerBrowsableState.Never)]
            private string DebuggerDisplay => (BaseEntity != null) ? $"{BaseEntity.Name} {Element}" : Element?.ToString();
        }

        private enum State
        {
            None,
            WithRecords,
            Records,
        }
    }
}
