// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Text;

namespace Snippetica.CodeGeneration
{
    public class SnippetCodeBuilder
    {
        public SnippetCodeBuilder()
        {
            StringBuilder = new StringBuilder();
        }

        public StringBuilder StringBuilder { get; }

        public char Delimiter { get; set; } = '$';

        public override string ToString()
        {
            return StringBuilder.ToString();
        }

        public void AppendPlaceholder(string identifier)
        {
            AppendDelimiter();
            Append(identifier);
            AppendDelimiter();
        }

        public void AppendEndPlaceholder()
        {
            AppendPlaceholder("end");
        }

        internal void AppendSelectedPlaceholder()
        {
            AppendPlaceholder("selected");
        }

        public void AppendDelimiter()
        {
            Append(Delimiter);
        }

        public void Append(string value)
        {
            StringBuilder.Append(value);
        }

        public void Append(char value)
        {
            StringBuilder.Append(value);
        }
    }
}
