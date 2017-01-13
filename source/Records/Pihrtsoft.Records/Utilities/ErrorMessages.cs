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

        public static string CannotAddItemToNonCollectionProperty(string propertyName)
        {
            return $"Cannot add item to a non-collection property '{propertyName}'.";
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
            return $"'{name}' element was not found.";
        }

        public static string UnknownElement(XElement element)
        {
            return $"'{element.Parent?.LocalName()}' element contains unknown '{element.LocalName()}' element.";
        }

        public static string MultipleElementsWithEqualName(XElement element)
        {
            return $"'{element.Parent?.LocalName()}' element cannot contains multiple '{element.LocalName()}' elements.";
        }

        public static string MissingBaseRecordIdentifier()
        {
            return $"Base record must define '{PropertyDefinition.Id.Name}' attribute.";
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
    }
}
