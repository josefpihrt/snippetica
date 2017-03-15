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
            using (IEnumerator<Command> en = CreateCommandFromElement(element).GetEnumerator())
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
                    (commands ?? (commands = new CommandCollection())).Add(CreateCommandFromAttribute(attribute));
                }
            }

            Record record = CreateRecord(id);

            if (commands != null)
                commands.ExecuteAll(record);

            GetChildCommands(element).ExecuteAll(record);

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

        private Command CreateCommandFromAttribute(XAttribute attribute)
        {
            if (DefaultComparer.NameEquals(attribute, AttributeNames.Tag))
            {
                return new AddTagCommand(GetValue(attribute));
            }
            else
            {
                string propertyName = GetPropertyName(attribute);

                string value = GetValue(attribute);

                PropertyDefinition propertyDefinition = Entity.FindProperty(propertyName);

                if (propertyDefinition?.IsCollection == true)
                    return new AddItemCommand(propertyName, value);

                return new SetCommand(propertyName, value);
            }
        }

        private IEnumerable<Command> GetChildCommands(XElement parent)
        {
            foreach (XElement element in parent.Elements())
            {
                Current = element;

                if (element.HasAttributes)
                {
                    if (element.Kind() != ElementKind.Command)
                        ThrowOnUnknownElement(element);

                    foreach (Command command in CreateCommandFromElement(element))
                        yield return command;
                }
                else
                {
                    string propertyName = GetPropertyName(element);

                    string value = GetValue(element);

                    PropertyDefinition propertyDefinition = Entity.FindProperty(propertyName);

                    if (propertyDefinition?.IsCollection == true)
                    {
                        yield return new AddItemCommand(propertyName, value);
                    }
                    else
                    {
                        yield return new SetCommand(propertyName, value);
                    }
                }
            }

            Current = parent;
        }

        private IEnumerable<Command> CreateCommandFromElement(XElement element)
        {
            Debug.Assert(element.HasAttributes, element.ToString());

            switch (element.LocalName())
            {
                case ElementNames.Set:
                    {
                        foreach (XAttribute attribute in element.Attributes())
                            yield return CreateCommandFromAttribute(attribute);

                        break;
                    }
                case ElementNames.Append:
                    {
                        foreach (XAttribute attribute in element.Attributes())
                            yield return new AppendCommand(GetPropertyName(attribute), GetValue(attribute));

                        break;
                    }
                case ElementNames.Prefix:
                    {
                        foreach (XAttribute attribute in element.Attributes())
                            yield return new PrefixCommand(GetPropertyName(attribute), GetValue(attribute));

                        break;
                    }
                case ElementNames.Tag:
                    {
                        XAttribute attribute = element
                            .Attributes()
                            .FirstOrDefault(f => f.LocalName() == AttributeNames.Value);

                        if (attribute != null)
                            yield return new AddTagCommand(GetValue(attribute));

                        break;
                    }
                case ElementNames.Add:
                    {
                        foreach (XAttribute attribute in element.Attributes())
                        {
                            string propertyName = GetAttributeName(attribute);

                            PropertyDefinition property = Entity.FindProperty(propertyName);

                            if (property == null)
                            {
                                Throw(ErrorMessages.PropertyIsNotDefined(propertyName));
                            }
                            else if (!property.IsCollection)
                            {
                                Throw(ErrorMessages.CannotAddItemToNonCollectionProperty(propertyName));
                            }

                            yield return new AddItemCommand(propertyName, GetValue(attribute));
                        }

                        break;
                    }
                default:
                    {
                        Throw(ErrorMessages.CommandIsNotDefined(element.LocalName()));
                        break;
                    }
            }
        }

        private string GetPropertyName(XAttribute attribute)
        {
            string propertyName = GetAttributeName(attribute);

            if (!Entity.ContainsProperty(propertyName))
                Throw(ErrorMessages.PropertyIsNotDefined(propertyName), attribute);

            return propertyName;
        }

        private string GetPropertyName(XElement element)
        {
            string propertyName = GetAttributeName(element);

            if (!Entity.ContainsProperty(propertyName))
                Throw(ErrorMessages.PropertyIsNotDefined(propertyName));

            return propertyName;
        }

        private string GetAttributeName(XAttribute attribute)
        {
            return attribute.LocalName();
        }

        private string GetAttributeName(XElement element)
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
