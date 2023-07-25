// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.ObjectModel;

namespace Snippetica.Records.Utilities
{
    internal static class Empty
    {
        public static PropertyDefinitionCollection PropertyDefinitionCollection { get; } = new(Array.Empty<PropertyDefinition>());

        public static VariableCollection VariableCollection { get; } = new(Array.Empty<Variable>());

        public static WithRecordCollection WithRecordCollection { get; } = new(Array.Empty<Record>());

        public static ReadOnlyCollection<T> ReadOnlyCollection<T>()
        {
            return Empty<T>.ReadOnlyCollection;
        }
    }
}
