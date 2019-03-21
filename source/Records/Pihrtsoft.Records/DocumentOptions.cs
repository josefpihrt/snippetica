// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Xml.Linq;

namespace Pihrtsoft.Records
{
    public class DocumentOptions
    {
        public static DocumentOptions Default { get; } = new DocumentOptions();

        public DocumentOptions(
            bool useVariables = false,
            char openVariableDelimiter = '{',
            char closeVariableDelimiter = '}',
            bool setLineInfo = true)
        {
            UseVariables = useVariables;
            OpenVariableDelimiter = openVariableDelimiter;
            CloseVariableDelimiter = closeVariableDelimiter;
            SetLineInfo = setLineInfo;
        }

        public bool UseVariables { get; }

        public char OpenVariableDelimiter { get; }

        public char CloseVariableDelimiter { get; }

        public bool SetLineInfo { get; }

        internal LoadOptions LoadOptions => (SetLineInfo) ? LoadOptions.SetLineInfo : LoadOptions.None;
    }
}
