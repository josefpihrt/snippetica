﻿// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;

namespace Snippetica.VisualStudio.Serializer.Comparers;

internal class LiteralIdentifierComparer : LiteralComparer
{
    protected override string GetValue(Literal literal)
    {
        if (literal is null)
            throw new ArgumentNullException(nameof(literal));

        return literal.Identifier;
    }
}
