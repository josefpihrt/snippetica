// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Pihrtsoft.Records.Operations
{
    internal interface IPropertyOperation : IKey<string>
    {
        string PropertyName { get; }

        string Value { get; }

        OperationKind Kind { get; }

        PropertyDefinition PropertyDefinition { get; }

        int Depth { get; }

        void Execute(Record record);

        bool SupportsExecute { get; }
    }
}
