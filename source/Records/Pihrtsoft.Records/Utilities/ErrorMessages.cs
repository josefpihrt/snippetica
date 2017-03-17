// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Xml.Linq;

namespace Pihrtsoft.Records.Utilities
{
    internal static class ErrorMessages
    {
        public static string PropertyIsRequired(string propertyName)
        {
            return $"Property '{propertyName}' is required.";
        }

        public static string DocumentVersionIsNotSupported(Version version, Version supportedVersion)
        {
            return $"Document version '{version}' is not supported. Version '{supportedVersion}' or lower is supported.";
        }

        public static string InvalidDocumentVersion()
        {
            return "Document version is invalid.";
        }

        public static string MissingElement(string name)
        {
            return $"Element '{name}' was not found.";
        }

        public static string UnknownElement(XElement element)
        {
            return $"Element '{element.Parent?.LocalName()}' contains unknown element '{element.LocalName()}'.";
        }

        public static string MultipleElementsWithEqualName(XElement element)
        {
            return $"Element '{element.Parent?.LocalName()}' cannot contains multiple elements with name '{element.LocalName()}'.";
        }

        public static string MissingBaseRecordIdentifier()
        {
            return $"Base record must define attribute '{PropertyDefinition.Id.Name}'.";
        }

        public static string ItemAlreadyDefined(string propertyName, string name)
        {
            return $"{propertyName} '{name}' is already defined.";
        }

        public static string PropertyAlreadyDefined(string propertyName, string entityName)
        {
            return $"Property '{propertyName}' in entity '{entityName}' is already defined in base entity.";
        }

        public static string PropertyNameIsReserved(string propertyName)
        {
            return $"Property name '{propertyName}' is reserved.";
        }

        public static string VariableAlreadyDefined(string variableName, string entityName)
        {
            return $"Variable '{variableName}' in entity '{entityName}' is already defined in base entity.";
        }

        public static string CommandIsNotDefined(string command)
        {
            return $"Command '{command}' is not defined.";
        }

        public static string PropertyIsNotDefined(string propertyName)
        {
            return $"Property '{propertyName}' is not defined.";
        }

        internal static string CannotUseCommandOnProperty(XElement element, string propertyName)
        {
            return $"Cannot use {element.LocalName()} command on property '{propertyName}'.";
        }

        internal static string CannotUseCommandOnCollectionProperty(XElement element, string propertyName)
        {
            return $"Cannot use {element.LocalName()} command on collection property '{propertyName}'.";
        }

        internal static string CannotUseCommandOnNonCollectionProperty(XElement element, string propertyName)
        {
            return $"Cannot use {element.LocalName()} command on non-collection property '{propertyName}'.";
        }
    }
}
