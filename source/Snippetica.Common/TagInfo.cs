// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Snippetica
{
    public struct TagInfo
    {
        public static TagInfo Default { get; } = new TagInfo();

        internal TagInfo(string name, string value, int keywordIndex)
        {
            Name = name;
            Value = value;
            KeywordIndex = keywordIndex;
        }

        public string Name { get; }

        public string Value { get; }

        public int KeywordIndex { get; }

        public bool Success
        {
            get
            {
                return Name != null
                    && Value != null;
            }
        }
    }
}
