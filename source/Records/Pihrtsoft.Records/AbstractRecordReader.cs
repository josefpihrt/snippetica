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
                    case ElementKind.Command:
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

                PropertyOperationCollection propertyOperations;
                if (!Operations.TryGetValue(operation.PropertyName, out propertyOperations))
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
                    IPropertyOperation operation = CreateOperationFromAttribute(element, attribute);

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
                        if (property.IsCollection)
                        {
                            record[property.Name] = new List<object>() { property.DefaultValue };
                        }
                        else
                        {
                            record[property.Name] = property.DefaultValue;
                        }
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

                            string value;
                            if (!pendingValues.TryGetValue(kind, out value))
                            {
                                pendingValues[kind] = operation.Value;
                            }
                            else
                            {
                                switch (kind)
                                {
                                    case OperationKind.MultiPostfix:
                                        {
                                            pendingValues[kind] = operation.Value + pendingValues[kind];
                                            break;
                                        }
                                    case OperationKind.MultiPrefix:
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
                    case OperationKind.MultiPostfix:
                        {
                            new PostfixOperation(propertyDefinition, pair.Value, Depth).Execute(record);
                            break;
                        }
                    case OperationKind.MultiPrefix:
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
            XAttribute attribute,
            bool throwOnId = false,
            bool throwOnTag = false,
            bool throwOnSet = false,
            bool throwOnAdd = false)
        {
            string attributeName = attribute.LocalName();

            if (throwOnId
                && DefaultComparer.NameEquals(attributeName, AttributeNames.Id))
            {
                Throw(ErrorMessages.CannotUseCommandOnProperty(element, attributeName));
            }

            if (DefaultComparer.NameEquals(attributeName, AttributeNames.Tag))
            {
                if (throwOnTag)
                    Throw(ErrorMessages.CannotUseCommandOnProperty(element, attributeName));

                return new AddTagOperation(GetValue(attribute), Depth);
            }

            PropertyDefinition property = GetProperty(attribute);

            if (property.IsCollection)
            {
                if (throwOnAdd)
                    Throw(ErrorMessages.CannotUseCommandOnCollectionProperty(element, property.Name));

                return new AddItemOperation(property, GetValue(attribute), Depth);
            }
            else
            {
                if (throwOnSet)
                    Throw(ErrorMessages.CannotUseCommandOnNonCollectionProperty(element, property.Name));

                return new SetOperation(property, GetValue(attribute), Depth);
            }
        }

        private IEnumerable<IPropertyOperation> CreateOperationsFromElement(XElement element)
        {
            Debug.Assert(element.HasAttributes, element.ToString());

            switch (element.LocalName())
            {
                case ElementNames.Set:
                    {
                        foreach (XAttribute attribute in element.Attributes())
                            yield return CreateOperationFromAttribute(element, attribute, throwOnId: true, throwOnTag: true, throwOnAdd: true);

                        break;
                    }
                case ElementNames.Postfix:
                    {
                        foreach (XAttribute attribute in element.Attributes())
                            yield return new PostfixOperation(GetProperty(attribute), GetValue(attribute), Depth);

                        break;
                    }
                case ElementNames.MultiPostfix:
                    {
                        foreach (XAttribute attribute in element.Attributes())
                            yield return new MultiPostfixOperation(GetProperty(attribute), GetValue(attribute), Depth);

                        break;
                    }
                case ElementNames.Prefix:
                    {
                        foreach (XAttribute attribute in element.Attributes())
                            yield return new PrefixOperation(GetProperty(attribute), GetValue(attribute), Depth);

                        break;
                    }
                case ElementNames.MultiPrefix:
                    {
                        foreach (XAttribute attribute in element.Attributes())
                            yield return new MultiPrefixOperation(GetProperty(attribute), GetValue(attribute), Depth);

                        break;
                    }
                case ElementNames.Tag:
                    {
                        XAttribute attribute = element.SingleAttributeOrThrow(AttributeNames.Value);

                        yield return new AddTagOperation(GetValue(attribute), Depth);

                        break;
                    }
                case ElementNames.Add:
                    {
                        foreach (XAttribute attribute in element.Attributes())
                            yield return CreateOperationFromAttribute(element, attribute, throwOnId: true, throwOnTag: true, throwOnSet: true);

                        break;
                    }
                default:
                    {
                        Throw(ErrorMessages.CommandIsNotDefined(element.LocalName()));
                        break;
                    }
            }
        }

        private PropertyDefinition GetProperty(XAttribute attribute)
        {
            string propertyName = attribute.LocalName();

            PropertyDefinition property;
            if (!Entity.TryGetProperty(propertyName, out property))
            {
                Throw(ErrorMessages.PropertyIsNotDefined(propertyName), attribute);
            }

            return property;
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
