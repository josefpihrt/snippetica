// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Pihrtsoft.Records.Utilities;

namespace Pihrtsoft.Records
{
    [DebuggerDisplay("{DebuggerDisplay,nq}")]
    public sealed class EntityDefinition : IKey<string>
    {
        internal EntityDefinition(
            string name,
            EntityDefinition baseEntity = null,
            ExtendedKeyedCollection<string, PropertyDefinition> properties = null,
            ExtendedKeyedCollection<string, Variable> variables = null)
        {
            Name = name;

            BaseEntity = baseEntity;

            Properties = (properties != null)
                ? new PropertyDefinitionCollection(properties)
                : Empty.PropertyDefinitionCollection;

            Variables = (variables != null)
                ? new VariableCollection(variables)
                : Empty.VariableCollection;
        }

        public static EntityDefinition Global { get; } = new EntityDefinition(
            name: GlobalName,
            baseEntity: null,
            properties: new ExtendedKeyedCollection<string, PropertyDefinition>(new PropertyDefinition[] { PropertyDefinition.Id }),
            variables: null);

        public string Name { get; }

        public EntityDefinition BaseEntity { get; }

        public PropertyDefinitionCollection Properties { get; }

        public VariableCollection Variables { get; }

        public bool IsGlobalEntity => object.ReferenceEquals(Name, GlobalName);

        internal static string GlobalName { get; } = "_Global";

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string DebuggerDisplay
        {
            get
            {
                string properties = string.Join(", ", AllProperties().Select(f => f.Name).OrderBy(f => f));

                return $"{Name} {BaseEntity.Name} Properties: {properties}";
            }
        }

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
            var entity = this;

            do
            {
                if (entity.Properties.TryGetValue(name, out PropertyDefinition property))
                    return property;

                entity = entity.BaseEntity;

            } while (entity != null);

            return default;
        }

        public bool TryGetProperty(string name, out PropertyDefinition property)
        {
            property = FindProperty(name);
            return property != null;
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
            return !FindVariable(variableName).IsDefault;
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
