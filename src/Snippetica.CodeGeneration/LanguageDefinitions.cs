// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Snippetica.CodeGeneration;

public static class LanguageDefinitions
{
    public static CSharpDefinition CSharp { get; } = new();

    public static VisualBasicDefinition VisualBasic { get; } = new();

    public static CppDefinition Cpp { get; } = new();
}
