// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.ObjectModel;

namespace Pihrtsoft.Records.Utilities
{
    internal static class Empty
    {
        public static PropertyDefinitionCollection PropertyDefinitionCollection { get; } = new PropertyDefinitionCollection(Array<PropertyDefinition>());

        public static VariableCollection VariableCollection { get; } = new VariableCollection(Array<Variable>());

        public static BaseRecordCollection BaseRecordCollection { get; } = new BaseRecordCollection(Array<Record>());

        public static RecordCollection RecordCollection { get; } = new RecordCollection(Array<Record>());

        public static T[] Array<T>()
        {
            return Empty<T>.Array;
        }

        public static ReadOnlyCollection<T> ReadOnlyCollection<T>()
        {
            return Empty<T>.ReadOnlyCollection;
        }
    }
}
