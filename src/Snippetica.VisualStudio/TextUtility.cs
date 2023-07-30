// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;

namespace Snippetica.VisualStudio;

internal static class TextUtility
{
    private const string CDataEnd = "]]>";

    public static bool ContainsCDataEnd(string value)
    {
        if (value is null)
            throw new ArgumentNullException(nameof(value));

        return value.IndexOf(CDataEnd, StringComparison.Ordinal) != -1;
    }
}
