// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Xml.Linq;
using Pihrtsoft.Records.Commands;
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

        private CommandCollection Commands { get; set; }
        private Stack<Variable> Variables { get; set; }

        public virtual bool ShouldCheckRequiredProperty { get; }

        public abstract Collection<Record> ReadRecords();

        protected abstract void AddRecord(Record record);

        protected abstract Record CreateRecord(string id);

        protected void Collect(IEnumerable<XElement> elements)
        {
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
                                AddPendingCommands(element);
                                Collect(element.Elements());
                                Commands.RemoveLast();
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
        }

        private void AddPendingCommands(XElement element)
        {
            using (IEnumerator<Command> en = CreateCommandsFromElement(element).GetEnumerator())
            {
                if (en.MoveNext())
                {
                    Command command = en.Current;

                    if (en.MoveNext())
                    {
                        var commands = new List<Command>()
                        {
                            command,
                            en.Current
                        };

                        while (en.MoveNext())
                            commands.Add(en.Current);

                        AddCommand(new GroupCommand(commands));
                    }
                    else
                    {
                        AddCommand(command);
                    }
                }
            }
        }

        private void AddCommand(Command command)
        {
            (Commands ?? (Commands = new CommandCollection())).Add(command);
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

            CommandCollection commands = null;

            foreach (XAttribute attribute in element.Attributes())
            {
                if (DefaultComparer.NameEquals(attribute, AttributeNames.Id))
                {
                    id = GetValue(attribute);
                }
                else
                {
                    Command command = CreateCommandFromAttribute(element, attribute);

                    (commands ?? (commands = new CommandCollection())).Add(command);
                }
            }

            Record record = CreateRecord(id);

            if (commands != null)
                commands.ExecuteAll(record);

            foreach (Command command in GetChildCommands(element))
                command.Execute(record);

            Commands?.ExecuteAll(record);

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

        private Command CreateCommandFromAttribute(
            XElement element,
            XAttribute attribute,
            bool throwOnId = false,
            bool throwOnTag = false,
            bool throwOnSet = false,
            bool throwOnAdd = false)
        {
            string attributeName = GetAttributeName(attribute);

            if (throwOnId
                && DefaultComparer.NameEquals(attributeName, AttributeNames.Id))
            {
                Throw(ErrorMessages.CannotUseCommandOnProperty(element, attributeName));
            }

            if (DefaultComparer.NameEquals(attributeName, AttributeNames.Tag))
            {
                if (throwOnTag)
                    Throw(ErrorMessages.CannotUseCommandOnProperty(element, attributeName));

                return new AddTagCommand(GetValue(attribute));
            }

            PropertyDefinition property = GetProperty(attribute);

            if (property.IsCollection)
            {
                if (throwOnAdd)
                    Throw(ErrorMessages.CannotUseCommandOnCollectionProperty(element, property.Name));

                return new AddItemCommand(property, GetValue(attribute));
            }
            else
            {
                if (throwOnSet)
                    Throw(ErrorMessages.CannotUseCommandOnNonCollectionProperty(element, property.Name));

                return new SetCommand(property, GetValue(attribute));
            }
        }

        private IEnumerable<Command> GetChildCommands(XElement parent)
        {
            foreach (XElement element in parent.Elements())
            {
                Current = element;

                foreach (Command command in CreateCommandsFromElement(element))
                    yield return command;
            }

            Current = parent;
        }

        private IEnumerable<Command> CreateCommandsFromElement(XElement element)
        {
            Debug.Assert(element.HasAttributes, element.ToString());

            switch (element.LocalName())
            {
                case ElementNames.Set:
                    {
                        foreach (XAttribute attribute in element.Attributes())
                            yield return CreateCommandFromAttribute(element, attribute, throwOnId: true, throwOnTag: true, throwOnAdd: true);

                        break;
                    }
                case ElementNames.Append:
                    {
                        foreach (XAttribute attribute in element.Attributes())
                        {
                            PropertyDefinition property = GetProperty(attribute);
                            yield return new AppendCommand(property, GetValue(attribute));
                        }

                        break;
                    }
                case ElementNames.Prepend:
                    {
                        foreach (XAttribute attribute in element.Attributes())
                        {
                            PropertyDefinition property = GetProperty(attribute);
                            yield return new PrependCommand(property, GetValue(attribute));
                        }

                        break;
                    }
                case ElementNames.Tag:
                    {
                        XAttribute attribute = element.SingleAttributeOrThrow(AttributeNames.Value);

                        yield return new AddTagCommand(GetValue(attribute));

                        break;
                    }
                case ElementNames.Add:
                    {
                        foreach (XAttribute attribute in element.Attributes())
                            yield return CreateCommandFromAttribute(element, attribute, throwOnId: true, throwOnTag: true, throwOnSet: true);

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
            string propertyName = GetAttributeName(attribute);

            PropertyDefinition property;
            if (!Entity.TryGetProperty(propertyName, out property))
            {
                Throw(ErrorMessages.PropertyIsNotDefined(propertyName), attribute);
            }

            return property;
        }

        private PropertyDefinition GetProperty(XElement element)
        {
            string propertyName = GetElementName(element);

            PropertyDefinition property;
            if (!Entity.TryGetProperty(propertyName, out property))
            {
                Throw(ErrorMessages.PropertyIsNotDefined(propertyName));
            }

            return property;
        }

        private string GetAttributeName(XAttribute attribute)
        {
            return attribute.LocalName();
        }

        private string GetElementName(XElement element)
        {
            return element.LocalName();
        }

        private string GetValue(XAttribute attribute)
        {
            return GetValue(attribute.Value, attribute);
        }

        private string GetValue(XElement element)
        {
            return GetValue(element.Value, element);
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
