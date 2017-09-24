// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Pihrtsoft.Records
{
    internal enum ElementKind
    {
        None,
        Document,
        Entity,
        Entities,
        Declarations,
        Variable,
        Property,
        BaseRecords,
        Records,
        New,
        Set,
        Add,
        AddRange,
        Remove,
        RemoveRange,
        Postfix,
        PostfixMany,
        Prefix,
        PrefixMany
    }
}
