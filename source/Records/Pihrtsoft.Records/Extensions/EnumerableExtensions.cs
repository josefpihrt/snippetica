// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

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
