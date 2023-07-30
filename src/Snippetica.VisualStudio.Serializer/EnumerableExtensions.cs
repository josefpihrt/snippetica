// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;

namespace Snippetica.VisualStudio.Serializer;

internal static class EnumerableExtensions
{
    public static bool CountExceeds<TSource>(this IEnumerable<TSource> collection, int value)
    {
        if (collection is null)
            throw new ArgumentNullException(nameof(collection));

        if (value < 0)
            throw new ArgumentOutOfRangeException(nameof(value));

        if (value == 0)
            return false;

        int cnt = 0;
        using (IEnumerator<TSource> en = collection.GetEnumerator())
        {
            while (en.MoveNext())
            {
                cnt++;
                if (cnt == value)
                    return en.MoveNext();
            }
        }

        return false;
    }

    public static List<TSource> ToList<TSource>(this IEnumerable<TSource> collection, int capacity)
    {
        if (collection is null)
            throw new ArgumentNullException(nameof(collection));

        var list = new List<TSource>(capacity);

        list.AddRange(collection);

        return list;
    }
}
