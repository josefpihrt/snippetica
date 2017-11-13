// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Xml.Linq;
using Pihrtsoft.Records.Utilities;

namespace Pihrtsoft.Records
{
    [DebuggerDisplay("{Name,nq} {BaseEntity.Name,nq} Properties: {PropertiesText,nq}")]
    public class EntityDefinition : IKey<string>
    {
        internal EntityDefinition(
            XElement element,
            EntityDefinition baseEntity = null,
            ExtendedKeyedCollection<string, PropertyDefinition> properties = null,
            ExtendedKeyedCollection<string, Variable> variables = null)
            : this(element, element.AttributeValueOrThrow(AttributeNames.Name), baseEntity, properties, variables)
        {
        }

        private EntityDefinition(
            XElement element,
            string name,
            EntityDefinition baseEntity = null,
            ExtendedKeyedCollection<string, PropertyDefinition> properties = null,
            ExtendedKeyedCollection<string, Variable> variables = null)
        {
            Name = name;

            if (baseEntity == null && !IsGlobalEntity)
                baseEntity = Global;

            BaseEntity = baseEntity;

            if (properties != null)
            {
                if (!IsGlobalEntity && baseEntity != null)
                {
                    foreach (PropertyDefinition property in properties)
                    {
                        if (FindProperty(property.Name, baseEntity) != null)
                            ThrowHelper.ThrowInvalidOperation(ErrorMessages.PropertyAlreadyDefined(property.Name, name), element);
                    }
                }

                Properties = new PropertyDefinitionCollection(properties);
            }
            else
            {
                Properties = Empty.PropertyDefinitionCollection;
            }

            if (variables != null)
            {
                Variables = new VariableCollection(variables);
            }
            else
            {
                Variables = Empty.VariableCollection;
            }
        }

        public static EntityDefinition Global { get; } = new EntityDefinition(
            element: null,
            name: GlobalName,
            baseEntity: null,
            properties: new ExtendedKeyedCollection<string, PropertyDefinition>(new PropertyDefinition[] { PropertyDefinition.Id }),
            variables: null);

        public string Name { get; }

        public EntityDefinition BaseEntity { get; }

        protected PropertyDefinitionCollection Properties { get; }

        protected VariableCollection Variables { get; }

        public bool IsGlobalEntity
        {
            get { return object.ReferenceEquals(Name, GlobalName); }
        }

        private string PropertiesText
        {
            get { return string.Join(", ", AllProperties().Select(f => f.Name).OrderBy(f => f)); }
        }

        internal static string GlobalName { get; } = "_Global";

        public IEnumerable<PropertyDefinition> AllProperties()
        {
            var entity = this;

            do
            {
                foreach (PropertyDefinition property in entity.Properties)
                    yield return property;

                entity = entity.BaseEntity;

            } while (entity != null);
        }

        public IEnumerable<Variable> AllVariables()
        {
            var entity = this;

            do
            {
                foreach (Variable variable in entity.Variables)
                    yield return variable;

                entity = entity.BaseEntity;

            } while (entity != null);
        }

        public PropertyDefinition FindProperty(string name)
        {
            return FindProperty(name, this);
        }

        public bool TryGetProperty(string name, out PropertyDefinition property)
        {
            property = FindProperty(name, this);
            return property != null;
        }

        private static PropertyDefinition FindProperty(string name, EntityDefinition entity)
        {
            do
            {
                if (entity.Properties.TryGetValue(name, out PropertyDefinition property))
                    return property;

                entity = entity.BaseEntity;

            } while (entity != null);

            return default(PropertyDefinition);
        }

        public Variable FindVariable(string name)
        {
            return FindVariable(name, this);
        }

        private static Variable FindVariable(string name, EntityDefinition entity)
        {
            do
            {
                if (entity.Variables.TryGetValue(name, out Variable variable))
                    return variable;

                entity = entity.BaseEntity;

            } while (entity != null);

            return default(Variable);
        }

        public bool ContainsProperty(string propertyName)
        {
            return FindProperty(propertyName) != null;
        }

        public bool ContainsVariable(string variableName)
        {
            return FindVariable(variableName) != null;
        }

        public IEnumerable<EntityDefinition> BaseEntities()
        {
            EntityDefinition type = BaseEntity;

            while (type != null)
            {
                yield return type;
                type = type.BaseEntity;
            }
        }

        public IEnumerable<EntityDefinition> BaseEntitiesAndSelf()
        {
            var type = this;

            do
            {
                yield return type;
                type = type.BaseEntity;

            } while (type != null);
        }

        string IKey<string>.GetKey()
        {
            return Name;
        }
    }
}
