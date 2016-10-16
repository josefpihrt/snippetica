using System.Collections.Generic;

namespace Pihrtsoft.Records
{
    internal static class ICollectionExtensions
    {
        public static void AddRange<T>(this ICollection<T> collection, IEnumerable<T> itemsToAdd)
        {
            foreach (T item in itemsToAdd)
                collection.Add(item);
        }
    }
}
