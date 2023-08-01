// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Snippetica.VisualStudio.Comparers;

internal class LiteralIdentifierComparer : LiteralComparer
{
    protected override string? GetValue(Literal? literal)
    {
        return literal?.Identifier;
    }
}
