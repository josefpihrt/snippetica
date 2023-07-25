﻿// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Pihrtsoft.Snippets;

namespace Snippetica.CodeGeneration;

public class VisualBasicDefinition : LanguageDefinition
{
    public override Language Language
    {
        get { return Language.VisualBasic; }
    }

    public override string GetTypeParameterList(string typeName)
    {
        return $"(Of {typeName})";
    }

    public override string GetDefaultParameter()
    {
        return $"parameter As {ObjectType.Keyword}";
    }

    public override string GetObjectInitializer(string value)
    {
        return " With {" + value + "}";
    }

    public override string GetDictionaryInitializer(string value)
    {
        return $" From {{{{0, {value}}}}}";
    }

    public override string GetCollectionInitializer(string value)
    {
        return " From {" + value + "}";
    }

    public override string GetArrayInitializer(string value)
    {
        return " {" + value + "}";
    }

    public override string GetVariableInitializer(string value)
    {
        return $" = {value}";
    }
}
