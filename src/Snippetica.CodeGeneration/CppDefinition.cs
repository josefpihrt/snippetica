// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using Pihrtsoft.Snippets;

namespace Snippetica.CodeGeneration;

public class CppDefinition : LanguageDefinition
{
    public override Language Language
    {
        get { return Language.Cpp; }
    }

    public override string GetObjectInitializer(string value)
    {
        throw new InvalidOperationException();
    }

    public override string GetArrayInitializer(string value)
    {
        return " = { " + value + " }";
    }

    public override string GetCollectionInitializer(string value)
    {
        throw new InvalidOperationException();
    }

    public override string GetDefaultParameter()
    {
        return "T parameter";
    }

    public override string GetDictionaryInitializer(string value)
    {
        throw new InvalidOperationException();
    }

    public override string GetVariableInitializer(string value)
    {
        return $" = {value}";
    }

    public override string GetTypeParameterList(string typeName)
    {
        return "";
    }

    public override string GetDefaultValue()
    {
        return "0";
    }
}
