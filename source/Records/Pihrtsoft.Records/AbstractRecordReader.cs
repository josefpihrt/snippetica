// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Xml.Linq;
using Pihrtsoft.Records.Operations;
using Pihrtsoft.Records.Utilities;
using static Pihrtsoft.Records.Utilities.ThrowHelper;

namespace Pihrtsoft.Records
{
    internal abstract class AbstractRecordReader
    {
        protected AbstractRecordReader(XElement element, EntityDefinition entity, DocumentSettings settings)
        {
            Element = element;
            Entity = entity;
            Settings = settings;
        }

        protected XElement Element { get; }
        public EntityDefinition Entity { get; }
        public DocumentSettings Settings { get; }
        private XElement Current { get; set; }
        private int Depth { get; set; } = -1;

        private StringKeyedCollection<PropertyOperationCollection> Operations { get; set; }

        private Stack<Variable> Variables { get; set; }

        public virtual bool ShouldCheckRequiredProperty { get; }

        public abstract Collection<Record> ReadRecords();

        protected abstract void AddRecord(Record record);

        protected abstract Record CreateRecord(string id);

        protected void Collect(IEnumerable<XElement> elements)
        {
            Depth++;

            foreach (XElement element in elements)
            {
                Current = element;

                switch (element.Kind())
                {
                    case ElementKind.New:
                        {
                            AddRecord(CreateRecord(element));

                            break;
                        }
                    case ElementKind.Set:
                    case ElementKind.Add:
                    case ElementKind.AddRange:
                    case ElementKind.Remove:
                    case ElementKind.RemoveRange:
                    case ElementKind.Postfix:
                    case ElementKind.PostfixMany:
                    case ElementKind.Prefix:
                    case ElementKind.PrefixMany:
                        {
                            if (element.HasElements)
                            {
                                PushOperations(element);
                                Collect(element.Elements());
                                PopOperations();
                            }

                            break;
                        }
                    case ElementKind.Variable:
                        {
                            if (element.HasElements)
                            {
                                AddVariable(element);
                                Collect(element.Elements());
                                Variables.Pop();
                            }

                            break;
                        }
                    default:
                        {
                            ThrowOnUnknownElement(element);
                            break;
                        }
                }

                Current = null;
            }

            Depth--;
        }

        private void PushOperations(XElement element)
        {
            foreach (IPropertyOperation operation in CreateOperationsFromElement(element))
            {
                Operations = Operations ?? new StringKeyedCollection<PropertyOperationCollection>();

                if (!Operations.TryGetValue(operation.PropertyName, out PropertyOperationCollection propertyOperations))
                {
                    propertyOperations = new PropertyOperationCollection(operation.PropertyDefinition);
                    Operations.Add(propertyOperations);
                }

                propertyOperations.Add(operation);
            }
        }

        private void PopOperations()
        {
            for (int i = 0; i < Operations.Count; i++)
            {
                PropertyOperationCollection propertyOperations = Operations[i];

                for (int j = propertyOperations.Count - 1; j >= 0; j--)
                {
                    if (propertyOperations[j].Depth == Depth)
                        propertyOperations.RemoveAt(j);
                }
            }
        }

        private void AddVariable(XElement element)
        {
            string name = element.AttributeValueOrThrow(AttributeNames.Name);
            string value = element.AttributeValueOrThrow(AttributeNames.Value);

            (Variables ?? (Variables = new Stack<Variable>())).Push(new Variable(name, value));
        }

        private Record CreateRecord(XElement element)
        {
            string id = null;

            Collection<IPropertyOperation> operations = null;

            foreach (XAttribute attribute in element.Attributes())
            {
                if (DefaultComparer.NameEquals(attribute, AttributeNames.Id))
                {
                    id = GetValue(attribute);
                }
                else
                {
                    IPropertyOperation operation = CreateOperationFromAttribute(element, ElementKind.New, attribute);

                    (operations ?? (operations = new Collection<IPropertyOperation>())).Add(operation);
                }
            }

            Record record = CreateRecord(id);

            operations?.ExecuteAll(record);

            ExecuteChildOperations(element, record);

            ExecutePendingOperations(record);

            foreach (PropertyDefinition property in Entity.AllProperties())
            {
                if (property.DefaultValue != null)
                {
                    if (!record.ContainsProperty(property.Name))
                    {
                        record[property.Name] = property.DefaultValue;
                    }
                }
                else if (ShouldCheckRequiredProperty
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
                Current = child;

                CreateOperationsFromElement(child).ExecuteAll(record);
            }

            Current = element;
        }

        private void ExecutePendingOperations(Record record)
        {
            if (Operations != null)
            {
                foreach (PropertyOperationCollection propertyOperations in Operations)
                {
                    Dictionary<OperationKind, string> pendingValues = null;

                    foreach (IPropertyOperation operation in propertyOperations)
                    {
                        if (operation.SupportsExecute)
                        {
                            operation.Execute(record);

                            if (pendingValues != null)
                                ProcessPendingValues(pendingValues, propertyOperations.PropertyDefinition, record);
                        }
                        else
                        {
                            OperationKind kind = operation.Kind;

                            pendingValues = pendingValues ?? new Dictionary<OperationKind, string>();

                            if (!pendingValues.TryGetValue(kind, out string value))
                            {
                                pendingValues[kind] = operation.Value;
                            }
                            else
                            {
                                switch (kind)
                                {
                                    case OperationKind.PostfixMany:
                                        {
                                            pendingValues[kind] = operation.Value + pendingValues[kind];
                                            break;
                                        }
                                    case OperationKind.PrefixMany:
                                        {
                                            pendingValues[kind] += operation.Value;
                                            break;
                                        }
                                    default:
                                        {
                                            Debug.Assert(false, kind.ToString());
                                            break;
                                        }
                                }
                            }
                        }
                    }

                    if (pendingValues != null)
                        ProcessPendingValues(pendingValues, propertyOperations.PropertyDefinition, record);
                }
            }
        }

        private void ProcessPendingValues(Dictionary<OperationKind, string> operationValues, PropertyDefinition propertyDefinition, Record record)
        {
            foreach (KeyValuePair<OperationKind, string> pair in operationValues)
            {
                switch (pair.Key)
                {
                    case OperationKind.PostfixMany:
                        {
                            new PostfixOperation(propertyDefinition, pair.Value, Depth).Execute(record);
                            break;
                        }
                    case OperationKind.PrefixMany:
                        {
                            new PrefixOperation(propertyDefinition, pair.Value, Depth).Execute(record);
                            break;
                        }
                    default:
                        {
                            Debug.Assert(false, pair.Key.ToString());
                            break;
                        }
                }
            }

            operationValues.Clear();
        }

        private IPropertyOperation CreateOperationFromAttribute(
            XElement element,
            ElementKind kind,
            XAttribute attribute,
            char separator = ',',
            bool throwOnId = false,
            bool throwOnCollection = false)
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

            if (throwOnCollection
                && property.IsCollection)
            {
                Throw(ErrorMessages.CannotUseOperationOnCollectionProperty(element, property.Name));
            }

            switch (kind)
            {
                case ElementKind.Add:
                    return new AddOperation(property, GetValue(attribute), Depth);
                case ElementKind.AddRange:
                    return new AddRangeOperation(property, GetValue(attribute), separator, Depth);
                case ElementKind.Remove:
                    return new RemoveOperation(property, GetValue(attribute), Depth);
                case ElementKind.RemoveRange:
                    return new RemoveRangeOperation(property, GetValue(attribute), separator, Depth);
                default:
                    {
                        Debug.Assert(kind == ElementKind.Set || kind == ElementKind.New, kind.ToString());

                        if (property.IsCollection)
                        {
                            return new AddOperation(property, GetValue(attribute), Depth);
                        }
                        else
                        {
                            return new SetOperation(property, GetValue(attribute), Depth);
                        }
                    }
            }
        }

        private IEnumerable<IPropertyOperation> CreateOperationsFromElement(XElement element)
        {
            Debug.Assert(element.HasAttributes, element.ToString());

            ElementKind kind = element.Kind();

            switch (kind)
            {
                case ElementKind.Set:
                    {
                        foreach (XAttribute attribute in element.Attributes())
                            yield return CreateOperationFromAttribute(element, kind, attribute, throwOnId: true, throwOnCollection: true);

                        break;
                    }
                case ElementKind.Postfix:
                    {
                        foreach (XAttribute attribute in element.Attributes())
                            yield return new PostfixOperation(GetProperty(attribute), GetValue(attribute), Depth);

                        break;
                    }
                case ElementKind.PostfixMany:
                    {
                        foreach (XAttribute attribute in element.Attributes())
                            yield return new PostfixManyOperation(GetProperty(attribute), GetValue(attribute), Depth);

                        break;
                    }
                case ElementKind.Prefix:
                    {
                        foreach (XAttribute attribute in element.Attributes())
                            yield return new PrefixOperation(GetProperty(attribute), GetValue(attribute), Depth);

                        break;
                    }
                case ElementKind.PrefixMany:
                    {
                        foreach (XAttribute attribute in element.Attributes())
                            yield return new PrefixManyOperation(GetProperty(attribute), GetValue(attribute), Depth);

                        break;
                    }
                case ElementKind.Add:
                case ElementKind.Remove:
                    {
                        foreach (XAttribute attribute in element.Attributes())
                            yield return CreateOperationFromAttribute(element, kind, attribute, throwOnId: true);

                        break;
                    }
                case ElementKind.AddRange:
                case ElementKind.RemoveRange:
                    {
                        char separator = ',';

                        foreach (XAttribute attribute in element.Attributes())
                        {
                            switch (attribute.LocalName())
                            {
                                case AttributeNames.Separator:
                                    {
                                        string separatorText = attribute.Value;

                                        if (separatorText.Length != 1)
                                            Throw("Separator must be a single character", attribute);

                                        separator = separatorText[0];
                                        break;
                                    }
                                default:
                                    {
                                        yield return CreateOperationFromAttribute(element, kind, attribute, separator: separator, throwOnId: true);
                                        break;
                                    }
                            }
                        }

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

            if (Entity.TryGetProperty(propertyName, out PropertyDefinition property))
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
                return AttributeValueParser.GetAttributeValue(value, this);
            }
            catch (InvalidValueException ex)
            {
                ThrowInvalidOperation("Error while parsing value.", xobject, ex);
            }

            return null;
        }

        internal Variable FindVariable(string name)
        {
            if (Variables != null)
            {
                Variable variable = Variables.FirstOrDefault(f => DefaultComparer.NameEquals(name, f.Name));

                if (variable != null)
                    return variable;
            }

            return Entity.FindVariable(name);
        }

        protected void Throw(string message, XObject @object = null)
        {
            ThrowInvalidOperation(message, @object ?? Current);
        }
    }
}
