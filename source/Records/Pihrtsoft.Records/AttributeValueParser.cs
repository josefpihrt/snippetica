using System.Text;

namespace Pihrtsoft.Records
{
    internal static class AttributeValueParser
    {
        public static string GetAttributeValue(string value, RecordReaderBase reader)
        {
            DocumentReaderSettings settings = reader.Settings;

            if (!settings.UseVariables)
                return value;

            for (int i = 0; i < value.Length; i++)
            {
                if (value[i] == settings.OpenVariableDelimiter)
                {
                    if (i == value.Length - 1)
                    {
                        Throw.CharacterMustBeEscaped(value, settings.OpenVariableDelimiter);
                    }
                    else if (value[i + 1] == settings.OpenVariableDelimiter)
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
                                if (value[i] == settings.OpenVariableDelimiter)
                                {
                                    fInside = true;
                                    startIndex = i;
                                }
                                else if (value[i] == settings.CloseVariableDelimiter)
                                {
                                    if (i == value.Length - 1
                                        || value[i + 1] != settings.CloseVariableDelimiter)
                                    {
                                        Throw.CharacterMustBeEscaped(value, settings.CloseVariableDelimiter);
                                    }
                                }
                            }
                            else
                            {
                                if (value[i] == settings.OpenVariableDelimiter)
                                {
                                    if (i - startIndex == 1)
                                    {
                                        fInside = false;
                                        startIndex = -1;
                                    }
                                    else
                                    {
                                        Throw.VariableNameCannotContainCharacter(value, settings.OpenVariableDelimiter);
                                    }
                                }
                                else if (value[i] == settings.CloseVariableDelimiter)
                                {
                                    int length = i - startIndex - 1;

                                    if (length == 0)
                                        Throw.VariableNameCannotBeEmpty(value);

                                    string variableName = value.Substring(startIndex + 1, length);

                                    Variable variable = reader.FindVariable(variableName);

                                    if (variable == null)
                                        Throw.VariableIsNotDefined(value, variableName);

                                    sb.Append(value, lastEndIndex, startIndex - lastEndIndex);
                                    sb.Append(variable.Value);

                                    fInside = false;
                                    startIndex = -1;
                                    lastEndIndex = i + 1;
                                }
                            }

                            i++;
                        }

                        if (fInside)
                            Throw.VariableMustBeClosed(value, settings.OpenVariableDelimiter);

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
