﻿// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Snippetica.VisualStudio;

namespace Snippetica.CodeGeneration;

public class LanguageExecutionContext : ExecutionContext
{
    public LanguageExecutionContext(Snippet snippet, LanguageDefinition language)
        : base(snippet)
    {
        Language = language;
    }

    public LanguageDefinition Language { get; }
}
