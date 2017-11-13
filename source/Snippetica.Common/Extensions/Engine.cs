// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;

namespace Snippetica
{
    public static class EnumerableExtensions
    {
        public static bool CountExceeds<TSource>(this IEnumerable<TSource> collection, int value)
        {
            if (collection == null)
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
    }
}
