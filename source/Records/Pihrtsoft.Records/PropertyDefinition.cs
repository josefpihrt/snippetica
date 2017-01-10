using System.Diagnostics;
using System.Xml.Linq;
using Pihrtsoft.Records.Utilities;

namespace Pihrtsoft.Records
{
    [DebuggerDisplay("Name = {Name,nq} Type = {Type,nq} DefaultValue = {DefaultValue} IsCollection = {IsCollection}")]
    public class PropertyDefinition : IKey<string>
    {
        internal PropertyDefinition(
            string name,
            string defaultValue = null,
            bool isCollection = false,
            XElement element = null)
        {
            if (!object.ReferenceEquals(name, IdName))
            {
                if (DefaultComparer.NameEquals(name, Id.Name))
                    ThrowHelper.ThrowInvalidOperation(ExceptionMessages.PropertyNameIsReserved(Id.Name), element);

                if (DefaultComparer.NameEquals(name, AttributeNames.Tag))
                    ThrowHelper.ThrowInvalidOperation(ExceptionMessages.PropertyNameIsReserved(AttributeNames.Tag), element);
            }

            Name = name;
            DefaultValue = defaultValue;
            IsCollection = isCollection;
        }

        internal static string IdName { get; } = "Id";

        internal static PropertyDefinition Id { get; } = new PropertyDefinition(IdName);

        public string Name { get; }
        public string DefaultValue { get; }
        public bool IsCollection { get; }

        public string GetKey()
        {
            return Name;
        }
    }
}
