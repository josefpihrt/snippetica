// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Xml.Linq;

namespace Pihrtsoft.Records
{
    public class DocumentSettings
    {
        public bool UseVariables { get; set; }

        public char OpenVariableDelimiter { get; set; } = '{';

        public char CloseVariableDelimiter { get; set; } = '}';

        public bool SetLineInfo { get; set; } = true;

        internal LoadOptions LoadOptions
        {
            get
            {
                if (SetLineInfo)
                {
                    return LoadOptions.SetLineInfo;
                }
                else
                {
                    return LoadOptions.None;
                }
            }
        }

        public void SetVariableDelimiter(char delimiter)
        {
            OpenVariableDelimiter = delimiter;
            CloseVariableDelimiter = delimiter;
        }
    }
}
