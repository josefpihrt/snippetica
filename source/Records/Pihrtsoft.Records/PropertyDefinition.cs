// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Diagnostics;
using System.Xml.Linq;
using Pihrtsoft.Records.Utilities;

namespace Pihrtsoft.Records
{
    [DebuggerDisplay("Name = {Name,nq} Type = {Type,nq} DefaultValue = {DefaultValue} IsCollection = {IsCollection}, IsRequired = {IsRequired}")]
    public class PropertyDefinition : IKey<string>
    {
        internal PropertyDefinition(
            string name,
            string defaultValue = null,
            bool isCollection = false,
            bool isRequired = false,
            XElement element = null)
        {
            if (!object.ReferenceEquals(name, IdName))
            {
                if (DefaultComparer.NameEquals(name, Id.Name))
                    ThrowHelper.ThrowInvalidOperation(ErrorMessages.PropertyNameIsReserved(Id.Name), element);

                if (DefaultComparer.NameEquals(name, AttributeNames.Tag))
                    ThrowHelper.ThrowInvalidOperation(ErrorMessages.PropertyNameIsReserved(AttributeNames.Tag), element);
            }

            Name = name;
            DefaultValue = defaultValue;
            IsCollection = isCollection;
            IsRequired = isRequired;
        }

        internal static string IdName { get; } = "Id";

        internal static PropertyDefinition Id { get; } = new PropertyDefinition(IdName);

        public string Name { get; }
        public string DefaultValue { get; }
        public bool IsCollection { get; }
        public bool IsRequired { get; }

        public string GetKey()
        {
            return Name;
        }
    }
}
