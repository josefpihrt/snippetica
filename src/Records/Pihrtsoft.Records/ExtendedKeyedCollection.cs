// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Pihrtsoft.Records
{
    internal class ExtendedKeyedCollection<TKey, TItem> : KeyedCollection<TKey, TItem> where TItem : IKey<TKey>
    {
        public ExtendedKeyedCollection()
        {
        }

        public ExtendedKeyedCollection(IList<TItem> list)
        {
            foreach (TItem item in list)
                Add(item);
        }

        public ExtendedKeyedCollection(IEqualityComparer<TKey> comparer)
            : base(comparer)
        {
        }

        public ExtendedKeyedCollection(IList<TItem> list, IEqualityComparer<TKey> comparer)
            : base(comparer)
        {
            foreach (TItem item in list)
                Add(item);
        }

        public bool TryGetValue(TKey key, out TItem value)
        {
            if (Dictionary != null)
                return Dictionary.TryGetValue(key, out value);

            value = default(TItem);
            return false;
        }

        protected override TKey GetKeyForItem(TItem item)
        {
            return item.GetKey();
        }
    }
}
