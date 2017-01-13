using System.Collections.Generic;

namespace Pihrtsoft.Records
{
    internal static class QueueExtensions
    {
        public static bool Any<T>(this Queue<T> queue)
        {
            return queue.Count > 0;
        }

        public static void EnqueueRange<T>(this Queue<T> queue, IEnumerable<T> items)
        {
            foreach (T item in items)
                queue.Enqueue(item);
        }
    }
}
