// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Diagnostics;
using System.Xml.Linq;
using Pihrtsoft.Records.Utilities;

namespace Pihrtsoft.Records
{
    [DebuggerDisplay("Name = {Name,nq} DefaultValue = {DefaultValue} IsCollection = {IsCollection}, IsRequired = {IsRequired}")]
    public class PropertyDefinition : IKey<string>
    {
        internal PropertyDefinition(
            string name,
            string defaultValue = null,
            bool isCollection = false,
            bool isRequired = false,
            XElement element = null)
        {
            if (!object.ReferenceEquals(name, IdName)
                && !object.ReferenceEquals(name, TagName))
            {
                if (DefaultComparer.NameEquals(name, IdName))
                    ThrowHelper.ThrowInvalidOperation(ErrorMessages.PropertyNameIsReserved(IdName), element);

                if (DefaultComparer.NameEquals(name, AttributeNames.Tag))
                    ThrowHelper.ThrowInvalidOperation(ErrorMessages.PropertyNameIsReserved(AttributeNames.Tag), element);
            }

            Name = name;
            DefaultValue = defaultValue;
            IsCollection = isCollection;
            IsRequired = isRequired;
        }

        internal static string IdName { get; } = "Id";
        internal static string TagName { get; } = "Tag";

        internal static PropertyDefinition IdProperty { get; } = new PropertyDefinition(IdName);
        internal static PropertyDefinition TagProperty { get; } = new PropertyDefinition(TagName);

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
