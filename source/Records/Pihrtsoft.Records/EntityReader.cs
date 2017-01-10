using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Xml.Linq;
using Pihrtsoft.Records.Utilities;

namespace Pihrtsoft.Records
{
    internal class EntityReader
    {
        private XElement _declarationsElement;
        private XElement _recordsElement;
        private XElement _entitiesElement;
        private XElement _baseRecordsElement;

        public EntityReader(XElement element, DocumentReaderSettings settings)
            : this(element, settings, null)
        {
        }

        public EntityReader(XElement element, DocumentReaderSettings settings, EntityDefinition baseEntity = null)
        {
            Settings = settings;

            Scan(element.Elements());

            ExtendedKeyedCollection<string, PropertyDefinition> properties = null;
            ExtendedKeyedCollection<string, Variable> variables = null;

            if (_declarationsElement != null)
                ScanDeclarations(_declarationsElement.Elements(), out properties, out variables);

            Entity = new EntityDefinition(
                element.AttributeValueOrThrow(AttributeNames.Name),
                baseEntity,
                properties,
                variables,
                element);
        }

        public DocumentReaderSettings Settings { get; }

        public EntityDefinition Entity { get; }

        public EntityDefinition BaseEntity
        {
            get { return Entity.BaseEntity; }
        }

        internal XElement Current { get; private set; }

        private void Scan(IEnumerable<XElement> elements)
        {
            foreach (XElement element in elements)
            {
                Current = element;

                switch (element.Kind())
                {
                    case ElementKind.Declarations:
                        {
                            if (_declarationsElement != null)
                                ThrowHelper.MultipleElementsWithEqualName(element);

                            _declarationsElement = element;
                            break;
                        }
                    case ElementKind.BaseRecords:
                        {
                            if (_baseRecordsElement != null)
                                ThrowHelper.MultipleElementsWithEqualName(element);

                            _baseRecordsElement = element;
                            break;
                        }
                    case ElementKind.Records:
                        {
                            if (_recordsElement != null)
                                ThrowHelper.MultipleElementsWithEqualName(element);

                            _recordsElement = element;
                            break;
                        }
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

                Current = null;
            }
        }

        private void ScanDeclarations(IEnumerable<XElement> elements, out ExtendedKeyedCollection<string, PropertyDefinition> properties, out ExtendedKeyedCollection<string, Variable> variables)
        {
            properties = null;
            variables = null;

            foreach (XElement element in elements)
            {
                Current = element;

                switch (element.Kind())
                {
                    case ElementKind.Variable:
                        {
                            if (variables == null)
                                variables = new ExtendedKeyedCollection<string, Variable>(DefaultComparer.StringComparer);

                            string variableName = element.AttributeValueOrThrow(AttributeNames.Name);

                            if (variables.Contains(variableName))
                                Throw(ExceptionMessages.ItemAlreadyDefined(ElementNames.Variable, variableName));

                            var variable = new Variable(
                                variableName,
                                element.AttributeValueOrThrow(AttributeNames.Value));

                            variables.Add(variable);
                            break;
                        }
                    case ElementKind.Property:
                        {
                            if (properties == null)
                                properties = new ExtendedKeyedCollection<string, PropertyDefinition>();

                            string propertyName = element.AttributeValueOrThrow(AttributeNames.Name);

                            if (properties.Contains(propertyName))
                                Throw(ExceptionMessages.ItemAlreadyDefined(ElementNames.Property, propertyName));

                            var property = new PropertyDefinition(
                                propertyName,
                                element.AttributeValueOrDefault(AttributeNames.DefaultValue),
                                element.AttributeValueAsBooleanOrDefault(AttributeNames.IsCollection),
                                element);

                            properties.Add(property);
                            break;
                        }
                    default:
                        {
                            ThrowHelper.UnknownElement(element);
                            break;
                        }
                }

                Current = null;
            }
        }

        public IEnumerable<EntityReader> GetEntityReaders()
        {
            if (_entitiesElement != null)
            {
                foreach (XElement element in _entitiesElement.Elements())
                {
                    if (element.Kind() != ElementKind.Entity)
                        ThrowHelper.UnknownElement(element);

                    yield return new EntityReader(element, Settings, Entity);
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

        public Collection<Record> ReadRecords()
        {
            if (_recordsElement != null)
            {
                var reader = new RecordReader(_recordsElement, Entity, Settings, ReadBaseRecords());

                return (Collection<Record>)reader.ReadRecords();
            }

            return null;
        }

        internal void Throw(string message, XObject @object = null)
        {
            ThrowHelper.ThrowInvalidOperation(message, @object ?? Current);
        }
    }
}
