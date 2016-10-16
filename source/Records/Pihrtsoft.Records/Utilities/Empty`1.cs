using System.Collections.ObjectModel;

namespace Pihrtsoft.Records.Utilities
{
    internal static class Empty<T>
    {
        public static T[] Array { get; } = new T[0];

        public static ReadOnlyCollection<T> ReadOnlyCollection { get; } = new ReadOnlyCollection<T>(Array);
    }
}
