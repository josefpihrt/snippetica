// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Snippetica.VisualStudio;

internal class NamespaceComparer : IComparer<string>
{
    private static readonly Regex _systemUsingRegex = new(@"\A\s*System\s*(\.|\z)");

    public NamespaceComparer(bool placeSystemFirst = false)
    {
        PlaceSystemFirst = placeSystemFirst;
    }

    public int Compare(string x, string y)
    {
        if (object.ReferenceEquals(x, y))
            return 0;

        if (x is null)
            return -1;

        if (y is null)
            return 1;

        if (PlaceSystemFirst)
        {
            if (_systemUsingRegex.IsMatch(x))
            {
                if (!_systemUsingRegex.IsMatch(y))
                    return -1;
            }
            else if (_systemUsingRegex.IsMatch(y))
            {
                return 1;
            }
        }

        return string.Compare(x, y, Snippetica.VisualStudio.Comparers.Comparer.StringComparison);
    }

    public bool PlaceSystemFirst { get; }
}
