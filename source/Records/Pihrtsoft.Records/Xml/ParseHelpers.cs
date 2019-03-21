// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Pihrtsoft.Records.Utilities;
using static Pihrtsoft.Records.Utilities.ThrowHelper;

namespace Pihrtsoft.Records.Xml
{
    internal static class ParseHelpers
    {
        private static readonly char[] _separatorsSeparator = new char[] { ' ' };

        public static char[] ParseSeparators(string value, XObject @object = null)
        {
            return value
                .Split(_separatorsSeparator, StringSplitOptions.RemoveEmptyEntries)
                .Select(s =>
                {
                    if (!char.TryParse(s, out char separator))
                        ThrowInvalidOperation(ErrorMessages.InvalidSeparator(s), @object);

                    return separator;
                })
                .ToArray();
        }

        public static string ParseAttributeValue(string value, XmlRecordReader reader)
        {
            DocumentOptions options = reader.Options;

            if (!options.UseVariables)
                return value;

            for (int i = 0; i < value.Length; i++)
            {
                if (value[i] == options.OpenVariableDelimiter)
                {
                    if (i == value.Length - 1)
                    {
                        Throw.CharacterMustBeEscaped(value, options.OpenVariableDelimiter);
                    }
                    else if (value[i + 1] == options.OpenVariableDelimiter)
                    {
                        i++;
                    }
                    else
                    {
                        bool fInside = true;
                        int startIndex = i;
                        int lastEndIndex = 0;
                        i++;

                        var sb = new StringBuilder();

                        while (i < value.Length)
                        {
                            if (!fInside)
                            {
                                if (value[i] == options.OpenVariableDelimiter)
                                {
                                    fInside = true;
                                    startIndex = i;
                                }
                                else if (value[i] == options.CloseVariableDelimiter)
                                {
                                    if (i == value.Length - 1
                                        || value[i + 1] != options.CloseVariableDelimiter)
                                    {
                                        Throw.CharacterMustBeEscaped(value, options.CloseVariableDelimiter);
                                    }
                                }
                            }
                            else if (value[i] == options.OpenVariableDelimiter)
                            {
                                if (i - startIndex == 1)
                                {
                                    fInside = false;
                                    startIndex = -1;
                                }
                                else
                                {
                                    Throw.VariableNameCannotContainCharacter(value, options.OpenVariableDelimiter);
                                }
                            }
                            else if (value[i] == options.CloseVariableDelimiter)
                            {
                                int length = i - startIndex - 1;

                                if (length == 0)
                                    Throw.VariableNameCannotBeEmpty(value);

                                string variableName = value.Substring(startIndex + 1, length);

                                Variable variable = reader.FindVariable(variableName);

                                if (variable.IsDefault)
                                    Throw.VariableIsNotDefined(value, variableName);

                                sb.Append(value, lastEndIndex, startIndex - lastEndIndex);
                                sb.Append(variable.Value);

                                fInside = false;
                                startIndex = -1;
                                lastEndIndex = i + 1;
                            }

                            i++;
                        }

                        if (fInside)
                            Throw.VariableMustBeClosed(value, options.OpenVariableDelimiter);

                        if (lastEndIndex > 0)
                            sb.Append(value, lastEndIndex, value.Length - lastEndIndex);

                        return sb.ToString();
                    }
                }
            }

            return value;
        }

        private static class Throw
        {
            public static void CharacterMustBeEscaped(string text, char value)
            {
                throw new InvalidValueException($"A '{value}' character must be escaped (by doubling) in an attribute value.", text);
            }

            public static void VariableNameCannotContainCharacter(string text, char value)
            {
                throw new InvalidValueException($"A variable name cannot contain '{value}' character.", text);
            }

            public static void VariableNameCannotBeEmpty(string text)
            {
                throw new InvalidValueException("A variable name cannot be empty.", text);
            }

            public static void VariableIsNotDefined(string text, string variableName)
            {
                throw new InvalidValueException($"Variable '{variableName}' is not defined.", text);
            }

            public static void VariableMustBeClosed(string text, char value)
            {
                throw new InvalidValueException($"A variable name must end with '{value}' character.", text);
            }
        }
    }
}
