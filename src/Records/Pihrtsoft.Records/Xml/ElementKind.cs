// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Pihrtsoft.Records.Xml
{
    internal enum ElementKind
    {
        None = 0,
        Document = 1,
        Entity = 2,
        Entities = 3,
        Declarations = 4,
        Variable = 5,
        Property = 6,
        Records = 7,
        New = 8,
        With = 9,
        Without = 10,
        Prefix = 11,
        Postfix = 12,
    }
}
