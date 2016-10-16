using System;

namespace Pihrtsoft.Records
{
    public class InvalidValueException : Exception
    {
        public InvalidValueException()
        {
        }

        public InvalidValueException(string message)
            : base(message)
        {
        }

        public InvalidValueException(string message, string value)
            : base(message)
        {
            Value = value;
        }

        public InvalidValueException(string message, Exception inner)
            : base(message, inner)
        {
        }

        public string Value { get; }
    }
}
