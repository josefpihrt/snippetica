// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Pihrtsoft.Snippets;

namespace Snippetica.CodeGeneration
{
    public abstract class LanguageDefinition
    {
        protected LanguageDefinition()
        {
            Modifiers = new ModifierDefinitionCollection();
            Types = new TypeDefinitionCollection();
            Keywords = new KeywordDefinitionCollection();
        }

        public abstract Language Language { get; }

        public ModifierDefinitionCollection Modifiers { get; }

        public TypeDefinitionCollection Types { get; }

        public KeywordDefinitionCollection Keywords { get; }

        public TypeDefinition ObjectType
        {
            get { return Types["Object"]; }
        }

        public ModifierDefinition StaticModifier
        {
            get { return Modifiers["Static"]; }
        }

        public ModifierDefinition VirtualModifier
        {
            get { return Modifiers["Virtual"]; }
        }

        public ModifierDefinition InlineModifier
        {
            get { return Modifiers["Inline"]; }
        }

        public ModifierDefinition ConstModifier
        {
            get { return Modifiers["Const"]; }
        }

        public ModifierDefinition ConstExprModifier
        {
            get { return Modifiers["ConstExpr"]; }
        }

        public abstract string GetTypeParameterList(string typeName);

        public abstract string GetDefaultParameter();

        public abstract string GetCollectionInitializer(string value);

        public abstract string GetDictionaryInitializer(string value);

        public abstract string GetArrayInitializer(string value);

        public abstract string GetVariableInitializer(string value);

        public virtual string GetDefaultValue()
        {
            return ObjectType.DefaultValue;
        }

        public static LanguageDefinition FromLanguage(Language language)
        {
            switch (language)
            {
                case Language.CSharp:
                    return LanguageDefinitions.CSharp;
                case Language.VisualBasic:
                    return LanguageDefinitions.VisualBasic;
                case Language.Cpp:
                    return LanguageDefinitions.Cpp;
                default:
                    return null;
            }
        }

        public static KeywordDefinitionCollection GetKeywords(Language language)
        {
            switch (language)
            {
                case Language.CSharp:
                    return LanguageDefinitions.CSharp.Keywords;
                case Language.VisualBasic:
                    return LanguageDefinitions.VisualBasic.Keywords;
                case Language.Cpp:
                    return LanguageDefinitions.Cpp.Keywords;
                default:
                    return null;
            }
        }
    }
}
