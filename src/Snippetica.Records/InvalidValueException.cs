// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;

namespace Snippetica.Records
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
