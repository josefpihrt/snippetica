using System.Collections.Generic;
using Pihrtsoft.Records.Commands;

namespace Pihrtsoft.Records
{
    internal static class EnumerableExtensions
    {
        public static void ExecuteAll(this IEnumerable<Command> commands, Record record)
        {
            foreach (Command command in commands)
                command.Execute(record);
        }
    }
}
