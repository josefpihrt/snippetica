// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Text.Json.Serialization;
using Pihrtsoft.Snippets;

namespace Snippetica;

public abstract class LanguageDefinition
{
    protected LanguageDefinition()
    {
        Modifiers = new ModifierDefinitionCollection();
        Types = new TypeDefinitionCollection();
        Keywords = new KeywordDefinitionCollection();
    }

    public abstract Language Language { get; }

    public ModifierDefinitionCollection Modifiers { get; init; }

    public TypeDefinitionCollection Types { get; init; }

    public KeywordDefinitionCollection Keywords { get; init; }

    [JsonIgnore]
    public TypeDefinition ObjectType => Types["Object"];

    [JsonIgnore]
    public ModifierDefinition StaticModifier => Modifiers["Static"];

    [JsonIgnore]
    public ModifierDefinition VirtualModifier => Modifiers["Virtual"];

    [JsonIgnore]
    public ModifierDefinition InlineModifier => Modifiers["Inline"];

    [JsonIgnore]
    public ModifierDefinition ConstModifier => Modifiers["Const"];

    [JsonIgnore]
    public ModifierDefinition ConstExprModifier => Modifiers["ConstExpr"];

    public abstract string GetTypeParameterList(string typeName);

    public abstract string GetDefaultParameter();

    public abstract string GetObjectInitializer(string value);

    public abstract string GetCollectionInitializer(string value);

    public abstract string GetDictionaryInitializer(string value);

    public abstract string GetArrayInitializer(string value);

    public abstract string GetVariableInitializer(string value);

    public virtual string GetDefaultValue() => ObjectType.DefaultValue;

    public class CSharpDefinition : LanguageDefinition
    {
        public override Language Language => Language.CSharp;

        public override string GetTypeParameterList(string typeName) => $"<{typeName}>";

        public override string GetDefaultParameter() => $"{ObjectType.Keyword} parameter";

        public override string GetObjectInitializer(string value) => " { " + value + " }";

        public override string GetDictionaryInitializer(string value) => $" {{ [0] = {value} }}";

        public override string GetCollectionInitializer(string value) => " { " + value + " }";

        public override string GetArrayInitializer(string value) => GetCollectionInitializer(value);

        public override string GetVariableInitializer(string value) => $" = {value}";
    }

    public class CppDefinition : LanguageDefinition
    {
        public override Language Language => Language.Cpp;

        public override string GetObjectInitializer(string value) => throw new InvalidOperationException();

        public override string GetArrayInitializer(string value) => " = { " + value + " }";

        public override string GetCollectionInitializer(string value) => throw new InvalidOperationException();

        public override string GetDefaultParameter() => "T parameter";

        public override string GetDictionaryInitializer(string value) => throw new InvalidOperationException();

        public override string GetVariableInitializer(string value) => $" = {value}";

        public override string GetTypeParameterList(string typeName) => "";

        public override string GetDefaultValue() => "0";
    }

    public class VisualBasicDefinition : LanguageDefinition
    {
        public override Language Language => Language.VisualBasic;

        public override string GetTypeParameterList(string typeName) => $"(Of {typeName})";

        public override string GetDefaultParameter() => $"parameter As {ObjectType.Keyword}";

        public override string GetObjectInitializer(string value) => " With {" + value + "}";

        public override string GetDictionaryInitializer(string value) => $" From {{{{0, {value}}}}}";

        public override string GetCollectionInitializer(string value) => " From {" + value + "}";

        public override string GetArrayInitializer(string value) => " {" + value + "}";

        public override string GetVariableInitializer(string value) => $" = {value}";
    }
}
