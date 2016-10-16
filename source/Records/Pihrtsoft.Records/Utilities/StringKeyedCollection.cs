using System.Collections.Generic;

namespace Pihrtsoft.Records.Utilities
{
    internal abstract class StringKeyedCollection<TValue> : ExtendedKeyedCollection<string, TValue> where TValue : IKey<string>
    {
        protected StringKeyedCollection()
        {
        }

        public StringKeyedCollection(IList<TValue> list)
            : base(list)
        {
        }

        public StringKeyedCollection(IEqualityComparer<string> comparer)
            : base(comparer)
        {
        }

        public StringKeyedCollection(IList<TValue> list, IEqualityComparer<string> comparer)
            : base(list, comparer)
        {
        }
    }
}
