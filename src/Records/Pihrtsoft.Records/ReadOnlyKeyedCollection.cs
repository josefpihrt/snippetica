// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Pihrtsoft.Records
{
    public class ReadOnlyKeyedCollection<TKey, TValue> : ReadOnlyCollection<TValue> where TValue : IKey<TKey>
    {
        private readonly ExtendedKeyedCollection<TKey, TValue> _keyedCollection;

        public ReadOnlyKeyedCollection(IList<TValue> list)
            : this(new ExtendedKeyedCollection<TKey, TValue>(list))
        {
        }

        internal ReadOnlyKeyedCollection(ExtendedKeyedCollection<TKey, TValue> collection)
            : base(collection)
        {
            _keyedCollection = (ExtendedKeyedCollection<TKey, TValue>)Items;
        }

        public bool TryGetValue(TKey key, out TValue value)
        {
            return _keyedCollection.TryGetValue(key, out value);
        }

        internal TValue Find(TKey key)
        {
            if (_keyedCollection.TryGetValue(key, out TValue value))
            {
                return value;
            }
            else
            {
                return default(TValue);
            }
        }

        public TValue this[TKey key]
        {
            get { return _keyedCollection[key]; }
        }
    }
}
