using System.Collections.Generic;
using System.Collections.ObjectModel;
using Pihrtsoft.Records.Utilities;

namespace Pihrtsoft.Records
{
    public class ReadOnlyKeyedCollection<TKey, TValue> : ReadOnlyCollection<TValue> where TValue : IKey<TKey>
    {
        private ExtendedKeyedCollection<TKey, TValue> _keyedCollection;

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
            TValue value;
            if (_keyedCollection.TryGetValue(key, out value))
                return value;

            return default(TValue);
        }

        public TValue this[TKey key]
        {
            get { return _keyedCollection[key]; }
        }
    }
}
