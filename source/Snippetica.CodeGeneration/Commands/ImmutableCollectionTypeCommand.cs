// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.IO;
using Pihrtsoft.Snippets;

namespace Snippetica.CodeGeneration.Commands
{
    public class ImmutableCollectionTypeCommand : CollectionTypeCommand
    {
        public ImmutableCollectionTypeCommand(TypeDefinition type)
            : base(type)
        {
        }

        protected override void ProcessFilePath(ExecutionContext context, Snippet snippet)
        {
            snippet.SetFileName(Path.GetFileName(snippet.FilePath).Replace("ImmutableCollection", Type.Name));
        }
    }
}
