// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Xml.Linq;
using Pihrtsoft.Records.Xml;

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

        public static string UnknownAttribute(XAttribute attribute)
        {
            return $"Element '{attribute.Parent?.LocalName()}' contains unknown attribute '{attribute.LocalName()}'.";
        }

        public static string MultipleElementsWithEqualName(XElement element)
        {
            return $"Element '{element.Parent?.LocalName()}' cannot contains multiple elements with name '{element.LocalName()}'.";
        }

        public static string MissingWithRecordIdentifier()
        {
            return $"Record must define attribute '{PropertyDefinition.IdName}'.";
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

        public static string OperationIsNotDefined(string operationName)
        {
            return $"Operation '{operationName}' is not defined.";
        }

        public static string PropertyIsNotDefined(string propertyName)
        {
            return $"Property '{propertyName}' is not defined.";
        }

        public static string CollectionIsNotDefined(string elementName)
        {
            return $"Collection with element name '{elementName}' is not defined.";
        }

        public static string CannotUseOperationOnProperty(XElement element, string propertyName)
        {
            return $"Cannot use {element.LocalName()} operation on property '{propertyName}'.";
        }

        public static string CannotUseOperationOnCollectionProperty(XElement element, string propertyName)
        {
            return $"Cannot use {element.LocalName()} operation on collection property '{propertyName}'.";
        }

        public static string CannotUseOperationOnNonCollectionProperty(XElement element, string propertyName)
        {
            return $"Cannot use {element.LocalName()} operation on non-collection property '{propertyName}'.";
        }

        public static string CollectionPropertyCannotDefineDefaultValue()
        {
            return "Collection property cannot define default value.";
        }

        internal static string CommandCannotBeUsedAsChildCommandOfNewCommand(XElement element)
        {
            return $"Command '{element.LocalName()}' cannot be used as a child command of 'New' command.";
        }

        public static string InvalidSeparator(string separator)
        {
            return $"Invalid separator '{separator}'.";
        }
    }
}
