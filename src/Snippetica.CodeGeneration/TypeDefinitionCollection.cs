// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.ObjectModel;

namespace Snippetica.CodeGeneration
{
    public class TypeDefinitionCollection : KeyedCollection<string, TypeDefinition>
    {
        protected override string GetKeyForItem(TypeDefinition item)
        {
            return item.Name;
        }
    }
}
