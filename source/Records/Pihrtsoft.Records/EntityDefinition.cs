using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Xml.Linq;
using Pihrtsoft.Records.Utilities;

namespace Pihrtsoft.Records
{
    [DebuggerDisplay("{Name,nq} Properties: {PropertiesText,nq}")]
    public class EntityDefinition : IKey<string>
    {
        internal EntityDefinition(
            string name,
            EntityDefinition baseEntity = null,
            ExtendedKeyedCollection<string, PropertyDefinition> properties = null,
            ExtendedKeyedCollection<string, Variable> variables = null,
            XElement element = null)
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
                            ThrowHelper.ThrowInvalidOperation(ExceptionMessages.PropertyAlreadyDefined(property.Name, name), element);
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
            GlobalName,
            null,
            new ExtendedKeyedCollection<string, PropertyDefinition>(new PropertyDefinition[] { PropertyDefinition.Id }),
            null);

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

        private static PropertyDefinition FindProperty(string name, EntityDefinition entity)
        {
            do
            {
                PropertyDefinition property;

                if (entity.Properties.TryGetValue(name, out property))
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
                Variable variable;

                if (entity.Variables.TryGetValue(name, out variable))
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

        public string GetKey()
        {
            return Name;
        }
    }
}
