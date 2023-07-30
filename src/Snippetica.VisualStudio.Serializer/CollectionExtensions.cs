// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;

namespace Snippetica.VisualStudio.Serializer;

internal static class CollectionExtensions
{
    public static void AddRange<TSource>(this ICollection<TSource> collection, IEnumerable<TSource> itemsToAdd)
    {
        if (collection is null)
            throw new ArgumentNullException(nameof(collection));

        if (itemsToAdd is null)
            throw new ArgumentNullException(nameof(itemsToAdd));

        foreach (TSource item in itemsToAdd)
            collection.Add(item);
    }
}
